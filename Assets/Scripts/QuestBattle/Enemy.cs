using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 【敵キャラクター】
/// ステータスと、出題する問題カテゴリの設定を持ちます。
/// </summary>
public class Enemy : MonoBehaviour
{
    public int EnemyID;
    public int MaxDP;     // 最大耐久値（バグの量）
    public int CurrentDP; // 現在の進捗
    public int Atk;

    [Header("出題設定")]
    public List<QuestCategory> QuestionCategories;

    // ダメージ（進捗）を受ける
    public void TakeDamage(int dmg)
    {
        CurrentDP += dmg;
    }
}