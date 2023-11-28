using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum BonusType
{
    Unknown = 0,
    GeneralBoost,
    UnitTypeBoost,
    UnitSpawn,
}

public class BonusManager : MonoBehaviour
{
    public static BonusManager Instance;

    public Bonus bonusPrefab;
    public GameObject bonusEffect;

    public float bonusSpawnRate = (float)TimeSpan.FromMinutes(5).TotalSeconds;
    public float bonusLiveTime = 15;

    [Space]
    public float generalBonusTime = 30;
    public float generalBonusMultiplier = 2;

    [Space]
    public float unitBonusTime = 30;
    public float unitBonusMultiplier = 7;

    public Dictionary<UnitType, float> unitTypeBonusMultipliers = new Dictionary<UnitType, float>();

    private Dictionary<BonusType, Action> bonuses;

    [Space]
    public int buttonSpawnGoblinLevel = 1;
    private void Awake()
    {
        Instance = this;
        bonuses = new Dictionary<BonusType, Action>();
        bonuses.Add(BonusType.GeneralBoost, ApplyGeneralBonus);
        bonuses.Add(BonusType.UnitTypeBoost, () => ApplyUnitBonus(UnitType.Goblin));
        bonuses.Add(BonusType.UnitSpawn, () => SpawnUnit(UnitType.Goblin));
        
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
        bonusEffect.SetActive(true);
        var unitTypes = new List<UnitType>(unitTypeBonusMultipliers.Keys);
        foreach (var unitType in unitTypes)
        {
            unitTypeBonusMultipliers[unitType] = generalBonusMultiplier;
        }
        StartCoroutine(ResetMultipliersRoutine(generalBonusTime));
    }

    public void ApplyUnitBonus(UnitType unitType)
    {
        bonusEffect.SetActive(true);
        unitTypeBonusMultipliers[unitType] = unitBonusMultiplier;
        StartCoroutine(ResetMultipliersRoutine(unitBonusTime));
    }

    public Bonus GenerateBonus(BonusType bonusType = BonusType.Unknown)
    {
        var bonusAction = bonuses.GetValueOrDefault(bonusType) ?? bonuses.GetRandom().Value;
        var createdBonus = Instantiate(bonusPrefab, UnitShopManager.Instance.GetRandomSpawnPostion(), Quaternion.identity);
        createdBonus.action = bonusAction;
        return createdBonus;
    }

    public void SpawnUnit(UnitType unitType)
    {
        var randomLevel = Random.Range(0, LevelMapper.GetHighestLevel(unitType) + 1);
        SpawnUnit(unitType, randomLevel);
    }

    // TODO: Implement unitType
    public void SpawnUnit(UnitType unitType, int level)
    {
        var position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
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
        bonusEffect.SetActive(false);
        var unitTypes = new List<UnitType>(unitTypeBonusMultipliers.Keys);
        foreach (var unitType in unitTypes)
        {
            unitTypeBonusMultipliers[unitType] = 1f;
        }
    }
}
