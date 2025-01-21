using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class DamageText : MonoBehaviour
{
    public TextMeshPro damageText;
    public float displayDuration = 0.5f;

    private Color initialColor;
    private Vector3 initialScale;

    private void Awake()
    {
        initialColor = damageText.color;
        initialScale = transform.localScale;
    }

    public void DisplayDamage(int damageAmount, bool isCritical)
    {
        damageText.text = isCritical ? $"<color=yellow>{damageAmount}</color>" : damageAmount.ToString();

        damageText.color = initialColor = new Color(initialColor.r, initialColor.g, initialColor.b, 0);
        transform.localScale = initialScale * 0.5f;

        Sequence sequence = DOTween.Sequence();

        sequence.Append(transform.DOScale(initialScale, 0.3f));
        sequence.Join(damageText.DOFade(1, 0.3f));

        sequence.AppendInterval(displayDuration);
        sequence.Append(damageText.DOFade(0, 0.5f));

        sequence.OnComplete(() => gameObject.SetActive(false));
    }
}
