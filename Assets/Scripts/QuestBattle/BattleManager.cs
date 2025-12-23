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
    private MultipleChoiceQuest choicechecker;
    private FillBlankQuest writechecker;
    [SerializeField] private UIManager uimanager;
    private void Awake()
    {
        GameManager.Instance.RegisterBattleManager(this);
        Debug.Log("GameManagerと接続");
    }
    private void Start()
    {
        questManager = GameManager.Instance.questManager;
        choicechecker = GetComponent<MultipleChoiceQuest>();
        writechecker = GetComponent<FillBlankQuest>();

        // ここで初めて battleManager が存在する
        Debug.Log("BattleManager Setup Completed");
    }

    /// <summary>
    /// 戦闘開始処理
    /// </summary>
    /// <param name="enemyId">CSVで定義された敵ID</param>
    public void StartBattle(int enemyId)
    {
        // プレイヤー取得（BattleScene側で責任を持つ）
        player = GameManager.Instance.player;
        neto = GameManager.Instance.neto;

        // 敵データ取得
        EnemyData data = GameManager.Instance.dataManager.GetEnemyById(enemyId);
        if (data == null)
        {
            Debug.LogError($"敵データが見つかりません: ID {enemyId}");
            return;
        }

        // 既存敵がいれば破棄
        if (currentEnemy != null)
        {
            Destroy(currentEnemy.gameObject);
        }

        // 敵生成
        currentEnemy = Instantiate(
            enemyPrefab,
            enemySpawnPoint.position,
            Quaternion.identity
        ).GetComponent<Enemy>();
        currentEnemy.Setup(data);

        // クエストカテゴリ初期化
        categories.Clear();
        categories.Add(QuestCategory.Variable_AdditionAndSubtraction);
        //categories.Add(QuestCategory.Variable_AdditionAndSubtraction);
        //categories.Add(QuestCategory.Variable_AdditionAndSubtraction);

        questManager.CreateDeck(categories);

        GameManager.Instance.SetMode(GameManager.GameMode.Battle);

        NextTurn();
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
        uimanager.TurnStart();
    }

    public void OnSubmitMultiChoiceAnswer(string code)//4択クイズの正解確認
    {
        if (currentQuestion == null) return;
        if (choicechecker.CheckAnswer(code, currentQuestion))
        {
            QuizCorrect();
        }
        else
        {
            QuizIncorrect();
        }
        NextTurn();
    }
    public void OnSubmitFillBrankAnswer(string code)
    {

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

        if (win)
        {
            Destroy(currentEnemy.gameObject);
            GameManager.Instance.MarkEnemyDefeated();
        }

        GameManager.Instance.SetMode(GameManager.GameMode.Field);
    }

}