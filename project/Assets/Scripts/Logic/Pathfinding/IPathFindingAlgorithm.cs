using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPathFindingAlgorithm {
	/// <summary>
	/// Calculates a path between 2 <code>ILevelTile</code>s and returns it as an accordingly ordered list of the <code>ILevelTile</code>s to traverse.
	/// When no path can be found <c>null</c> will be returned.
	/// </summary>
	IList<ILevelTile> CalculatePath(ILevelTile from, ILevelTile to);
}
