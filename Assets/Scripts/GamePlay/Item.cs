using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public ItemData data; //아이템의 데이터 (아이콘, 이름, 설명)
    public int level; //현재 아이템 레벨
    public Weapon weapon; // 무기 아이템에 대한 참조
    public Gear gear; //장비 아이템에 대한 참조
    

    Image icon;
    Text textLevel;
    Text textName;
    

    private void Awake()
    {
        //자식 오브젝트에서 아이콘 이미지 컴포넌트를 찾음
        icon = GetComponentsInChildren<Image>()[1];
        icon.sprite = data.itemIcon;

        //자식 오브젝트에서 텍스트 컴포넌트를 찾음
        Text[] texts = GetComponentsInChildren<Text>();
        textLevel = texts[0];
        textName = texts[1];
        

        textName.text = data.itemName; //아이템의 이름을 설정
    }

    private void OnEnable()
    {
        //아이템의 레벨을 표시
        textLevel.text = "Lv." + (level + 1);
    }

    public void UpdateDescriptionText(Text commonDescriptionText)
    {
        switch (data.itemType)
        {
            case ItemData.ItemType.Melee:
            case ItemData.ItemType.Range:
            case ItemData.ItemType.Sword:
                //근접 무기 또는 원거리 무기의 설명을 설정
                commonDescriptionText.text = string.Format(data.itemDesc, data.damages[level] * 100, data.counts[level]);
                break;
            case ItemData.ItemType.Glove:
            case ItemData.ItemType.Shoe:
                //장갑 또는 신발의 설명을 설정
                commonDescriptionText.text = string.Format(data.itemDesc, data.damages[level] * 100);
                break;
            default:
                //기본 설명을 설정
                commonDescriptionText.text = string.Format(data.itemDesc);
                break;


        }
    }               
        
    public void OnClick()
    {
        //아이템 클릭 시의 동작을 정의
        switch (data.itemType)
        {
            case ItemData.ItemType.Melee:
            case ItemData.ItemType.Range:
            case ItemData.ItemType.Sword:
                if(level == 0)
                {
                    //아이템의 첫번째 레벨업 시 새로운 무기 객체를 생성
                    GameObject newWeapon = new GameObject();
                    weapon = newWeapon.AddComponent<Weapon>();
                    weapon.Init(data); //무기 초기화
                }
                else
                {
                    //레벨업 시 무기의 데미지와 카운트를 업데이트
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
                    //아이템의 첫번째 레벨업시 새로운 장비 객체를 생성
                    GameObject newGear = new GameObject();
                    gear = newGear.AddComponent<Gear>();
                    gear.Init(data);
                }
                else
                {
                    //레벨업 시 장비의 효과를 업데이트
                    float nextRate = data.damages[level];
                    gear.LevelUp(nextRate);
                }
                level++;
                break;
            case ItemData.ItemType.Heal:
                //회복 아이템 사용 시 플레이어의 체력을 최대치로 회복
                GameManager.instance.health = GameManager.instance.maxhealth;
                return;
        }
        
        //아이템의 최대 레벨에 도달하면 버튼을 비활성화
        if(level == data.damages.Length)
        {
            GetComponent<Button>().interactable = false;
        }
    }
}
