using System;
using UnityEngine;

[RequireComponent(typeof(DragableObject))]
public abstract class Unit : MonoBehaviour
{
    public Type Type { get => GetType(); }

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

    public void RecalculateEfficiency()
    {
        currentEfficiency = startEfficiency * Mathf.Pow(2.5f, level) * bonusCoof;
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
