using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public ItemData data; //�������� ������ (������, �̸�, ����)
    public int level; //���� ������ ����
    public Weapon weapon; // ���� �����ۿ� ���� ����
    public Gear gear; //��� �����ۿ� ���� ����
    

    Image icon;
    Text textLevel;
    Text textName;
    

    private void Awake()
    {
        //�ڽ� ������Ʈ���� ������ �̹��� ������Ʈ�� ã��
        icon = GetComponentsInChildren<Image>()[1];
        icon.sprite = data.itemIcon;

        //�ڽ� ������Ʈ���� �ؽ�Ʈ ������Ʈ�� ã��
        Text[] texts = GetComponentsInChildren<Text>();
        textLevel = texts[0];
        textName = texts[1];
        

        textName.text = data.itemName; //�������� �̸��� ����
    }

    private void OnEnable()
    {
        //�������� ������ ǥ��
        textLevel.text = "Lv." + (level + 1);
    }

    public void UpdateDescriptionText(Text commonDescriptionText)
    {
        switch (data.itemType)
        {
            case ItemData.ItemType.Melee:
            case ItemData.ItemType.Range:
            case ItemData.ItemType.Sword:
                //���� ���� �Ǵ� ���Ÿ� ������ ������ ����
                commonDescriptionText.text = string.Format(data.itemDesc, data.damages[level] * 100, data.counts[level]);
                break;
            case ItemData.ItemType.Glove:
            case ItemData.ItemType.Shoe:
                //�尩 �Ǵ� �Ź��� ������ ����
                commonDescriptionText.text = string.Format(data.itemDesc, data.damages[level] * 100);
                break;
            default:
                //�⺻ ������ ����
                commonDescriptionText.text = string.Format(data.itemDesc);
                break;


        }
    }               
        
    public void OnClick()
    {
        //������ Ŭ�� ���� ������ ����
        switch (data.itemType)
        {
            case ItemData.ItemType.Melee:
            case ItemData.ItemType.Range:
            case ItemData.ItemType.Sword:
                if(level == 0)
                {
                    //�������� ù��° ������ �� ���ο� ���� ��ü�� ����
                    GameObject newWeapon = new GameObject();
                    weapon = newWeapon.AddComponent<Weapon>();
                    weapon.Init(data); //���� �ʱ�ȭ
                }
                else
                {
                    //������ �� ������ �������� ī��Ʈ�� ������Ʈ
                    float nextDamage = data.baseDamage;
                    int nextCount = 0;

                    nextDamage += data.baseDamage * data.damages[level];
                    nextCount += data.counts[level];

                    weapon.LevelUp(nextDamage, nextCount);
                }
                level++;
                break;
            case ItemData.ItemType.Glove:
            case ItemData.ItemType.Shoe:
                if (level == 0)
                {
                    //�������� ù��° �������� ���ο� ��� ��ü�� ����
                    GameObject newGear = new GameObject();
                    gear = newGear.AddComponent<Gear>();
                    gear.Init(data);
                }
                else
                {
                    //������ �� ����� ȿ���� ������Ʈ
                    float nextRate = data.damages[level];
                    gear.LevelUp(nextRate);
                }
                level++;
                break;
            case ItemData.ItemType.Heal:
                //ȸ�� ������ ��� �� �÷��̾��� ü���� �ִ�ġ�� ȸ��
                GameManager.instance.health = GameManager.instance.maxhealth;
                return;
        }
        
        //�������� �ִ� ������ �����ϸ� ��ư�� ��Ȱ��ȭ
        if(level == data.damages.Length)
        {
            GetComponent<Button>().interactable = false;
        }
    }
}
