using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using static GameManager;

/// <summary>
/// 【UI管理】
/// UIすべての表示切替を担ってるよ。クラスの分業を検討するほどに過労死枠（圧倒的コード量）だよ。
/// </summary>
public class UIManager : MonoBehaviour
{
    [Header("HUD (常時表示するもの)")]
    [SerializeField] private Text PlayerStatusText; // プレイヤーのHPなどを表示
    [SerializeField] private Text NetoStatusText;   // ネトのHPを表示
    [SerializeField] private Text EnemyStatusText;
    [SerializeField] private Slider PlayerStatusSlider; 
    [SerializeField] private Slider NetoStatusSlider;   
    [SerializeField] private Slider EnemyStatusSlider;
    [SerializeField] private Text logText1;
    [SerializeField] private Text logText2;
    [SerializeField] private string[] logKeeper = new string[2];  

    [Header("各モードの画面パネル")]
    [SerializeField] private GameObject battlePanel;    // 戦闘画面
    [SerializeField] private Text battleQuestText;      // 問題表示
    [SerializeField] private Text battleInfoText;       
    [SerializeField] private Text PlSelectLabelText;    
    [SerializeField] private Text NetoSelectLabelText;  
    [SerializeField] private Text DifficultSelectText;  
    [SerializeField] private InputField answerInput;    // 記述式回答の入力欄
    [SerializeField] private GameObject shopPanel;      // お店画面
    [SerializeField] private GameObject dojoPanel;      // 道場画面
    [SerializeField] private GameObject itemDebugPanel; // アイテムデバッグ画面

    [Header("戦闘パネルの各要素")]
    [SerializeField] private GameObject PlSelectPanel;
    [SerializeField] private GameObject NetoSelectPanel;
    [SerializeField] private GameObject HealthDpSlidersAndCharactersPanel;
    [SerializeField] private GameObject DifficultSelectPanel;
    [SerializeField] private GameObject QuestFramePanel;
    [SerializeField] private GameObject DifficultAndCheckButtonFramePanel;
    [SerializeField] private GameObject DifficultAndSelectButtonFramePanel;
    [SerializeField] private Slider TimeLimitSlider;
    [SerializeField] private Text TimeLimitText;
    [SerializeField] private Text PlayerName;
    [SerializeField] private Image EnemyImage;
    [SerializeField] private Text EnemyName;
    Color skyblue = new Color32(0, 224, 255, 255);
    Color yellow = new Color32(224, 255, 0, 255);
    Color red = new Color32(255, 96, 96, 255);

    [Header("メニュー画面の各パネル")]
    [SerializeField] private GameObject MenuPanel;
    [SerializeField] private GameObject MenuBarPanel;
    [SerializeField] private GameObject MenuButtonPanel;
    [SerializeField] private GameObject ItemPanel;
    [SerializeField] private GameObject ItemTargetSelectPanel;
    [SerializeField] private GameObject ItemConfirmPanel;
    [SerializeField] private GameObject EquipandStatusPanel;
    [SerializeField] private GameObject ConfigPanelGameEnd;
    [SerializeField] private GameObject ConfigPanel;
    [SerializeField] private GameObject KeyBindPanel;
    [SerializeField] private GameObject CharaSelector;
    [SerializeField] private GameObject NetoSelector;
    [SerializeField] private GameObject EquipPanelDisable1;
    [SerializeField] private GameObject EquipPanelDisable2;
    [SerializeField] private GameObject EquipPanelDisable3;
    [SerializeField] private GameObject PointerPanel1;
    [SerializeField] private GameObject PointerPanel2;
    [SerializeField] private GameObject PointerPanel3;
    [SerializeField] private GameObject PointerPanel4;
    [SerializeField] private GameObject PointerPanel5;
    [SerializeField] private GameObject PointerPanel6;
    [SerializeField] private Slider PlHpSlider;
    [SerializeField] private Slider NetoHpSlider;
    [SerializeField] private Text PlHpText;
    [SerializeField] private Text NetoHpText;

    [Header("装備&ステータス画面の各テキスト&ボタン")]
    [SerializeField] private Text CharaNameText;
    [SerializeField] private Text EquipWeaponName;
    [SerializeField] private Text EquipAccessoryName;
    [SerializeField] private Text LevelText;
    [SerializeField] private Text ExpText;
    [SerializeField] private Text CurrentHPText;
    [SerializeField] private Text CurrentAtkText;
    [SerializeField] private Text CurrentDefText;
    [SerializeField] private Text CurrentDebugLimitText;
    [SerializeField] private Text EquipItemSelectSlot1;
    [SerializeField] private Text EquipItemSelectSlot2;
    [SerializeField] private Text EquipItemSelectSlot3;
    [SerializeField] private Text EquipItemSelectSlot4;
    [SerializeField] private Text EquipItemSelectSlot5;
    [SerializeField] private Text InventoryItemSelectSlot1;
    [SerializeField] private Text InventoryItemSelectSlot2;
    [SerializeField] private Text InventoryItemSelectSlot3;
    [SerializeField] private Text InventoryItemSelectSlot4;
    [SerializeField] private Text InventoryItemSelectSlot5;
    [SerializeField] private Text InventoryItemValue1;
    [SerializeField] private Text InventoryItemValue2;
    [SerializeField] private Text InventoryItemValue3;
    [SerializeField] private Text InventoryItemValue4;
    [SerializeField] private Text InventoryItemValue5;
    [SerializeField] private Text PlayerItemValidText;
    [SerializeField] private Text NetoItemValidText;
    [SerializeField] private Text ItemFlavorText;
    [SerializeField] private GameObject StatusDetailPanel;
    [SerializeField] private Text StatusDetailText;
    [SerializeField] private GameObject ItemDetailPanel;
    [SerializeField] private Text ItemDetailText;
    [SerializeField] private GameObject EquipItemFlavorPanel;
    [SerializeField] private Text EquipItemFlavorText;


    [Header("細かいUIの調整用")]
    //例えば、スロット2にある選択中に下にスクロールした場合、Entered,Exit制御では
    //前のアイテムが表示され続けてしまう問題がある。上から、
    //アイテム画面からのアイテム選択時
    private int SelectorItemIDKeeper=1;
    //戦闘画面からのアイテム選択時
    //private int SelectorItemIDKeeper_VerBattle=0;
    //装備画面での装備選択時
    private int SelectorEquipIDKeeper=1; 

    [Header("会話イベントなどに使用するいろいろ")]
    [SerializeField] private GameObject TalkTextBoxPanel;
    [SerializeField] private Text TalkTextBox;
    [SerializeField] private GameObject SaveLoadPanel;
    [SerializeField] private Text SaveDetailText;
    [SerializeField] private GameObject TalkBranchPanel;

    //装備&ステータス画面の制御系
    private bool EquipCharacterSelecter;
    private bool EquipSlots;
    private bool EquipChangeSelecter;
    private bool OnWeaponEquipSelecting;
    private bool OnPlayerEquipSelecting;
    private int EquipSelectorcursorPosition=0;
    private int InventoryItemCursor = 0;
    private int InventorySlotIdHolder=0;
    private int CharaIdHolder = 0;
    private string WeaponItemName;
    private string AccessoryItemName;
    private bool PlayerTarget;
    private bool NetoTarget;
    private int SaveSlotId = 1;

