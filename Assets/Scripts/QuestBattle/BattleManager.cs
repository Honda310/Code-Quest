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
    [SerializeField] private DamagePop damagePop;
    [System.NonSerialized] public Enemy currentEnemy;
    private Player player;
    private Neto neto;
    private QuestData currentQuestion;
    List<QuestCategory> categories = new List<QuestCategory>();
    private QuestManager questManager;
    [SerializeField] private MultipleChoiceQuest choicechecker;
    [SerializeField] private FillBlankQuest writechecker;
    [SerializeField] private UIManager uimanager;
    private bool hitPlayer;
    private void Awake()
    {
        GameManager.Instance.RegisterBattleManager(this);
    }
    private void Start()
    {
        questManager = GameManager.Instance.questManager;
        //choicechecker = GetComponent<MultipleChoiceQuest>();
        //writechecker = GetComponent<FillBlankQuest>();
    }

    /// <summary>
    /// 戦闘開始処理
    /// </summary>
    /// <param name="enemyId">CSVで定義された敵ID</param>
    public void StartBattle(int enemyId,Enemy enemy)
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

        // 敵生成
        currentEnemy = enemy;
        
        currentEnemy.MaxDP = data.MaxDP;
        currentEnemy.CurrentDP = 0;
        currentEnemy.Atk = data.Atk;

        categories.Clear();
        categories.Add(QuestCategory.Variable_AdditionAndSubtraction);
        categories.Add(QuestCategory.Variable_IncrementAndCompoundAssignmentPrecedence);
        categories.Add(QuestCategory.Variable_MultiplicationAndDivisionAndRemainder);

        questManager.CreateDeck(categories);

        GameManager.Instance.SetMode(GameManager.GameMode.Battle);
        UIManager.Active?.ShowLog();

        NextTurn();
    }

    public void NextTurn()
    {
        currentQuestion = questManager.GetNextQuestion();
        if (currentQuestion != null)
        {
            uimanager.UpdateBattleMessage($"出力される内容を答えてネト！\n\n{currentQuestion.QuestionText}",currentQuestion.Options);
        }
        else
        {
            uimanager.UpdateBattleMessage("問題切れ！");
        }
        uimanager.TurnStart();
    }

    public void OnSubmitMultiChoiceAnswer(string code)//4択クイズの正解確認
    {
        if (choicechecker.CheckAnswer(code, currentQuestion))
        {
            StartCoroutine(QuizCorrect());
        }
        else
        {
            StartCoroutine(QuizIncorrect());
        }
    }
    public void OnSubmitFillBrankAnswer(string code)
    {

    }

    private IEnumerator QuizCorrect()
    {
        yield return new WaitForSecondsRealtime(0.4f);
        currentEnemy.TakeDamage(player.CurrentAtk);
        UIManager.Active?.ShowLog($"問題に正解、{player.CurrentAtk}DPを与えた！");
        uimanager.UpdateStatus(player, neto, currentEnemy);
        damagePop.EnemyDpPlay(player.CurrentAtk);
        if (currentEnemy.CurrentDP >= currentEnemy.MaxDP)
        {
            StartCoroutine(EndBattle(true));
        }
        else
        {
            StartCoroutine(EnemyTurn());
        }
    }

    private IEnumerator QuizIncorrect()
    {
        yield return new WaitForSecondsRealtime(0.4f);
        UIManager.Active.ShowLog("不正解、ダメージを与えられなかった…");
        StartCoroutine(EnemyTurn());
    }

    private IEnumerator EnemyTurn()
    {
        yield return new WaitForSecondsRealtime(0.2f);
        if (player.CurrentHP>0 && neto.CurrentHP > 0)
        {
            hitPlayer = Random.value > 0.5f;
        }else if (player.CurrentHP <= 0)
        {
            hitPlayer = false;
        }else if (neto.CurrentHP <= 0)
        {
            hitPlayer = true;
        }
        
        int dmg = currentEnemy.Atk;
        if (hitPlayer)
        {
            int realDmg = Mathf.Max(0, dmg - neto.CurrentDef);
            player.CurrentHP -= realDmg;
            UIManager.Active?.ShowLog($"プレイヤーが{realDmg}のダメージを受けた！");
            damagePop.PlayerDamagePlay(realDmg);
        }
        else
        {
            int realDmg = Mathf.Max(0, dmg - neto.CurrentDef);
            neto.CurrentHP -= realDmg;
            UIManager.Active?.ShowLog($"ネトが{realDmg}のダメージを受けた！");
            damagePop.NetoDamagePlay(realDmg);
        }

        GameManager.Instance.SetBattleTime(GameManager.BattleTag.TurnEnd);
        if(player.CurrentHP <= 0)
        {
            player.CurrentHP = 0;
        }
        if(neto.CurrentHP <= 0)
        {
            neto.CurrentHP = 0;
        }
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
        yield return new WaitForSecondsRealtime(0.5f);
        if (win)
        {
            Destroy(currentEnemy.gameObject);
            GameManager.Instance.MarkEnemyDefeated();
        }
        player.ClearBuffs();
        neto.ClearBuffs();
        GameManager.Instance.SetMode(GameManager.GameMode.Field);
    }

}