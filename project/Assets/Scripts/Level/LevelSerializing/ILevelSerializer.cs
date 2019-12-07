using System.Collections.Generic;

public interface ILevelSerializer {
    void SerializeLevel(string pathToSave, Level level, IList<ILevelElement> levelElements, IList<Item> levelItems, IList<Enemy> enemies);
}
