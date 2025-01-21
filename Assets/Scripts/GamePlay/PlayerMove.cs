using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed; // �÷��̾� �̵��ӵ�

    public GameManager manager; // ���ӸŴ���
    public Vector2 inputVec; // �÷��̾��� �Էº���
    public Scanner scanner; // �÷��̾��� ��ĳ��
    public JoyStick joystick; // ���̽�ƽ �Է�ó��
    

    SpriteRenderer Spr;
    Rigidbody2D rigid;
    Animator anim;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        Spr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
    }

    private void FixedUpdate()
    {
        // ������ Ȱ��ȭ�� ���¿����� �̵� ó��
        if (!GameManager.instance.isLive)
        {
            return;
        }

        // ���̽�ƽ �Է��� �޾� ���� �Է� ���͸� ���
        Vector2 joystickInput = joystick.GetInputVector();
        float joystickMagnitude = joystick.GetInputMagnitude();

        Vector2 finalInput = (inputVec + joystickInput).normalized * (moveSpeed * joystickMagnitude);

        // ���� �Է� ���͸� �ٷ� ����Ͽ� �̵�
        rigid.MovePosition(rigid.position + finalInput * Time.deltaTime);
        
        anim.SetFloat("Speed", finalInput.magnitude);
    }

    private void LateUpdate()
    {
        // ������ Ȱ��ȭ�� ���¿����� �ִϸ��̼� ó��
        if (!GameManager.instance.isLive)
        {
            return;
        }

        // ���̽�ƽ �Է��� �޾� ���� �Էº��͸� ���
        Vector2 joystickInput = joystick.GetInputVector();
        Vector2 finalInput = inputVec + joystickInput;

        // �̵����⿡ ���� ��������Ʈ ���� ��ȯ
        if (finalInput.x != 0)
        {
            Spr.flipX = finalInput.x < 0;
        }
    }

    private void OnMove(InputValue value)
    {
        // �Էº��͸� ������Ʈ
        inputVec = value.Get<Vector2>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            // ���� ��ġ�� ���� ��ü�� ����
            GameObject explosion = GameManager.instance.pool.Get(19); // ���� �������� �ε����� 19��� ����

            // ���� ��ġ�� ������ ��Ƽ� ��Ȯ�� ����
            Vector3 hitPosition = collision.contacts[0].point; // OnCollision���� �浹 ���� ��������
            explosion.transform.position = hitPosition;

            // �θ� ������ ���� ��ġ�� ����
            explosion.transform.SetParent(this.transform);

            // �ִϸ��̼� Ʈ���� ����
            Animator explosionAnimator = explosion.GetComponent<Animator>();
            if (explosionAnimator != null)
            {
                explosionAnimator.SetTrigger("White"); // �Ǵ� "White" Ʈ���� ����
            }

            // ���� �浹 �� ����ó��
            OnDamaged();
        }

        if (collision.gameObject.CompareTag("ExpOrb"))
        {
            // ����ġ ���� ó��
            GameManager.instance.GetExp();

            // ���� ����
            collision.gameObject.SetActive(false);
        }
    }

    void OnDamaged()
    {
        // anim.SetBool("isDamaged", true);
        InvokeRepeating("BlinkEffect", 0f, 0.1f);               

        // 2�� �� ���� ���� ����
        Invoke("OffDamaged", 0.5f);
    }

    void BlinkEffect()
    {
        if (Spr.color == Color.red)
        {
            Spr.color = Color.white;
        }
        else
        {
            Spr.color = Color.red;
        }
    }

    void OffDamaged()
    {
        // ���ػ��� ����
        gameObject.layer = 6;
        Spr.color = Color.white;
        // anim.SetBool("isDamaged", false);
        CancelInvoke("BlinkEffect");
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // ������ Ȱ��ȭ�� ���¿����� ü�� ���� ó��
        if (!GameManager.instance.isLive)
        {
            return;
        }

        // ü�� ���� ó��(�ʴ� 10��ŭ ����)
        if (collision.gameObject.CompareTag("Enemy"))
        {
            GameManager.instance.health -= Time.deltaTime * 10;
        }

        // ü���� 0���Ϸ� �������� ���ó��
        if (GameManager.instance.health < 0)
        {
            // �÷��̾��� �ڽ� ������Ʈ ��Ȱ��ȭ (����� ó��)
            for (int index = 2; index < transform.childCount; index++)
            {
                transform.GetChild(index).gameObject.SetActive(false);
            }

            // anim.SetTrigger("Dead");
            GameManager.instance.GameOver();
        }
    }
}
