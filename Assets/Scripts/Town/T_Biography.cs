using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class T_Biography : MonoBehaviour
{
    public GameObject biographyPanel;    
    public RectTransform biographyPanelRect;
    public RectTransform buttonRect;

    public RectTransform contentRect;

    private bool isBiographyOpen = false;

    public void OnBiographyClick()
    {
        isBiographyOpen = !isBiographyOpen;
        biographyPanel.SetActive(isBiographyOpen);

        if(isBiographyOpen)
        {
            BiographyPanelPos();
            AdjustBiographyPanelHeight();
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(contentRect);        
    }

    void BiographyPanelPos()
    {
        biographyPanelRect.anchoredPosition = new Vector2(0, -buttonRect.sizeDelta.y);
    }

    void AdjustBiographyPanelHeight()
    {
        float textHeight = biographyPanelRect.GetComponentInChildren<TMPro.TextMeshProUGUI>().preferredHeight;

        biographyPanelRect.sizeDelta = new Vector2(biographyPanelRect.sizeDelta.x, textHeight + 20);
    }
}
