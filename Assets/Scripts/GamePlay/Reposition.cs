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
        //충돌체가 Area 태그를 가진 객체와 충돌이 종료된 경우 처리
        if (!collision.CompareTag("Area"))
        {
            return; //Area 태그가 아니면 처리하지 않음
        }

        //플레이어와 현재 객체의 위치를 가져옴
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 myPos = transform.position;
        float diffX = Mathf.Abs(playerPos.x - myPos.x); //X축 거리 차이
        float diffY = Mathf.Abs(playerPos.y - myPos.y); //Y축 거리 차이

        //플레이어의 이동방향을 가져옴
        PlayerMove player = GameManager.instance.player.GetComponent<PlayerMove>();
        Vector3 playerDir = GameManager.instance.player.inputVec + player.joystick.GetInputVector();
        float dirX = playerDir.x < 0 ? -1 : 1; //X축 방향 결정
        float dirY = playerDir.y < 0 ? -1 : 1; //Y축 방향 결정

        //현재 객체의 태그에 따라 이동방식 결정
        switch (transform.tag)
        {
            case "Map":
                //태그가 Map인 경우, x축과 y축 거리 차이에 따라 이동방향 결정
                if(diffX > diffY)
                {
                    //x축 거리차이가 더 클 경우 x축으로 이동
                    transform.Translate(Vector3.right * dirX * 40);
                }
                else if (diffX < diffY)
                {
                    //y축 거리차이가 더 클 경우 y축으로 이동
                    transform.Translate(Vector3.up * dirY * 40);
                }
                break;

            case "Enemy":
                //태크가 enemy인 경우, 플레이어의 이동방향과 무작위 오프셋을 더해 이동
                if (collider.enabled)
                {
                    transform.Translate(playerDir * 20 + new Vector3(Random.Range(-3f, 3f), Random.Range(-3f,3f), 0f));
                }
                break;
        }

    }
}
