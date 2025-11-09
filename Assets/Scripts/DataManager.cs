using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class DataManager
{
    private static Dictionary<int, PinStatus> _levelStatuses = new ();
    private static int _enemiesNumber; // number of enemies of the current level
    private static bool _isInitialized;

    public static int GetNumberOfLevels()
    {
        return _levelStatuses.Count;
    }

    public static void Initialize(List<Level> levels)
    {
        if (_isInitialized) return;

        _isInitialized = true;
        _levelStatuses = levels.ToDictionary(x => x.LevelID, x => x.GetStatus());
    }

    public static int GetEnemiesNumber() 
    { 
        return _enemiesNumber;
    }

    public static void SetEnemiesNumber(int num)
    {
        _enemiesNumber = num;
    }

    public static PinStatus GetStatus(int levelId)
    {
        if(_levelStatuses.TryGetValue(levelId, out var status))
        {
            return status;
        }
        return PinStatus.Available;
    }

    public static void SetStatus(int levelId, PinStatus status)
    {
        _levelStatuses[levelId] = status;
    }
}
