using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float baseDamage; // �Ѿ��� �⺻ ������
    private float damage; //���� ������ ������
    public int per; //�Ѿ��� ������ �� �ִ� �ִ� �� ��
    private float critChance; //ġ��Ÿ Ȯ��
    private float critMultiplier; //ġ��Ÿ ����

    public float Damage => damage;
    public bool IsCriticalHit {  get; private set; }

    Rigidbody2D rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }
        

    public void Init(float damage, int per, Vector3 dir, float critChance, float critMultiplier)
    {
        this.baseDamage = damage;
        this.per = per;
        this.critChance = critChance;
        this.critMultiplier = critMultiplier;

        IsCriticalHit = Random.value < critChance;
        this.damage = IsCriticalHit ? baseDamage * critMultiplier : baseDamage;

        if(per > -1)
        {
            rigid.velocity = dir * 10f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //�浹�� ��ü�� enemy �±׸� ������ ���� �ʰų� per���� -1�� ��� ����
        if (!collision.CompareTag("Enemy") || per == -1)
        {
            if (collision.CompareTag("Chest"))
            {
                GameManager.instance.chest.OpenChest();
            }
            return;
        }

        EnemyMove enemyMove = collision.GetComponent<EnemyMove>();
        if(enemyMove != null)
        {
            per--; // �Ѿ��� ������ �� �� ����

            enemyMove.TakeDamage(damage, IsCriticalHit);
        }
                

        //per���� -1�� �Ǹ� �Ѿ��� ������Ű�� ��Ȱ��ȭ
        if(per == -1)
        {
            rigid.velocity = Vector2.zero;
            gameObject.SetActive(false);
        }
    }
}
