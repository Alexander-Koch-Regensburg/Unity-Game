using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[TestClass]
public class PathCacheTest : MonoBehaviour {

	private IList<ILevelTile> tiles;
	private IList<IList<ILevelTile>> testPaths;

	[SetUp]
	public void SetupTestPaths() {
		// Create level tiles
		ILevelTile t0 = new LevelTile(new Vector2Int(0, 0));
		ILevelTile t1 = new LevelTile(new Vector2Int(0, 1));
		ILevelTile t2 = new LevelTile(new Vector2Int(0, 2));
		ILevelTile t3 = new LevelTile(new Vector2Int(1, 1));
		ILevelTile t4 = new LevelTile(new Vector2Int(1, 2));
		tiles = new List<ILevelTile>() {
			t0,
			t1,
			t2,
			t3,
			t4
		};

		// Paths
		IList<ILevelTile> path0 = new List<ILevelTile>() {
			t0
		};
		IList<ILevelTile> path1 = new List<ILevelTile>() {
			t0,
			t1,
			t2
		};
		IList<ILevelTile> path2 = new List<ILevelTile>() {
			t1,
			t3,
			t4
		};
		IList<ILevelTile> path3 = new List<ILevelTile>() {
			t1,
			t2,
			t1
		};
		IList<ILevelTile> path4 = new List<ILevelTile>();
		testPaths = new List<IList<ILevelTile>>() {
			path0,
			path1,
			path2,
			path3,
			path4
		};
	}

	[Test]
	public void CachePath_NullInput_NoException() {
		PathCache cache = new PathCache();
		cache.CachePath(null);
	}

	[Test]
	public void CachePath_EmptyPath_NotCached() {
		PathCache cache = new PathCache();
		cache.CachePath(testPaths[4]);

		NUnit.Framework.Assert.IsFalse(cache.TryGetPath(tiles[0], tiles[0], out IList<ILevelTile> path));
		NUnit.Framework.Assert.IsNull(path);
	}

	[Test]
	public void CachePath_CachesPath() {
		PathCache cache = new PathCache();
		cache.CachePath(testPaths[0]);
		cache.CachePath(testPaths[1]);

		IList<ILevelTile> path0;
		IList<ILevelTile> path1;
		NUnit.Framework.Assert.IsTrue(cache.TryGetPath(tiles[0], tiles[0], out path0));
		NUnit.Framework.Assert.IsTrue(cache.TryGetPath(tiles[0], tiles[2], out path1));
		NUnit.Framework.Assert.IsTrue(path0.SequenceEqual(testPaths[0]));
		NUnit.Framework.Assert.IsTrue(path1.SequenceEqual(testPaths[1]));
	}

	[Test]
	public void CachePath_DoesNotCacheAboveLimit() {
		PathCache cache = new PathCache(2);
		cache.CachePath(testPaths[0]);
		cache.CachePath(testPaths[1]);
		cache.CachePath(testPaths[2]);
		cache.CachePath(testPaths[3]);

		NUnit.Framework.Assert.IsFalse(cache.TryGetPath(tiles[0], tiles[0], out IList<ILevelTile> path0));
		NUnit.Framework.Assert.IsNull(path0);
		NUnit.Framework.Assert.IsFalse(cache.TryGetPath(tiles[0], tiles[2], out IList<ILevelTile> path1));
		NUnit.Framework.Assert.IsNull(path1);
		NUnit.Framework.Assert.IsTrue(cache.TryGetPath(tiles[1], tiles[4], out IList<ILevelTile> path2));
		NUnit.Framework.Assert.IsTrue(path2.SequenceEqual(testPaths[2]));
		NUnit.Framework.Assert.IsTrue(cache.TryGetPath(tiles[1], tiles[1], out IList<ILevelTile> path3));
		NUnit.Framework.Assert.IsTrue(path3.SequenceEqual(testPaths[3]));
	}

