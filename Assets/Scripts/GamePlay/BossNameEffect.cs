using System.Collections;
using UnityEngine;
using TMPro;

public class BossNameEffect : MonoBehaviour
{
    public TextMeshProUGUI bossNameText;
    private string fullText = "�ΰ��屺\n�差";
    public float letterDelay;  // ���ڰ� ������ �ӵ� ������ ���� ���� �ð�

    public void StartEffect()
    {
        bossNameText.text = ""; // �ʱ� ���¿��� �ؽ�Ʈ�� ���ϴ�.
        StartCoroutine(NameEffect());
    }

    IEnumerator NameEffect()
    {
        // �� ���ڸ� ���������� �߰��ϸ鼭 �ؽ�Ʈ�� �� ���ھ� ǥ��
        for (int i = 0; i < fullText.Length; i++)
        {
            bossNameText.text += fullText[i];  // �� ���ھ� �ؽ�Ʈ�� �߰�
            yield return new WaitForSeconds(letterDelay); // �� ���� ������ ���� �ð�
        }
    }
}
