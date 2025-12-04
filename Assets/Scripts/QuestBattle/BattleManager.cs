using UnityEngine;
using System.Collections;

/// <summary>
/// 【戦闘管理】
/// 敵IDに基づいてデータを取得し、戦闘をセットアップします。
/// </summary>
public class BattleManager : MonoBehaviour
{
    public Enemy currentEnemy; // シーン上の敵オブジェクト（プレハブインスタンス）
    private Player player;
    private Neto neto;
    private QuestData currentQuestion;

    private QuestManager questManager;
    private WriteProgramQuest checker;

    void Start()
    {
        questManager = GameManager.Instance.questManager;
        checker = GetComponent<WriteProgramQuest>();
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
            questManager.CreateDeck(currentEnemy.QuestionCategories);

            // 4. UI表示開始
            GameManager.Instance.uiManager.ToggleBattle(true);
            GameManager.Instance.uiManager.ShowLog($"{data.Name} が現れた！");

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
            GameManager.Instance.uiManager.UpdateBattleMessage($"問題:\n{currentQuestion.QuestionText}");
        }
        else
        {
            GameManager.Instance.uiManager.UpdateBattleMessage("問題切れ！");
        }
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
    }

    private void QuizCorrect()
    {
        GameManager.Instance.uiManager.ShowLog("正解！ 攻撃！");
        currentEnemy.TakeDamage(player.CurrentAtk);

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
        GameManager.Instance.uiManager.ShowLog("不正解...");
        StartCoroutine(EnemyTurn());
    }

    private IEnumerator EnemyTurn()
    {
        yield return new WaitForSeconds(1f);

        bool hitPlayer = Random.value > 0.5f;
        int dmg = currentEnemy.Atk;

        if (hitPlayer)
        {
            player.CurrentHP -= Mathf.Max(0, dmg - player.CurrentDef);
            GameManager.Instance.uiManager.ShowLog($"プレイヤーに {dmg} ダメージ");
        }
        else
        {
<<<<<<< HEAD
            neto.CurrentHP -= Mathf.Max(0, dmg - neto.Def);
            GameManager.Instance.uiManager.ShowLog($"ネトに {dmg} ダメージ");
=======
            int realDmg = Mathf.Max(0, dmg - neto.CurrentDef);
            neto.CurrentHP -= realDmg;
            GameManager.Instance.uiManager.ShowLog($"ネトに {realDmg} のダメージ！");
>>>>>>> honda
        }

        GameManager.Instance.uiManager.UpdateStatus(player, neto);

<<<<<<< HEAD
        if (player.CurrentHP <= 0 || neto.CurrentHP <= 0)
=======
        // 敗北判定
        if (player.CurrentHP <= 0 && neto.CurrentHP <= 0)
>>>>>>> honda
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
        yield return new WaitForSeconds(1f);
        GameManager.Instance.uiManager.ToggleBattle(false);

        if (win)
        {
            GameManager.Instance.uiManager.ShowLog("勝利！");
            // 敵を非表示にする、あるいは破壊するなどの処理
            // Destroy(currentEnemy.gameObject); 
            // ※再利用する場合は非アクティブ化だけにするなど調整してください
        }
        else
        {
            GameManager.Instance.uiManager.ShowLog("敗北...");
        }
    }
}