using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MapNamePopUp : MonoBehaviour
{
    [SerializeField] Text MapNameJpText;
    [SerializeField] Text MapNameEnText;
    [SerializeField] GameObject MapNamePopUpPanel;
    private float duration = 3.0f;
    [SerializeField] private AnimationCurve MapPanelCurve;
    [SerializeField] private AnimationCurve MapNameCurve;

    void Awake()
    {
        if (MapPanelCurve == null || MapPanelCurve.length == 0)
        {
            MapPanelCurve = new AnimationCurve(
                new Keyframe(1f, 1f),
                new Keyframe(2.0f, 1f),
                new Keyframe(3.0f, 0f)
            );
        }
        if (MapNameCurve == null || MapNameCurve.length == 0)
        {
            MapNameCurve = new AnimationCurve(
                new Keyframe(0f, 0f),
                new Keyframe(0.5f, 1f),
                new Keyframe(2.0f, 1f),
                new Keyframe(3.0f, 0f)
            );
        }
    }
    public void MapNamePopUP(string MapNameEn)
    {
        string MapNameJp = GameManager.Instance.mapManager.MapNameConvertor(MapNameEn);
        MapNameJpText.text= MapNameJp;
        MapNameEnText.text= MapNameEn;
        MapNamePopUpPanel.SetActive(true);
        StartCoroutine(MapPanelPopAnimate());
        StartCoroutine(MapNameEnPopAnimate());
        StartCoroutine(MapNameJpPopAnimate());
        StartCoroutine(MapPanelPopAndDelete());
    }
    IEnumerator MapPanelPopAnimate()
    {
        float t = 0f;
        Color c = Color.black;

        while (t < duration)
        {
            float normalized = t / duration;
            c.a = MapPanelCurve.Evaluate(normalized);
            MapNamePopUpPanel.GetComponent<Image>().color = c;

            t += Time.deltaTime;
            yield return null;
        }
    }
    IEnumerator MapNameEnPopAnimate()
    {
        float t = 0f;
        Color c = Color.white;

        while (t < duration)
        {
            float normalized = t / duration;
            c.a = MapNameCurve.Evaluate(normalized);
            MapNameEnText.color = c;

            t += Time.deltaTime;
            yield return null;
        }
    }
    IEnumerator MapNameJpPopAnimate()
    {
        float t = 0f;
        Color c = Color.white;

        while (t < duration)
        {
            float normalized = t / duration;
            c.a = MapNameCurve.Evaluate(normalized);
            MapNameJpText.color = c;

            t += Time.deltaTime;
            yield return null;
        }
    }
    IEnumerator MapPanelPopAndDelete()
    {
        yield return new WaitForSeconds(4.0f);
        MapNamePopUpPanel.SetActive(false);
    }
}
