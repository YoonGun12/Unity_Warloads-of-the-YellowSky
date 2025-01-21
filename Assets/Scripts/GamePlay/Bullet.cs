using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float baseDamage; // 총알의 기본 데미지
    private float damage; //실제 적용할 데미지
    public int per; //총알이 명중할 수 있는 최대 적 수
    private float critChance; //치명타 확률
    private float critMultiplier; //치명타 배율

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
        //충돌한 객체가 enemy 태그를 가지고 있지 않거나 per값이 -1인 경우 무시
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
            per--; // 총알이 명중한 적 수 감소

            enemyMove.TakeDamage(damage, IsCriticalHit);
        }
                

        //per값이 -1이 되면 총알을 정지시키고 비활성화
        if(per == -1)
        {
            rigid.velocity = Vector2.zero;
            gameObject.SetActive(false);
        }
    }
}
