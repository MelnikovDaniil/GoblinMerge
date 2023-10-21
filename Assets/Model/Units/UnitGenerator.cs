using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitGenerator : MonoBehaviour
{
    public static UnitGenerator Instance;

    public Unit goblinPrefab;

    private List<Unit> unitPool = new List<Unit>();

    private void Awake()
    {
        Instance = this;
        RestoreAll();
        SoundManager.Sounds.ForEach(x => x.Stop());
    }

    private void Start()
    {
    }

    public IEnumerable<Unit> GetAliveUnits()
    {
        return unitPool.Where(x => x.IsAlive);
    }

    public void RestoreAll()
    {
        var unitInfos = UnitMapper.GetUnits();
        foreach (var unitInfo in unitInfos)
        {
            var unit = Instantiate(goblinPrefab, unitInfo.Position, Quaternion.identity);
            unit.SetUp(unitInfo.Level);
            unitPool.Add(unit);
        }
    }

    public Unit GetNewUnit(Vector2 spawnPosition, int level = 0)
    {
        var unit = unitPool.FirstOrDefault(x => !x.IsAlive);
        if (unit == null)
        {
            unit = Instantiate(goblinPrefab);
            unitPool.Add(unit);
        }
        SoundManager.PlaySound("Spawn");
        unit.transform.position = spawnPosition;
        unit.SetUp(level);
        return unit;
    }

    private void OnApplicationQuit()
    {
        UnitMapper.SaveUnits(unitPool);
    }
}
