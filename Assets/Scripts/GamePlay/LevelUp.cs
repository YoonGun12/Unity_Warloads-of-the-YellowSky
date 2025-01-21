using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LevelUp : MonoBehaviour
{
    RectTransform rect;
    Item[] items;
    public SkillLevelUI skillLevelUi;
    public Text commonDescriptionText; // 공용 설명 텍스트
    public Button confirmButton; // 확인 버튼
    private Item selectedItem; // 현재 선택된 아이템을 저장할 변수

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        items = GetComponentsInChildren<Item>(true);
        skillLevelUi = GetComponent<SkillLevelUI>();
        confirmButton.interactable = false; // 처음에 확인 버튼 비활성화
        confirmButton.onClick.AddListener(ApplySelectedItem); // 확인 버튼 이벤트 설정
    }

    public void Show()
    {
        Next(); // 다음 아이템 설정
        rect.localScale = Vector3.one; // 레벨업 UI 표시
        GameManager.instance.Stop(); // 게임 일시정지
    }

    public void Hide()
    {
        rect.localScale = Vector3.zero; // 레벨업 UI 숨김
        GameManager.instance.Resume(); // 게임 재개
    }

    public void Select(int index)
    {
        selectedItem = items[index]; // 선택한 아이템 저장
        selectedItem.UpdateDescriptionText(commonDescriptionText); // 공용 설명 텍스트 업데이트
        confirmButton.interactable = true; // 확인 버튼 활성화
    }

    private void ApplySelectedItem()
    {
        if (selectedItem != null)
        {
            selectedItem.OnClick(); // 선택한 아이템 적용
            if (skillLevelUi == null)
            {
                skillLevelUi = GameObject.Find("HUD").GetComponentInChildren<SkillLevelUI>();
            }

            if (skillLevelUi != null)
            {
                skillLevelUi.AddSkill(selectedItem);
            }

            selectedItem = null; // 선택 초기화
            confirmButton.interactable = false; // 확인 버튼 비활성화
            Hide(); // 레벨업 UI 숨김
        }
    }

    void Next()
    {
        // 모든 아이템 비활성화
        foreach (Item item in items)
        {
            item.gameObject.SetActive(false);
        }

        // 랜덤하게 3개 아이템 활성화
        int[] ran = new int[3];
        while (true)
        {
            ran[0] = Random.Range(0, items.Length);
            ran[1] = Random.Range(0, items.Length);
            ran[2] = Random.Range(0, items.Length);

            if (ran[0] != ran[1] && ran[1] != ran[2] && ran[2] != ran[0])
                break;
        }

        for (int index = 0; index < ran.Length; index++)
        {
            Item ranItem = items[ran[index]];

            if (ranItem.level == ranItem.data.damages.Length)
            {
                items[4].gameObject.SetActive(true);
            }
            else
            {
                ranItem.gameObject.SetActive(true);
            }
        }
    }

    public void StartGameItem(int index)
    {
        selectedItem = items[index];
        if(selectedItem != null)
        {

            ApplySelectedItem();
        }
        
    }
}
