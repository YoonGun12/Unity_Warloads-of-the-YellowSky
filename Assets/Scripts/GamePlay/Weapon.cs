using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int id; //���� ID
    public int prefabId; //���� ������ ID
    public float damage; //���� ������
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
        //������ ���� ������ Ȯ��
        if (!GameManager.instance.isLive) return;
        
        //���� ID�� ���� ���� ����
        switch (id)
        {
            case 0:
                //ID�� 0�� ������ ��� ȸ�� ó��
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
                break;
            case 1:
                //�ٸ� ������ ��� �߻� Ÿ�̸� ó��
                timer += Time.deltaTime;

                //Ÿ�̸Ӱ� �ӵ��� �ʰ��ϸ� �߻�
                if (timer > speed)
                {
                    timer = 0f;
                    Fire();
                }
                break;
            case 5:
                //ID�� 5�� ���� ���
                timer += Time.deltaTime;
                if(timer> speed)
                {
                    timer = 0f;
                    Swing();                   
                }
                break;
            
        }

        
    }

    //���� ������ ó��
    public void LevelUp(float damage, int count)
    {
        this.damage = damage;
        this.count += count;

        //���� ID�� 0�� ��� ��ġ ó��
        if (id == 0)
        {
            Batch();
        }
        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    //���� �ʱ�ȭ
    public void Init(ItemData data)
    {
        //�⺻ ����
        name = "Weapon" + data.itemId; //���� �̸� ����
        transform.parent = player.transform; //�÷��̾��� �ڽ����� ����
        transform.localPosition = Vector3.zero; //��ġ �ʱ�ȭ

        //�Ӽ� ����
        id = data.itemId; //����ID ����
        damage = data.baseDamage;//�⺻������ ����
        count = data.baseCount;

        //������ ID ����
        for(int index =0; index < GameManager.instance.pool.prefabs.Length; index++)
        {
            if(data.projectile == GameManager.instance.pool.prefabs[index])
            {
                prefabId = index;
                break;
            }
        }

        //���� ID�� ���� �ӵ� ����
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

            //���� ��ġ�� ȸ�� �ʱ�ȭ
            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            //���� ȸ�� ó��
            Vector3 rotVec = Vector3.forward * 360 * index / count;
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * 2f, Space.World);

            bullet.GetComponent<Bullet>().Init(damage, -1, Vector3.zero, critChance, critMultiplier); // -1 is Infinity Per
        }
    }

    void Fire()
    {
        //Ÿ���� ������ �߻����� ����
        if (!player.scanner.nearestTarget) return;        

        //Ÿ�� ��ġ�� ���� ���
        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 dir = (targetPos - transform.position).normalized;        

        //Ǯ���� �Ѿ� ��������
        Transform bullet = GameManager.instance.pool.Get(prefabId).transform;
        bullet.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);

        bullet.GetComponent<Bullet>().Init(damage, count, dir, critChance, critMultiplier);

    }

    void Swing()
    {
        Transform target = player.scanner.nearestTarget;
        //Ÿ���� ������ Swing ����
        if (target == null) return;
                             
        if (sword == null)
        {
            sword = transform.Find("Weapon5");
            if(sword == null)
            {
                //���� ������ pool���� ���ο� ���� ������
                sword = GameManager.instance.pool.Get(prefabId).transform;
                sword.parent = transform;

                sword.GetComponent<Bullet>().Init(damage, -1, Vector3.zero, critChance, critMultiplier);
            }
                       
        }            

        Animator anim = sword.GetComponent<Animator>();
        if (anim != null && !anim.GetCurrentAnimatorStateInfo(0).IsName("Swing"))
        {                        
            anim.SetTrigger("Swing");

            //�÷��̾�� �� ���� ���� ���
            Vector3 direction = (target.position - player.transform.position).normalized;
            //���� ��ġ�� �÷��̾� ��ġ���� �� �������� �����Ÿ� �̵� ��Ŵ
            Vector3 swordPos = player.transform.position + direction * 1.5f;
            sword.position = swordPos;
            //�� ȸ��
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;//������ ���ȿ��� ���� ��ȯ
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward); //���� �ش� ������ ȸ��

        }           
    }
}
