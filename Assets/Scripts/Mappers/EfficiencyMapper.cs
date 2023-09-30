using System;
using UnityEngine;

public static class EfficiencyMapper
{
    private const string MapperName = "Efficiency";

    public static void SaveExitDate()
    {
        PlayerPrefs.SetString(MapperName + "ExitData", DateTime.UtcNow.ToString());
    }

    public static DateTime GetExitDate()
    {
        if (!DateTime.TryParse(PlayerPrefs.GetString(MapperName + "ExitData", string.Empty), out var date))
        {
            return DateTime.UtcNow;
        }

        return date;
    }

    public static void SaveEfficiencyInSec(float efficiency)
    {
        PlayerPrefs.SetFloat(MapperName, efficiency);
    }

    public static float GetEfficiencyInSec()
    {
        return PlayerPrefs.GetFloat(MapperName, 0);
    }

    public static float CalculateLevelEfficieny(float startEfficiency, int level)
    {
        return startEfficiency * Mathf.Pow(2.5f, level);
    }
}
