using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    // Header: UI Elements
    [Header("UI Elements")]
    public Image fadePanel; //인트로 페이드인 판넬
    public GameObject dialogueUI; //대화창
    public TextMeshProUGUI dialogueText; //대화창 내 대사
    public RectTransform dialogueCursor; //대화창 커서

    // Header: Zhuge Liang (NPC) Settings
    [Header("Zhuge Liang Settings")]
    public GameObject zhugeLiang; //제갈량 오브젝트
    public Transform zhugeLiangAppearPos; //제갈량 등장위치

    // Header: Exclamation Mark Settings
    [Header("Exclamation Mark Settings")]
    public GameObject exclamationMark; //느낌표 오브젝트
    public Transform playerHeadPos; //느낌표 등장위치
    public TextMeshProUGUI skip; //스킵버튼텍스트

    // Header: Camera and Effects
    [Header("Camera & Effects")]
    public Camera mainCamera; //메인카메라
    public Vector3 dialogueCameraPos = new Vector3(16f, 0f, -10f); //제갈량과 대화시 카메라 위치
    public float cameraFollowSpeed = 2f; //카메라가 플레이어를 따라다니는 속도
    
    [Header("Player & Movement")]
    public TutorialPlayerMove tutorialPlayerMove; //플레이어 이동 스크립트

    [Header("Aura effects")]
    public GameObject lightPrefab; 
    public Transform[] statuePositions;
    public Transform playerTransform;
    private List<GameObject> lights = new List<GameObject>();
    public ParticleSystem auraEffect;

    // Private Variables    
    private int dialogueIndex = 0;
    private bool isTyping;
    private bool isDialogueActive = false;    
    private Vector3 targetCameraStopPos = new Vector3(29f, 0f, -10f);
    private List<string> dialogues = new List<string>
    {
        "그대, 이 사당에 이끌려 온 것이 스스로도 의아할 것이오.\n왜 이곳에 왔는지, 왜 나를 보게 되었는지 말이오.",
        "하지만 때로는 사람의 의지를 넘어선 힘이 그 길을 인도하는 법이오.\n그대가 이 곳에 이른 것도 그러한 운명이라 할 수 있소.",
        "지금의 한나라는 혼란과 위기로 흔들리고, 수많은 백성이 고통 속에 있소.",
        "이 사당은 그러한 시대를 넘어 미래의 영웅들이 잠든 곳이오.\n그들은 그대를 선택했고, 그대의 힘이 되기원하오.",
        "황건적의 난을 평정하고, 어지러운 천하를 안정시키는 임무는 결코 쉽지 않을 것이오.\n하지만 그대라면 가능하리라 믿소.",
        "이제 모든 힘이 그대에게 쏟아질 것이오.\n이를 받아들여 그대의 길을 열고, 세상을 구할 힘을 얻으시오."
    };
    private void Start()
    {
        BlinkSkip();
    }

    private void Update()
    {        
        // Handle touch input to skip typing
        if (isDialogueActive && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            if(isTyping)
            {
                CompleteDialogue();
            }
            else if (dialogueCursor.gameObject.activeSelf)
            {
                OnNextDialogue();
            }
            
        }
    }

    // Initialization Methods
    public void InitializeFadeIn()
    {
        isDialogueActive = false ;
        tutorialPlayerMove.MoveToCenter();
        fadePanel.color = new Color(0, 0, 0, 1);
        fadePanel.DOFade(0, 3).OnComplete(() =>
        {
            fadePanel.gameObject.SetActive(false);
            StartShakingEffect();
        });
        dialogueCursor.gameObject.SetActive(false);
    }

    // Screen Shake Effect and Zhuge Liang Appearance
    private void StartShakingEffect()
    {
        exclamationMark.SetActive(true);
        exclamationMark.transform.position = playerHeadPos.position;
        
        
        mainCamera.transform.DOShakePosition(3f, 2f, 10, 90, false, true).OnComplete(() =>
        {            
            exclamationMark.SetActive(false);
            ShowZhugeLiang();
        });
    }

    // Zhuge Liang's Appearance and Dialogue Start
    public void ShowZhugeLiang()
    {
        zhugeLiang.transform.position = zhugeLiangAppearPos.position;
        zhugeLiang.SetActive(true);
        SpriteRenderer zhugeSpr = zhugeLiang.GetComponent<SpriteRenderer>();
        zhugeSpr.color = new Color(zhugeSpr.color.r, zhugeSpr.color.g, zhugeSpr.color.b, 0);

        mainCamera.transform.DOMove(dialogueCameraPos, 1.5f).SetEase(Ease.InOutSine);
        zhugeSpr.DOFade(1, 2f).OnComplete(() => 
        {
            isDialogueActive = true;
            ShowDialogue();
        });
    }

    private void ShowDialogue()
    {
        dialogueUI.SetActive(true);
        StartCoroutine(TypeDialogue(dialogues[dialogueIndex]));
    }
    private IEnumerator TypeDialogue(string sentence)
    {
        dialogueText.text = "";
        dialogueCursor.gameObject.SetActive(false);
        isTyping = true;

        foreach (char letter in sentence)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.05f);

            if (!isTyping) yield break;
        }
        isTyping = false;
        ActivateDialogueCursor();
    }

    private void CompleteDialogue()
    {
        isTyping = false;
        dialogueText.text = dialogues[dialogueIndex];
        ActivateDialogueCursor();
    }

    private void ActivateDialogueCursor()
    {
        dialogueCursor.DOKill();
        dialogueCursor.anchoredPosition = new Vector2(dialogueCursor.anchoredPosition.x, 80);

        dialogueCursor.gameObject.SetActive(true);
        dialogueCursor.DOAnchorPosY(-1.5f, 0.5f).SetLoops(-1, LoopType.Yoyo);
    }

    // Progress to Next Dialogue or End Dialogue
    public void OnNextDialogue()
    {
        dialogueIndex++;
        dialogueCursor.DOKill();
        if (dialogueIndex < dialogues.Count)
        {
            ShowDialogue();
        }
        else
        {
            EndDialogue();
        }
    }
    private void EndDialogue()
    {
        isDialogueActive = false;
        dialogueUI.SetActive(false);

        mainCamera.transform.DOMove(new Vector3(playerTransform.position.x, mainCamera.transform.position.y, mainCamera.transform.position.z), 1.5f).SetEase(Ease.InOutSine);
        StartCoroutine(StartEnergyGatheringEffect());
    }

    IEnumerator StartEnergyGatheringEffect()
    {
        for (int i = 0; i < statuePositions.Length; i++)
        {
            Transform statuePos = statuePositions[i];
            GameObject light = Instantiate(lightPrefab, statuePos.position, Quaternion.identity);
            lights.Add(light);

            LightController lightController = light.GetComponent<LightController>();
            if (lightController != null)
            {
                lightController.ActivateLight();
            }

            // 빛 이동, 크기 및 투명도 변화 설정
            float travelDuration = 1.5f;
            light.transform.DOMove(playerTransform.position, travelDuration).SetEase(Ease.InOutSine);
            light.transform.DOScale(0.1f, travelDuration).SetEase(Ease.InQuad); // 빛이 작아지면서 흡수되는 효과

            // Light2D의 색상 알파값을 조절하여 투명해지는 효과
            var light2D = light.GetComponent<UnityEngine.Rendering.Universal.Light2D>();
            if (light2D != null)
            {
                Color initialColor = light2D.color;
                Color targetColor = new Color(initialColor.r, initialColor.g, initialColor.b, 0); // 알파값을 0으로 설정
                DOTween.To(() => light2D.color, x => light2D.color = x, targetColor, travelDuration).SetEase(Ease.InQuad);
            }

            // 마지막 빛만 완료될 때까지 대기
            if (i == statuePositions.Length - 1)
            {
                yield return new WaitForSeconds(travelDuration);
            }
            else
            {
                yield return new WaitForSeconds(0.5f); // 다음 빛이 출발하는 딜레이
            }
        }

        ApplyAuraEffect();
    }

    void ApplyAuraEffect()
    {
        if (auraEffect != null)
        {
            auraEffect.gameObject.SetActive(true);
            auraEffect.Play();
        }

        foreach (GameObject light in lights)
        {
            light.SetActive(false);
        }

        Invoke("ShowEnemyApproach", 1f);
    }

    public void ShowEnemyApproach()
    {
        //audioSource.PlayOneShot(enemyApporachSound);
        mainCamera.transform.DOJump(mainCamera.transform.position, 0.5f, 2, 1.5f)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
        Invoke("ShowPlayerRaction", 0.5f);
    }

    private void ShowPlayerRaction()
    {
        exclamationMark.SetActive(true);
        exclamationMark.transform.position = playerHeadPos.position;

        Invoke("MovePlayerOutside", 2.0f);
    }

    private void MovePlayerOutside()
    {
        exclamationMark.SetActive(false);
        tutorialPlayerMove.MoveToExit();

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

   
      
}
