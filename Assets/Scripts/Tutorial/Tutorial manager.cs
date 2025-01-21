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
    public Image fadePanel; //��Ʈ�� ���̵��� �ǳ�
    public GameObject dialogueUI; //��ȭâ
    public TextMeshProUGUI dialogueText; //��ȭâ �� ���
    public RectTransform dialogueCursor; //��ȭâ Ŀ��

    // Header: Zhuge Liang (NPC) Settings
    [Header("Zhuge Liang Settings")]
    public GameObject zhugeLiang; //������ ������Ʈ
    public Transform zhugeLiangAppearPos; //������ ������ġ

    // Header: Exclamation Mark Settings
    [Header("Exclamation Mark Settings")]
    public GameObject exclamationMark; //����ǥ ������Ʈ
    public Transform playerHeadPos; //����ǥ ������ġ
    public TextMeshProUGUI skip; //��ŵ��ư�ؽ�Ʈ

    // Header: Camera and Effects
    [Header("Camera & Effects")]
    public Camera mainCamera; //����ī�޶�
    public Vector3 dialogueCameraPos = new Vector3(16f, 0f, -10f); //�������� ��ȭ�� ī�޶� ��ġ
    public float cameraFollowSpeed = 2f; //ī�޶� �÷��̾ ����ٴϴ� �ӵ�
    
    [Header("Player & Movement")]
    public TutorialPlayerMove tutorialPlayerMove; //�÷��̾� �̵� ��ũ��Ʈ

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
        "�״�, �� ��翡 �̲��� �� ���� �����ε� �Ǿ��� ���̿�.\n�� �̰��� �Դ���, �� ���� ���� �Ǿ����� ���̿�.",
        "������ ���δ� ����� ������ �Ѿ ���� �� ���� �ε��ϴ� ���̿�.\n�״밡 �� ���� �̸� �͵� �׷��� ����̶� �� �� �ּ�.",
        "������ �ѳ���� ȥ���� ����� ��鸮��, ������ �鼺�� ���� �ӿ� �ּ�.",
        "�� ����� �׷��� �ô븦 �Ѿ� �̷��� �������� ��� ���̿�.\n�׵��� �״븦 �����߰�, �״��� ���� �Ǳ���Ͽ�.",
        "Ȳ������ ���� �����ϰ�, �������� õ�ϸ� ������Ű�� �ӹ��� ���� ���� ���� ���̿�.\n������ �״��� �����ϸ��� �ϼ�.",
        "���� ��� ���� �״뿡�� ����� ���̿�.\n�̸� �޾Ƶ鿩 �״��� ���� ����, ������ ���� ���� �����ÿ�."
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

            // �� �̵�, ũ�� �� ���� ��ȭ ����
            float travelDuration = 1.5f;
            light.transform.DOMove(playerTransform.position, travelDuration).SetEase(Ease.InOutSine);
            light.transform.DOScale(0.1f, travelDuration).SetEase(Ease.InQuad); // ���� �۾����鼭 ����Ǵ� ȿ��

            // Light2D�� ���� ���İ��� �����Ͽ� ���������� ȿ��
            var light2D = light.GetComponent<UnityEngine.Rendering.Universal.Light2D>();
            if (light2D != null)
            {
                Color initialColor = light2D.color;
                Color targetColor = new Color(initialColor.r, initialColor.g, initialColor.b, 0); // ���İ��� 0���� ����
                DOTween.To(() => light2D.color, x => light2D.color = x, targetColor, travelDuration).SetEase(Ease.InQuad);
            }

            // ������ ���� �Ϸ�� ������ ���
            if (i == statuePositions.Length - 1)
            {
                yield return new WaitForSeconds(travelDuration);
            }
            else
            {
                yield return new WaitForSeconds(0.5f); // ���� ���� ����ϴ� ������
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
