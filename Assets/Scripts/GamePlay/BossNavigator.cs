using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossNavigator : MonoBehaviour
{
    public Transform boss;
    public GameObject bossNavigator;
    public Transform arrow;
    public float horizontalRadius;
    public float verticalRadius;

    private void Start()
    {
        bossNavigator.transform.localScale = Vector3.zero;
    }

    public void ShowNavigator()
    {
        bossNavigator.transform.localScale = Vector3.one;
    }

    public void HideNavigator()
    {
        bossNavigator.transform.localScale = Vector3.zero;
    }

    private void Update()
    {
        if(boss != null && bossNavigator.activeSelf)
        {
            Vector3 direction = (boss.position - transform.position).normalized;

            float angleToBoss = Mathf.Atan2(direction.y, direction.x);
            float x = Mathf.Cos(angleToBoss) * horizontalRadius;
            float y = Mathf.Sin(angleToBoss) * verticalRadius;

            arrow.localPosition = new Vector3(x, y, 0);

            arrow.right = direction;
        }
    }
}