	[Test]
	public void InvalidatePath_All_ClearsCache() {
		PathCache cache = new PathCache();
		cache.CachePath(testPaths[0]);
		cache.CachePath(testPaths[1]);
		cache.CachePath(testPaths[2]);
		cache.CachePath(testPaths[3]);

		cache.InvalidateCache();

		NUnit.Framework.Assert.IsFalse(cache.TryGetPath(tiles[0], tiles[0], out IList<ILevelTile> path1));
		NUnit.Framework.Assert.IsNull(path1);
		NUnit.Framework.Assert.IsFalse(cache.TryGetPath(tiles[0], tiles[2], out IList<ILevelTile> path2));
		NUnit.Framework.Assert.IsNull(path2);
		NUnit.Framework.Assert.IsFalse(cache.TryGetPath(tiles[1], tiles[4], out IList<ILevelTile> path3));
		NUnit.Framework.Assert.IsNull(path3);
		NUnit.Framework.Assert.IsFalse(cache.TryGetPath(tiles[1], tiles[1], out IList<ILevelTile> path4));
		NUnit.Framework.Assert.IsNull(path4);
	}

	[Test]
	public void InvalidatePath_From_ClearsFrom() {
		PathCache cache = new PathCache();
		cache.CachePath(testPaths[0]);
		cache.CachePath(testPaths[1]);
		cache.CachePath(testPaths[2]);
		cache.CachePath(testPaths[3]);

		cache.InvalidateCache(tiles[0]);

		NUnit.Framework.Assert.IsFalse(cache.TryGetPath(tiles[0], tiles[0], out IList<ILevelTile> path0));
		NUnit.Framework.Assert.IsNull(path0);
		NUnit.Framework.Assert.IsFalse(cache.TryGetPath(tiles[0], tiles[2], out IList<ILevelTile> path1));
		NUnit.Framework.Assert.IsNull(path1);
		NUnit.Framework.Assert.IsTrue(cache.TryGetPath(tiles[1], tiles[4], out IList<ILevelTile> path2));
		NUnit.Framework.Assert.IsTrue(path2.SequenceEqual(testPaths[2]));
		NUnit.Framework.Assert.IsTrue(cache.TryGetPath(tiles[1], tiles[1], out IList<ILevelTile> path3));
		NUnit.Framework.Assert.IsTrue(path3.SequenceEqual(testPaths[3]));
	}

	[Test]
	public void InvalidatePath_FromTo_ClearsRightPaths() {
		PathCache cache = new PathCache();
		cache.CachePath(testPaths[0]);
		cache.CachePath(testPaths[1]);
		cache.CachePath(testPaths[2]);
		cache.CachePath(testPaths[3]);

		cache.InvalidateCache(tiles[0], tiles[0]);
		cache.InvalidateCache(tiles[1], tiles[1]);

		NUnit.Framework.Assert.IsFalse(cache.TryGetPath(tiles[0], tiles[0], out IList<ILevelTile> path0));
		NUnit.Framework.Assert.IsNull(path0);
		NUnit.Framework.Assert.IsTrue(cache.TryGetPath(tiles[0], tiles[2], out IList<ILevelTile> path1));
		NUnit.Framework.Assert.IsTrue(path1.SequenceEqual(testPaths[1]));
		NUnit.Framework.Assert.IsTrue(cache.TryGetPath(tiles[1], tiles[4], out IList<ILevelTile> path2));
		NUnit.Framework.Assert.IsTrue(path2.SequenceEqual(testPaths[2]));
		NUnit.Framework.Assert.IsFalse(cache.TryGetPath(tiles[1], tiles[1], out IList<ILevelTile> path3));
		NUnit.Framework.Assert.IsNull(path3);
	}

	[Test]
	public void TryGetPath_Null_OutputNull() {
		PathCache cache = new PathCache();
		cache.CachePath(testPaths[0]);

		NUnit.Framework.Assert.IsFalse(cache.TryGetPath(tiles[0], null, out IList<ILevelTile> path0));
		NUnit.Framework.Assert.IsNull(path0);
		NUnit.Framework.Assert.IsFalse(cache.TryGetPath(null, tiles[0], out IList<ILevelTile> path1));
		NUnit.Framework.Assert.IsNull(path1);
	}
}
