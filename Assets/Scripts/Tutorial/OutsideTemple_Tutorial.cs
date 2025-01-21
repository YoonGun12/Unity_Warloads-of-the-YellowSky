using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using UnityEngine.SceneManagement;

public class OutsideTemple_Tutorial : MonoBehaviour
{
    public GameObject tree1; // �������� �̵��� ����
    public GameObject tree2; // �������� �̵��� ����
    public Image fadePanel; // ���̵� ȿ���� ���� �̹���

    public float treeSpreadDuration; // ���� ������� �ִϸ��̼� �ð�
    public float treeSpreadDistance; // ������ ������� �Ÿ�    

    private Vector3 initialTree1Pos; // Tree1�� �ʱ� ��ġ
    private Vector3 initialTree2Pos; // Tree2�� �ʱ� ��ġ

    public TextMeshProUGUI skip;

    private void Start()
    {
        // ������ �ʱ� ��ġ ����
        initialTree1Pos = tree1.transform.position;
        initialTree2Pos = tree2.transform.position;

        BlinkSkip();

        // ���̵��� ȿ�� �� ���� ������ ����
        FadeIn(() =>
        {
            StartCoroutine(SpreadTrees());
        });
    }

    IEnumerator SpreadTrees()
    {
        // Tree1 ���� �̵�
        tree1.transform.DOMoveX(initialTree1Pos.x - treeSpreadDistance, treeSpreadDuration)
            .SetEase(Ease.OutSine);

        yield return new WaitForSeconds(0.75f);

        // Tree2 ������ �̵�
        tree2.transform.DOMoveX(initialTree2Pos.x + treeSpreadDistance, treeSpreadDuration)
            .SetEase(Ease.OutSine);

        yield return new WaitForSeconds(treeSpreadDuration);

        // ���̵�ƿ� ȿ�� �� �� ��ȯ
        FadeOut(ChangeScene);
    }

    void ChangeScene()
    {
        SceneManager.LoadScene("Tutorial");
        DOTween.KillAll();
    }

    void BlinkSkip()
    {
        skip.DOFade(0f, 1.5f).SetLoops(-1, LoopType.Yoyo);
    }

    public void SkipButton()
    {
        LoadingSceneController.LoadScene("TutorialBattle");
        DOTween.KillAll();
    }

    void FadeIn(TweenCallback onComplete)
    {
        fadePanel.color = new Color(0, 0, 0, 1); // ������ �˰� ����
        fadePanel.gameObject.SetActive(true);
        fadePanel.DOFade(0f, 1.5f).OnComplete(() =>
        {
            fadePanel.gameObject.SetActive(false);
            onComplete?.Invoke();
        });
    }

    void FadeOut(TweenCallback onComplete)
    {
        fadePanel.color = new Color(0, 0, 0, 0); // ������ �����ϰ� ����
        fadePanel.gameObject.SetActive(true);
        fadePanel.DOFade(1f, 1.5f).OnComplete(() =>
        {
            onComplete?.Invoke();
        });
    }
}
