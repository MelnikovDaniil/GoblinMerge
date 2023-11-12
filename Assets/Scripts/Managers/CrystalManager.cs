using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class CrystalManager : MonoBehaviour
{
    public static CrystalManager Instance;
    public event Action<float> OnCristalsAmountChange;

    public float CristalsAmount 
    {
        get { return currentCristalsAmount; }
        set 
        {
            currentCristalsAmount = value;
            OnCristalsAmountChange?.Invoke(currentCristalsAmount);
            CrystalMapper.ChangeAmount(currentCristalsAmount);
            crystalsCountText.text = currentCristalsAmount.ToString("n0");
        }
    }

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
    private List<ParticleSystem> crystalParticlesPool = new List<ParticleSystem>();

    private float currentCristalsAmount;
    
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        var crystals = CrystalMapper.GetAmount();
        var exitDate = EfficiencyMapper.GetExitDate();
        var timeDifference = DateTime.UtcNow - exitDate;
        var collectedCrystalsOnBg = EfficiencyMapper.GetEfficiencyInSec() * (float)timeDifference.TotalSeconds;
        CristalsAmount = crystals + collectedCrystalsOnBg;
    }

    public void CrystallClick()
    {
        var crystalsCount = 1;

        var mouseWorldPoint = Input.mousePosition;
        mouseWorldPoint.z = 10.0f;
        mouseWorldPoint = Camera.main.ScreenToWorldPoint(mouseWorldPoint);

        HitCrystal(crystalsCount, mouseWorldPoint + Vector3.up, mouseWorldPoint);
    }

    public void HitCrystal(float crystalAmount, Vector2 postion, Vector2 hitPosition)
    {
        var crystalInfo = crystalInfoPool.FirstOrDefault(x => !x.isBusy);
        var particles = crystalParticlesPool.FirstOrDefault(x => !x.isPlaying);
        if (crystalInfo == null)
        {
            if (crystalInfoPool.Count < poolLimit)
            {
                crystalInfo = Instantiate(crystalInfoPrefab, displayCanvas.transform);
                crystalInfo.canvas = displayCanvas;
                crystalInfoPool.Add(crystalInfo);

                particles = Instantiate(crystalParticles);
                crystalParticlesPool.Add(particles);
            }
            else
            {
                crystalInfo = crystalInfoPool.First();
                particles = crystalParticlesPool.First();
            }
        }


        particles.transform.position = hitPosition;
        particles.Play();
        CristalsAmount += crystalAmount;
        crystalInfo.ShowCrystalInfo(crystalAmount, postion);
    }

    private void OnApplicationQuit()
    {
        EfficiencyMapper.SaveExitDate();
    }
}
