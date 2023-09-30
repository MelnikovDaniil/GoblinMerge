using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UnitShopManager : MonoBehaviour
{
    public Unit goblinPrefab;
    public PurchaseButton goblinPurchaseButton;

    [SerializeField]
    private ParticleSystem spawnParticles;

    [SerializeField]
    private Vector2 spawnSize;
    private int crystalMask;

    private List<Unit> unitPool = new List<Unit>();

    private void Awake()
    {
        crystalMask = LayerMask.GetMask("Crystal");
        goblinPurchaseButton.button.onClick.AddListener(Purchase);
    }
    private void Start()
    {
        CrystalManager.Instance.OnCristalsAmountChange += UpdateButtons;
        UpdateButtons();
    }

    public void Purchase()
    {
        CrystalManager.Instance.CristalsAmount -= 5;
        var spawnPosition = GetRandomSpawnPostion();
        spawnParticles.transform.position = spawnPosition;
        spawnParticles.Play();
        var unit = unitPool.FirstOrDefault(x => !x.IsAlive);
        if (unit == null)
        {
            unit = Instantiate(goblinPrefab);
            unitPool.Add(unit);
        }

        unit.transform.position = spawnPosition;
        unit.SetUp();
    }

    public void UpdateButtons()
    {
        goblinPurchaseButton.costText.text = 5.ToString();
        if (goblinPurchaseButton.button.interactable && CrystalManager.Instance.CristalsAmount < 5)
        {
            goblinPurchaseButton.button.interactable = false;
        }
        else if (!goblinPurchaseButton.button.interactable && CrystalManager.Instance.CristalsAmount >= 5)
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(Vector2.zero, spawnSize);
    }


}
