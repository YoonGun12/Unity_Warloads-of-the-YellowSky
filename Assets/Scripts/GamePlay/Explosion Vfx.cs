using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionVfx : MonoBehaviour
{
    public Transform target;

    private void Update()
    {
        if(target != null)
        {
            transform.position = target.position;
        }
    }
    public void DeactiveObject()
    {        
        gameObject.SetActive(false);
    }
}
