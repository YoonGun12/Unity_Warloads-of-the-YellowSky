using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public enum EnemyType { Farmer, Infantry, Archer, Spearman, Assassin, EliteInfantry, EliteAssassin, Guard, Mystic, AssaultHound, DefensiveInfantry, HeavySwordsman, AxeInfantry }

    [Header("Enemy Attributes")]
    public EnemyType enemyType;
    public float enemySpeed;
    public float health;
    public float maxHealth;
    public float attackRange; //궁병의 공격 범위

    [Header("Attack State")]
    private bool isAttacking = false;
    private Vector3 initialTargetDirection;

    [Header("Morale State")]
    public static float healthMultiplier = 1.0f;
    public static float speedMultiplier = 1.0f;

    [Header("References")]
    private Transform player;
    private Rigidbody2D rigid;
    private Animator anim;
    private Collider2D coll;
    private SpriteRenderer spr;
    private bool isLive;
    private WaitForFixedUpdate wait;
    private Vector3 hitPos; //적의 피격 위치 저장 변수
    public Transform damageTextPos;   


    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        spr = GetComponent<SpriteRenderer>();
        player = GameManager.instance.player.transform;        
        wait = new WaitForFixedUpdate();
    }

    private void Update()
    {
        if (!isLive) return;

        if (enemyType == EnemyType.Archer)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            if (distanceToPlayer <= attackRange)
            {
                //Archer가 공격 중이 아니고 공격 범위에 있을 때 공격 시작
                if (!isAttacking)
                {
                    isAttacking = true;
                    initialTargetDirection = (player.position - transform.position).normalized;
                    StartCoroutine(AttackArcher());
                }
                
            }
            else
            {
                //공격 범위를 벗어났을 때 이동
                isAttacking = false;
                FollowPlayer();
            }
        }
        else if (enemyType == EnemyType.Farmer && !isAttacking)
        {
            FollowPlayer();
        }
    }

    private void FixedUpdate()
    {
        if (!isLive || isAttacking || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit") || anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            return;

        if (enemyType != EnemyType.Archer)
            FollowPlayer();
    }

    private void FollowPlayer()
    {
        if (isAttacking) return;

        Vector2 direction = (player.position - transform.position).normalized;
        rigid.MovePosition(rigid.position + direction * enemySpeed * Time.fixedDeltaTime);
        rigid.velocity = Vector2.zero;
    }

    private IEnumerator AttackArcher()
    {
        //anim.SetTrigger("Shoot");
        yield return new WaitForSeconds(2f); //화살의 재장전 속도

        FireArrow();
        isAttacking = false;
    }

    private void FireArrow()
    {
        //화살 생성
        GameObject arrow = GameManager.instance.pool.Get(18); 
        arrow.transform.position = transform.position;

        Arrow_Enemy arrowScript = arrow.GetComponent<Arrow_Enemy>();
        arrowScript.SetArcher(transform);

        //화살 속도 설정
        Rigidbody2D arrowRb = arrow.GetComponent<Rigidbody2D>();
        arrowRb.velocity = initialTargetDirection * 3f; 

        //화살 회전 설정
        float angle = Mathf.Atan2(initialTargetDirection.y, initialTargetDirection.x) * Mathf.Rad2Deg;
        arrow.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isLive) return;
                

        if (collision.collider.CompareTag("Player") && enemyType == EnemyType.Farmer && !isAttacking)
        {
            isAttacking = true;
            anim.SetBool("Attack", true);
            StartCoroutine(AttackFarmer());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isLive) return;

        if (collision.CompareTag("Bullet"))
        {
            Bullet bullet = collision.GetComponent<Bullet>();
            hitPos = collision.ClosestPoint(transform.position);
            

            
            TakeDamage(bullet.Damage, bullet.IsCriticalHit);
        }

    }

    private IEnumerator AttackFarmer()
    {
        yield return new WaitForSeconds(0.5f);

        if (Vector2.Distance(transform.position, player.position) < 1f)
            GameManager.instance.health -= 10;

        anim.SetBool("Attack", false);
        isAttacking = false;
    }

    void ShowDamageText(int damageAmount, bool isCritical)
    {
        GameObject damageTextObj = GameManager.instance.pool.Get(20);
        damageTextObj.transform.position = damageTextPos.position;
        damageTextObj.SetActive(true);
        
        DamageText damageText = damageTextObj.GetComponent<DamageText>();
        damageText.DisplayDamage(damageAmount, isCritical);
    }
    public void TakeDamage(float damage, bool isCritical)
    {
        health -= damage;
        int displayDamage = Mathf.RoundToInt(damage);

        ShowDamageText(displayDamage, isCritical);

        GameObject explosion = GameManager.instance.pool.Get(19);                
        explosion.transform.position = hitPos;

        ExplosionVfx explosionFollow = explosion.GetComponent<ExplosionVfx>();
        if(explosionFollow != null)
        {
            explosionFollow.target = this.transform;
        }
        explosion.SetActive(true);

        Animator explosionVfx = explosion.GetComponent<Animator>();
        if (explosionVfx != null)
        {
            explosionVfx.SetTrigger(isCritical ? "Critical" : "Red");
        }

        if (health <= 0)
        {
            isLive = false;
            coll.enabled = false;
            rigid.simulated = false;
            spr.sortingOrder = 1;

            GameManager.instance.kill++;
            if (!GameManager.instance.moraleSystem.isBossSpawned)
                GameManager.instance.moraleSystem.IncreaseMorale(0.01f);

            anim.SetTrigger("Dead");
        }
        else
        {
            OnDamaged(isCritical);
            StartCoroutine(KnockBack());
        }
    }                

    private void OnDamaged(bool isCritical)
    {
        spr.color = Color.red;
        Invoke("OffDamaged", 0.5f);                
        
    }

    private void OffDamaged()
    {
        spr.color = Color.white;
    }

    private void Die()
    {
        gameObject.SetActive(false);

        GameObject expOrb = GameManager.instance.pool.Get(3);
        expOrb.transform.position = transform.position;
    }
        

    private IEnumerator KnockBack()
    {        
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        rigid.AddForce(dirVec.normalized * 2, ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.2f);

        rigid.velocity = Vector2.zero;

        isAttacking = false;
    }

    public void Init(SpawnData data)
    {       
        enemySpeed = data.speed * speedMultiplier;
        maxHealth = data.health * healthMultiplier;
        health = maxHealth;
    }

    private void LateUpdate()
    {
        if (!isLive) return;
        spr.flipX = player.position.x < transform.position.x;
    }

    private void OnEnable()
    {
        isLive = true;
        health = maxHealth;
        coll.enabled = true;
        rigid.simulated = true;
        spr.sortingOrder = 2;

    }
}
