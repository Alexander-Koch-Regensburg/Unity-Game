using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Locks the global roation of this transform to (0, 0, 0). Can be used to prevent a child object from rotating with its parent.
/// </summary>
public class LockRotation : MonoBehaviour {

    private void Update() {
		transform.transform.eulerAngles = Vector3.zero;
    }
}
