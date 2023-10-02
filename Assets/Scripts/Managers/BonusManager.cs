using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BonusManager : MonoBehaviour
{
    public static BonusManager Instance;

    public Bonus bonusPrefab;
    public float bonusSpawnRate = (float)TimeSpan.FromMinutes(5).TotalSeconds;
    public float bonusLiveTime = 15;

    [Space]
    public float generalBonusTime = 30;
    public float generalBonusMultiplier = 2;

    [Space]
    public float unitBonusTime = 30;
    public float unitBonusMultiplier = 7;

    public Dictionary<UnitType, float> unitTypeBonusMultipliers = new Dictionary<UnitType, float>();

    private List<Action> bonuses;
    private void Awake()
    {
        Instance = this;
        bonuses = new List<Action>
        {
            ApplyGeneralBonus,
            () => ApplyUnitBonus(UnitType.Goblin),
            () => SpawnUnit(UnitType.Goblin),
        };
    }

    private void Start()
    {
        var unitTypes = Enum.GetNames(typeof(UnitType));
        foreach (var stringType in unitTypes)
        {
            unitTypeBonusMultipliers.Add(Enum.Parse<UnitType>(stringType), 1f);
        }

        StartCoroutine(BonusGeneratorRoutine());
    }

    public void ApplyGeneralBonus()
    {
        var unitTypes = new List<UnitType>(unitTypeBonusMultipliers.Keys);
        foreach (var unitType in unitTypes)
        {
            unitTypeBonusMultipliers[unitType] = generalBonusMultiplier;
        }
        StartCoroutine(ResetMultipliersRoutine(generalBonusTime));
    }

    public void ApplyUnitBonus(UnitType unitType)
    {
        unitTypeBonusMultipliers[unitType] = unitBonusMultiplier;
        StartCoroutine(ResetMultipliersRoutine(unitBonusTime));
    }

    public Bonus GenerateBonus()
    {
        var bonusAction = bonuses.GetRandom();
        var createdBonus = Instantiate(bonusPrefab, UnitShopManager.Instance.GetRandomSpawnPostion(), Quaternion.identity);
        createdBonus.action = bonusAction;
        return createdBonus;
    }

    private void SpawnUnit(UnitType unitType)
    {
        var position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var level = Random.Range(0, LevelMapper.GetHighestLevel(unitType) + 1);
        UnitGenerator.Instance.GetNewUnit(position, level);
    }

    private IEnumerator BonusGeneratorRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(bonusSpawnRate);
            var bonus = GenerateBonus();
            yield return new WaitForSeconds(bonusLiveTime);
            bonus.animator.SetTrigger("close");
            Destroy(bonus.gameObject, 0.3f);
        }
    }

    private IEnumerator ResetMultipliersRoutine(float bonusTime)
    {
        yield return new WaitForSeconds(bonusTime);
        var unitTypes = new List<UnitType>(unitTypeBonusMultipliers.Keys);
        foreach (var unitType in unitTypes)
        {
            unitTypeBonusMultipliers[unitType] = 1f;
        }
    }
}
