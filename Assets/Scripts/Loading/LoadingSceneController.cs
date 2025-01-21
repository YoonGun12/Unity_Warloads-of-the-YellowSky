using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingSceneController : MonoBehaviour
{
    static string nextScene;

    [SerializeField]
    Image LoadingBar;

    [SerializeField]
    Text loadingText;
    public string[] loadingMessages;

    [SerializeField]
    public Image background;
    public Sprite[] backgroundImages;

    public static void LoadScene(string SceneName)
    {
        nextScene = SceneName;
        SceneManager.LoadScene("Loading");
    }
    private void Start()
    {
        SetRandomBackgroundImage();
        SetRandomLoadingMessage();
        StartCoroutine(LoadSceneProcess());
    }

    void SetRandomLoadingMessage()
    {
        int randomIndex = Random.Range(0, loadingMessages.Length);
        loadingText.text = loadingMessages[randomIndex];
    }
    void SetRandomBackgroundImage()
    {
        int randomIndex = Random.Range(0, backgroundImages.Length);
        background.sprite = backgroundImages[randomIndex];
    }

    IEnumerator LoadSceneProcess()
    {
        AsyncOperation op =  SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;

        float timer = 0f;
        while(!op.isDone)
        {
            yield return null;

            if (op.progress < 0.9f)
            {
                LoadingBar.fillAmount = op.progress;
            }
            else
            {
                timer += Time.unscaledDeltaTime;
                LoadingBar.fillAmount = Mathf.Lerp(0.9f, 1f, timer);
                if(LoadingBar.fillAmount >= 1f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }
}
