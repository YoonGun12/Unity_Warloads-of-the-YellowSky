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
        "<경험치>\n그대가 적을 쓰러뜨릴 때마다 경험치가 쌓여가오. 경험치 바가 가득 차면 그대의 힘이 한층 더 강해질 것이오.",
        "<사기 게이지>\n사기 게이지는 적을 무찌를 때마다 상승하여 일정 수준에 오르면 적들이 강해지는 효과를 불러오네.\n또한, 시간이 흐르면 서서히 줄어들고 게이지가 다 차면 적 우두머리가 나타나니 끊임없이 적을 쓰러뜨리며 게이지를 조절하는 것이 승리의 열쇠가 될 것이오.",
        "<레벨>\n그대의 경지와 성장을 나타내는 것이오. \n적을 쓰러뜨려 경험치를 쌓고, 한 단계 더 높은 힘에 도달하시오.",
        "<시간>\n시간이 흐름에 따라 더 강한 적들이 몰려오니, 그대의 발걸음을 재촉해야 할 것이오.",
        "<적 처치수>\n그대가 처치한 적들의 수를 세겠소. \n난세를 잠재우는 그대의 흔적이 될 것이오.",
        "<스킬 레벨>\n그대가 획득한 무기와 장비들이 여기에 표시되니, 그 발전을 주목해 보시오.",
        "<체력 바>\n그대의 생명력이오. \n이를 잃으면 전장에서 물러나야 할 것이니 주의하시오.",
        "<이동 방법>\n조이스틱을 사용하여 그대의 몸을 움직이시오. \n적의 공격을 피하고, 빈틈을 노려 치는 것이 중요하오.",
        "<경험치 수집>\n적을 쓰러뜨리면 경험치 구슬이 남을 것이오. \n이를 모아 성장의 기반을 쌓으시오. \n충분한 경험 없이는 이 난세를 평정할 수 없소.",
        "<레벨업>\n그대의 힘이 성장하여 더 강한 무기나 능력을 얻을 순간이 찾아왔소!\n기본 무기뿐 아니라, 그대 안에 잠들어 있는 명장들의 힘도 가끔 모습을 드러낼 것이오.\n주어진 세 가지 중 하나를 선택하여 그대의 무기를 한층 더 날카롭게 벼르시오.",
        "<보스 등장>\n보스가 모습을 드러냈소!\n이들은 백성을 고통에 빠뜨린 무리의 우두머리이오. \n어서 그들을 물리쳐 백성들을 구제하고, 난세를 평정하시오.",
        "<보스 길잡이>\n보스의 위치가 지도에 표시될 것이니, 이 길잡이를 따라가서 마주하시오. \n그를 향해 나아가시오.",
        "<승리 조건>\n승리는 단순히 적을 쓰러뜨리는 것에서 끝나지 않소.\n그대의 목표는 이 혼란스러운 전장에서 살아남으며, 강적을 처치하고 명성을 쌓아 백성들에게 평화를 되찾아주는 것이오.\n모두 그대의 결의와 결단력에 달려 있소."

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
        if (isGuideOn && (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))) // 터치나 클릭 입력
        {
            NextGuideStep();
        }

        if(GameManager.instance.kill == 1 && !experienceCollected)
        {
            experienceCollected=true;
            ShowGuideStep(8); //경험치 수집
        }

        if(GameManager.instance.level == 1 && !levelUpShown)
        {
            levelUpShown=true;
            ShowGuideStep(9); //레벨업
        }

        if(GameManager.instance.bossSpawned == true && !bossSpawnGuideShown)
        {
            bossSpawnGuideShown=true;
            ShowGuideStep(10); //보스등장
        }

        if(GameManager.instance.bossDefeated == true && !bossDefeatedGuideShown)
        {
            bossDefeatedGuideShown=true;
            ShowGuideStep(12);//승리조건
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
            case 0://경험치바
                SetHighlight(new Vector2(0, 806), new Vector2(991, 58));  // 위치와 크기 설정
                guideTextBox.localPosition = new Vector2(145, 180);
                break;
            case 1://사기 게이지
                SetHighlight(new Vector2(0, 748), new Vector2(625, 58));
                guideTextBox.localPosition = new Vector2(145, 180);
                break;
            case 2://레벨
                SetHighlight(new Vector2(-384, 683), new Vector2(222, 100));
                guideTextBox.localPosition = new Vector2(145, 180);
                break;
            case 3://시간
                SetHighlight(new Vector2(0, 685), new Vector2(222, 100));
                guideTextBox.localPosition = new Vector2(145, 180);
                break;
            case 4://적 처치수
                SetHighlight(new Vector2(-345, 571), new Vector2(300, 115));
                guideTextBox.localPosition = new Vector2(145, 180);
                break;
            case 5://스킬레벨
                SetHighlight(new Vector2(-426, 178), new Vector2(137, 695));
                guideTextBox.localPosition = new Vector2(145, -70);
                break;
            case 6://체력 바
                SetHighlight(new Vector2(0, -89), new Vector2(138, 94));
                guideTextBox.localPosition = new Vector2(0, -500);
                break;
            case 7://이동방법
                SetHighlight(new Vector2(0, -10), new Vector2(991, 1670));
                guideTextBox.localPosition = new Vector2(0, -750);
                break;
            case 8://경험치 수집
                SetHighlight(new Vector2(0, -10), new Vector2(991, 1670));
                guideTextBox.localPosition = new Vector2(0, -750);
                break;
            case 9://레벨업
                SetHighlight(new Vector2(0, -10), new Vector2(991, 1670));
                guideTextBox.localPosition = new Vector2(0, -750);
                break;
            case 10://보스 등장
                SetHighlight(new Vector2(0, -10), new Vector2(991, 1670));
                guideTextBox.localPosition = new Vector2(0, -750);
                break;
            case 11://보스 길잡이
                SetHighlight(new Vector2(0, 0), new Vector2(150, 150));
                guideTextBox.localPosition = new Vector2(0, -500);
                break;
            case 12://승리조건
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

        highlightRect.sizeDelta = size;     // 원하는 크기 설정

        highlightRect.localPosition = position;
    }
    
}
