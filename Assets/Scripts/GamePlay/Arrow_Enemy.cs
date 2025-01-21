using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow_Enemy : MonoBehaviour
{
    public float damage = 5f;
    Transform archer;
    public float maxDistance = 15f; //화살 비활성화 거리
    Vector3 initialPos;

    private void OnEnable()
    {
        initialPos = transform.position;
    }

    public void SetArcher(Transform archerTransform)
    {
        archer = archerTransform;
    }

    private void Update()
    {
        if(Vector3.Distance(initialPos, transform.position) > maxDistance)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.instance.health -= damage;
            gameObject.SetActive(false);
        }
        
    }
          
    
}
