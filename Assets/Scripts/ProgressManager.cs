using UnityEngine;
using UnityEngine.UI;
using System;

public class ProgressManager : MonoBehaviour {

    public string savefileName;
    public string savefileExtention;
    public int seasonCount;
    public int levelCount;

    void Start()
    {
        SaveLoad.fileName = savefileName;
        SaveLoad.fileExtention = savefileExtention;
        if (!SaveLoad.Load())
        {
            Game.seasonCount = seasonCount;
            Season.levelCount = levelCount;
            Game.current = new Game();
            Game.current.seasons[0].available = true;
            Game.current.seasons[0].levels[0].available = true;
            SaveLoad.Save();
        }

    }

    public void LoadStats(Transform levelsLayout)
    {
        SaveLoad.Load();
        Transform body = levelsLayout.FindChild("Body");
        //int levelCount = body.childCount;
        Transform levelTile;
        int currSeasonIndex = body.parent.GetSiblingIndex() - 3;
        Level currLevel;
        for (int j = 0; j < levelCount; j++)
        {
            levelTile = body.GetChild(j);
            currLevel = Game.current.seasons[currSeasonIndex].levels[j];
            levelTile.FindChild("Deaths").GetComponent<Text>().text = currLevel.deaths + "";
            levelTile.FindChild("Time").GetComponent<Text>().text = string.Format("{0:0}:{1:00}.{2:00}", currLevel.time.Minutes, currLevel.time.Seconds, currLevel.time.Milliseconds);
        }
    }

    public void SaveStats(int currSeasonIndex, int currLevelIndex, float time, int deaths)
    {
        Level currLevel = Game.current.seasons[currSeasonIndex].levels[currLevelIndex];
        if (currLevelIndex + 1 < levelCount)
        {
            Game.current.seasons[currSeasonIndex].levels[currLevelIndex + 1].available = true;
        }
        else if(currSeasonIndex + 1 <seasonCount)
        {
            Game.current.seasons[currSeasonIndex + 1].available = true;
        }
        else { /*конгратулатіонс!*/ }

        currLevel.time = Level.FromIntToTimeSpan(time);
        currLevel.deaths = deaths;
        SaveLoad.Save();
    }

}
