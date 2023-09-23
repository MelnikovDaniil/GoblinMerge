using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class CrystalManager : MonoBehaviour
{
    public static CrystalManager Instance;

    [SerializeField]
    private RectTransform displayCanvas;
    [SerializeField]
    private TextMeshProUGUI crystalsCountText;
    [SerializeField]
    private CrystalInfo crystalInfoPrefab;

    [SerializeField]
    private int poolLimit = 50;
    private List<CrystalInfo> crystalInfoPool = new List<CrystalInfo>();

    private float cristalsCount;
    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            CrystallClick();
        }
    }

    private void CrystallClick()
    {
        var crystalInfo = crystalInfoPool.FirstOrDefault(x => !x.isBusy);
        if (crystalInfo == null)
        {
            if (crystalInfoPool.Count < poolLimit)
            {
                crystalInfo = Instantiate(crystalInfoPrefab, displayCanvas);
                crystalInfoPool.Add(crystalInfo);
            }
            else
            {
                crystalInfo = crystalInfoPool.First();
            }
        }

        var crystalsCount = 50;
        AddCrystals(crystalsCount);
        crystalInfo.ShowCrystalInfo(crystalsCount, Input.mousePosition);
    }

    private void AddCrystals(float count)
    {
        cristalsCount += count;
        crystalsCountText.text = cristalsCount.ToString("n0");
    }
}
