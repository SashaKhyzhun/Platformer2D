using System;
using UnityEngine;

[System.Serializable]
public class Level
{
    public TimeSpan time;
    public int deaths;
    public bool available;

    public Level()
    {
        time = FromIntToTimeSpan(0);
        deaths = 0;
        available = false;
    }

    public static TimeSpan FromIntToTimeSpan(float seconds)
    {
        return new TimeSpan(0, 0, 0, 0, (int)(seconds * 1000));
    }
}
