using UnityEngine;
using System.Collections;

/// <summary>
/// 【戦闘管理】
/// 敵とのエンカウントから戦闘終了までを管理するクラスです。
/// </summary>
public class BattleManager : MonoBehaviour
{
    public Enemy currentEnemy;
    private Player player;
    private Neto neto;
    private QuestData currentQuestion;

    private QuestManager questManager;
    private WriteProgramQuest checker;

    private void Start()
    {
        questManager = GameManager.Instance.questManager;
        checker = GetComponent<WriteProgramQuest>();
    }

    /// <summary>
    /// 戦闘を開始する
    /// </summary>
    public void StartBattle(Player p, Neto n, Enemy e)
    {
        player = p;
        neto = n;
        currentEnemy = e;

        // 敵のカテゴリに合わせて問題デッキを作成
        questManager.CreateDeck(e.QuestionCategories);

        GameManager.Instance.uiManager.ToggleBattle(true);
        GameManager.Instance.uiManager.ShowLog($"{e.name} が現れた！");

        NextTurn();
    }

    /// <summary>
    /// 次のターン（問題出題）へ
    /// </summary>
    public void NextTurn()
    {
        currentQuestion = questManager.GetNextQuestion();

        if (currentQuestion != null)
        {
            GameManager.Instance.uiManager.UpdateBattleMessage($"問題:\n{currentQuestion.QuestionText}");
        }
        else
        {
            GameManager.Instance.uiManager.UpdateBattleMessage("問題切れ！デッキを再生成します（仮）");
        }
    }

    /// <summary>
    /// 解答が提出されたときの処理
    /// </summary>
    public void OnSubmitAnswer(string code)
    {
        if (currentQuestion == null) return;

        // 正誤判定
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
        GameManager.Instance.uiManager.ShowLog("正解！ 攻撃します！");

        // 敵にダメージ（進捗）を与える
        currentEnemy.TakeDamage(player.CurrentAtk);

        // 勝利判定
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
        GameManager.Instance.uiManager.ShowLog("不正解... 敵の反撃を受けます。");
        StartCoroutine(EnemyTurn());
    }

    // 敵のターン（演出用の待ち時間を作る）
    private IEnumerator EnemyTurn()
    {
        yield return new WaitForSeconds(1.0f);

        // ランダムでプレイヤーかネトが被弾
        bool hitPlayer = Random.value > 0.5f;
        int dmg = currentEnemy.Atk;

        if (hitPlayer)
        {
            int realDmg = Mathf.Max(0, dmg - player.CurrentDef);
            player.CurrentHP -= realDmg;
            GameManager.Instance.uiManager.ShowLog($"プレイヤーに {realDmg} のダメージ！");
        }
        else
        {
            int realDmg = Mathf.Max(0, dmg - neto.Def);
            neto.CurrentHP -= realDmg;
            GameManager.Instance.uiManager.ShowLog($"ネトに {realDmg} のダメージ！");
        }

        GameManager.Instance.uiManager.UpdateStatus(player, neto);

        // 敗北判定
        if (player.CurrentHP <= 0 || neto.CurrentHP <= 0)
        {
            StartCoroutine(EndBattle(false));
        }
        else
        {
            NextTurn();
        }
    }

    // 戦闘終了処理
    private IEnumerator EndBattle(bool win)
    {
        yield return new WaitForSeconds(1.0f);
        GameManager.Instance.uiManager.ToggleBattle(false);

        if (win)
        {
            GameManager.Instance.uiManager.ShowLog("デバッグ完了（勝利）！");
            Destroy(currentEnemy.gameObject);
        }
        else
        {
            GameManager.Instance.uiManager.ShowLog("敗北しました...");
        }
    }
}