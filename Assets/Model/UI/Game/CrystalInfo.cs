using System.Collections;
using TMPro;
using UnityEngine;

public class CrystalInfo : MonoBehaviour
{
    public bool isBusy;
    public Canvas canvas;

    [SerializeField]
    private float animationTime;
    [SerializeField]
    private TextMeshProUGUI lable;

    private Camera _camera;
    private Animator _animator;
    private RectTransform _rectTransform;
    private Vector3 _screenCenter;

    private void Awake()
    {
        _camera = Camera.main;
        _animator = GetComponent<Animator>();
        _rectTransform = GetComponent<RectTransform>();
       _screenCenter = new Vector2(Screen.width, Screen.height) / 2f;
    }

    public void ShowCrystalInfo(float count, Vector3 worldPosition)
    {
        StopAllCoroutines();
        isBusy = true;
        gameObject.SetActive(true);
        var newPos = _camera.WorldToScreenPoint(worldPosition);
        newPos -= _screenCenter;

        _rectTransform.anchoredPosition = newPos;
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
