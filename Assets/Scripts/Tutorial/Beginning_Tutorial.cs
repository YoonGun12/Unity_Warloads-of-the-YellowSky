using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Beginning_Tutorial : MonoBehaviour
{    
    public GameObject sky;
    public GameObject forest;
    public GameObject army;
    public GameObject tree1;
    public GameObject tree2;

    public Image startPanel;
    public Image endPanel;

    public TextMeshProUGUI skip;

    public float skySpeed;
    public float forestSpeed;
    public float tree1Speed;
    public float tree2Speed;

    private Vector3 skyStartPos;
    private Vector3 forestStartPos;
    private Vector3 tree1StartPos;
    private Vector3 tree2StartPos;



    private void Start()
    {
        // ��� �ʱ� ��ġ ����
        skyStartPos = sky.transform.position;
        forestStartPos = forest.transform.position;
        tree1StartPos = tree1.transform.position;
        tree2StartPos = tree2.transform.position;

        BlinkSkip();

        // ȭ�� ���̵��� �� Army �̵� ����
        startPanel.DOFade(0f, 0.1f).OnComplete(() =>
        {
            startPanel.gameObject.SetActive(false);
            MoveArmy();
            StartBackgroundMovement();
        });

        Invoke("EndPanelSlideOut", 4f);
    }

    void MoveArmy()
    {
        // Army ��ü�� ó�� ��ġ�� �̵���Ű�� �̵� �ִϸ��̼� ����
        army.transform.position = new Vector3(-6.57f, -2.67f, 0f);
        army.transform.DOMove(new Vector3(-12.07f, -6.63f, 0f), 6f).SetEase(Ease.InOutSine);

    }

    void StartBackgroundMovement()
    {
        // Sky �̵�
        sky.transform.DOMoveX(skyStartPos.x - 10f, 20f / skySpeed).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);

        // Forest �̵�
        forest.transform.DOMoveX(forestStartPos.x - 10f, 20f / forestSpeed).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);


        // Tree1 ���� ��ũ��
        StartCoroutine(ScrollTree(tree1, tree1Speed, tree1StartPos));

        // Tree2 ���� ��ũ��
        StartCoroutine(ScrollTree(tree2, tree2Speed, tree2StartPos));
    }

    private IEnumerator ScrollTree(GameObject tree, float speed, Vector3 startPos)
    {
        while (true)
        {
            // ������ �������� �̵�
            tree.transform.DOMoveX(startPos.x - 40f, 10f / speed).SetEase(Ease.Linear).OnComplete(() =>
            {
                // ���� ���� �����ϸ� �ٽ� �ʱ� ��ġ�� �̵�
                tree.transform.position = startPos;
            });

            yield return new WaitForSeconds(10f / speed);
        }
    }

    void EndPanelSlideOut()
    {
        endPanel.gameObject.SetActive(true);

        endPanel.DOFade(1f, 2f).OnComplete(() =>
        {
            ChangeScene();
        });
    }

    void ChangeScene()
    {
        SceneManager.LoadScene("OutsideTemple_Tutorial");
        DOTween.KillAll();
    }

    void BlinkSkip()
    {
        Color originalColor = skip.color;
        skip.DOFade(0f, 1.5f).SetLoops(-1, LoopType.Yoyo);
    }

    public void SkipButton()
    {
        LoadingSceneController.LoadScene("TutorialBattle");
        DOTween.KillAll();
    }
}

