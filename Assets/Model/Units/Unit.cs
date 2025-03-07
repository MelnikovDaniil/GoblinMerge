using System;
using UnityEngine;

[RequireComponent(typeof(DragableObject))]
public abstract class Unit : MonoBehaviour
{
    public abstract UnitType Type { get; }

    public bool IsAlive { get; set; }

    public int level;

    public float bonusCoof = 1;

    public float startEfficiency;

    private float levelEfficiency;

    protected float currentEfficiency;

    protected DragableObject _dragableObject;

    protected void Awake()
    {
        currentEfficiency = startEfficiency;
        _dragableObject = GetComponent<DragableObject>();
    }

    private void FixedUpdate()
    {
        currentEfficiency = levelEfficiency * BonusManager.Instance.unitTypeBonusMultipliers[Type];
    }

    public virtual void SetUp(int level = default)
    {
        IsAlive = true;
        _dragableObject.isDragging = false;
        gameObject.SetActive(true);
        this.level = level;
        RecalculateEfficiency();
    }

    public virtual void Disable()
    {
        IsAlive = false;
        _dragableObject.isDragging = false;
        gameObject.SetActive(false);
    }

    public void RecalculateEfficiency()
    {
        levelEfficiency = EfficiencyMapper.CalculateLevelEfficieny(startEfficiency, level);
    }

    public bool IsMatch(Unit unit)
    {
        if (Type == unit.Type
            && level == unit.level)
        {
            return true;
        }

        return false;
    }

    public void Merge()
    {
        level++;
        LevelMapper.SaveLevelIfHighest(Type, level);
        RecalculateEfficiency();
        Upgrade();
    }

    protected abstract void Upgrade();
}
