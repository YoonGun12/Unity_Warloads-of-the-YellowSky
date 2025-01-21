using UnityEngine;

public class ExpOrb : MonoBehaviour
{
    private Transform player;  // 플레이어 위치 참조
    public float attractionRange = 5f;  // 플레이어에게 끌리기 시작하는 범위
    public float attractionSpeed = 1f;  // 기본 속도
    public float pullSpeed = 3f;  // 흡입 속도

    private bool isBeingPulled = false;

    private void Start()
    {
        // GameManager를 통해 player의 Transform 할당
        player = GameManager.instance.player.transform;
    }

    private void Update()
    {
        if (player == null) return;  // 예외 방지

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // 플레이어가 범위 안에 들어오면 흡입 효과 시작
        if (distanceToPlayer <= attractionRange)
        {
            isBeingPulled = true;
        }

        // 흡입 중이면 플레이어 위치로 점점 가까워지도록 이동
        if (isBeingPulled)
        {
            // Lerp로 부드럽게 이동하면서 현재 플레이어 위치를 따라감
            transform.position = Vector3.Lerp(transform.position, player.position, pullSpeed * Time.deltaTime);

            // 플레이어와의 거리가 매우 가까워지면 구슬 비활성화
            if (distanceToPlayer < 0.1f)
            {
                gameObject.SetActive(false);
            }
        }
        else
        {
            // 흡입 중이 아니면 천천히 이동
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * attractionSpeed * Time.deltaTime;
        }
    }
}
