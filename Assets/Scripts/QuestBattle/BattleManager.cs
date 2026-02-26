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
    [SerializeField] private DamagePop damagePop;
    [System.NonSerialized] public Enemy currentEnemy;
    private Player player;
    private Neto neto;
    private QuestData currentQuestion;
    private QuestCategory categories;
    private QuestManager questManager;
    [SerializeField] private MultipleChoiceQuest choicechecker;
    [SerializeField] private FillBlankQuest writechecker;
    [SerializeField] private UIManager uimanager;
    private bool hitPlayer;
    public bool ChallengableHard=false;
    
    private void Awake()
    {
        GameManager.Instance.RegisterBattleManager(this);
    }
    private void Start()
    {
        questManager = GameManager.Instance.questManager;
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
        currentEnemy = enemy;
        currentEnemy.MaxDP = data.MaxDP;
        currentEnemy.CurrentDP = 0;
        currentEnemy.Atk = data.Atk;
        currentEnemy.Exp = data.Exp;
        categories = data.Categories;
        questManager.CreateDeckNormal(categories);
        if (!(categories == QuestCategory.Variable_AdditionAndSubtraction || categories == QuestCategory.Variable_IncrementAndCompoundAssignmentPrecedence || categories == QuestCategory.Variable_MultiplicationAndDivisionAndRemainder))
        {
            questManager.CreateDeckHard(categories);
            ChallengableHard = true;
        }
        else
        {
            ChallengableHard = false;
        }
        GameManager.Instance.SetMode(GameManager.GameMode.Battle);
        uimanager.ShowLog();
        uimanager.EnemySetUp(data.ImageFileName,data.Name);
        damagePop.TextReset();
        uimanager.EnemyScanCount = 0;
        uimanager.TurnStart();
    }
    public void QuestSet(bool hard)
    {
        if (hard)
        {
            currentQuestion = questManager.GetNextQuestionHard();
            if (currentQuestion != null)
            {
                uimanager.UpdateBattleMessage($"出力される内容を答えてね！\n{currentQuestion.QuestionText}");
            }
            else
            {
                questManager.CreateDeckHard(categories);
                currentQuestion = questManager.GetNextQuestionHard();
                uimanager.UpdateBattleMessage($"出力される内容を答えてね！\n{currentQuestion.QuestionText}");
            }
        }
        else
        {
            currentQuestion = questManager.GetNextQuestionNormal();
            if (currentQuestion != null)
            {
                uimanager.UpdateBattleMessage($"出力される内容を答えてね！\n{currentQuestion.QuestionText}", currentQuestion.Options);
            }
            else
            {
                questManager.CreateDeckNormal(categories);
                currentQuestion = questManager.GetNextQuestionNormal();
                uimanager.UpdateBattleMessage($"出力される内容を答えてね！\n{currentQuestion.QuestionText}", currentQuestion.Options);
            }
        }
    }
    public void OnSubmitMultiChoiceAnswer(string code)
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
        if (writechecker.CheckAnswer(code, currentQuestion.CorrectAnswer))
        {
            StartCoroutine(QuizCorrect());
        }
        else
        {
            StartCoroutine(QuizIncorrect());
        }
    }
    public void TimeExpired()
    {
        StartCoroutine(QuizIncorrect());
    }
    private IEnumerator QuizCorrect()
    {
        yield return new WaitForSecondsRealtime(0.4f);
        int dmg = player.CurrentAtk + (uimanager.EnemyScanCount - 1) * 5;
        currentEnemy.TakeDamage(dmg);
        UIManager.Active?.ShowLog($"問題に正解、{dmg}DPを与えた！");
        uimanager.UpdateStatus(player, neto, currentEnemy);
        damagePop.EnemyDpPlay(dmg);
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
    public void NotAttackTurn()
    {
        StartCoroutine(EnemyTurn());
    }
    private IEnumerator EnemyTurn()
    {
        yield return new WaitForSecondsRealtime(1.0f);
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
            int realDmg = Mathf.Max(0, dmg - player.CurrentDef);
            player.CurrentHP -= realDmg;
            UIManager.Active?.ShowLog($"敵からの攻撃！プレイヤーに{realDmg}のダメージ！");
            damagePop.PlayerDamagePlay(realDmg);
        }
        else
        {
            int realDmg = Mathf.Max(0, dmg - neto.CurrentDef);
            neto.CurrentHP -= realDmg;
            UIManager.Active?.ShowLog($"敵からの攻撃！ネトに{realDmg}のダメージ！");
            damagePop.NetoDamagePlay(realDmg);
        }
        yield return new WaitForSecondsRealtime(0.5f);
        GameManager.Instance.SetBattleTime(GameManager.BattleTag.TurnEnd);
        if(player.CurrentHP <= 0)
        {
            player.CurrentHP = 0;
        }
        if(neto.CurrentHP <= 0)
        {
            neto.CurrentHP = 0;
        }
        if (player.CurrentHP <= 0 && neto.CurrentHP <= 0)
        {
            StartCoroutine(EndBattle(false));
        }
        else
        {
            uimanager.TurnStart();
        }
    }

    private IEnumerator EndBattle(bool win)
    {
        yield return new WaitForSecondsRealtime(0.5f);
        if (win)
        {
            Destroy(currentEnemy.gameObject);
            GameManager.Instance.enemyList.EnemyDefeat(currentEnemy.EnemyID);
        }
        player.ClearBuffs();
        neto.ClearBuffs();
        player.GainExperience(currentEnemy.Exp);
        GameManager.Instance.SetMode(GameManager.GameMode.Field);
    }
}