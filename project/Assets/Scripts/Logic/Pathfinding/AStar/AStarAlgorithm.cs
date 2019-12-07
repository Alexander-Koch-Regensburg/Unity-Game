using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarAlgorithm : IPathFindingAlgorithm {
	private INavigationGraphProvider2D graphProvider;
	private IAStarHeuristic heuristic;

	private ILevelTile target;

	private FibonacciHeap<float> openNodes;
	private ISet<AStarNode> closedNodes;
	private Dictionary<ILevelTile, AStarNode> tilesToNodes;

	public AStarAlgorithm(INavigationGraphProvider2D graphProvider, IAStarHeuristic heuristic) {
		if (graphProvider == null) {
			Debug.LogError("A new instance of an AStarAlgorithm needs to initialized with a graph-provider!");
		}
		this.graphProvider = graphProvider;

		if (heuristic == null) {
			this.heuristic = new EuclidianHeuristic();
		} else {
			this.heuristic = heuristic;
		}
	}

	public IList<ILevelTile> CalculatePath(ILevelTile from, ILevelTile to) {
		target = to;
		openNodes = new FibonacciHeap<float>();
		closedNodes = new HashSet<AStarNode>();
		tilesToNodes = new Dictionary<ILevelTile, AStarNode>();

		AStarNode startNode = new AStarNode(from, heuristic.GetDistance(from.CenterPos, to.CenterPos));
		startNode.cost = 0;
		tilesToNodes.Add(from, startNode);
		openNodes.Insert(startNode);

		do {
			AStarNode currNode = (AStarNode) openNodes.ExtractMin();
			if (currNode.tile.Equals(to)) {
				return GetPathToNode(currNode);
			}

			closedNodes.Add(currNode);

			ExpandNode(currNode);
		} while (openNodes.IsEmpty() == false);

		// No path found
		return null;
	}

	private void ExpandNode(AStarNode node) {
		ISet<ILevelTile> neighbours = graphProvider.GetConnectedNavPoints(node.tile);
		foreach (ILevelTile neighbour in neighbours) {
			// Obtain node for neighbour point
			if (tilesToNodes.TryGetValue(neighbour, out AStarNode neighbourNode) == false) {
				neighbourNode = new AStarNode(neighbour, heuristic.GetDistance(neighbour.CenterPos, target.CenterPos));
				tilesToNodes.Add(neighbour, neighbourNode);
			}

			if (closedNodes.Contains(neighbourNode)) {
				continue;
			}

			float newCost = node.cost + Vector2.Distance(node.tile.CenterPos, neighbour.CenterPos);
			if (openNodes.Contains(neighbourNode)) {
				if (newCost >= neighbourNode.cost) {
					continue;
				}
			}

			neighbourNode.predecessor = node;
			neighbourNode.cost = newCost;
			float fVal = neighbourNode.cost + neighbourNode.heuristicValue;
			if (openNodes.Contains(neighbourNode)) {
				openNodes.DecreaseKey(neighbourNode, fVal);
			} else {
				neighbourNode.key = fVal;
				openNodes.Insert(neighbourNode);
			}
		}
	}

	private IList<ILevelTile> GetPathToNode(AStarNode node) {
		List<ILevelTile> ret = new List<ILevelTile>();
		while (node != null) {
			ret.Insert(0, node.tile);
			node = node.predecessor;
		}
		return ret;
	}
}
