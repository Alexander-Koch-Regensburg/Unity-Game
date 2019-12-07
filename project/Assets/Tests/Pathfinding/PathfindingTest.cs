using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[TestClass]
public class PathfindingTest {

	private TestNavigationGraphProvider graphProvider;

	/// <summary>
	/// A simple graph provider as substitute for the level
	/// </summary>
	private class TestNavigationGraphProvider : INavigationGraphProvider2D {
		private Vector2Int size;
		private ISet<Vector2Int> excludedNavPoints;

		private Dictionary<Vector2Int, ILevelTile> pointsToLevelTile;

		public TestNavigationGraphProvider(Vector2Int size, ISet<Vector2Int> excludedNavPoints) {
			this.size = size;
			this.excludedNavPoints = excludedNavPoints;
			if (this.excludedNavPoints == null) {
				this.excludedNavPoints = new HashSet<Vector2Int>();
			}
			pointsToLevelTile = new Dictionary<Vector2Int, ILevelTile>();
		}

		public ILevelTile GetTileAtPos(Vector2Int pos) {
			ILevelTile ret;
			if (pointsToLevelTile.TryGetValue(pos, out ret) == false) {
				ret = new LevelTile(pos);
				pointsToLevelTile.Add(pos, ret);
			}
			return ret;
		}

		public ISet<ILevelTile> GetConnectedNavPoints(ILevelTile tile) {
			ISet<ILevelTile> ret = new HashSet<ILevelTile>();
			if (tile.Pos.x > 0) {
				Vector2Int left = new Vector2Int(tile.Pos.x - 1, tile.Pos.y);
				if (excludedNavPoints.Contains(left) == false) {
					ret.Add(GetTileAtPos(left));
				}
			}
			if (tile.Pos.x < size.x - 1) {
				Vector2Int right = new Vector2Int(tile.Pos.x + 1, tile.Pos.y);
				if (excludedNavPoints.Contains(right) == false) {
					ret.Add(GetTileAtPos(right));
				}
			}
			if (tile.Pos.y > 0) {
				Vector2Int bottom = new Vector2Int(tile.Pos.x, tile.Pos.y - 1);
				if (excludedNavPoints.Contains(bottom) == false) {
					ret.Add(GetTileAtPos(bottom));
				}
			}
			if (tile.Pos.y < size.y - 1) {
				Vector2Int top = new Vector2Int(tile.Pos.x, tile.Pos.y + 1);
				if (excludedNavPoints.Contains(top) == false) {
					ret.Add(GetTileAtPos(top));
				}
			}

			return ret;
		}
	}

	[SetUp]
	public void SetupTestGraph() {
		// Looks like this (0 = traversable, X != traversable)
		// 0 0 0 0 0 X 0 0
		// 0 0 X X 0 X 0 0
		// 0 0 0 X 0 X X X
		// X X X X 0 0 0 0
		// 0 0 0 0 X 0 0 0
		// 0 0 X 0 0 X 0 0
		// 0 0 X 0 0 0 0 0
		// 0 0 0 0 0 0 0 0
		ISet<Vector2Int> excludedNavPoints = new HashSet<Vector2Int>();
		excludedNavPoints.Add(new Vector2Int(0, 4));
		excludedNavPoints.Add(new Vector2Int(1, 4));
		excludedNavPoints.Add(new Vector2Int(2, 4));
		excludedNavPoints.Add(new Vector2Int(2, 6));
		excludedNavPoints.Add(new Vector2Int(3, 4));
		excludedNavPoints.Add(new Vector2Int(3, 5));
		excludedNavPoints.Add(new Vector2Int(3, 6));

		excludedNavPoints.Add(new Vector2Int(5, 7));
		excludedNavPoints.Add(new Vector2Int(5, 6));
		excludedNavPoints.Add(new Vector2Int(5, 5));
		excludedNavPoints.Add(new Vector2Int(6, 5));
		excludedNavPoints.Add(new Vector2Int(7, 5));

		excludedNavPoints.Add(new Vector2Int(4, 3));
		excludedNavPoints.Add(new Vector2Int(5, 2));

		excludedNavPoints.Add(new Vector2Int(2, 1));
		excludedNavPoints.Add(new Vector2Int(2, 2));
		this.graphProvider = new TestNavigationGraphProvider(new Vector2Int(8, 8), excludedNavPoints);
	}

