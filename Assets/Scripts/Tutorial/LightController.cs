using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightController : MonoBehaviour
{
    private Light2D lightComponent;

    private void Awake()
    {
        // Light2D 컴포넌트를 가져옴
        lightComponent = GetComponent<Light2D>();

        // Light2D 컴포넌트가 없으면 오류 메시지 출력
        if (lightComponent == null)
        {
            Debug.LogError("Light2D component is missing on " + gameObject.name);
        }
        else
        {
            // 초기 상태로 강도를 낮춤
            lightComponent.intensity = 0;
        }
    }

    public void ActivateLight()
    {
        if (lightComponent != null)
        {
            StartCoroutine(ExpandLight());
        }
        else
        {
            Debug.LogError("Light2D component is missing on " + gameObject.name);
        }
    }

    private IEnumerator ExpandLight()
    {
        float duration = 1f;
        float targetIntensity = 10f;

        float elapsed = 0;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            lightComponent.intensity = Mathf.Lerp(0, targetIntensity, elapsed / duration);
            yield return null;
        }
    }
}
