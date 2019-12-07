using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface State
{
    void OnMouseButton(Vector3 mousePosition, StateInformation stateInformation);
}
