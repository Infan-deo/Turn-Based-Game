using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class CombatTextUI : MonoBehaviour, IPoolable
{
    [SerializeField] private TextMeshProUGUI floatingTextUI;

    public void Show(int floatingNum, bool isheal = false, Action onCompleted = null)
    {
        string plusOrMinus = isheal ? "+" : "-";
        floatingTextUI.text = $"{plusOrMinus}{floatingNum}";
        floatingTextUI.color = isheal ? Color.green : Color.red;

        transform.localScale = Vector3.one * 0.5f;

        Sequence seq = DOTween.Sequence();

        seq.Append(transform.DOScale(1f, 1f));

        seq.Join(transform.DOMoveY(transform.position.y + 1.5f, 4f));

        seq.Join(
            floatingTextUI.DOFade(0f, 3f)
        );

        seq.OnComplete(() =>
        {
            gameObject.SetActive(false);
            onCompleted?.Invoke();
        });
    }

    private void OnEnable()
    {
        floatingTextUI.alpha = 1f;
    }

    public void OnSpawn()
    {
        transform.DOKill();
    }

    public void OnDespawn()
    {
        transform.DOKill();
    }
}