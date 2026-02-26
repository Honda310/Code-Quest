using UnityEngine;
using System.Collections.Generic;
using System;

/// <summary>
/// 【敵キャラクター】
/// ステータス管理と、画像の動的ロードを担当します。
/// </summary>
[RequireComponent(typeof(SpriteRenderer))] 
public class Enemy : MonoBehaviour
{
    public EnemyData Data { get; private set; }

    public int CurrentDP { get; set; }
    public int EnemyID;
    [NonSerialized] public int MaxDP;
    [NonSerialized] public int Atk;
    [NonSerialized] public string EnemyName;

    [Header("出題設定")]
    public QuestCategory QuestionCategories;

    [NonSerialized] public int Exp;
    [NonSerialized] public bool Defeated;
    private void Awake()
    {
        Setup(GameManager.Instance.dataManager.EnemyMaster[EnemyID]);
    }

    /// <summary>
    /// DataManagerのデータを使って敵を初期化します
    /// </summary>
    public void Setup(EnemyData data)
    {
        // 基本ステータスのコピー
        EnemyID = data.ID;
        MaxDP = data.MaxDP;
        Atk = data.Atk;
        CurrentDP = 0;

        // カテゴリ設定のコピー
        QuestionCategories = data.Categories;

        // 画像のロードと設定
        Exp = data.Exp;
        EnemyName = data.Name;
    }
    public void TakeDamage(int dmg)
    {
        CurrentDP += dmg;
    }
}