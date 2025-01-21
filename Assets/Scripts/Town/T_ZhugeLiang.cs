using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_ZhugeLiang : MonoBehaviour
{
    public Transform player;
    SpriteRenderer Spr;

    private void Awake()
    {
        Spr = GetComponent<SpriteRenderer>();

        BlinkEffect();
        StartHoverEffect();
    }

    private void Update()
    {
        if(player != null)
        {
            float direction = player.position.x - transform.position.x;

            Spr.flipX = direction < 0;
        }
    }

    void BlinkEffect()
    {
        Sequence blinkSequence = DOTween.Sequence()
            .Append(Spr.DOFade(0.3f, 1.5f))
            .Append(Spr.DOFade(1f, 1.5f))
            .SetLoops(-1, LoopType.Yoyo);
    }

    void StartHoverEffect()
    {
        transform.DOMoveY(transform.position.y + 0.1f, 1f)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo); // 위아래로 천천히 흔들림
    }
}
