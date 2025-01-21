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
    public Text commonDescriptionText; // ���� ���� �ؽ�Ʈ
    public Button confirmButton; // Ȯ�� ��ư
    private Item selectedItem; // ���� ���õ� �������� ������ ����

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        items = GetComponentsInChildren<Item>(true);
        skillLevelUi = GetComponent<SkillLevelUI>();
        confirmButton.interactable = false; // ó���� Ȯ�� ��ư ��Ȱ��ȭ
        confirmButton.onClick.AddListener(ApplySelectedItem); // Ȯ�� ��ư �̺�Ʈ ����
    }

    public void Show()
    {
        Next(); // ���� ������ ����
        rect.localScale = Vector3.one; // ������ UI ǥ��
        GameManager.instance.Stop(); // ���� �Ͻ�����
    }

    public void Hide()
    {
        rect.localScale = Vector3.zero; // ������ UI ����
        GameManager.instance.Resume(); // ���� �簳
    }

    public void Select(int index)
    {
        selectedItem = items[index]; // ������ ������ ����
        selectedItem.UpdateDescriptionText(commonDescriptionText); // ���� ���� �ؽ�Ʈ ������Ʈ
        confirmButton.interactable = true; // Ȯ�� ��ư Ȱ��ȭ
    }

    private void ApplySelectedItem()
    {
        if (selectedItem != null)
        {
            selectedItem.OnClick(); // ������ ������ ����
            if (skillLevelUi == null)
            {
                skillLevelUi = GameObject.Find("HUD").GetComponentInChildren<SkillLevelUI>();
            }

            if (skillLevelUi != null)
            {
                skillLevelUi.AddSkill(selectedItem);
            }

            selectedItem = null; // ���� �ʱ�ȭ
            confirmButton.interactable = false; // Ȯ�� ��ư ��Ȱ��ȭ
            Hide(); // ������ UI ����
        }
    }

    void Next()
    {
        // ��� ������ ��Ȱ��ȭ
        foreach (Item item in items)
        {
            item.gameObject.SetActive(false);
        }

        // �����ϰ� 3�� ������ Ȱ��ȭ
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
