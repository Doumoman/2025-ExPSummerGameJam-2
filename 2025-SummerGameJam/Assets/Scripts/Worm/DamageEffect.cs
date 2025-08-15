using DG.Tweening;
using TMPro;
using UnityEngine;

public class DamageEffect : MonoBehaviour
{
    public TextMeshProUGUI damageText;

    public float minDamage = 3f;
    public float maxDamage = 1000f;
    public float minFontSize = 100f;
    public float maxFontSize = 400f;

    public float growDuration = 0.5f;
    public float fadeDuration = 1.0f;

    public void ShowDamage(int damage)
    {
        if (damage <= 0) return;
        
        DOTween.Kill(damageText); // 이전 트윈 제거
        

        float t = Mathf.InverseLerp(minDamage, maxDamage, damage);
        float startSize = Mathf.Lerp(minFontSize, maxFontSize, t);
        float growSize = startSize * 1.3f;
        float endSize = startSize * 0.5f;

        damageText.text = damage.ToString();
        damageText.fontSize = startSize;
        var c = damageText.color; c.a = 1f; damageText.color = c;

        Sequence seq = DOTween.Sequence();

        // 0.5초 동안 폰트 크기 커지기
        seq.Append(
            DOTween.To(() => damageText.fontSize, x => damageText.fontSize = x, growSize, growDuration)
                .SetEase(Ease.OutBack)
        );

        // 1초 동안 폰트 크기 줄이기 + 페이드아웃 동시 진행
        seq.Append(
            DOTween.To(() => damageText.fontSize, x => damageText.fontSize = x, endSize, fadeDuration)
                .SetEase(Ease.InQuad)
        );
        seq.Join(damageText.DOFade(0f, fadeDuration));

        seq.OnComplete(() =>
        {
            Destroy(gameObject);
        });
    }
}