    //先に呼ばれても変なことにならないための予防用初期値
    private double MaxTimeLimit=10.0;
    private double CurrentTimeLimit = 10.0;
    private bool QuestionStart = false;

    private GameManager gm;
    private Inventory inventory;
    private Player p;
    private Neto n;
    public static UIManager Active { get; private set; }
    private void Awake()
    {
        Active = this;
    }

    private void Start()
    {
        gm = GameManager.Instance;
        p = GameManager.Instance.player;
        n = GameManager.Instance.neto;
        inventory = GameManager.Instance.inventory;
        UpdateStatus(p,n);
        AllPanelClose();
    }
    public void Update()
    {
        if(gm == null) return; //GameManagerを取得しようとしたはいいけど、UpdateがStartより先に呼び出される！ってケースがあるらしい。マジ？
        if (gm.CurrentMode == GameMode.Battle)
        {
            battlePanel.SetActive(true);
        }
        else if(gm.CurrentMode == GameMode.Shop)
        {
            shopPanel.SetActive(true);
        }
        else if (gm.CurrentMode==GameMode.Field)
        {
            battlePanel.SetActive(false);
            shopPanel.SetActive(false);
        }
        if (QuestionStart)
        {
            TimeLimitSlider.value = (float)CurrentTimeLimit;
            Image fill = TimeLimitSlider.fillRect.GetComponent<Image>();
            float ratio = TimeLimitSlider.value / TimeLimitSlider.maxValue;
            TimeLimitText.text= (Math.Floor(TimeLimitSlider.value * 100) / 100).ToString("F1") + "s";
            if (ratio > 0.5f)
            {
                fill.color = skyblue;
                TimeLimitText.color = skyblue;
            }else if (ratio > 0.25f)
            {
                fill.color = yellow;
                TimeLimitText.color = yellow;
            }
            else
            {
                fill.color = red;
                TimeLimitText.color = red;
            }
        }
    }
    private void FixedUpdate()
    {
        if (QuestionStart)
        {
            if (CurrentTimeLimit <= 0 && QuestionStart)
            {
                QuestionStart = false;
                GameManager.Instance.BattleManager.TimeExpired();
                return;
            }
            CurrentTimeLimit = Math.Max(0, CurrentTimeLimit - Time.fixedDeltaTime);
        }
    }
    ///<summary>
    ///メインの画面を除き、すべてのパネルを閉じるメソッド。
    ///特にデバッグ時とか、あるいはビルド後にパネルのデフォルトがTrueになってしまっているときに回避するために作成したよ。
    ///不都合があるなら、Start内の当該メソッドをコメントアウトすること！
    ///あと、これが使える他の場面があれば積極的に使っていいよ～
    ///</summary>>
    public void AllPanelClose()
    {
        MenuPanel.SetActive(false);
        battlePanel.SetActive(false);
        shopPanel.SetActive(false);
        itemDebugPanel.SetActive(false);
        dojoPanel.SetActive(false);
        MenuBarPanel.SetActive(false);
        EquipandStatusPanel.SetActive(false);
        ItemPanel.SetActive(false);
        KeyBindPanel.SetActive(false);
        ConfigPanel.SetActive(false);
        ConfigPanelGameEnd.SetActive(false);
        ItemTargetSelectPanel.SetActive(false);
        ItemConfirmPanel.SetActive(false);
    }
    ///<summary>
    ///フィールド上などで、キャラの現在ステータスをUIに反映するためのメソッドです。
    ///戦闘中はPlayer,Neto,Enemyの3引数がある方を使ってね。
    ///</summary>
    public void UpdateStatus(Player p, Neto n)
    {
        if (PlayerStatusText != null)
        {
            PlayerStatusText.text = $"Player HP: {p.CurrentHP}/{p.MaxHP}\nATK: {p.CurrentAtk} DEF: {p.CurrentDef}";
        }

        if (NetoStatusText != null)
        {
            NetoStatusText.text = $"Neto HP: {n.CurrentHP}/{n.MaxHP}";
        }
    }
    public void UpdateStatus(Player p)
    {
        if (PlayerStatusText != null)
        {
            LevelText.text = $"  LV  : {p.CurrentLv}";
            ExpText.text = $"  EXP : {p.CurrentExp}/{p.NextExp}";
            CurrentHPText.text = $"  HP  : {p.CurrentHP}/{p.MaxHP}";
            CurrentAtkText.text= $"  ATK : {p.CurrentAtk}";
            CurrentDefText.text= $"  DEF : {p.CurrentDef}";
            CurrentDebugLimitText.text=$"  Lim : {p.DebugLimit}s.";
        }
    }
    public void UpdateStatus(Neto n)
    {
        if (NetoStatusText != null)
        {
            LevelText.text = $"  LV  : None";
            ExpText.text = $"  EXP : None";
            CurrentHPText.text = $"  HP  : {n.CurrentHP}/{n.MaxHP}";
            CurrentAtkText.text = $"  ATK : None";
            CurrentDefText.text = $"  DEF : {n.CurrentDef}";
            CurrentDebugLimitText.text = $"  Lim : None";
        }
    }
    public void UpdateStatus()
    {
        LevelText.text = $"  LV  :";
        ExpText.text = $"  EXP :";
        CurrentHPText.text = $"  HP  :";
        CurrentAtkText.text = $"  ATK :";
        CurrentDefText.text = $"  DEF :";
        CurrentDebugLimitText.text = $"  Lim :";
    }
    public void UpdateHpSlider()
    {
        PlHpSlider.maxValue = p.MaxHP;
        PlHpSlider.value = p.CurrentHP;
        PlHpText.text = $"{PlHpSlider.value}/{PlHpSlider.maxValue}";
        NetoHpSlider.maxValue = n.MaxHP;
        NetoHpSlider.value = n.CurrentHP;
        NetoHpText.text = $"{NetoHpSlider.value}/{NetoHpSlider.maxValue}";
    }
    ///<summary>
    ///バトル中など、エネミーがいる際の現在ステータスをUIに反映するためのメソッドです。
    ///player,netoのみを処理する、フィールド上でメニュー画面を開いた際を想定したものもあります。
    ///戦闘以外でこれを呼び出すと、多分NullRef吐きます。
    ///</summary>
    public void UpdateStatus(Player p, Neto n,Enemy e)
    {
        // テキストコンポーネントが存在する場合のみ更新します
        if (PlayerStatusText != null)
        {
            PlayerStatusText.text = $"{p.CurrentHP}/{p.MaxHP}";
            PlayerStatusSlider.maxValue = p.MaxHP;
            PlayerStatusSlider.minValue = 0;
            PlayerStatusSlider.value = p.CurrentHP;
        }
        if (NetoStatusText != null)
        {
            NetoStatusText.text = $"{n.CurrentHP}/{n.MaxHP}";
            NetoStatusSlider.maxValue = n.MaxHP;
            NetoStatusSlider.minValue = 0;
            NetoStatusSlider.value = n.CurrentHP;

        }
        if (EnemyStatusText != null)
        {
            EnemyStatusText.text = $"{e.CurrentDP}/{e.MaxDP}";
            EnemyStatusSlider.maxValue = e.MaxDP;
            EnemyStatusSlider.minValue = 0;
            EnemyStatusSlider.value = e.CurrentDP;
        }
    }
    public void EnemySetUp(string path,string name)
    {
        PlayerName.text = p.PlayerName ;
        EnemyName.text = name ;
        EnemyImage.sprite = Resources.Load<Sprite>($"Image/Enemy/{path}");
    }
    public void ShowLog()
    {
        logText1.text = "";
        logText2.text = "";
    }
    /// <summary>
    /// 戦闘ログを更新するためのメソッド。現状、通常会話を振り返ったりできる機能はない…でいいんだよね？
    /// </summary>
    public void ShowLog(string message)
    {
        if(logText1.text == "")
        {
            logText1.text = message;
        }
        else if(logText2.text == "")
        {
            logText2.text = message;
        }
        else
        {
            logText1.text = logText2.text;
            logText2.text = message;
        }
    }
    public void HideLog()
    {
        logKeeper[0] = logText1.text;
        logKeeper[1] = logText2.text;
        logText1.text = "";
        logText2.text = "";
    }
    public void RebootLog()
    {
        logText1.text = logKeeper[0];
        logText2.text = logKeeper[1];
        logKeeper[0] = "";
        logKeeper[1] = "";
    }
    /// <summary>
    /// 
    /// </summary>
    public void UpdateBattleMessage(string text)
    {
        if (battleQuestText != null)
        {
            battleQuestText.text = text;
        }
    }
    /// <summary>
    /// 4択問題の回答を表示するよ。
    /// </summary>
    public void UpdateBattleMessage(string text, string[] opts)
    {
        if (battleQuestText != null)
        {
            battleQuestText.text = text+"\n"+"A:"+opts[0]+ "　B:" + opts[1] + "　C:" + opts[2] + "　D:" + opts[3];

        }
    }
    /// <summary>
    /// 4択問題でA～Dのいずれかのボタンを押されたとき、または穴埋め問題で「解答送信」ボタンが押されたときに呼ばれるよ。
    /// クリックされたボタンのテキストを読み取ってanswerに代入→A～Dのいずれかなら4択問題用の処理を、
    /// それ以外（基本というか、間違いがなければ送信が送られたことになるけど）なら穴埋め問題用の処理を行うよ。
    /// </summary>
    public void OnSubmitButtonClicked(Button clickedButton)
    {

        if (QuestionStart == false) return;
        QuestionStart = false;
        string buttonText = clickedButton.GetComponentInChildren<Text>().text;
        string answer = buttonText;

        if ((answer=="A" || answer == "B" || answer == "C" || answer == "D")==false)
        {
            answer = answerInput.text;
            GameManager.Instance.BattleManager.OnSubmitFillBrankAnswer(answer);
            Debug.Log("fill");
        }
        else
        {
            GameManager.Instance.BattleManager.OnSubmitMultiChoiceAnswer(answer);
        }
            // 入力欄を空にします
            answerInput.text = "";
            QuestFramePanel.SetActive(false);
    }
    //テンプレ用
    //public void OnButtonClicked(Button clickedButton)
    //{

