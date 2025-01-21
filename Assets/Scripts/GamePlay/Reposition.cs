using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reposition : MonoBehaviour
{
    new Collider2D collider;

    private void Awake()
    {
        collider = GetComponent<Collider2D>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //�浹ü�� Area �±׸� ���� ��ü�� �浹�� ����� ��� ó��
        if (!collision.CompareTag("Area"))
        {
            return; //Area �±װ� �ƴϸ� ó������ ����
        }

        //�÷��̾�� ���� ��ü�� ��ġ�� ������
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 myPos = transform.position;
        float diffX = Mathf.Abs(playerPos.x - myPos.x); //X�� �Ÿ� ����
        float diffY = Mathf.Abs(playerPos.y - myPos.y); //Y�� �Ÿ� ����

        //�÷��̾��� �̵������� ������
        PlayerMove player = GameManager.instance.player.GetComponent<PlayerMove>();
        Vector3 playerDir = GameManager.instance.player.inputVec + player.joystick.GetInputVector();
        float dirX = playerDir.x < 0 ? -1 : 1; //X�� ���� ����
        float dirY = playerDir.y < 0 ? -1 : 1; //Y�� ���� ����

        //���� ��ü�� �±׿� ���� �̵���� ����
        switch (transform.tag)
        {
            case "Map":
                //�±װ� Map�� ���, x��� y�� �Ÿ� ���̿� ���� �̵����� ����
                if(diffX > diffY)
                {
                    //x�� �Ÿ����̰� �� Ŭ ��� x������ �̵�
                    transform.Translate(Vector3.right * dirX * 40);
                }
                else if (diffX < diffY)
                {
                    //y�� �Ÿ����̰� �� Ŭ ��� y������ �̵�
                    transform.Translate(Vector3.up * dirY * 40);
                }
                break;

            case "Enemy":
                //��ũ�� enemy�� ���, �÷��̾��� �̵������ ������ �������� ���� �̵�
                if (collider.enabled)
                {
                    transform.Translate(playerDir * 20 + new Vector3(Random.Range(-3f, 3f), Random.Range(-3f,3f), 0f));
                }
                break;
        }

    }
}
