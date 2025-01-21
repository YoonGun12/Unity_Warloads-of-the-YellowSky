using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Background : MonoBehaviour
{
    public Image Panel;
    float time = 0f;
    float F_time = 1f;
    bool isSkipped = false;

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; //Scene 로드 시 이벤트 등록
        Fade();
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Fade();
    }
    public void Fade()
    {
        StartCoroutine(FadeFlow());
    }

    IEnumerator FadeFlow()
    {
        Panel.gameObject.SetActive(true);
        time = 0f;
        Color alpha = Panel.color;

        while(alpha.a >0f && !isSkipped) //Fade In
        {
            time += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(1, 0, time);
            Panel.color = alpha;
            yield return null;
        }

        if(isSkipped)
        {
            alpha.a = 0;
            Panel.color = alpha;
        }

        Panel.gameObject.SetActive(false);
        yield return null;
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0)||Input.touchCount >0)
        {
            isSkipped = true;
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; //Scene 변경 시 이벤트 제거
    }
}
