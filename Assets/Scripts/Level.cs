using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Level
{
    public int LevelID;
    public string Name;
    public int EnemiesNumber;
    public PinStatus Status;

    public PinStatus GetStatus()
    {
        return Status;
    }
}
