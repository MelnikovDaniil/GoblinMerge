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

    protected float currentEfficiency;

    protected DragableObject _dragableObject;

    protected void Awake()
    {
        currentEfficiency = startEfficiency;
        _dragableObject = GetComponent<DragableObject>();
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
        currentEfficiency = EfficiencyMapper.CalculateLevelEfficieny(startEfficiency, level) * bonusCoof;
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
        RecalculateEfficiency();
        Upgrade();
    }

    protected abstract void Upgrade();
}
