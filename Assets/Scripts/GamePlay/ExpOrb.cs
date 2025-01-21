using UnityEngine;

public class ExpOrb : MonoBehaviour
{
    private Transform player;  // �÷��̾� ��ġ ����
    public float attractionRange = 5f;  // �÷��̾�� ������ �����ϴ� ����
    public float attractionSpeed = 1f;  // �⺻ �ӵ�
    public float pullSpeed = 3f;  // ���� �ӵ�

    private bool isBeingPulled = false;

    private void Start()
    {
        // GameManager�� ���� player�� Transform �Ҵ�
        player = GameManager.instance.player.transform;
    }

    private void Update()
    {
        if (player == null) return;  // ���� ����

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // �÷��̾ ���� �ȿ� ������ ���� ȿ�� ����
        if (distanceToPlayer <= attractionRange)
        {
            isBeingPulled = true;
        }

        // ���� ���̸� �÷��̾� ��ġ�� ���� ����������� �̵�
        if (isBeingPulled)
        {
            // Lerp�� �ε巴�� �̵��ϸ鼭 ���� �÷��̾� ��ġ�� ����
            transform.position = Vector3.Lerp(transform.position, player.position, pullSpeed * Time.deltaTime);

            // �÷��̾���� �Ÿ��� �ſ� ��������� ���� ��Ȱ��ȭ
            if (distanceToPlayer < 0.1f)
            {
                gameObject.SetActive(false);
            }
        }
        else
        {
            // ���� ���� �ƴϸ� õõ�� �̵�
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * attractionSpeed * Time.deltaTime;
        }
    }
}
