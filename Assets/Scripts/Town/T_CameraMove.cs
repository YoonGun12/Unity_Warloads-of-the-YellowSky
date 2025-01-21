using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_CameraMove : MonoBehaviour
{
    public Transform player;
    public float minX = -14f;
    public float maxX = 17f;

    private void LateUpdate()
    {
        Vector3 cameraPos = transform.position;

        cameraPos.x = Mathf.Clamp(player.position.x, minX, maxX);

        transform.position = cameraPos;
    }
}
