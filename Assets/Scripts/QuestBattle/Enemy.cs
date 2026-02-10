using UnityEngine;
using System.Collections.Generic;
using System;

/// <summary>
/// 【敵キャラクター】
/// ステータス管理と、画像の動的ロードを担当します。
/// </summary>
[RequireComponent(typeof(SpriteRenderer))] // SpriteRendererを必須にする
public class Enemy : MonoBehaviour
{
    public EnemyData Data { get; private set; }

    public int CurrentDP { get; set; }
    public int EnemyID;
    [NonSerialized] public int MaxDP;
    [NonSerialized] public int Atk;

    [Header("出題設定")]
    public List<QuestCategory> QuestionCategories;

    private SpriteRenderer spriteRenderer;
    [NonSerialized] public int Exp;
    [NonSerialized] public bool Defeated;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
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
        QuestionCategories = new List<QuestCategory>(data.Categories);

        // 画像のロードと設定
        LoadSprite(data.ImageFileName);
        Exp = data.Exp;
    }

    /// <summary>
    /// Resourcesフォルダから画像を読み込みます
    /// </summary>
    private void LoadSprite(string fileName)
    {
        if (string.IsNullOrEmpty(fileName)) return;

        string path = "Sprites/Enemies/" + fileName;

        Sprite loadedSprite = Resources.Load<Sprite>(path);

        if (loadedSprite != null)
        {
            spriteRenderer.sprite = loadedSprite;
        }
        else
        {
            Debug.LogWarning($"[Enemy] 画像が見つかりません: {path}");
        }
    }

    public void TakeDamage(int dmg)
    {
        CurrentDP += dmg;
    }
}