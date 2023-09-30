using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class UnitMapper
{
    private const string MapperName = "Unit";

    public static void SaveUnits(List<Unit> units)
    {
        var efficiency = 0.0f;
        units = units.Where(x => x.IsAlive).ToList();
        PlayerPrefs.SetInt(MapperName + "NumberOfUnit", units.Count);
        for (int i = 0; i < units.Count; i++)
        {
            PlayerPrefs.SetInt(MapperName + i + "Type", (int)units[i].Type);
            PlayerPrefs.SetInt(MapperName + i + "Level", units[i].level);
            PlayerPrefs.SetFloat(MapperName + i + "PosX", units[i].transform.position.x);
            PlayerPrefs.SetFloat(MapperName + i + "PosY", units[i].transform.position.y);
            efficiency += EfficiencyMapper.CalculateLevelEfficieny(units[i].startEfficiency, units[i].level);
        }
        EfficiencyMapper.SaveEfficiencyInSec(efficiency);
    }

    public static List<UnitInfo> GetUnits()
    {
        var amount = PlayerPrefs.GetInt(MapperName + "NumberOfUnit", 0);
        var units = new List<UnitInfo>();

        for (int i = 0; i < amount; i++)
        {
            units.Add(new UnitInfo
            {
                Type = (UnitType)PlayerPrefs.GetInt(MapperName + i + "Type"),
                Level = PlayerPrefs.GetInt(MapperName + i + "Level"),
                Position = new Vector2(
                    PlayerPrefs.GetFloat(MapperName + i + "PosX"),
                    PlayerPrefs.GetFloat(MapperName + i + "PosY"))
            });
        }
        return units;
    }
}
