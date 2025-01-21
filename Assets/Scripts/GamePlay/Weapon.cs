using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int id; //무기 ID
    public int prefabId; //무기 프리팹 ID
    public float damage; //무기 데미지
    public int count; 
    public float speed;
    Transform sword;

    [Header("Critical Hit Attributes")]
    public float critChance = 0.1f;
    public float critMultiplier = 1.2f;

    float timer;
    PlayerMove player;   



    private void Awake()
    {
        player = GameManager.instance.player;        
    }

    

    void Update()
    {
        //게임이 진행 중인지 확인
        if (!GameManager.instance.isLive) return;
        
        //무기 ID에 따라 동작 수행
        switch (id)
        {
            case 0:
                //ID가 0인 도끼의 경우 회전 처리
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
                break;
            case 1:
                //다른 무기의 경우 발사 타이머 처리
                timer += Time.deltaTime;

                //타이머가 속도를 초과하면 발사
                if (timer > speed)
                {
                    timer = 0f;
                    Fire();
                }
                break;
            case 5:
                //ID가 5인 검의 경우
                timer += Time.deltaTime;
                if(timer> speed)
                {
                    timer = 0f;
                    Swing();                   
                }
                break;
            
        }

        
    }

    //무기 레벨업 처리
    public void LevelUp(float damage, int count)
    {
        this.damage = damage;
        this.count += count;

        //무기 ID가 0일 경우 배치 처리
        if (id == 0)
        {
            Batch();
        }
        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    //무기 초기화
    public void Init(ItemData data)
    {
        //기본 설정
        name = "Weapon" + data.itemId; //무기 이름 설정
        transform.parent = player.transform; //플레이어의 자식으로 설정
        transform.localPosition = Vector3.zero; //위치 초기화

        //속성 설정
        id = data.itemId; //무기ID 설정
        damage = data.baseDamage;//기본데미지 설정
        count = data.baseCount;

        //프리팹 ID 설정
        for(int index =0; index < GameManager.instance.pool.prefabs.Length; index++)
        {
            if(data.projectile == GameManager.instance.pool.prefabs[index])
            {
                prefabId = index;
                break;
            }
        }

        //무기 ID에 따라 속도 설정
        switch (id)
        {
            case 0:
                speed = 150;
                Batch();
                break;
            case 1:
                speed = 1;
                break;
            case 5:
                speed = 1f;
                break;
            default:
                speed = 0.3f;
                break;
        }

        player.BroadcastMessage("ApplyGear",SendMessageOptions.DontRequireReceiver);
    }

    void Batch()
    {
        for(int index = 0; index < count; index++)
        {
            Transform bullet;
            if(index < transform.childCount)
            {
                bullet = transform.GetChild(index);
            }
            else
            {
                bullet = GameManager.instance.pool.Get(prefabId).transform;
                bullet.parent = transform;
            }                       

            //무기 위치와 회전 초기화
            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            //무기 회전 처리
            Vector3 rotVec = Vector3.forward * 360 * index / count;
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * 2f, Space.World);

            bullet.GetComponent<Bullet>().Init(damage, -1, Vector3.zero, critChance, critMultiplier); // -1 is Infinity Per
        }
    }

    void Fire()
    {
        //타겟이 없으면 발사하지 않음
        if (!player.scanner.nearestTarget) return;        

        //타켓 위치와 방향 계산
        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 dir = (targetPos - transform.position).normalized;        

        //풀에서 총알 가져오기
        Transform bullet = GameManager.instance.pool.Get(prefabId).transform;
        bullet.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);

        bullet.GetComponent<Bullet>().Init(damage, count, dir, critChance, critMultiplier);

    }

    void Swing()
    {
        Transform target = player.scanner.nearestTarget;
        //타겟이 없으면 Swing 중지
        if (target == null) return;
                             
        if (sword == null)
        {
            sword = transform.Find("Weapon5");
            if(sword == null)
            {
                //검이 없으면 pool에서 새로운 검을 가져옴
                sword = GameManager.instance.pool.Get(prefabId).transform;
                sword.parent = transform;

                sword.GetComponent<Bullet>().Init(damage, -1, Vector3.zero, critChance, critMultiplier);
            }
                       
        }            

        Animator anim = sword.GetComponent<Animator>();
        if (anim != null && !anim.GetCurrentAnimatorStateInfo(0).IsName("Swing"))
        {                        
            anim.SetTrigger("Swing");

            //플레이어와 적 간의 방향 계산
            Vector3 direction = (target.position - player.transform.position).normalized;
            //검의 위치를 플레이어 위치에서 적 방향으로 일정거리 이동 시킴
            Vector3 swordPos = player.transform.position + direction * 1.5f;
            sword.position = swordPos;
            //검 회전
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;//각도를 라디안에서 도로 변환
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward); //검을 해당 각도로 회전

        }           
    }
}
