using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : MonoBehaviour
{
    public ItemData.ItemType type; //�������� Ÿ��
    public float rate; //����� ȿ�� ����

    //��� �ʱ�ȭ
    public void Init(ItemData data)
    {
        //�⺻ ����
        name = "Gear" + data.itemId; //����� �̸� ����
        transform.parent = GameManager.instance.player.transform; //�÷��̾��� �ڽ����� ����
        transform.localScale = Vector3.zero; //�ʱ� ũ�� ����

        //�Ӽ� ����
        type = data.itemType; //������ Ÿ�� ����
        rate = data.damages[0]; //���ȿ�� ���� �ʱ�ȭ
        ApplyGear(); //��� ȿ�� ����

    }

    //��� ������
    public void LevelUp(float rate)
    {
        this.rate = rate;
        ApplyGear();
    }

    void ApplyGear()
    {
        //��� Ÿ�Կ� ���� ȿ�� ����
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
        //�÷��̾ ������ ��� ���� �˻�
        Weapon[] weapons = transform.parent.GetComponentsInChildren<Weapon>();

        //�� ���⿡ ���� ȿ�� ����
        foreach(Weapon weapon in weapons)
        {
            switch(weapon.id)
            {
                case 0:
                    weapon.speed = 150 + (150 * rate); //�⺻�ӵ� 150�� ������ ���� ���� ����
                    break;
                case 1:
                    weapon.speed = 1 * (1 - rate);
                    break;
                case 5:
                    weapon.speed = 1f * (1f - rate); //�⺻�ӵ� 0.5�� ������ ���� ���� ����
                    break;
            }
        }
    }

    void SpeedUp()
    {
        float speed = 5; //�⺻ �ӵ� ����
        GameManager.instance.player.moveSpeed = speed + speed * rate; //�⺻ �ӵ��� ������ ���� ���� ����
    }
        
}
