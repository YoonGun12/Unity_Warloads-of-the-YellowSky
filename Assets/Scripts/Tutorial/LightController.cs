using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightController : MonoBehaviour
{
    private Light2D lightComponent;

    private void Awake()
    {
        // Light2D ������Ʈ�� ������
        lightComponent = GetComponent<Light2D>();

        // Light2D ������Ʈ�� ������ ���� �޽��� ���
        if (lightComponent == null)
        {
            Debug.LogError("Light2D component is missing on " + gameObject.name);
        }
        else
        {
            // �ʱ� ���·� ������ ����
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
