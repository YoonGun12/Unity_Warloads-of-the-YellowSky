using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : MonoBehaviour
{
    public ItemData.ItemType type; //아이템의 타입
    public float rate; //장비의 효과 비율

    //장비 초기화
    public void Init(ItemData data)
    {
        //기본 설정
        name = "Gear" + data.itemId; //장비의 이름 설정
        transform.parent = GameManager.instance.player.transform; //플레이어의 자식으로 설정
        transform.localScale = Vector3.zero; //초기 크기 설정

        //속성 설정
        type = data.itemType; //아이템 타입 설정
        rate = data.damages[0]; //장비효과 비율 초기화
        ApplyGear(); //장비 효과 적용

    }

    //장비 레벨업
    public void LevelUp(float rate)
    {
        this.rate = rate;
        ApplyGear();
    }

    void ApplyGear()
    {
        //장비 타입에 따라 효과 적용
        switch(type)
        {
            case ItemData.ItemType.Glove:
                RateUp();
                break;
            case ItemData.ItemType.Shoe:
                SpeedUp();
                break;
            

        }
    }

    void RateUp()
    {
        //플레이어가 장착한 모든 무기 검색
        Weapon[] weapons = transform.parent.GetComponentsInChildren<Weapon>();

        //각 무기에 대해 효과 적용
        foreach(Weapon weapon in weapons)
        {
            switch(weapon.id)
            {
                case 0:
                    weapon.speed = 150 + (150 * rate); //기본속도 150에 비율에 따른 증가 적용
                    break;
                case 1:
                    weapon.speed = 1 * (1 - rate);
                    break;
                case 5:
                    weapon.speed = 1f * (1f - rate); //기본속도 0.5에 비율에 따른 증가 적용
                    break;
            }
        }
    }

    void SpeedUp()
    {
        float speed = 5; //기본 속도 설정
        GameManager.instance.player.moveSpeed = speed + speed * rate; //기본 속도에 비율에 따른 증가 적용
    }
        
}
