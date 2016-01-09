using System;
using System.Collections.Generic;

[System.Serializable]
public class Game {

    public static Game current;
    public string name;
    public static int seasonCount = 12;

    public Season[] seasons;

    public void DefineLevelsArray()
    {
        seasons = new Season[seasonCount];
    }

    public Game()
    {
        name = "save_" + DateTime.Now;
        DefineLevelsArray();
        for (int i = 0; i < seasonCount; i++)
        {
            seasons[i] = new Season();
        }
    }
}
