using System.Collections.Generic;
using UnityEngine;

public class NavMesh2D : MonoBehaviour, INavigationGraphProvider2D {
	private const int TILE_SIZE = 1;

	public static NavMesh2D instance;

	/// <summary>
	/// Determines how far a agents new destination has to be apart from its last one, to which a path was calculated,
	/// in order to retrigger a recalculation of the agents path.
	/// </summary>
	public float pathRecalculationThreshold = .5f;

	private PathCache cache;

	private IPathFindingAlgorithm pathFindingAlgorithm;

	private ISet<NavAgent> agents = new HashSet<NavAgent>();
	/// <summary>
	/// The last destinations of the agents to which an actual path was calculated
	/// </summary>
	private Dictionary<NavAgent, Vector2?> lastCalculatedDestinations = new Dictionary<NavAgent, Vector2?>();

	private IPathProcessor pathProcessor;

	private void Awake() {
		instance = this;
		cache = new PathCache();

		pathProcessor = new RemoveRedundantPathPoints(new RemoveEndBacktracking(new RemoveStartBacktracking(null)));
	}

	public void RegisterAgent(NavAgent agent) {
		if (agent == null) {
			return;
		}
		agents.Add(agent);
		agent.OnDestinationChanged += RecalcNavAgentPath;
	}

	public void UnregisterAgent(NavAgent agent) {
		if (agent == null) {
			return;
		}
		agents.Remove(agent);
		lastCalculatedDestinations.Remove(agent);
	}

	public ISet<ILevelTile> GetConnectedNavPoints(ILevelTile tile) {
		ISet<ILevelTile> ret = new HashSet<ILevelTile>();
		LevelController lvlController = LevelController.instance;
		if (lvlController == null) {
			Debug.LogError("No active LevelController. Cannot provide navigation graph!");
			return ret;
		}

		// Orthogonal connected points
		ILevelTile upper = lvlController.GetTileAtWorldPos(new Vector2(tile.Pos.x, tile.Pos.y + TILE_SIZE));
		bool upperValid = upper != null && upper.IsTraversable();
		if (upperValid) {
			ret.Add(upper);
		}

		ILevelTile right = lvlController.GetTileAtWorldPos(new Vector2(tile.Pos.x + TILE_SIZE, tile.Pos.y));
		bool rightValid = right != null && right.IsTraversable();
		if (rightValid) {
			ret.Add(right);
		}

		ILevelTile lower = lvlController.GetTileAtWorldPos(new Vector2(tile.Pos.x, tile.Pos.y - TILE_SIZE));
		bool lowerValid = lower != null && lower.IsTraversable();
		if (lowerValid) {
			ret.Add(lower);
		}

		ILevelTile left = lvlController.GetTileAtWorldPos(new Vector2(tile.Pos.x - TILE_SIZE, tile.Pos.y));
		bool leftValid = left != null && left.IsTraversable();
		if (leftValid) {
			ret.Add(left);
		}

		// Diagonal connected points
		ret.UnionWith(GetDiagonalConnectedPoints(tile, lvlController, upperValid, rightValid, lowerValid, leftValid));

		return ret;
	}

	private ISet<ILevelTile> GetDiagonalConnectedPoints(ILevelTile tile, LevelController lvlController, bool upperValid, bool rightValid, bool lowerValid, bool leftValid) {
		ISet<ILevelTile> ret = new HashSet<ILevelTile>();

		if (upperValid && rightValid) {
			ILevelTile upperRight = lvlController.GetTileAtWorldPos(new Vector2(tile.Pos.x + TILE_SIZE, tile.Pos.y + TILE_SIZE));
			if (upperRight != null && upperRight.IsTraversable()) {
				ret.Add(upperRight);
			}
		}

		if (rightValid && lowerValid) {
			ILevelTile lowerRight = lvlController.GetTileAtWorldPos(new Vector2(tile.Pos.x + TILE_SIZE, tile.Pos.y - TILE_SIZE));
			if (lowerRight != null && lowerRight.IsTraversable()) {
				ret.Add(lowerRight);
			}
		}

		if (upperValid && leftValid) {
			ILevelTile upperLeft = lvlController.GetTileAtWorldPos(new Vector2(tile.Pos.x - TILE_SIZE, tile.Pos.y + TILE_SIZE));
			if (upperLeft != null && upperLeft.IsTraversable()) {
				ret.Add(upperLeft);
			}
		}

		if (lowerValid && leftValid) {
			ILevelTile lowerLeft = lvlController.GetTileAtWorldPos(new Vector2(tile.Pos.x - TILE_SIZE, tile.Pos.y - TILE_SIZE));
			if (lowerLeft != null && lowerLeft.IsTraversable()) {
				ret.Add(lowerLeft);
			}
		}

		return ret;
	}

	public IList<Vector2> GetPathTo(Vector2 from, Vector2 to) {
		if (pathFindingAlgorithm == null) {
			pathFindingAlgorithm = new AStarAlgorithm(this, new EuclidianHeuristic());
		}
		ILevelTile fromTile = LevelController.instance.GetTileAtWorldPos(from);
		ILevelTile toTile = LevelController.instance.GetTileAtWorldPos(to);

		if (cache.TryGetPath(fromTile, toTile, out IList<ILevelTile> path) == false) {
			path = pathFindingAlgorithm.CalculatePath(fromTile, toTile);
			cache.CachePath(path);
		}
		if (path == null) {
			return null;
		}
		// Convert Tile to Point-Path
		IList<Vector2> ret = new List<Vector2>();
		foreach (ILevelTile tile in path) {
			ret.Add(tile.CenterPos);
		}
		// Add last if not included in tilepath
		if (ret.Count != 0) {
			if ((ret[ret.Count - 1] - to).magnitude > 0.1f) {
				ret.Add(to);
			}
		}
		// insert start point in between for path processing, then remove it again as agents shoul always move to the next pont.
		ret.Insert(0, from);
		pathProcessor.ProcessPath(ret);
		ret.RemoveAt(0);

		return ret;
	}

	private void RecalcNavAgentPath(NavAgent agent, Vector2? destination) {
		if (destination.HasValue == false) {
			agent.UpdatePath(null);
			lastCalculatedDestinations[agent] = null;
			return;
		}
		// If the new destination is not far enough apart from the last one -> do nothing
		if (lastCalculatedDestinations.TryGetValue(agent, out Vector2? lastDestination)) {
			if (lastDestination.HasValue) {
				if (Vector2.Distance(destination.Value, lastDestination.Value) < pathRecalculationThreshold) {
					return;
				}
			}
		}
		Vector2 agentPos = agent.transform.position;
		IList<Vector2> newPath = GetPathTo(agentPos, destination.Value);
		agent.UpdatePath(newPath);
		lastCalculatedDestinations[agent] = destination;
	}
}
