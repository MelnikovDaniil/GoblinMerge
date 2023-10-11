using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class UnitShopManager : MonoBehaviour
{
    public static UnitShopManager Instance;
    public PurchaseButton goblinPurchaseButton;
    public TextAsset goblinCostsFile;

    [SerializeField]
    private ParticleSystem spawnParticles;

    [SerializeField]
    private Vector2 spawnSize;
    private int crystalMask;

    private List<int> goblinCosts;

    private void Awake()
    {
        Instance = this;
        crystalMask = LayerMask.GetMask("Crystal");
        var stringCosts = goblinCostsFile.text.Split(Environment.NewLine);
        goblinCosts = stringCosts.Select(x => float.TryParse(x, out float num) ? (int)num : 0).ToList();
    }
    private void Start()
    {
        RecalculateCosts();
        CrystalManager.Instance.OnCristalsAmountChange += UpdateButtons;
        UpdateButtons();
    }

    public void Purchase(float cost)
    {
        CrystalManager.Instance.CristalsAmount -= cost;
        var spawnPosition = GetRandomSpawnPostion();
        spawnParticles.transform.position = spawnPosition;
        spawnParticles.Play();

        UnitGenerator.Instance.GetNewUnit(spawnPosition);
        RecalculateCosts();
    }

    public void UpdateButtons()
    {
        if (goblinPurchaseButton.button.interactable 
            && CrystalManager.Instance.CristalsAmount < goblinPurchaseButton.cost)
        {
            goblinPurchaseButton.button.interactable = false;
        }
        else if (!goblinPurchaseButton.button.interactable 
            && CrystalManager.Instance.CristalsAmount >= goblinPurchaseButton.cost)
        {
            goblinPurchaseButton.button.interactable = true;
        }
    }

    public Vector2 GetRandomSpawnPostion()
    {
        var x = spawnSize.x / 2f;
        var y = spawnSize.y / 2f;
        var spawnPosition = new Vector2(Random.Range(-x, x), Random.Range(-y, y));
        var point = Physics2D.OverlapPoint(spawnPosition, crystalMask);
        if (point != null)
        {
            return GetRandomSpawnPostion();
        }

        return spawnPosition;
    }

    private void RecalculateCosts()
    {
        var units = UnitGenerator.Instance.GetAliveUnits();
        var goblinsCount = units.Sum(x => (int)Math.Pow(2, x.level));
        var buttonCost = goblinCosts[goblinsCount];
        goblinPurchaseButton.cost = buttonCost;
        goblinPurchaseButton.button.onClick.RemoveAllListeners();
        goblinPurchaseButton.button.onClick.AddListener(() => Purchase(buttonCost));
        goblinPurchaseButton.costText.text = goblinPurchaseButton.cost.ToString();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(Vector2.zero, spawnSize);
    }


}
