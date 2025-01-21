using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class BossMove : MonoBehaviour
{
    public float bossSpeed;
    public float bossHealth = 500f;
    public Transform player;
    public GameObject bossUI;
    public Slider bossHealthBar;
    

    Animator anim;
    SpriteRenderer Spr;
    Rigidbody2D rigid;
    float maxBossHealth;

    public BossUIAnim[] BossAnims;
    BossNameEffect bossNameEffect;

    CinemachineVirtualCamera cinemachineCam;
    

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        player = GameManager.instance.player.transform;
        anim = GetComponent<Animator>();
        Spr = GetComponent<SpriteRenderer>();

        bossUI = GameObject.Find("BossUI");
        bossHealthBar = GameObject.Find("BossHealthBar").GetComponent<Slider>();
        bossNameEffect = bossUI.GetComponentInChildren<BossNameEffect>();       
        BossAnims =FindObjectsOfType<BossUIAnim>();

        if (GameManager.instance.isTutorialMode)
        {
            bossHealth = 50f;
            maxBossHealth = bossHealth;
        }
        else
        {
            maxBossHealth = bossHealth;
        }
        maxBossHealth = bossHealth;
        cinemachineCam = GameObject.Find("Virtual Camera").GetComponent<CinemachineVirtualCamera>();

        

        StartCoroutine(BossAppear());

    }
    IEnumerator BossAppear()
    {
        anim.SetTrigger("Appear");

        cinemachineCam.Follow = transform;

        //UI 활성화
        bossUI.transform.localScale = Vector3.one;

        StartCoroutine(AnimateBossUI());

        yield return new WaitForSeconds(2.5f);

        cinemachineCam.Follow = player;
        
        
        StartCoroutine(MoveToPlayer());
    }

    IEnumerator AnimateBossUI()
    {           
        
        //보스 이름 효과
        bossNameEffect.StartEffect();          
        
        //보스 체력바 효과
        bossHealthBar.value = 0;
        float healthFillSpeed = 0.6f;
        while (bossHealthBar.value < 1)
        {
            bossHealthBar.value += healthFillSpeed * Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(1f);

        //Slash 애니메이션
        foreach (var bossAnim in BossAnims)
        {
            bossAnim.PlaySlashAnimation();
        }

        gameObject.layer = LayerMask.NameToLayer("Enemy");

    }
            

    IEnumerator MoveToPlayer()
    {
        while (bossHealth > 0)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * bossSpeed * Time.deltaTime;

            bossHealthBar.value = bossHealth/maxBossHealth;

            yield return null;
        }
        if (!GameManager.instance.bossDefeated)
        {
            GameManager.instance.DefeatBoss();
        }
        
        Destroy(gameObject);

    }

    private void LateUpdate()
    {
        Vector3 direction = player.position -transform.position;
        Spr.flipX = direction.x < 0;       
        
    }       
       

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet"))
        {
            return;
        }

        Bullet bullet = collision.GetComponent<Bullet>();
        if (bullet == null) return;

        bossHealth -= bullet.Damage;


        if(bossHealth > 0)
        {
            OnDamaged(bullet.IsCriticalHit);
        }
        else
        {
            GameManager.instance.DefeatBoss();
            collision.enabled = false;
            rigid.simulated = false;
            Spr.sortingOrder = 1;

            GameManager.instance.kill++;
            anim.SetTrigger("Dead");
        }
    }

    void OnDamaged(bool isCritical)
    {
        //anim.SetBool("isAttacked", true);
        Spr.color = isCritical ? Color.yellow : Color.red;
        Invoke("OffDamaged", 0.5f);
    }

    void OffDamaged()
    {
        Spr.color = Color.white;
        //anim.SetBool("isAttaked", false);
    }
      

    

    
}
