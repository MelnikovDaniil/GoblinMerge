using System.Collections;
using TMPro;
using UnityEngine;

public class CrystalInfo : MonoBehaviour
{
    public bool isBusy;

    [SerializeField]
    private float animationTime;
    [SerializeField]
    private TextMeshProUGUI lable;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void ShowCrystalInfo(int count, Vector2 position)
    {
        StopAllCoroutines();
        isBusy = true;
        gameObject.SetActive(true);
        transform.position = position;
        lable.text = count.ToString();

        _animator.SetFloat("speedMultiplier", 1.0f / animationTime);
        _animator.Play("CrystalInfo_Show", 0, 0);
        StartCoroutine(HideRoutine());
    }

    private IEnumerator HideRoutine()
    {
        yield return new WaitForSeconds(animationTime);
        isBusy = false;
        gameObject.SetActive(false);
    }
}
