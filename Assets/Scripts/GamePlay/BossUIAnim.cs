using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossUIAnim : MonoBehaviour
{
    public Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void PlaySlashAnimation()
    {
        anim.SetTrigger("Slash");
        StartCoroutine(Close());
    }

    IEnumerator Close()
    {
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }
}
