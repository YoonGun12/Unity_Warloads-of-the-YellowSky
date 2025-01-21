using System.Collections;
using UnityEngine;
using TMPro;

public class BossNameEffect : MonoBehaviour
{
    public TextMeshProUGUI bossNameText;
    private string fullText = "인공장군\n장량";
    public float letterDelay;  // 글자가 나오는 속도 조절을 위한 지연 시간

    public void StartEffect()
    {
        bossNameText.text = ""; // 초기 상태에서 텍스트를 비웁니다.
        StartCoroutine(NameEffect());
    }

    IEnumerator NameEffect()
    {
        // 각 글자를 순차적으로 추가하면서 텍스트에 한 글자씩 표시
        for (int i = 0; i < fullText.Length; i++)
        {
            bossNameText.text += fullText[i];  // 한 글자씩 텍스트에 추가
            yield return new WaitForSeconds(letterDelay); // 각 글자 사이의 지연 시간
        }
    }
}
