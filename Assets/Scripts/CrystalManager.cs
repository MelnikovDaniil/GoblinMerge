using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class CrystalManager : MonoBehaviour
{
    public static CrystalManager Instance;

    [SerializeField]
    private ParticleSystem crystalParticles;
    [SerializeField]
    private Canvas displayCanvas;
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

    public void CrystallClick()
    {
        var crystalsCount = 1;

        var mouseWorldPoint = Input.mousePosition;
        mouseWorldPoint.z = 10.0f;
        mouseWorldPoint = Camera.main.ScreenToWorldPoint(mouseWorldPoint);

        HitCrystal(crystalsCount, mouseWorldPoint);
    }

    public void HitCrystal(float crystalAmount, Vector2 postion)
    {
        var crystalInfo = crystalInfoPool.FirstOrDefault(x => !x.isBusy);
        if (crystalInfo == null)
        {
            if (crystalInfoPool.Count < poolLimit)
            {
                crystalInfo = Instantiate(crystalInfoPrefab, displayCanvas.transform);
                crystalInfo.canvas = displayCanvas;
                crystalInfoPool.Add(crystalInfo);
            }
            else
            {
                crystalInfo = crystalInfoPool.First();
            }
        }


        crystalParticles.transform.position = postion;
        crystalParticles.Play();
        AddCrystals(crystalAmount);
        crystalInfo.ShowCrystalInfo(crystalAmount, postion);
    }

    private void AddCrystals(float count)
    {
        cristalsCount += count;
        crystalsCountText.text = cristalsCount.ToString("n0");
    }
}
