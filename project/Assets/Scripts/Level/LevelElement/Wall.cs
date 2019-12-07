using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Wall : TileMapLevelElement {

    public override bool IsSolid() {
        return true;
    }
}