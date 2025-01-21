using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialGuideManager : MonoBehaviour
{
    public RectTransform guideTextBox;
    public TextMeshProUGUI guideText;
    public RectTransform highlightRect;
    public Image fadeBackground;

    private int currentStep = 0;
    private bool isGuideOn = false;
    private List<string> guideMessages = new List<string>
    {
        "<����ġ>\n�״밡 ���� �����߸� ������ ����ġ�� �׿�����. ����ġ �ٰ� ���� ���� �״��� ���� ���� �� ������ ���̿�.",
        "<��� ������>\n��� �������� ���� ��� ������ ����Ͽ� ���� ���ؿ� ������ ������ �������� ȿ���� �ҷ�����.\n����, �ð��� �帣�� ������ �پ��� �������� �� ���� �� ��θӸ��� ��Ÿ���� ���Ӿ��� ���� �����߸��� �������� �����ϴ� ���� �¸��� ���谡 �� ���̿�.",
        "<����>\n�״��� ������ ������ ��Ÿ���� ���̿�. \n���� �����߷� ����ġ�� �װ�, �� �ܰ� �� ���� ���� �����Ͻÿ�.",
        "<�ð�>\n�ð��� �帧�� ���� �� ���� ������ ��������, �״��� �߰����� �����ؾ� �� ���̿�.",
        "<�� óġ��>\n�״밡 óġ�� ������ ���� ���ڼ�. \n������ ������ �״��� ������ �� ���̿�.",
        "<��ų ����>\n�״밡 ȹ���� ����� ������ ���⿡ ǥ�õǴ�, �� ������ �ָ��� ���ÿ�.",
        "<ü�� ��>\n�״��� ������̿�. \n�̸� ������ ���忡�� �������� �� ���̴� �����Ͻÿ�.",
        "<�̵� ���>\n���̽�ƽ�� ����Ͽ� �״��� ���� �����̽ÿ�. \n���� ������ ���ϰ�, ��ƴ�� ��� ġ�� ���� �߿��Ͽ�.",
        "<����ġ ����>\n���� �����߸��� ����ġ ������ ���� ���̿�. \n�̸� ��� ������ ����� �����ÿ�. \n����� ���� ���̴� �� ������ ������ �� ����.",
        "<������>\n�״��� ���� �����Ͽ� �� ���� ���⳪ �ɷ��� ���� ������ ã�ƿԼ�!\n�⺻ ����� �ƴ϶�, �״� �ȿ� ���� �ִ� ������� ���� ���� ����� �巯�� ���̿�.\n�־��� �� ���� �� �ϳ��� �����Ͽ� �״��� ���⸦ ���� �� ��ī�Ӱ� �����ÿ�.",
        "<���� ����>\n������ ����� �巯�¼�!\n�̵��� �鼺�� ���뿡 ���߸� ������ ��θӸ��̿�. \n� �׵��� ������ �鼺���� �����ϰ�, ������ �����Ͻÿ�.",
        "<���� ������>\n������ ��ġ�� ������ ǥ�õ� ���̴�, �� �����̸� ���󰡼� �����Ͻÿ�. \n�׸� ���� ���ư��ÿ�.",
        "<�¸� ����>\n�¸��� �ܼ��� ���� �����߸��� �Ϳ��� ������ �ʼ�.\n�״��� ��ǥ�� �� ȥ�������� ���忡�� ��Ƴ�����, ������ óġ�ϰ� ���� �׾� �鼺�鿡�� ��ȭ�� ��ã���ִ� ���̿�.\n��� �״��� ���ǿ� ��ܷ¿� �޷� �ּ�."

    };

    private bool experienceCollected = false;
    private bool levelUpShown = false;
    private bool bossSpawnGuideShown = false;
    private bool bossDefeatedGuideShown = false;

    private void Start()
    {
        ShowGuideStep(0);
    }

    private void Update()
    {
        if (isGuideOn && (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))) // ��ġ�� Ŭ�� �Է�
        {
            NextGuideStep();
        }

        if(GameManager.instance.kill == 1 && !experienceCollected)
        {
            experienceCollected=true;
            ShowGuideStep(8); //����ġ ����
        }

        if(GameManager.instance.level == 1 && !levelUpShown)
        {
            levelUpShown=true;
            ShowGuideStep(9); //������
        }

        if(GameManager.instance.bossSpawned == true && !bossSpawnGuideShown)
        {
            bossSpawnGuideShown=true;
            ShowGuideStep(10); //��������
        }

        if(GameManager.instance.bossDefeated == true && !bossDefeatedGuideShown)
        {
            bossDefeatedGuideShown=true;
            ShowGuideStep(12);//�¸�����
        }
    }

    private void NextGuideStep()
    {
        if(currentStep == 9 || currentStep == 8)
        {
            EndGuide();
            return;
        }

        currentStep++;

        if (currentStep <= 7 )
        {
            ShowGuideStep(currentStep);
        }        
        else if(currentStep == 10 && bossSpawnGuideShown)
        {
            ShowGuideStep(11);
        }
        else
        {
            EndGuide();
        }
    }

    public void ShowGuideStep(int stepIndex)
    {
        currentStep = stepIndex;
        guideText.text = guideMessages[stepIndex];
        guideTextBox.gameObject.SetActive(true);
        fadeBackground.gameObject.SetActive(true);
        isGuideOn = true;
        GameManager.instance.Stop();

        switch (currentStep)
        {
            case 0://����ġ��
                SetHighlight(new Vector2(0, 806), new Vector2(991, 58));  // ��ġ�� ũ�� ����
                guideTextBox.localPosition = new Vector2(145, 180);
                break;
            case 1://��� ������
                SetHighlight(new Vector2(0, 748), new Vector2(625, 58));
                guideTextBox.localPosition = new Vector2(145, 180);
                break;
            case 2://����
                SetHighlight(new Vector2(-384, 683), new Vector2(222, 100));
                guideTextBox.localPosition = new Vector2(145, 180);
                break;
            case 3://�ð�
                SetHighlight(new Vector2(0, 685), new Vector2(222, 100));
                guideTextBox.localPosition = new Vector2(145, 180);
                break;
            case 4://�� óġ��
                SetHighlight(new Vector2(-345, 571), new Vector2(300, 115));
                guideTextBox.localPosition = new Vector2(145, 180);
                break;
            case 5://��ų����
                SetHighlight(new Vector2(-426, 178), new Vector2(137, 695));
                guideTextBox.localPosition = new Vector2(145, -70);
                break;
            case 6://ü�� ��
                SetHighlight(new Vector2(0, -89), new Vector2(138, 94));
                guideTextBox.localPosition = new Vector2(0, -500);
                break;
            case 7://�̵����
                SetHighlight(new Vector2(0, -10), new Vector2(991, 1670));
                guideTextBox.localPosition = new Vector2(0, -750);
                break;
            case 8://����ġ ����
                SetHighlight(new Vector2(0, -10), new Vector2(991, 1670));
                guideTextBox.localPosition = new Vector2(0, -750);
                break;
            case 9://������
                SetHighlight(new Vector2(0, -10), new Vector2(991, 1670));
                guideTextBox.localPosition = new Vector2(0, -750);
                break;
            case 10://���� ����
                SetHighlight(new Vector2(0, -10), new Vector2(991, 1670));
                guideTextBox.localPosition = new Vector2(0, -750);
                break;
            case 11://���� ������
                SetHighlight(new Vector2(0, 0), new Vector2(150, 150));
                guideTextBox.localPosition = new Vector2(0, -500);
                break;
            case 12://�¸�����
                SetHighlight(new Vector2(0, -10), new Vector2(991, 1670));
                guideTextBox.localPosition = new Vector2(0, -750);
                break;

        }

    }

    private void EndGuide()
    {
        guideTextBox.gameObject.SetActive(false);
        highlightRect.gameObject.SetActive(false);
        fadeBackground.gameObject.SetActive(false);
        isGuideOn = false;   
        if(currentStep != 9)
        {
            GameManager.instance.Resume();
        }
        
    }

    public void SetHighlight(Vector2 position, Vector2 size)
    {       
        highlightRect.gameObject.SetActive(true);

        highlightRect.anchorMin = new Vector2(0.5f, 0.5f);
        highlightRect.anchorMax = new Vector2(0.5f, 0.5f);
        highlightRect.pivot = new Vector2(0.5f, 0.5f);

        highlightRect.sizeDelta = size;     // ���ϴ� ũ�� ����

        highlightRect.localPosition = position;
    }
    
}
