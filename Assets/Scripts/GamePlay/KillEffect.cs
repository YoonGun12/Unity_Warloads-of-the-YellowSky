using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillEffect : MonoBehaviour
{
    public Text killText;
    public Image killIcon;
    public Animator anim;
    private int killCount;

    private void Start()
    {
        killCount = GameManager.instance.kill;
        UpdateKillUI();
    }

    private void Update()
    {
        if(killCount != GameManager.instance.kill)
        {
            killCount = GameManager.instance.kill;
            UpdateKillUI();
            CheckKillMilestone();
        }
    }

    void UpdateKillUI()
    {
        killText.text = killCount.ToString();
    }

    void CheckKillMilestone()
    {
        if(killCount > 0 && killCount % 5 ==0)
        {
            PlayKillEffect();
        }
    }

    void PlayKillEffect()
    {
        if(anim != null)
        {
            anim.SetTrigger("PlayEffect");
        }
    }
}
