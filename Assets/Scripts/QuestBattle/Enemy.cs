using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 【敵キャラクター】
/// ステータス管理と、画像の動的ロードを担当します。
/// </summary>
[RequireComponent(typeof(SpriteRenderer))] // SpriteRendererを必須にする
public class Enemy : MonoBehaviour
{
    public int EnemyID;
    public int MaxDP;
    public int CurrentDP;
    public int Atk;

    [Header("出題設定")]
    public List<QuestCategory> QuestionCategories;

    private SpriteRenderer spriteRenderer;

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
        gameObject.name = data.Name; // オブジェクト名も変更しておくと分かりやすい
        MaxDP = data.MaxDP;
        Atk = data.Atk;
        CurrentDP = 0;

        // カテゴリ設定のコピー
        QuestionCategories = new List<QuestCategory>(data.Categories);

        // 画像のロードと設定
        LoadSprite(data.ImageFileName);
    }

    /// <summary>
    /// Resourcesフォルダから画像を読み込みます
    /// </summary>
    private void LoadSprite(string fileName)
    {
        if (string.IsNullOrEmpty(fileName)) return;

        // パス: Assets/Resources/Sprites/Enemies/ファイル名
        // ※フォルダがない場合は作成してください
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