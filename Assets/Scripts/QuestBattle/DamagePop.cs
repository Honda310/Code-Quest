using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class DamagePop : MonoBehaviour
{
    [SerializeField] Text PlayerFluctuationText;
    [SerializeField] Text NetoFluctuationText;
    [SerializeField] Text EnemyFluctuationText;
    [SerializeField] float duration = 1.3f;
    [SerializeField] AnimationCurve alphaCurve;

    void Awake()
    {
        if (alphaCurve == null || alphaCurve.length == 0)
        {
            alphaCurve = new AnimationCurve(
                new Keyframe(0f, 0f),
                new Keyframe(0.15f, 1f),
                new Keyframe(0.8f, 1f),
                new Keyframe(1.3f, 0f)
            );
        }
    }

    public void PlayerDamagePlay(int damage)
    {
        PlayerFluctuationText.text = damage.ToString();
        StartCoroutine(DamageAnimate(PlayerFluctuationText));
    }
    public void PlayerHealPlay(int heal)
    {
        PlayerFluctuationText.text = heal.ToString();
        StartCoroutine(HealAnimate(PlayerFluctuationText));
    }
    public void NetoDamagePlay(int damage)
    {
        NetoFluctuationText.text = damage.ToString();
        StartCoroutine(DamageAnimate(NetoFluctuationText));
    }
    public void NetoHealPlay(int heal)
    {
        NetoFluctuationText.text = heal.ToString();
        StartCoroutine(HealAnimate(NetoFluctuationText));
    }
    public void EnemyDpPlay(int damage)
    {
        EnemyFluctuationText.text = damage.ToString();
        StartCoroutine(DpDealAnimate(EnemyFluctuationText));
    }
    IEnumerator DamageAnimate(Text Damage)
    {
        float t = 0f;
        Color c = Color.red;

        while (t < duration)
        {
            float normalized = t / duration;
            c.a = alphaCurve.Evaluate(normalized);
            Damage.color = c;

            t += Time.deltaTime;
            yield return null;
        }
    }
    IEnumerator DpDealAnimate(Text Damage)
    {
        float t = 0f;
        Color c = new Color32(134, 104, 238, 255);

        while (t < duration)
        {
            float normalized = t / duration;
            c.a = alphaCurve.Evaluate(normalized);
            Damage.color = c;

            t += Time.deltaTime;
            yield return null;
        }
    }
    IEnumerator HealAnimate(Text Damage)
    {
        float t = 0f;
        Color c = Color.green;

        while (t < duration)
        {
            float normalized = t / duration;
            c.a = alphaCurve.Evaluate(normalized);
            Damage.color = c;

            t += Time.deltaTime;
            yield return null;
        }
    }
    public void TextReset()
    {
        PlayerFluctuationText.text = "";
        NetoFluctuationText.text = "";
        EnemyFluctuationText.text = "";
    }
}
