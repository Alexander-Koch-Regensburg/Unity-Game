using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INavigationGraphProvider2D {

	/// <summary>
	/// Returns all points reachable from the given point
	/// </summary>
	ISet<ILevelTile> GetConnectedNavPoints(ILevelTile tile);
}
