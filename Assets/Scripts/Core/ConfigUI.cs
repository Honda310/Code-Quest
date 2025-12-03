using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 【設定画面】
/// 音量スライダーの操作を受け取り、AudioManagerに反映させます。
/// </summary>
public class ConfigUI : MonoBehaviour
{
    public Slider bgmSlider;
    public Slider seSlider;

    // スライダーの値が変わったら呼ばれる
    public void OnVolumeChanged()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetVolume(bgmSlider.value, seSlider.value);
        }
    }
}