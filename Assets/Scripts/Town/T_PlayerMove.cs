using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_PlayerMove : MonoBehaviour
{
    public float moveSpeed = 5f;
    public T_Joystick joystick;
    Animator anim;
    SpriteRenderer Spr;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        Spr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        float horizontalInput = joystick.GetHorizontalInput();
        Vector3 moveDirection = new Vector3 (horizontalInput, 0, 0);
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

        anim.SetFloat("Speed", Mathf.Abs(horizontalInput));
    }

    private void LateUpdate()
    {
        float horizontalInput = joystick.GetHorizontalInput();

        if (horizontalInput > 0) Spr.flipX = false;
        else if (horizontalInput < 0) Spr.flipX = true;
        
    }
}