	[Test]
	public void AStarEuclidian_StartSameAsEnd() {
		IPathFindingAlgorithm algo = new AStarAlgorithm(graphProvider, new EuclidianHeuristic());
		ILevelTile from = graphProvider.GetTileAtPos(new Vector2Int(1, 1));
		ILevelTile to = graphProvider.GetTileAtPos(new Vector2Int(1, 1));
		IList<ILevelTile> result = algo.CalculatePath(from, to);
		NUnit.Framework.Assert.IsTrue(result.Count == 1);
	}

	[Test]
	public void AStarEuclidian_NoPathExistent() {
		IPathFindingAlgorithm algo = new AStarAlgorithm(graphProvider, new EuclidianHeuristic());
		ILevelTile from = graphProvider.GetTileAtPos(new Vector2Int(6, 7));
		ILevelTile to = graphProvider.GetTileAtPos(new Vector2Int(1, 1));
		IList<ILevelTile> result = algo.CalculatePath(from, to);
		NUnit.Framework.Assert.IsNull(result);
	}

	[Test]
	public void AStarEuclidian_CorrectPathFound_OneCorrectPath() {
		List<ILevelTile> expected = new List<ILevelTile>() {
			graphProvider.GetTileAtPos(new Vector2Int(2, 5)),
			graphProvider.GetTileAtPos(new Vector2Int(1, 5)),
			graphProvider.GetTileAtPos(new Vector2Int(1, 6)),
			graphProvider.GetTileAtPos(new Vector2Int(1, 7)),
			graphProvider.GetTileAtPos(new Vector2Int(2, 7)),
			graphProvider.GetTileAtPos(new Vector2Int(3, 7)),
			graphProvider.GetTileAtPos(new Vector2Int(4, 7)),
			graphProvider.GetTileAtPos(new Vector2Int(4, 6)),
			graphProvider.GetTileAtPos(new Vector2Int(4, 5)),
			graphProvider.GetTileAtPos(new Vector2Int(4, 4)),
			graphProvider.GetTileAtPos(new Vector2Int(5, 4)),
			graphProvider.GetTileAtPos(new Vector2Int(6, 4))
		};

		IPathFindingAlgorithm algo = new AStarAlgorithm(graphProvider, new EuclidianHeuristic());
		ILevelTile from = graphProvider.GetTileAtPos(new Vector2Int(2, 5));
		ILevelTile to = graphProvider.GetTileAtPos(new Vector2Int(6, 4));
		IList<ILevelTile> result = algo.CalculatePath(from, to);

		NUnit.Framework.Assert.IsTrue(result.Count == expected.Count);
		for (int i = 0; i < result.Count; ++i) {
			NUnit.Framework.Assert.IsTrue(result[i].Equals(expected[i]));
		}
	}

	[Test]
	public void AStarEuclidian_CorrectPathFound_MultipleCorrectPaths() {

		IPathFindingAlgorithm algo = new AStarAlgorithm(graphProvider, new EuclidianHeuristic());
		ILevelTile from = graphProvider.GetTileAtPos(new Vector2Int(2, 5));
		ILevelTile to = graphProvider.GetTileAtPos(new Vector2Int(0, 1));
		IList<ILevelTile> result = algo.CalculatePath(from, to);

		NUnit.Framework.Assert.IsTrue(result.Count == 23);

		float pathLength = 0f;
		for (int i = 1; i < result.Count; ++i) {
			pathLength += Vector2.Distance(result[i - 1].Pos, result[i].Pos);
		}
		NUnit.Framework.Assert.AreEqual(pathLength, 22f, 0.1f);
	}

}
