using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 【戦闘管理】
/// 敵IDに基づいてデータを取得し、戦闘をセットアップします。
/// </summary>
public class BattleManager : MonoBehaviour
{
    [Header("Enemy Prefab")]
    [SerializeField] private Enemy enemyPrefab;
    [Header("Spawn Point")]
    [SerializeField] private Transform enemySpawnPoint;
    public Enemy currentEnemy; // シーン上の敵オブジェクト（プレハブインスタンス）
    private Player player;
    private Neto neto;
    private QuestData currentQuestion;
    List<QuestCategory> categories = new List<QuestCategory>();
    private QuestManager questManager;
    private MultipleChoiceQuest checker;

    void Start()
    {
        questManager = GameManager.Instance.questManager;
        checker = GetComponent<MultipleChoiceQuest>();
    }

    /// <summary>
    /// 戦闘開始処理
    /// </summary>
    /// <param name="enemyId">CSVで定義された敵ID</param>
    public void StartBattle(Player p, Neto n, int enemyId)
    {
        player = p;
        neto = n;

        // 1. 敵データの取得
        Debug.Log("SetEnemyData(ID："+enemyId+")");
        EnemyData data = GameManager.Instance.dataManager.GetEnemyById(enemyId);

        if (data == null)
        {
            Debug.LogError($"敵データが見つかりません: ID {enemyId}");
            return;
        }

        // 2. 敵オブジェクトのセットアップ（ステータス・画像）
        // ※currentEnemyがシーンに既に存在するか、生成済みであることを前提としています
        if (currentEnemy != null)
        {
            currentEnemy.Setup(data);

            // 3. 敵のカテゴリに合わせて問題デッキ作成
            categories.Add(QuestCategory.Variable_AdditionAndSubtraction);
            categories.Add(QuestCategory.Variable_AdditionAndSubtraction);
            categories.Add(QuestCategory.Variable_AdditionAndSubtraction);
            questManager.CreateDeck(categories);

            // 4. UI表示開始
            GameManager.Instance.SetMode(GameManager.GameMode.Battle);
            Time.timeScale = 0f;
            //GameManager.Instance.uiManager.ShowLog($"{data.Name} が現れた！");

            NextTurn();
        }
        else
        {
            Debug.LogError("BattleManagerにEnemyオブジェクトが割り当てられていません。");
        }
    }

    public void NextTurn()
    {
        currentQuestion = questManager.GetNextQuestion();
        if (currentQuestion != null)
        {
            //GameManager.Instance.uiManager.UpdateBattleMessage($"問題:\n{currentQuestion.QuestionText}",currentQuestion.Options);
        }
        else
        {
            //GameManager.Instance.uiManager.UpdateBattleMessage("問題切れ！");
        }
        GameManager.Instance.SetBattleTime(GameManager.BattleTag.TurnStart);
    }

    public void OnSubmitAnswer(string code)
    {
        if (currentQuestion == null) return;
        if (checker.CheckAnswer(code, currentQuestion))
        {
            QuizCorrect();
        }
        else
        {
            QuizIncorrect();
        }
        NextTurn();
    }

    private void QuizCorrect()
    {
        //GameManager.Instance.uiManager.ShowLog("正解！ 攻撃！");
        currentEnemy.TakeDamage(player.CurrentAtk);
        Debug.Log(currentEnemy.CurrentDP);
        Debug.Log(currentEnemy.MaxDP);
        if (currentEnemy.CurrentDP >= currentEnemy.MaxDP)
        {
            StartCoroutine(EndBattle(true));
        }
        else
        {
            StartCoroutine(EnemyTurn());
        }
    }

    private void QuizIncorrect()
    {
        //GameManager.Instance.uiManager.ShowLog("不正解...");
        StartCoroutine(EnemyTurn());
    }

    private IEnumerator EnemyTurn()
    {
        yield return new WaitForSecondsRealtime(0.5f);

        bool hitPlayer = Random.value > 0.5f;
        int dmg = currentEnemy.Atk;

        if (hitPlayer)
        {
            int realDmg = Mathf.Max(0, dmg - neto.CurrentDef);
            player.CurrentHP -= realDmg;
            //GameManager.Instance.uiManager.ShowLog($"プレイヤーに {realDmg} ダメージ");
        }
        else
        {
            int realDmg = Mathf.Max(0, dmg - neto.CurrentDef);
            neto.CurrentHP -= realDmg;
            //GameManager.Instance.uiManager.ShowLog($"ネトに {realDmg} のダメージ！");
        }

        GameManager.Instance.SetBattleTime(GameManager.BattleTag.TurnEnd);

        // 敗北判定
        if (player.CurrentHP <= 0 && neto.CurrentHP <= 0)
        {
            StartCoroutine(EndBattle(false));
        }
        else
        {
            NextTurn();
        }
    }

    private IEnumerator EndBattle(bool win)
    {
        yield return new WaitForSecondsRealtime(1f);

        GameManager.Instance.SetMode(GameManager.GameMode.Field);

        if (win)
        {
            Destroy(currentEnemy.gameObject);
            // Destroy(currentEnemy.gameObject); 
            Time.timeScale = 1f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

}