using UnityEngine;

/// <summary>
/// 【ネト】
/// パートナーキャラクターのクラスです。
/// </summary>
public class Neto : MonoBehaviour
{
    public int MaxHP = 80;
    public int CurrentHP;
    public int Def = 5;

    private void Start()
    {
        CurrentHP = MaxHP;
    }

    // ヒント機能などがあればここに実装
    public void SpeakHint()
    {
        // 未実装
    }
}