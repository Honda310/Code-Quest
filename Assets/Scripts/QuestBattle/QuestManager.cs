using UnityEngine;
using System.Collections.Generic; // ListやQueueを使うために必要

/// <summary>
/// 【問題管理マネージャー】
/// CSVから問題を読み込み、敵のカテゴリ設定に合わせて
/// ランダムな出題デッキ（山札）を作成するクラスです。
/// </summary>
public class QuestManager : MonoBehaviour
{
    // ゲーム内の全ての問題データを保持しておくリスト
    private List<QuestData> allQuests = new List<QuestData>();

    // 現在の戦闘で使用する「問題の山札」（キュー）
    // Queue（キュー）は「先に入れたものを先に出す」データ構造です
    private Queue<QuestData> currentDeck = new Queue<QuestData>();

    /// <summary>
    /// ゲーム起動時（GameManagerから）に呼ばれ、全ての問題CSVを読み込みます。
    /// </summary>
    public void LoadQuests()
    {
        // リストを一度クリア（重複読み込み防止）
        allQuests.Clear();

        // カテゴリごとにCSVファイルを読み込んでリストに追加します
        // ※Resources/Data/ フォルダ内のファイル名と一致させてください
        LoadFromCSV("Data/Variable", QuestCategory.Variable); // 変数
        LoadFromCSV("Data/IF", QuestCategory.If);             // IF文
        LoadFromCSV("Data/Loop", QuestCategory.Loop);         // 繰り返し
        LoadFromCSV("Data/Array", QuestCategory.Array);       // 配列

        Debug.Log($"[QuestManager] 問題ロード完了: 全 {allQuests.Count} 問");
    }

    /// <summary>
    /// 指定されたパスのCSVを読み込み、リストに追加する内部メソッド
    /// </summary>
    private void LoadFromCSV(string path, QuestCategory category)
    {
        // CSVReaderを使って文字列のリストとして読み込む
        List<string[]> csvData = CSVReader.Read(path);

        // 1行ずつ処理する
        foreach (string[] line in csvData)
        {
            // データ列が足りない行（空行など）はエラーになるのでスキップ
            // [0]:ID, [1]:問題文, [2]:正解コード
            if (line.Length < 3) continue;

            // 新しい問題データを作成
            QuestData newQuest = new QuestData(line[0], category, line[1], line[2]);

            // 全リストに追加
            allQuests.Add(newQuest);
        }
    }

    /// <summary>
    /// 敵が持っているカテゴリリストに基づいて、ランダムな出題デッキを作成します。
    /// （LINQを使わず、foreachとループで実装）
    /// </summary>
    /// <param name="targetCategories">敵が出題するカテゴリのリスト</param>
    public void CreateDeck(List<QuestCategory> targetCategories)
    {
        // 前の戦闘のデッキが残っていたら消す
        currentDeck.Clear();

        // 1. 今回使うカテゴリの問題だけを一時リストに集める
        List<QuestData> filteredList = new List<QuestData>();

        foreach (QuestData quest in allQuests)
        {
            // 問題のカテゴリが、敵のカテゴリリストに含まれているか確認
            if (targetCategories.Contains(quest.Category))
            {
                filteredList.Add(quest);
            }
        }

        // 対象の問題がない場合は警告を出して終了
        if (filteredList.Count == 0)
        {
            Debug.LogWarning("[QuestManager] 対象カテゴリの問題が見つかりません。デッキが空です。");
            return;
        }

        // 2. リストをシャッフルする（ランダムに入れ替える）
        // リストの後ろから順番に、ランダムな位置の要素と交換していく方法です
        for (int i = filteredList.Count - 1; i > 0; i--)
        {
            // 0 から i までのランダムな場所を選ぶ
            int randomIndex = UnityEngine.Random.Range(0, i + 1);

            // 要素を入れ替える (Swap)
            QuestData temp = filteredList[i];
            filteredList[i] = filteredList[randomIndex];
            filteredList[randomIndex] = temp;
        }

        // 3. シャッフルされた問題をデッキ（キュー）に積む
        foreach (QuestData quest in filteredList)
        {
            currentDeck.Enqueue(quest);
        }

        Debug.Log($"[QuestManager] デッキ作成完了: {currentDeck.Count}問");
    }

    /// <summary>
    /// デッキから次の問題を1つ取り出します。
    /// </summary>
    /// <returns>取り出した問題データ。デッキが空ならnull</returns>
    public QuestData GetNextQuestion()
    {
        // デッキに問題が残っているか確認
        if (currentDeck.Count > 0)
        {
            // 先頭から取り出して返す（取り出したデータはデッキから消える）
            return currentDeck.Dequeue();
        }

        // 問題切れの場合
        return null;
    }
}