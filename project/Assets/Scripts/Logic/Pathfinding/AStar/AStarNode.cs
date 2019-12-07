using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarNode : FibonacciNode<float> {
	public readonly ILevelTile tile;
	public readonly float heuristicValue;
	public float cost;


	public AStarNode predecessor;

	public AStarNode(ILevelTile tile, float heuristicValue) : base(float.MaxValue) {
		this.tile = tile;
		this.heuristicValue = heuristicValue;
		cost = float.MaxValue;
	}

	public int CompareTo(AStarNode other) {
		return key.CompareTo(other.key);
	}
}
