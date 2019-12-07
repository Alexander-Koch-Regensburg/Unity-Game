using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILevelController
{
    void SaveLevel(bool selectPath);
    void LoadLevel();
    Level GetLevel();
    void SetLevel(Level lvl);
    void SetLevelDataCollection(LevelDataCollection levelDataCollection);
    LevelDataCollection GetLevelDataCollection();
}
