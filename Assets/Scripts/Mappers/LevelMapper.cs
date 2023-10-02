using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LevelMapper
{
    private const string MapperName = "Level";

    private static Dictionary<UnitType, int> bufferedLevels = new Dictionary<UnitType, int>(); 

    public static void SaveLevelIfHighest(UnitType unitType, int level)
    {
        if (GetHighestLevel(unitType) < level)
        {
            bufferedLevels[unitType] = level;
            PlayerPrefs.SetInt(MapperName + unitType.ToString(), level);
        }
    }

    public static int GetHighestLevel(UnitType unitType)
    {
        if (!bufferedLevels.TryGetValue(unitType, out var level))
        {
            level = PlayerPrefs.GetInt(MapperName + unitType.ToString(), 0);
            bufferedLevels.Add(unitType, level);
        }

        return level;
    }
}
