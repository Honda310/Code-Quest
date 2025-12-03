using UnityEngine;

/// <summary>
/// 【音響管理】
/// BGMや効果音の再生、音量調整を行います。
/// </summary>
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioSource bgmSource; // BGM用スピーカー
    [SerializeField] private AudioSource seSource;  // SE用スピーカー

    private void Awake()
    {
        // シングルトン化
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // BGM再生
    public void PlayBGM(AudioClip clip)
    {
        // 同じ曲なら再生し直さない
        if (bgmSource.clip == clip) return;

        bgmSource.clip = clip;
        bgmSource.Play();
    }

    // SE再生（一回鳴らす）
    public void PlaySE(AudioClip clip)
    {
        seSource.PlayOneShot(clip);
    }

    // 音量設定
    public void SetVolume(float bgmVol, float seVol)
    {
        bgmSource.volume = bgmVol;
        seSource.volume = seVol;
    }
}