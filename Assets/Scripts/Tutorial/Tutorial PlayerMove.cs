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

    bool cameraFollowEnabled = false; //ī�޶� ����ٴϱ� Ȱ��ȭ ����
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
        cameraPos.x = transform.position.x; // ī�޶��� x���� �÷��̾� x�࿡ ����
        mainCamera.transform.position = cameraPos;
    }


}