    //}
    public void OnSelectNormalButtonClicked()
    {
        DifficultAndCheckButtonFramePanel.SetActive(true);
        DifficultAndSelectButtonFramePanel.SetActive(false);
    }
	public void OnSelectNormalButtonSelected()
	{
        DifficultSelectText.text = ("＊4択問題に挑戦する");
	}
	public void OnSelectNormalButtonDeSelected()
	{
        DifficultSelectText.text = ("＊挑戦する問題を選択してください");
    }
    public void OnSelectHardButtonClicked()
    {
		
    }
    public void OnSelectHardButtonSelected()
    {
        DifficultSelectText.text = ("＊穴埋め問題に挑戦する");
    }
    public void OnSelectHardButtonDeSelected()
    {
        DifficultSelectText.text = ("＊挑戦する問題を選択してください");
    }
    public void OnReselectButtonClicked()
    {
        PlSelectPanel.SetActive(true);
        DifficultSelectPanel.SetActive(false);
        DifficultAndCheckButtonFramePanel.SetActive(false);
        RebootLog();
    }
	public void OnReselectButtonSelected()
	{
        DifficultSelectText.text = ("＊行動を再選択する");
	}
	public void OnReselectButtonDeSelected()
	{
        DifficultSelectText.text=("＊挑戦する問題を選択してください");
	}
    public void OnAcceptButtonClicked()
    {
        DifficultAndCheckButtonFramePanel.SetActive(false);
        DifficultSelectPanel.SetActive(false);
        NetoSelectPanel.SetActive(true);
    }
    public void OnAcceptButtonSelected()
	{
        DifficultSelectText.text = ("＊行動を確定します");
	}
    public void OnAcceptButtonDeSelected()
	{
        DifficultSelectText.text = ("＊行動を確定しますか？");
	}
    public void OnCancelButtonClicked()
    {
        DifficultAndCheckButtonFramePanel.SetActive(false);
        DifficultAndSelectButtonFramePanel.SetActive(true);
    }
    public void OnCancelButtonSelected()
    {
        DifficultSelectText.text = ("＊行動の選択をやり直します");
	}
    public void OnCancelButtonDeSelected()
    {
        DifficultSelectText.text = ("＊行動を確定しますか？");
	}
    public void OnPlayerDebugButtonClicked()
    {
        PlSelectPanel.SetActive(false);
        NetoSelectPanel.SetActive(false);
        DifficultSelectPanel.SetActive(true);
        DifficultAndSelectButtonFramePanel.SetActive(true);
        DifficultSelectText.text = ("＊挑戦する問題を選択してください");
        HideLog();
    }
    public void OnPlayerDebugButtonSelected()
    {
        PlSelectLabelText.text = ("デバッグ");
    }
    public void OnPlayerDebugButtonDeSelected()
    {
        PlSelectLabelText.text = ("");
    }
    public void OnPlayerItemButtonClicked()
    {
		
    }
    public void OnPlayerItemButtonSelected()
    {
        PlSelectLabelText.text=("アイテム");
    }
    public void OnPlayerItemButtonDeSelected()
    {
        PlSelectLabelText.text=("");
    }
    public void OnNetoSearchButtonClicked()
    {
        NetoSelectPanel.SetActive(false);
        HealthDpSlidersAndCharactersPanel.SetActive(false);
        QuestFramePanel.SetActive(true);
        QuizStart();
    }
    public void OnNetoSearchButtonSelected()
    {
        NetoSelectLabelText.text = ("スキャン");
    }
    public void OnNetoSearchButtonDeSelected()
    {
        NetoSelectLabelText.text = ("");
    }
    public void OnNetoItemButtonClicked()
    {
		
    }
    public void OnNetoItemButtonSelected()
    {
        NetoSelectLabelText.text = ("アイテム");
    }
    public void OnNetoItemButtonDeSelected()
    {
        NetoSelectLabelText.text = ("");
    }
    public void TurnStart()
    {
        PlSelectPanel.SetActive(false);
        NetoSelectPanel.SetActive(false);
        QuestFramePanel.SetActive(false);
        DifficultSelectPanel.SetActive(false);
        DifficultAndCheckButtonFramePanel.SetActive(false);
        DifficultAndSelectButtonFramePanel.SetActive(false);

        HealthDpSlidersAndCharactersPanel.SetActive(true);
        if (p.CurrentHP > 0)
        {
            PlSelectPanel.SetActive(true);
        }
        else
        {
            NetoSelectPanel.SetActive(true);
        }
        UpdateStatus(p, n, GameManager.Instance.BattleManager.currentEnemy);
        MaxTimeLimit = p.DebugLimit;
    }
    public void QuizStart()
    {
        CurrentTimeLimit = MaxTimeLimit;
        QuestionStart = true;
        TimeLimitSlider.maxValue = (float)MaxTimeLimit;
        TimeLimitSlider.value = (float)MaxTimeLimit;
    }
    public void OnInventoryButtonClicked()
    {
        if (ItemPanel.activeSelf)
        {
            ItemPanel.SetActive(false);
        }
        else
        {
            ItemPanel.SetActive(true);
            EquipandStatusPanel.SetActive(false);
            ConfigPanel.SetActive(false);
            ConfigPanelGameEnd.SetActive(false);
            KeyBindPanel.SetActive(false);
            List<CarryItem> supportItems = inventory.GetItemsByType(Item.ItemType.SupportItem);
            SupportItemSelectorChange(0, supportItems);
        }
        
    }
    public void OnEquipAndStatusButtonClicked()
    {
        LevelText.text = $"  LV  :";
        ExpText.text = $"  EXP :";
        CurrentHPText.text = $"  HP  :";
        CurrentAtkText.text = $"  ATK :";
        CurrentDefText.text = $"  DEF :";
        CurrentDebugLimitText.text = $"  Lim :";
        CharaNameText.text = "Chara Name";
        if (EquipandStatusPanel.activeSelf)
        {
            EquipandStatusPanel.SetActive(false);
        }
        else
        {
            ItemPanel.SetActive(false);
            EquipandStatusPanel.SetActive(true);
            ConfigPanel.SetActive(false);
            ConfigPanelGameEnd.SetActive(false);
            KeyBindPanel.SetActive(false);
        }
        EquipCharacterSelecter = true;
        EquipSlots = false;
        EquipChangeSelecter = false;
        EquipPanelDisable2.SetActive(true);
        EquipPanelDisable3.SetActive(true);
    }
    public void OnDojoButtonClicked()
    {
        if (SceneManager.GetActiveScene().name == "Dojo")
        {
            OnMenuCloseButtonClicked();
            GameManager.Instance.ReturnBeforeMap();
            GameManager.Instance.SetMode(GameMode.Field);
        }
        else
        {
            OnMenuCloseButtonClicked();
            GameManager.Instance.StackMapNameAndPosition(SceneManager.GetActiveScene().name,p.transform.position,n.transform.position);
            GameManager.Instance.mapManager.TransAnotherMap("Dojo", 99);
            GameManager.Instance.SetMode(GameMode.Field);
        }
    }
    public void OnConfigButtonClicked()
    {
        if (ConfigPanel.activeSelf)
        {
            ConfigPanel.SetActive(false);
        }
        else
        {
            ItemPanel.SetActive(false);
            EquipandStatusPanel.SetActive(false);
            ConfigPanel.SetActive(true);
            KeyBindPanel.SetActive(false);
        }
        
    }
    public void OnKeyBindButtonClicked()
    {
        if (KeyBindPanel.activeSelf)
        {
            KeyBindPanel.SetActive(false);
        }
        else
        {
            ItemPanel.SetActive(false);
            EquipandStatusPanel.SetActive(false);
            ConfigPanel.SetActive(false);
            ConfigPanelGameEnd.SetActive(false);
            KeyBindPanel.SetActive(true);
        }
        
    }
    public void OnMenuCloseButtonClicked()
    {
        MenuPanel.SetActive(false);
        ItemPanel.SetActive(false);
        EquipandStatusPanel.SetActive(false);
        ConfigPanel.SetActive(false);
        ConfigPanelGameEnd.SetActive(false);
        KeyBindPanel.SetActive(false);
        MenuBarPanel.SetActive(false);
        ItemPanel.SetActive(false);
        ItemTargetSelectPanel.SetActive(false);
        ItemConfirmPanel.SetActive(false);
        CharaSelector.SetActive(false);
        NetoSelector.SetActive(false);
        EquipPanelDisable1.SetActive(true);
        EquipPanelDisable2.SetActive(true);
        EquipPanelDisable3.SetActive(true);
        EquipCharacterSelecter=false;
        EquipSlots=false;
        EquipChangeSelecter = false;
        OnWeaponEquipSelecting = false;
        OnPlayerEquipSelecting = false;
        EquipSelectorcursorPosition = 0;
        InventoryItemCursor = 0;
        InventorySlotIdHolder = 0;
        CharaIdHolder = 0;
        WeaponItemName="Weapon";
        EquipWeaponName.text = WeaponItemName;
        AccessoryItemName ="Accessory";
        EquipAccessoryName.text = AccessoryItemName;
        PlayerTarget =false;
        NetoTarget=false;
        gm.SetMode(GameMode.Field);
}
    
