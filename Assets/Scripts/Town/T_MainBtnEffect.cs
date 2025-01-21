using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class T_MainBtnEffect : MonoBehaviour
{
    public RectTransform[] buttonRects;
    public float normalSizeRatio = 0.18f;
    public float selectedSizeRatio = 0.4f;
    public float animationDuration = 0.3f;
    public RectTransform parentContainter;

    private void Start()
    {
        foreach(RectTransform buttonRect in buttonRects)
        {
            Button button = buttonRect.GetComponent<Button>();
            button.onClick.AddListener(() => OnButtonClick(buttonRect));
        }
    }

    void OnButtonClick(RectTransform selectedButton)
    {
        float containerWidth = parentContainter.rect.width;

        foreach(RectTransform buttonRect in buttonRects)
        {            
            if(buttonRect == selectedButton)
            {
                float targetWidth = containerWidth * selectedSizeRatio;
                buttonRect.DOSizeDelta(new Vector2(targetWidth, buttonRect.sizeDelta.y),animationDuration).SetEase(Ease.OutQuad);
            }
            else
            {
                float targetWidth = containerWidth * (1 - selectedSizeRatio) / (buttonRects.Length - 1);
                buttonRect.DOSizeDelta(new Vector2(targetWidth, buttonRect.sizeDelta.y), animationDuration).SetEase(Ease.OutQuad);
            }
        }
    }

}
