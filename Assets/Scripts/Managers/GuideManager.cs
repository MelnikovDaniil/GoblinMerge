using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using static GuideMapper;

public class GuideManager : MonoBehaviour
{
    public int firstGoblinCost;

    public GameObject clickingHand;
    public GameObject buyerHand;
    public MergeHand mergeHand;
    public GameObject mergePanel;

    private bool isClickingGuideFinished;
    private bool isBuyerGuideFinished;
    private bool isMergeGuideFinished;
    
    private void Start()
    {
        isClickingGuideFinished = GuideMapper.GetGuideStep(GuideStep.Click);
        isBuyerGuideFinished = GuideMapper.GetGuideStep(GuideStep.Buyer);
        isMergeGuideFinished = GuideMapper.GetGuideStep(GuideStep.Merge);
        clickingHand.SetActive(false);
        buyerHand.SetActive(false);
        mergeHand.gameObject.SetActive(false);


        if (!isBuyerGuideFinished)
        {
            UnitShopManager.Instance.goblinPurchaseButton.gameObject.SetActive(false);
            CrystalManager.Instance.OnCristalsAmountChange += CheckGoblinCost;
        }

        if (!isClickingGuideFinished)
        {
            StartCoroutine(ClickingRoutine());
        }

        if (!isMergeGuideFinished)
        {
            UnitGenerator.Instance.OnUnitCreation += CheckForMerge;
            CheckForMerge();
            ClickAndDragWithRigidbody.Instance.OnMerge += MergeGuideComplete;
        }
    }

    private void CheckGoblinCost(float amountOfCrystalls)
    {
        if (!isClickingGuideFinished && amountOfCrystalls > 0)
        {
            isClickingGuideFinished = true;
            clickingHand.SetActive(false);
            GuideMapper.FinishGuideStep(GuideStep.Click);
        }

        if (!buyerHand.activeSelf && amountOfCrystalls >= firstGoblinCost)
        {
            CrystalManager.Instance.OnCristalsAmountChange -= CheckGoblinCost;
            UnitShopManager.Instance.goblinPurchaseButton.gameObject.SetActive(true);
            buyerHand.SetActive(true);


            UnitShopManager.Instance.goblinPurchaseButton.button.onClick.AddListener(DisableByerHand);
        }
    }

    private void DisableByerHand()
    {
        buyerHand.SetActive(false);
        GuideMapper.FinishGuideStep(GuideStep.Buyer);
        UnitShopManager.Instance.goblinPurchaseButton.button.onClick.RemoveListener(DisableByerHand);
    }

    private void MergeGuideComplete()
    {
        ClickAndDragWithRigidbody.Instance.OnMerge -= MergeGuideComplete;
        GuideMapper.FinishGuideStep(GuideStep.Merge);
        mergeHand.gameObject.SetActive(false);
    }

    private void CheckForMerge()
    {
        var units = UnitGenerator.Instance.GetAliveUnits();
        if (units.Count() >= 2)
        {
            mergePanel.SetActive(true);
            UnitGenerator.Instance.OnUnitCreation -= CheckForMerge;
            mergeHand.gameObject.SetActive(true);
            mergeHand.SetMovement(units.First(), units.Last());
        }
    }

    private IEnumerator ClickingRoutine()
    {
        yield return new WaitForSeconds(10f);
        if (!isClickingGuideFinished)
        {
            clickingHand.SetActive(true);
        }
    }
}