    public void MenuToggle()
    {
        if (MenuPanel.activeSelf == false)
        {
            MenuPanel.SetActive(true);
            MenuBarPanel.SetActive(true);
            CharaSelector.SetActive(false);
            NetoSelector.SetActive(false);
            EquipPanelDisable1.SetActive(false);
            EquipPanelDisable2.SetActive(false);
            EquipPanelDisable3.SetActive(false);
            EquipSlots = false;
            EquipCharacterSelecter = false;
            EquipChangeSelecter = false;
            for (int i = 0; i<6; i++)
            {
                OnMenuButtonTriggerExit(i);
                OnStatusDetailTriggerExit();
                OnItemDetailTriggerExit();
                OnEquipSelectorExit();
                OnItemSelectorExit();
            }
            
            gm.SetMode(GameMode.Menu);
            UpdateHpSlider();
        }
        //ここから装備&ステータス画面のEsc制御
        else if (MenuPanel.activeSelf && (((ItemPanel.activeSelf || EquipandStatusPanel.activeSelf) || (ConfigPanel.activeSelf || KeyBindPanel.activeSelf))== false))
        {
            MenuPanel.SetActive(false);
            MenuBarPanel.SetActive(false);
            gm.SetMode(GameMode.Field);
        }
        else if (MenuPanel.activeSelf && EquipSlots)
        {
            CharaNameText.text = "Chara Name";
            CharaSelector.SetActive(false);
            NetoSelector.SetActive(false);
            EquipSlots = false;
            EquipPanelDisable2.SetActive(true);
            OnPlayerEquipSelecting = false;
            EquipWeaponName.text = "Weapon";
            EquipAccessoryName.text = "Accessory";
            UpdateStatus();
        }
        else if (MenuPanel.activeSelf && EquipChangeSelecter)
        {
            EquipSlots = true;
            EquipCharacterSelecter = true;
            EquipChangeSelecter = false;
            EquipPanelDisable1.SetActive(false);
            EquipPanelDisable2.SetActive(false);
            EquipPanelDisable3.SetActive(true);
            if (OnWeaponEquipSelecting)
            {
                EquipWeaponName.text = WeaponItemName;
            }
            else
            {
                EquipAccessoryName.text = AccessoryItemName;
            }
        }
        //ここまで
        //ここからインベントリパネルの制御用
        else if(MenuPanel.activeSelf && ItemTargetSelectPanel.activeSelf)
        {
            ItemTargetSelectPanel.SetActive(false);
            ItemFlavorText.text = "";
        }
        else if(MenuPanel.activeSelf && ItemConfirmPanel.activeSelf)
        {
            OnItemReconfirmCancel();
        }
        //ここまで
        //ここからシステムパネルの制御用
        else if (MenuPanel.activeSelf && ConfigPanel.activeSelf && ConfigPanelGameEnd.activeSelf)
        {
            ConfigPanelGameEnd.SetActive(false);
        }
        //ここまで
        else
        {
            ItemPanel.SetActive(false);
            EquipandStatusPanel.SetActive(false);
            ConfigPanel.SetActive(false);
            ConfigPanelGameEnd.SetActive(false);
            KeyBindPanel.SetActive(false);
        }
    }
    //メニュー画面表層のUI制御
    public void OnMenuButtonTriggerEnter(int i)
    {
        switch (i)
        {
            case 0:
                PointerPanel1.SetActive(true);
                break;
            case 1:
                PointerPanel2.SetActive(true);
                break;
            case 2:
                PointerPanel3.SetActive(true);
                break;
            case 3:
                PointerPanel4.SetActive(true);
                break;
            case 4:
                PointerPanel5.SetActive(true);
                break;
            case 5:
                PointerPanel6.SetActive(true);
                break;
        }
    }
    public void OnMenuButtonTriggerExit(int i)
    {
        switch (i)
        {
            case 0:
                PointerPanel1.SetActive(false);
                break;
            case 1:
                PointerPanel2.SetActive(false);
                break;
            case 2:
                PointerPanel3.SetActive(false);
                break;
            case 3:
                PointerPanel4.SetActive(false);
                break;
            case 4:
                PointerPanel5.SetActive(false);
                break;
            case 5:
                PointerPanel6.SetActive(false);
                break;
        }
    }
    public void OnStatusDetailTriggerEnter(int i)
    {
        if (CharaSelector.activeSelf) {
            switch (i)
            {
                case 0:
                    StatusDetailPanel.SetActive(true);
                    StatusDetailText.text = $"詳細情報なし";
                    break;
                case 1:
                    StatusDetailPanel.SetActive(true);
                    StatusDetailText.text = $"次のレベルまで：{p.NextExp - p.CurrentExp}\n累計経験値　　：{p.TotalExp}\n　　　　　　　 ({p.TotalExp}/{p.NextTotalExp})\n次のレベルになるために\n必要になる経験値は\n現在のレベル×100";
                    break;
                case 2:
                    StatusDetailPanel.SetActive(true);
                    StatusDetailText.text = $"レベル1ごと、\n最大HPは5上がる";
                    break;
                case 3:
                    StatusDetailPanel.SetActive(true);
                    StatusDetailText.text = $"基礎攻撃力　　：{p.BaseAtk}\n装備補正　　　：{p.WeaponAtk}\nアイテム補正　：{p.TemporaryAtk}\nレベル1ごと、\n基礎攻撃力も1上がる";
                    break;
                case 4:
                    StatusDetailPanel.SetActive(true);
                    StatusDetailText.text = $"基礎防御力　　：{p.BaseDef}\n装備補正　　　：{p.AccessoryDef}\nアイテム補正　：{p.TemporaryDef}\nレベル1ごと、\n基礎防御力も1上がる";
                    break;
                case 5:
                    StatusDetailPanel.SetActive(true);
                    StatusDetailText.text = $"デバッグの制限時間は\nレベルに依存せず、\n装備武器で決定される";
                    break;
            }
        }else if (NetoSelector.activeSelf)
        {
            switch (i)
            {
                case 0:
                    StatusDetailPanel.SetActive(true);
                    StatusDetailText.text = $"レベルが存在しない\nキャラクター";
                    break;
                case 1:
                    StatusDetailPanel.SetActive(true);
                    StatusDetailText.text = $"レベルが存在しない\nキャラクター";
                    break;
                case 2:
                    StatusDetailPanel.SetActive(true);
                    StatusDetailText.text = $"このキャラクターの\n最大HPは固定です";
                    break;
                case 3:
                    StatusDetailPanel.SetActive(true);
                    StatusDetailText.text = $"攻撃力が存在しない\nキャラクター";
                    break;
                case 4:
                    StatusDetailPanel.SetActive(true);
                    StatusDetailText.text = $"基礎防御力　　：{n.BaseDef}\n装備補正　　　：{n.AccessoryDef}\nアイテム補正　：{n.TemporaryDef}\nこのキャラクターの\n基礎防御力は固定です";
                    break;
                case 5:
                    StatusDetailPanel.SetActive(true);
                    StatusDetailText.text = $"制限時間が存在しない\nキャラクター";
                    break;
            }
        }
        else
        {
            switch (i)
            {
                case 0:
                    StatusDetailPanel.SetActive(true);
                    StatusDetailText.text = $"詳細情報なし";
                    break;
                case 1:
                    StatusDetailPanel.SetActive(true);
                    StatusDetailText.text = $"次のレベルになるために\n必要になる経験値は\n現在のレベル×100";
                    break;
                case 2:
                    StatusDetailPanel.SetActive(true);
                    StatusDetailText.text = $"レベル1ごと、\n最大HPは5上がる";
                    break;
                case 3:
                    StatusDetailPanel.SetActive(true);
                    StatusDetailText.text = $"レベル1ごと、\n基礎攻撃力も1上がる";
                    break;
                case 4:
                    StatusDetailPanel.SetActive(true);
                    StatusDetailText.text = $"レベル1ごと、\n基礎防御力も1上がる";
                    break;
                case 5:
                    StatusDetailPanel.SetActive(true);
                    StatusDetailText.text = $"デバッグの制限時間は\nレベルに依存せず、\n装備武器で決定される";
                    break;
            }
        }
    }
    public void OnStatusDetailTriggerExit()
    {
        StatusDetailPanel.SetActive(false);
    }
    public void OnItemDetailTriggerExit()
    {
        ItemDetailPanel.SetActive(false);
        ItemDetailText.text = "";
    }
    //ここからは、インベントリ関連のUI制御
    //この辺にあるif([任意の文字列]==false) return;は、本来選べないボタンを選択/クリックさせないためのやつ
    public void OnPlayerIconClicked()
    {
        if (EquipCharacterSelecter == false)
        {
            Debug.Log("そのアイコンはfalseだよ");
            return;
        }
        CharaNameText.text = p.PlayerName;
        EquipWeaponName.text = p.EquipWeaponName;
        EquipAccessoryName.text = p.EquipAccessoryName;
        CharaSelector.SetActive(true);
        NetoSelector.SetActive(false);
        UpdateStatus(p);
        OnPlayerEquipSelecting = true;
        Debug.Log("Charaアイコンが選択されたよ");
        EquipSlots = true;
        EquipPanelDisable2.SetActive(false);
    }
    public void OnNetoIconClicked()
    {
        if (EquipCharacterSelecter == false)
        {
            Debug.Log("そのアイコンはfalseだよ");
            return;
        }
        CharaNameText.text = "Neto";
        EquipWeaponName.text = "装備不可";
        EquipAccessoryName.text = n.EquipAccessoryName;
        CharaSelector.SetActive(false);
        NetoSelector.SetActive(true);
        UpdateStatus(n);
        OnPlayerEquipSelecting = false;
        Debug.Log("Netoアイコンが選択されたよ");
        EquipSlots = true;
        EquipPanelDisable2.SetActive(false);
    }
    public void OnWeaponSlotClicked(GameObject gameObject)
    {
        if (EquipSlots == false || OnPlayerEquipSelecting == false)
        {
            Debug.Log("そのボタンはfalseだよ～");
            return;
        }
        WeaponItemName = gameObject.GetComponentInChildren<Text>().text;
        OnWeaponEquipSelecting = true;
        EquipSelectorcursorPosition = 0;
        List<CarryItem> weaponItems = inventory.GetItemsByType(Item.ItemType.Weapon);
        WeaponSelectorChange(0,weaponItems);
        Debug.Log("武器スロットが選択されたよ");
        gameObject.GetComponentInChildren<Text>().text = "Selected";
        EquipSlots = false;
        EquipCharacterSelecter = false;
        EquipChangeSelecter = true;
        EquipPanelDisable1.SetActive(true);
        EquipPanelDisable2.SetActive(true);
        EquipPanelDisable3.SetActive(false);
    }
    public void OnAccessorySlotClicked(GameObject gameObject)
    {
        if (EquipSlots == false )
        {
            Debug.Log("そのボタンはfalseだよ～");
            return;
        }
        AccessoryItemName = gameObject.GetComponentInChildren<Text>().text;
        OnWeaponEquipSelecting = false;
        EquipSelectorcursorPosition = 0;
        List<CarryItem> AccessoryItems = inventory.GetItemsByType(Item.ItemType.Accessory);
        AccessorySelectorChange(0,AccessoryItems);
        Debug.Log("アクセサリスロットが選択されたよ");
        gameObject.GetComponentInChildren<Text>().text = "Selected";
        EquipSlots = false;
        EquipCharacterSelecter = false;
        EquipChangeSelecter = true;
        EquipPanelDisable1.SetActive(true);
        EquipPanelDisable2.SetActive(true);
        EquipPanelDisable3.SetActive(false);
    }
    public void WeaponSelectorChange(int i, List<CarryItem> weaponItems)
    {
        try
        {
            if (0 + i < weaponItems.Count)
                EquipItemSelectSlot1.text = weaponItems[0 + i].item.ItemName;
            if (1 + i < weaponItems.Count)
                EquipItemSelectSlot2.text = weaponItems[1 + i].item.ItemName;
            if (2 + i < weaponItems.Count)
                EquipItemSelectSlot3.text = weaponItems[2 + i].item.ItemName;
            if (3 + i < weaponItems.Count)
                EquipItemSelectSlot4.text = weaponItems[3 + i].item.ItemName;
            if (4 + i < weaponItems.Count) 
                EquipItemSelectSlot5.text = weaponItems[4 + i].item.ItemName;
        }
        catch (IndexOutOfRangeException)
        {

        }
    }
    public void AccessorySelectorChange(int i, List<CarryItem> accessoryItems)
    {
        try
        {
            if (0 + i < accessoryItems.Count)
                EquipItemSelectSlot1.text = accessoryItems[0 + i].item.ItemName;

            if (1 + i < accessoryItems.Count)
                EquipItemSelectSlot2.text = accessoryItems[1 + i].item.ItemName;

            if (2 + i < accessoryItems.Count)
                EquipItemSelectSlot3.text = accessoryItems[2 + i].item.ItemName;

            if (3 + i < accessoryItems.Count)
                EquipItemSelectSlot4.text = accessoryItems[3 + i].item.ItemName;

            if (4 + i < accessoryItems.Count)
                EquipItemSelectSlot5.text = accessoryItems[4 + i].item.ItemName;
        }
        catch (IndexOutOfRangeException)
        {

        }
    }
    public void EquipSelectorAllowDown()
    {
        if (EquipChangeSelecter)
        {
            if (OnWeaponEquipSelecting)
            {
                List<CarryItem> weaponItems = inventory.GetItemsByType(Item.ItemType.Weapon);
                if (weaponItems == null || weaponItems.Count <= 5) return;
                EquipSelectorcursorPosition = Mathf.Min(++EquipSelectorcursorPosition, weaponItems.Count - 5);
                WeaponSelectorChange(EquipSelectorcursorPosition,weaponItems);
            }
            else
            {
                List<CarryItem> accessoryItems = inventory.GetItemsByType(Item.ItemType.Accessory);
                if (accessoryItems == null || accessoryItems.Count<=5) return;
                EquipSelectorcursorPosition = Mathf.Min(++EquipSelectorcursorPosition, accessoryItems.Count - 5);
                AccessorySelectorChange(EquipSelectorcursorPosition, accessoryItems);
            }
            EquipDetailTextUpdate(SelectorEquipIDKeeper);
        }
        else if (ItemPanel.activeSelf)
        {
            ItemListAllowDown();
        }
    }
    public void EquipSelectorAllowUp()
    {
        if (EquipChangeSelecter)
        {
            if (OnWeaponEquipSelecting)
            {
                List<CarryItem> weaponItems = inventory.GetItemsByType(Item.ItemType.Weapon);
                if (weaponItems == null || weaponItems.Count <= 5) return;
                EquipSelectorcursorPosition = Mathf.Max(--EquipSelectorcursorPosition, 0);
                WeaponSelectorChange(EquipSelectorcursorPosition, weaponItems);
            }
            else
            {
                List<CarryItem> accessoryItems = inventory.GetItemsByType(Item.ItemType.Accessory);
                if (accessoryItems == null || accessoryItems.Count <= 5) return;
                EquipSelectorcursorPosition = Mathf.Max(--EquipSelectorcursorPosition, 0);
                AccessorySelectorChange(EquipSelectorcursorPosition, accessoryItems);
            }
            EquipDetailTextUpdate(SelectorEquipIDKeeper);
        }
        else if (ItemPanel.activeSelf)
        {
            ItemListAllowUp();
        }
    }
    public void OnEquipSelectorClicked(int slotID)
    {
        if (OnPlayerEquipSelecting == true)
        {
            if (OnWeaponEquipSelecting)
            {
                List<CarryItem> weaponItems = inventory.GetItemsByType(Item.ItemType.Weapon);
                EquipWeaponName.text = weaponItems[EquipSelectorcursorPosition+slotID - 1].item.ItemName;
                p.EquipWeapon(weaponItems[EquipSelectorcursorPosition+slotID - 1].item);
            }
            else
            {
                List<CarryItem> accessoryItems = inventory.GetItemsByType(Item.ItemType.Accessory);
                EquipAccessoryName.text = accessoryItems[slotID - 1].item.ItemName;
                p.EquipAccessory(accessoryItems[EquipSelectorcursorPosition + slotID - 1].item);
            }
            UpdateStatus(p);
        }
        else
        {
            List<CarryItem> accessoryItems = inventory.GetItemsByType(Item.ItemType.Accessory);
            EquipAccessoryName.text = accessoryItems[slotID - 1].item.ItemName;
            n.EquipAccessory(accessoryItems[EquipSelectorcursorPosition + slotID - 1].item);
            UpdateStatus(n);
        }
        EquipSlots = true;
        EquipCharacterSelecter = true;
        EquipChangeSelecter = false;
        EquipPanelDisable1.SetActive(false);
        EquipPanelDisable2.SetActive(false);
        EquipPanelDisable3.SetActive(true);
        EquipItemSelectSlot1.text = "";
        EquipItemSelectSlot2.text = "";
        EquipItemSelectSlot3.text = "";
        EquipItemSelectSlot4.text = "";
        EquipItemSelectSlot5.text = "";
        OnEquipSelectorExit();
    }
    public void OnEquipSelectorEntered(int slotID)
    {
        SelectorItemIDKeeper = slotID;
        EquipItemFlavorPanel.SetActive(true);
        EquipDetailTextUpdate(slotID);
    }
    public void EquipDetailTextUpdate(int slotID)
    {
        if (OnWeaponEquipSelecting)
        {
            List<CarryItem> weaponItems = inventory.GetItemsByType(Item.ItemType.Weapon);
            Item item = weaponItems[EquipSelectorcursorPosition + slotID - 1].item;
            Weapon SelectWeapon = item as Weapon;
            EquipItemFlavorText.text = $"攻撃力：{SelectWeapon.Atk}\nデバッグ時間：{SelectWeapon.TimeLimit}s\nレアリティ：{SelectWeapon.Rarity}\n\n{SelectWeapon.Flavor}";
        }
        else
        {
            List<CarryItem> accessoryItems = inventory.GetItemsByType(Item.ItemType.Accessory);
            Item item = accessoryItems[EquipSelectorcursorPosition + slotID - 1].item;
            Accessory SelectAccessory = item as Accessory;
            EquipItemFlavorText.text = $"防御力：{SelectAccessory.Def}\nレアリティ：{SelectAccessory.Rarity}\n\n{SelectAccessory.Flavor}";
        }
    }
    public void OnEquipSelectorExit()
    {
        EquipItemFlavorPanel.SetActive(false);
        EquipItemFlavorText.text = "";
    }
    //ここからインベントリ関連の制御
    public void SupportItemSelectorChange(int i, List<CarryItem> supportItems)
    {
        try
        {

            if (0 + i < supportItems.Count)
            {
                InventoryItemSelectSlot1.text = supportItems[0 + i].item.ItemName;
                InventoryItemValue1.text = $"{supportItems[0 + i].quantity}";
            }
            if(1 + i < supportItems.Count)
            {
                InventoryItemSelectSlot2.text = supportItems[1 + i].item.ItemName;
                InventoryItemValue2.text = $"{supportItems[1 + i].quantity}";
            }
            if(2 + i < supportItems.Count)
            {
                InventoryItemSelectSlot3.text = supportItems[2 + i].item.ItemName;
                InventoryItemValue3.text = $"{supportItems[2 + i].quantity}";
            }
            if(3 + i < supportItems.Count)
            {
                InventoryItemSelectSlot4.text = supportItems[3 + i].item.ItemName;
                InventoryItemValue4.text = $"{supportItems[3 + i].quantity}";
            }
            if(4 + i < supportItems.Count)
            {
                InventoryItemSelectSlot5.text = supportItems[4 + i].item.ItemName;
                InventoryItemValue5.text = $"{supportItems[4 + i].quantity}";
            }
        }
        catch(IndexOutOfRangeException)
        {

        }
    }
    public void OnItemSelectorClicked(int slotID)
    {
        List<CarryItem> supportItems = inventory.GetItemsByType(Item.ItemType.SupportItem);
        SupportItem focus = supportItems[slotID + InventoryItemCursor - 1].item as SupportItem;
        switch (focus.EffectID)
        {
            case 1:
                if(p.MaxHP > p.CurrentHP)
                {
                    PlayerItemValidText.text = $"HPが{Mathf.Min(focus.EffectSize,p.MaxHP-p.CurrentHP)}回復";
                    PlayerTarget = true;
                }
                else
                {
                    PlayerItemValidText.text = "HPが最大値";
                    PlayerTarget = false;
                }
                if (n.MaxHP > n.CurrentHP)
                {
                    NetoItemValidText.text = $"HPが{Mathf.Min(focus.EffectSize, n.MaxHP - n.CurrentHP)}回復";
                    NetoTarget = true;
                }
                else
                {
                    NetoItemValidText.text = "HPが最大値";
                    NetoTarget = false;
                }
                break;
            case 2:
                if (focus.EffectSize > p.TemporaryAtk)
                {
                    PlayerItemValidText.text = $"攻撃力+{focus.EffectSize - p.TemporaryAtk}";
                    PlayerTarget = true;
                }
                else
                {
                    PlayerItemValidText.text = "より強い効果を使用中";
                    PlayerTarget = false;
                }
                NetoItemValidText.text = "ネトは攻撃力を持ちません";
                NetoTarget = false;
                break;
            case 3:
                if (focus.EffectSize > p.TemporaryDef)
                {
                    PlayerItemValidText.text = $"防御力+{focus.EffectSize - p.TemporaryDef}";
                    PlayerTarget = true;
                }
                else
                {
                    PlayerItemValidText.text = "より強い効果を使用中";
                    PlayerTarget = false;
                }
                if (focus.EffectSize > n.TemporaryDef)
                {
                    NetoItemValidText.text = $"防御力+{focus.EffectSize - n.TemporaryDef}";
                    NetoTarget = true;
                }
                else
                {
                    NetoItemValidText.text = "より強い効果を使用中";
                    NetoTarget = false;
                }
                break;
        }
        InventorySlotIdHolder = slotID;
        ItemTargetSelectPanel.SetActive(true);
        ItemFlavorText.text = focus.Flavor;
    }
    public void OnItemSelectorEntered(int slotID)
    {
        SelectorEquipIDKeeper = slotID;
        ItemDetailUpdate(slotID);
    }
    public void OnItemSelectorExit()
    {
        ItemFlavorText.text = "";
    }
    public void ItemDetailUpdate(int slotID)
    {
        List<CarryItem> supportItems = inventory.GetItemsByType(Item.ItemType.SupportItem);
        SupportItem focus = supportItems[slotID + InventoryItemCursor - 1].item as SupportItem;
        ItemFlavorText.text = focus.Flavor;
    }
    public void OnItemTargetButton(int CharaID)
    {
        if (CharaID == 0 && PlayerTarget == false) return;
        if (CharaID == 1 && NetoTarget == false) return;
        ItemTargetSelectPanel.SetActive(false);
        ItemConfirmPanel.SetActive(true);
        CharaIdHolder = CharaID;
    }
    public void OnItemReconfirmAccept()
    {
        List<CarryItem> supportItems = inventory.GetItemsByType(Item.ItemType.SupportItem);
        Item focus = supportItems[InventorySlotIdHolder + InventoryItemCursor - 1].item;
        if (CharaIdHolder == 0)
        {
            p.ApplyEffect(focus);
        }
        else if(CharaIdHolder == 1)
        {
            n.ApplyEffect(focus);
        }
        UpdateHpSlider();
        ItemFlavorText.text = "";
        ItemConfirmPanel.SetActive(false);
        inventory.RemoveItem(focus.ItemID, 1);
        List<CarryItem> ResupportItems = inventory.GetItemsByType(Item.ItemType.SupportItem);
        
        if (InventoryItemCursor + 5 > ResupportItems.Count)
        {
            InventoryItemCursor--;
        }
        SupportItemSelectorChange(InventoryItemCursor, ResupportItems);
    }
    public void OnItemReconfirmCancel()
    {
        ItemConfirmPanel.SetActive(false);
        ItemTargetSelectPanel.SetActive(true);
    }
    public void ItemListAllowDown()
    {
        List<CarryItem> supportItems = inventory.GetItemsByType(Item.ItemType.SupportItem);
        if (supportItems == null || supportItems.Count <= 5) return;
        InventoryItemCursor = Mathf.Min(++InventoryItemCursor, supportItems.Count - 5);
        SupportItemSelectorChange(InventoryItemCursor, supportItems);
        if (ItemFlavorText.text == "") return;
        ItemDetailUpdate(SelectorItemIDKeeper);
    }
    public void ItemListAllowUp()
    {
        List<CarryItem> supportItems = inventory.GetItemsByType(Item.ItemType.SupportItem);
        if (supportItems == null || supportItems.Count <= 5) return;
        InventoryItemCursor = Mathf.Max(--InventoryItemCursor, 0);
        SupportItemSelectorChange(InventoryItemCursor, supportItems);
        if (ItemFlavorText.text == "") return;
        ItemDetailUpdate(SelectorItemIDKeeper);
    }
    public void ChangeScreenSize(int i)
    {
        Screen.SetResolution(320*i, 180*i, FullScreenMode.Windowed);
    }
    public void OnGameEndButton()
    {
        ConfigPanelGameEnd.SetActive(true);
    }
    public void OnTitleBackCancel()
    {
        ConfigPanelGameEnd.SetActive(false);
    }
    public void OnTitleBackExectute()
    {
        SceneManager.LoadScene("BootScene");
    }
    //ここから会話系の制御
    public void TalkingEventStart()
    {
        TalkTextBox.text = "";
        TalkTextBoxPanel.SetActive(true);
        GameManager.Instance.SetMode(GameMode.Talk);
    }
    public void TalkingFowarded(string text)
    {
        TalkTextBox.text = text;
    }
    public void TalkingEventEnd()
    {
        TalkTextBoxPanel.SetActive(false);
        GameManager.Instance.SetMode(GameMode.Field);
    }
    public void TreasureTakeEventStart(string itemname)
    {
        TalkTextBoxPanel.SetActive(true);
        TalkTextBox.text = itemname + "を手に入れた！";
        GameManager.Instance.SetMode(GameMode.Talk);
    }
    public void TreasureTakeEventEnd()
    {
        TalkTextBox.text = "";
        TalkTextBoxPanel.SetActive(false);
        GameManager.Instance.SetMode(GameMode.Field);
    }
    public void DojoTalkingEventStart()
    {
        TalkTextBox.text = "";
        TalkTextBoxPanel.SetActive(true);
        GameManager.Instance.SetMode(GameMode.Talk);
    }
    //セーブのためのパネルを表示するメソッド
    public void SavePanelEnable()
    {
        SaveLoadPanel.SetActive(true);
        SaveSlotId = 1;
        Scene currentScene = SceneManager.GetActiveScene();
        string path1 = Path.Combine(Application.persistentDataPath, "save1.json");
        string path2 = Path.Combine(Application.persistentDataPath, "save2.json");
        try
        {
            if (SaveSlotId == 1)
            {
                string json1 = File.ReadAllText(path1);
                SaveLoadManager.SaveData loadedData1 = JsonUtility.FromJson<SaveLoadManager.SaveData>(json1);
                SaveDetailText.text = $"{loadedData1.playername}\n{GameManager.Instance.mapManager.MapNameConvertor(loadedData1.currentMapName)}  {loadedData1.saveDate}\nLv{loadedData1.currentlv}  Exp:{loadedData1.exp}/{loadedData1.currentlv * 100}";
            }
            else
            {
                string json2 = File.ReadAllText(path2);
                SaveLoadManager.SaveData loadedData2 = JsonUtility.FromJson<SaveLoadManager.SaveData>(json2);
                SaveDetailText.text = $"{loadedData2.playername}\n{GameManager.Instance.mapManager.MapNameConvertor(loadedData2.currentMapName)}  {loadedData2.saveDate}\nLv{loadedData2.currentlv}  Exp:{loadedData2.exp}/{loadedData2.currentlv * 100}";
            }
        }
        catch (Exception)
        {
            SaveDetailText.text = "セーブデータがありません。";
        }
    }
    public void SaveExecute()
    {
        GameManager.Instance.SaveManage(SaveSlotId);
        string path1 = Path.Combine(Application.persistentDataPath, "save1.json");
        string path2 = Path.Combine(Application.persistentDataPath, "save2.json");
        try
        {
            if (SaveSlotId == 1)
            {
                string json1 = File.ReadAllText(path1);
                SaveLoadManager.SaveData loadedData1 = JsonUtility.FromJson<SaveLoadManager.SaveData>(json1);
                SaveDetailText.text = $"{loadedData1.playername}\n{GameManager.Instance.mapManager.MapNameConvertor(loadedData1.currentMapName)}  {loadedData1.saveDate}\nLv{loadedData1.currentlv}  Exp:{loadedData1.exp}/{loadedData1.currentlv * 100}";
            }
            else
            {
                string json2 = File.ReadAllText(path2);
                SaveLoadManager.SaveData loadedData2 = JsonUtility.FromJson<SaveLoadManager.SaveData>(json2);
                SaveDetailText.text = $"{loadedData2.playername}\n{GameManager.Instance.mapManager.MapNameConvertor(loadedData2.currentMapName)}  {loadedData2.saveDate}\nLv{loadedData2.currentlv}  Exp:{loadedData2.exp}/{loadedData2.currentlv * 100}";
            }
        }
        catch (Exception)
        {
            SaveDetailText.text = "セーブデータがありません。";

        }
    }
    public void SavePanelDisable()
    {
        GameManager.Instance.SetMode(GameManager.GameMode.Field);
        SaveLoadPanel.SetActive(false);
    }
    public void SaveSlotChange(int i)
    {
        SaveSlotId = i;
        string path1 = Path.Combine(Application.persistentDataPath, "save1.json");
        string path2 = Path.Combine(Application.persistentDataPath, "save2.json");
        try 
        {
            if (SaveSlotId == 1)
            {
                string json1 = File.ReadAllText(path1);
                SaveLoadManager.SaveData loadedData1 = JsonUtility.FromJson<SaveLoadManager.SaveData>(json1);
                SaveDetailText.text = $"{loadedData1.playername}\n{GameManager.Instance.mapManager.MapNameConvertor(loadedData1.currentMapName)}  {loadedData1.saveDate}\nLv{loadedData1.currentlv}  Exp:{loadedData1.exp}/{loadedData1.currentlv * 100}";
            }
            else
            {
                string json2 = File.ReadAllText(path2);
                SaveLoadManager.SaveData loadedData2 = JsonUtility.FromJson<SaveLoadManager.SaveData>(json2);
                SaveDetailText.text = $"{loadedData2.playername}\n{GameManager.Instance.mapManager.MapNameConvertor(loadedData2.currentMapName)}  {loadedData2.saveDate}\nLv{loadedData2.currentlv}  Exp:{loadedData2.exp}/{loadedData2.currentlv * 100}";
            }
        }
        catch (Exception)
        {
            SaveDetailText.text = "セーブデータがありません。";
        }
    }
}