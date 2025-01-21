using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Unity �����Ϳ��� scriptableobject�� ������ �� �ֵ��� ����
[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Object/ItemData")]
public class ItemData : ScriptableObject
{
    public enum ItemType { Melee, Range, Glove, Shoe, Heal, Sword}

    [Header("# Main Info")] //�⺻���� ����
    public ItemType itemType; //�������� ����
    public int itemId; //�������� ���� ID
    public string itemName; //�������� �̸�
    [TextArea]
    public string itemDesc; //������ ����
    public Sprite itemIcon; //������ ������(UI)

    [Header("# Level Data")] //������ ������ ����
    public float baseDamage; //�������� �⺻ ������ (����0)
    public int baseCount; //�������� �⺻ ī��Ʈ (����0)
    public float[] damages; //�� �������� �����ϴ� ������ �迭
    public int[] counts; //�� �������� �����ϴ� ����, ���� �迭

    [Header("# Weapon")] //������� ������ ����
    public GameObject projectile; //�߻�ü (������ ��� ���� �߻�ü)
}
