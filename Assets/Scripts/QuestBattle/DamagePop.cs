using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class DamagePop : MonoBehaviour
{
    [SerializeField] Text damageText;
    [SerializeField] float duration = 1.3f;
    [SerializeField] AnimationCurve alphaCurve;

    void Awake()
    {
        if (alphaCurve == null || alphaCurve.length == 0)
        {
            alphaCurve = new AnimationCurve(
                new Keyframe(0f, 0f),
                new Keyframe(0.2f, 1f),
                new Keyframe(0.8f, 1f),
                new Keyframe(1f, 0f)
            );
        }
    }

    public void Play(int damage)
    {
        damageText.text = damage.ToString();
        StartCoroutine(Animate());
    }

    IEnumerator Animate()
    {
        float t = 0f;
        Color c = damageText.color;

        while (t < duration)
        {
            float normalized = t / duration;
            c.a = alphaCurve.Evaluate(normalized);
            damageText.color = c;

            t += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}
