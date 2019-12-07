using System.Collections.Generic;

public class PathCache {

	private int maxSize;

	private Dictionary<ILevelTile, Dictionary<ILevelTile, IList<ILevelTile>>> cache = new Dictionary<ILevelTile, Dictionary<ILevelTile, IList<ILevelTile>>>();

	/// <summary>
	/// Tracks, which paths are buffered
	/// </summary>
	private List<(ILevelTile, ILevelTile)> invalidationBuffer = new List<(ILevelTile from, ILevelTile to)>();

	/// <param name="maxSize">The maximum amount of cached paths</param>
	public PathCache(int maxSize = 1024) {
		this.maxSize = maxSize;
	}

	/// <summary>
	/// Invalidates the cached paths(s) for the given parameters.
	/// If "from" is <code>null</code> all paths get cleared.
	/// If "from" is not <code>null</code> but "to" is, all paths starting from the "from" get cleared.
	/// Otherwise just the path from "from" to "to" gets cleared.
	/// </summary>
	public void InvalidateCache(ILevelTile from = null, ILevelTile to = null) {
		if (from == null) {
			cache.Clear();
		} else {
			if (cache.TryGetValue(from, out Dictionary<ILevelTile, IList<ILevelTile>> fromCache)) {
				if (to == null) {
					fromCache.Clear();
				} else {
					fromCache.Remove(to);
				}
			}
		}
	}

	/// <summary>
	/// Caches the path between the two given <code>ILevelTile</code>s
	/// </summary>
	public void CachePath(IList<ILevelTile> path) {
		if ((path == null) || (path.Count == 0)) {
			return;
		}
		ILevelTile from = path[0];
		ILevelTile to = path[path.Count - 1];
		Dictionary<ILevelTile, IList<ILevelTile>> fromCache;
		if (cache.TryGetValue(from, out fromCache)) {
			fromCache[to] = path;
		} else {
			fromCache = new Dictionary<ILevelTile, IList<ILevelTile>>();
			fromCache[to] = path;
			cache[from] = fromCache;
		}

		invalidationBuffer.Add((from, to));
		RemoveNextFromInvalidationBuffer();
	}

	/// <summary>
	/// Invalidates the next path if the buffer is full
	/// </summary>
	private void RemoveNextFromInvalidationBuffer() {
		if (invalidationBuffer.Count > maxSize) {
			(ILevelTile, ILevelTile) nextToInvalidate = invalidationBuffer[0];
			invalidationBuffer.RemoveAt(0);
			InvalidateCache(nextToInvalidate.Item1, nextToInvalidate.Item2);
		}
	}

	/// <summary>
	/// Tries to obtain a cached path between the given points. Returns true if successful, false otherwise.
	/// </summary>
	public bool TryGetPath(ILevelTile from, ILevelTile to, out IList<ILevelTile> path) {
		if ((from != null) && (to != null)) {
			if (cache.TryGetValue(from, out Dictionary<ILevelTile, IList<ILevelTile>> fromCache)) {
				if (fromCache.TryGetValue(to, out IList<ILevelTile> cachedPath)) {
					path = new List<ILevelTile>(cachedPath);
					// Reset position in buffer
					invalidationBuffer.Remove((from, to));
					invalidationBuffer.Add((from, to));
					return true;
				}
			}
		}
		path = null;
		return false;
	}
}
