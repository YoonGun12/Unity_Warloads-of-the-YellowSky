using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Unity 에디터에서 scriptableobject를 생성할 수 있도록 설정
[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Object/ItemData")]
public class ItemData : ScriptableObject
{
    public enum ItemType { Melee, Range, Glove, Shoe, Heal, Sword}

    [Header("# Main Info")] //기본정보 섹션
    public ItemType itemType; //아이템의 종류
    public int itemId; //아이템의 고유 ID
    public string itemName; //아이템의 이름
    [TextArea]
    public string itemDesc; //아이템 설명
    public Sprite itemIcon; //아이템 아이콘(UI)

    [Header("# Level Data")] //레벨별 데이터 섹션
    public float baseDamage; //아이템의 기본 데미지 (레벨0)
    public int baseCount; //아이템의 기본 카운트 (레벨0)
    public float[] damages; //각 레벨에서 증가하는 데미지 배열
    public int[] counts; //각 레벨에서 증가하는 관통, 개수 배열

    [Header("# Weapon")] //무기관련 데이터 섹션
    public GameObject projectile; //발사체 (무기일 경우 사용될 발사체)
}
