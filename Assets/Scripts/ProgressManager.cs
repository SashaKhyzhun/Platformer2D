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

    public void LoadLevelStats(Transform layout)
    {
        SaveLoad.Load();
        Transform body = layout.FindChild("Body");
        Transform tile;
        Transform stats;
        int childCount = body.childCount;
        int currSeasonIndex = body.parent.GetSiblingIndex() - 3;
        bool isLevel = childCount == levelCount;
        bool isSeason = childCount == seasonCount;
        Level currLevel;
        Season currSeason;
        for (int j = 0; j < childCount; j++)
        {
            tile = body.GetChild(j);
            if (isLevel)
            {
                stats = tile.FindChild("Stats");
                currLevel = Game.current.seasons[currSeasonIndex].levels[j];

                tile.FindChild("Locked").gameObject.SetActive(!currLevel.available);
                stats.gameObject.SetActive(currLevel.available);

                //if (currLevel.available) { tile.FindChild("Locked").gameObject.SetActive(false); stats.gameObject.SetActive(true); }
                //else { tile.FindChild("Locked").gameObject.SetActive(true); stats.gameObject.SetActive(false); }    
                        
                if (currLevel.deaths < 0) { stats.FindChild("Deaths").GetComponent<Text>().text = "-"; }
                else { stats.FindChild("Deaths").GetComponent<Text>().text = currLevel.deaths + ""; }
                if (currLevel.time.TotalSeconds == 0) { stats.FindChild("Time").GetComponent<Text>().text = "--:--.--"; }
                else { stats.FindChild("Time").GetComponent<Text>().text = string.Format("{0:0}:{1:00}.{2:00}", currLevel.time.Minutes, currLevel.time.Seconds, currLevel.time.Milliseconds/10); }// + currLevel.time.Milliseconds.ToString("N2"); }
                tile.FindChild("LevelName").GetComponent<Text>().text = "LEVEL " + (j + 1).ToString();
            }
            else if(isSeason)
            {
                currSeason = Game.current.seasons[j];
                tile.FindChild("Locked").gameObject.SetActive(!currSeason.available);
                //if (currSeason.available) {  }
                //else { tile.FindChild("Locked").gameObject.SetActive(!currSeason.available); }
            }
        }
    }

    public void SaveStats(int currSeasonIndex, int currLevelIndex, float time, int deaths)
    {
        Level currLevel = Game.current.seasons[currSeasonIndex].levels[currLevelIndex];
        if (currLevelIndex + 1 < levelCount)
        {
            Game.current.seasons[currSeasonIndex].levels[currLevelIndex + 1].available = true;
        }
        else if(currSeasonIndex + 1 < seasonCount)
        {
            Game.current.seasons[currSeasonIndex + 1].available = true;
            Game.current.seasons[currSeasonIndex + 1].levels[0].available = true;
        }
        else { /*конгратулатіонс!*/ }
        if (currLevel.time.TotalSeconds == 0 || time < currLevel.time.TotalSeconds) { currLevel.time = Level.FromIntToTimeSpan(time); }
        if (currLevel.deaths < 0 || deaths < currLevel.deaths) { currLevel.deaths = deaths; }
        SaveLoad.Save();
    }

}
