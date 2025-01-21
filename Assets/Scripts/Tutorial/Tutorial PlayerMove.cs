using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TutorialPlayerMove : MonoBehaviour
{
    public Transform targetPos;
    public Transform exitTargetPos;
    public float moveDuration = 5f;
    Animator anim;
    public TutorialManager tutorialManager;
    public Camera mainCamera;

    bool cameraFollowEnabled = false; //카메라 따라다니기 활성화 여부
    bool isCenterReached = false;

    private void Start()
    {
        anim = GetComponent<Animator>();     
    }

    private void Update()
    {
        if(cameraFollowEnabled)
        {
            FollowCamera();          
        }
    }

    public void MoveToCenter()
    {
        anim.SetTrigger("Run");
        cameraFollowEnabled = true;
        transform.DOMove(targetPos.position, moveDuration).SetEase(Ease.InOutSine)
            .OnUpdate(() =>
            {
                if(!isCenterReached && Mathf.Abs(transform.position.x - mainCamera.transform.position.x) < 0.1f)
                {
                    isCenterReached = true;
                    cameraFollowEnabled = true;
                }
            })
            .OnComplete(() =>
            {
                anim.SetTrigger("Idle");
            }
            );
    }

    public void MoveToExit()
    {
        anim.SetTrigger("Run");
        cameraFollowEnabled = true;
        transform.DOMove(exitTargetPos.position, moveDuration).SetEase(Ease.InOutSine)            
            .OnComplete(() =>
            {
                anim.SetTrigger("Idle");
                cameraFollowEnabled = false;
                DOTween.KillAll();
                LoadingSceneController.LoadScene("TutorialBattle");
            });
    }

    private void FollowCamera()
    {
        Vector3 cameraPos = mainCamera.transform.position;        
        cameraPos.x = transform.position.x; // 카메라의 x축을 플레이어 x축에 맞춤
        mainCamera.transform.position = cameraPos;
    }


}
