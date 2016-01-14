using UnityEngine;
using System.Collections;

[System.Serializable]
public class Season {

    public bool available;
    public static int levelCount = 12;
    public Level[] levels;

    public void DefineLevelsArray()
    {
        levels = new Level[levelCount];
    }

    public Season()
    {
        DefineLevelsArray();
        available = false;
        for (int i = 0; i < levelCount; i++)
        {
            levels[i] = new Level();
        }
    }
}
