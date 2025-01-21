using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed; // 플레이어 이동속도

    public GameManager manager; // 게임매니저
    public Vector2 inputVec; // 플레이어의 입력벡터
    public Scanner scanner; // 플레이어의 스캐너
    public JoyStick joystick; // 조이스틱 입력처리
    

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
        // 게임이 활성화된 상태에서만 이동 처리
        if (!GameManager.instance.isLive)
        {
            return;
        }

        // 조이스틱 입력을 받아 최종 입력 벡터를 계산
        Vector2 joystickInput = joystick.GetInputVector();
        float joystickMagnitude = joystick.GetInputMagnitude();

        Vector2 finalInput = (inputVec + joystickInput).normalized * (moveSpeed * joystickMagnitude);

        // 최종 입력 벡터를 바로 사용하여 이동
        rigid.MovePosition(rigid.position + finalInput * Time.deltaTime);
        
        anim.SetFloat("Speed", finalInput.magnitude);
    }

    private void LateUpdate()
    {
        // 게임이 활성화된 상태에서만 애니메이션 처리
        if (!GameManager.instance.isLive)
        {
            return;
        }

        // 조이스틱 입력을 받아 최종 입력벡터를 계산
        Vector2 joystickInput = joystick.GetInputVector();
        Vector2 finalInput = inputVec + joystickInput;

        // 이동방향에 따라 스프라이트 방향 전환
        if (finalInput.x != 0)
        {
            Spr.flipX = finalInput.x < 0;
        }
    }

    private void OnMove(InputValue value)
    {
        // 입력벡터를 업데이트
        inputVec = value.Get<Vector2>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            // 맞은 위치에 폭발 객체를 생성
            GameObject explosion = GameManager.instance.pool.Get(19); // 폭발 프리팹의 인덱스가 19라고 가정

            // 맞은 위치를 변수에 담아서 정확히 설정
            Vector3 hitPosition = collision.contacts[0].point; // OnCollision에서 충돌 지점 가져오기
            explosion.transform.position = hitPosition;

            // 부모를 설정해 맞은 위치에 고정
            explosion.transform.SetParent(this.transform);

            // 애니메이션 트리거 설정
            Animator explosionAnimator = explosion.GetComponent<Animator>();
            if (explosionAnimator != null)
            {
                explosionAnimator.SetTrigger("White"); // 또는 "White" 트리거 설정
            }

            // 적과 충돌 시 피해처리
            OnDamaged();
        }

        if (collision.gameObject.CompareTag("ExpOrb"))
        {
            // 경험치 증가 처리
            GameManager.instance.GetExp();

            // 구슬 제거
            collision.gameObject.SetActive(false);
        }
    }

    void OnDamaged()
    {
        // anim.SetBool("isDamaged", true);
        InvokeRepeating("BlinkEffect", 0f, 0.1f);               

        // 2초 후 피해 상태 해제
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
        // 피해상태 해제
        gameObject.layer = 6;
        Spr.color = Color.white;
        // anim.SetBool("isDamaged", false);
        CancelInvoke("BlinkEffect");
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // 게임이 활성화된 상태에서만 체력 감소 처리
        if (!GameManager.instance.isLive)
        {
            return;
        }

        // 체력 감소 처리(초당 10만큼 감소)
        if (collision.gameObject.CompareTag("Enemy"))
        {
            GameManager.instance.health -= Time.deltaTime * 10;
        }

        // 체력이 0이하로 떨어지면 사망처리
        if (GameManager.instance.health < 0)
        {
            // 플레이어의 자식 오브젝트 비활성화 (사망시 처리)
            for (int index = 2; index < transform.childCount; index++)
            {
                transform.GetChild(index).gameObject.SetActive(false);
            }

            // anim.SetTrigger("Dead");
            GameManager.instance.GameOver();
        }
    }
}
