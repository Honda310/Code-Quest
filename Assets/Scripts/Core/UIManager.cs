using System;
using System.Collections.Generic;
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

    [Header("メニュー画面の各パネル")]
    [SerializeField] private GameObject MenuPanel;
    [SerializeField] private GameObject MenuBarPanel;
    [SerializeField] private GameObject MenuButtonPanel;
    [SerializeField] private GameObject ItemPanel;
    [SerializeField] private GameObject ItemTargetSelectPanel;
    [SerializeField] private GameObject ItemConfirmPanel;
    [SerializeField] private GameObject EquipandStatusPanel;
    [SerializeField] private GameObject ConfigPanel;
    [SerializeField] private GameObject KeyBindPanel;
    [SerializeField] private GameObject CharaSelector;
    [SerializeField] private GameObject NetoSelector;
    [SerializeField] private GameObject EquipPanelDisable1;
    [SerializeField] private GameObject EquipPanelDisable2;
    [SerializeField] private GameObject EquipPanelDisable3;

    [Header("装備&ステータス画面の各テキスト&ボタン")]
    [SerializeField] private Text CharaNameText;
    [SerializeField] private Text EquipWeaponName;
    [SerializeField] private Text EquipAccessoryName;
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

    [Header("会話イベントなどに使用するいろいろ")]
    [SerializeField] private GameObject TalkTextBoxPanel;
    [SerializeField] private Text TalkTextBox;

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
        if(gm == null) return; //Startより先に呼び出されるケースがあるらしい。マジ？
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
            CurrentHPText.text = $"  HP  : {n.CurrentHP}/{p.MaxHP}";
            CurrentAtkText.text = $"  ATK : None";
            CurrentDefText.text = $"  DEF : {n.CurrentDef}";
            CurrentDebugLimitText.text = $"  Lim : None";
        }
    }
    public void UpdateStatus()
    {
        if (NetoStatusText != null)
        {
            CurrentHPText.text = $"  HP  :";
            CurrentAtkText.text = $"  ATK :";
            CurrentDefText.text = $"  DEF :";
            CurrentDebugLimitText.text = $"  Lim :";
        }
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
            battleQuestText.text = text+"\n\n"+"A:"+opts[0]+ "　B:" + opts[1] + "　C:" + opts[2] + "　D:" + opts[3] ;
        }
    }
    /// <summary>
    /// 4択問題でA～Dのいずれかのボタンを押されたとき、または穴埋め問題で「解答送信」ボタンが押されたときに呼ばれるよ。
    /// クリックされたボタンのテキストを読み取ってanswerに代入→A～Dのいずれかなら4択問題用の処理を、
    /// それ以外（基本というか、間違いがなければ送信が送られたことになるけど）なら穴埋め問題用の処理を行うよ。
    /// </summary>
    public void OnSubmitButtonClicked(Button clickedButton)
    {

        string buttonText = clickedButton.GetComponentInChildren<Text>().text;
        string answer = buttonText;
        battleInfoText.text = answer;
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
            KeyBindPanel.SetActive(false);
            List<CarryItem> supportItems = inventory.GetItemsByType(Item.ItemType.SupportItem);
            SupportItemSelectorChange(0, supportItems);
        }
        
    }
    public void OnEquipAndStatusButtonClicked()
    {
        CurrentHPText.text = $"  HP:";
        CurrentAtkText.text = $"  ATK :";
        CurrentDefText.text = $"  DEF :";
        CurrentDebugLimitText.text = $"  Lim :";
        if (EquipandStatusPanel.activeSelf)
        {
            EquipandStatusPanel.SetActive(false);
        }
        else
        {
            ItemPanel.SetActive(false);
            EquipandStatusPanel.SetActive(true);
            ConfigPanel.SetActive(false);
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
            KeyBindPanel.SetActive(true);
        }
        
    }
    public void OnMenuCloseButtonClicked()
    {
        MenuPanel.SetActive(false);
        ItemPanel.SetActive(false);
        EquipandStatusPanel.SetActive(false);
        ConfigPanel.SetActive(false);
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
}
    
    public void MenuToggle()
    {
        if (MenuPanel.activeSelf == false)
        {
            CharaSelector.SetActive(false);
            NetoSelector.SetActive(false);
            MenuPanel.SetActive(true);
            MenuBarPanel.SetActive(true);
            EquipPanelDisable1.SetActive(false);
            EquipPanelDisable2.SetActive(false);
            EquipPanelDisable3.SetActive(false);
            EquipSlots = false;
            EquipCharacterSelecter = false;
            EquipChangeSelecter = false;
            gm.SetMode(GameMode.Menu);
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
        else
        {
            ItemPanel.SetActive(false);
            EquipandStatusPanel.SetActive(false);
            ConfigPanel.SetActive(false);
            KeyBindPanel.SetActive(false);
        }
    }
    //この辺にあるif([任意の文字列]==false) return;は、本来選べないボタンを選択/クリックさせないためのやつ
    public void OnPlayerIconClicked()
    {
        if (EquipCharacterSelecter == false)
        {
            Debug.Log("そのアイコンはfalseだよ");
            return;
        }
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
            if (weaponItems[0 + i].item != null) EquipItemSelectSlot1.text = weaponItems[0 + i].item.ItemName;
            if (weaponItems[1 + i].item != null) EquipItemSelectSlot2.text = weaponItems[1 + i].item.ItemName;
            if (weaponItems[2 + i].item != null) EquipItemSelectSlot3.text = weaponItems[2 + i].item.ItemName;
            if (weaponItems[3 + i].item != null) EquipItemSelectSlot4.text = weaponItems[3 + i].item.ItemName;
            if (weaponItems[4 + i].item != null) EquipItemSelectSlot5.text = weaponItems[4 + i].item.ItemName;
        }
        catch (IndexOutOfRangeException)
        {

        }
    }
    public void AccessorySelectorChange(int i, List<CarryItem> accessoryItems)
    {
        try
        {
            if (accessoryItems[0 + i].item != null) EquipItemSelectSlot1.text = accessoryItems[0 + i].item.ItemName;
            if (accessoryItems[1 + i].item != null) EquipItemSelectSlot2.text = accessoryItems[1 + i].item.ItemName;
            if (accessoryItems[2 + i].item != null) EquipItemSelectSlot3.text = accessoryItems[2 + i].item.ItemName;
            if (accessoryItems[3 + i].item != null) EquipItemSelectSlot4.text = accessoryItems[3 + i].item.ItemName;
            if (accessoryItems[4 + i].item != null) EquipItemSelectSlot5.text = accessoryItems[4 + i].item.ItemName;
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
                if (weaponItems == null) return;
                EquipSelectorcursorPosition = Mathf.Min(++EquipSelectorcursorPosition, weaponItems.Count - 5);
                WeaponSelectorChange(EquipSelectorcursorPosition,weaponItems);
            }
            else
            {
                List<CarryItem> accessoryItems = inventory.GetItemsByType(Item.ItemType.Accessory);
                if (accessoryItems == null) return;
                EquipSelectorcursorPosition = Mathf.Min(++EquipSelectorcursorPosition, accessoryItems.Count - 5);
                AccessorySelectorChange(EquipSelectorcursorPosition, accessoryItems);

            }
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
                if (weaponItems == null) return;
                EquipSelectorcursorPosition = Mathf.Max(--EquipSelectorcursorPosition, 0);
                WeaponSelectorChange(EquipSelectorcursorPosition, weaponItems);
            }
            else
            {
                List<CarryItem> accessoryItems = inventory.GetItemsByType(Item.ItemType.Accessory);
                if (accessoryItems == null) return;
                EquipSelectorcursorPosition = Mathf.Max(--EquipSelectorcursorPosition, 0);
                AccessorySelectorChange(EquipSelectorcursorPosition, accessoryItems);
            }
        }
        else if (ItemPanel.activeSelf)
        {
            ItemListAllowUp();
        }
    }
    public void OnEquipSelecterClicked(int slotID)
    {
        if (EquipChangeSelecter == false)
        {
            Debug.Log("EquipChangeSelectorがfalse");
            return;
        }
        Debug.Log(slotID+"個目のスロットが選択されたよ～");
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
    }
    //ここからインベントリ関連の制御
    public void SupportItemSelectorChange(int i, List<CarryItem> supportItems)
    {
        try
        {
            if (supportItems[0 + i].item != null) InventoryItemSelectSlot1.text = supportItems[0 + i].item.ItemName;
            if (supportItems[0 + i].item != null) InventoryItemValue1.text = $"{supportItems[0 + i].quantity}";
            if (supportItems[1 + i].item != null) InventoryItemSelectSlot2.text = supportItems[1 + i].item.ItemName;
            if (supportItems[0 + i].item != null) InventoryItemValue2.text = $"{supportItems[1 + i].quantity}";
            if (supportItems[2 + i].item != null) InventoryItemSelectSlot3.text = supportItems[2 + i].item.ItemName;
            if (supportItems[0 + i].item != null) InventoryItemValue3.text = $"{supportItems[2 + i].quantity}";
            if (supportItems[3 + i].item != null) InventoryItemSelectSlot4.text = supportItems[3 + i].item.ItemName;
            if (supportItems[0 + i].item != null) InventoryItemValue4.text = $"{supportItems[3 + i].quantity}";
            if (supportItems[4 + i].item != null) InventoryItemSelectSlot5.text = supportItems[4 + i].item.ItemName;
            if (supportItems[0 + i].item != null) InventoryItemValue5.text = $"{supportItems[4 + i].quantity}";
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
                if (p.MaxHP > p.CurrentHP)
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
        if (supportItems == null) return;
        InventoryItemCursor = Mathf.Min(++InventoryItemCursor, supportItems.Count - 5);
        SupportItemSelectorChange(InventoryItemCursor, supportItems);
    }
    public void ItemListAllowUp()
    {
        List<CarryItem> supportItems = inventory.GetItemsByType(Item.ItemType.SupportItem);
        if (supportItems == null) return;
        InventoryItemCursor = Mathf.Max(--InventoryItemCursor, 0);
        SupportItemSelectorChange(InventoryItemCursor, supportItems);
    }
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
        TalkTextBox.text = itemname + "を獲得した。";
        GameManager.Instance.SetMode(GameMode.Talk);
    }
    public void TreasureTakeEventEnd()
    {
        TalkTextBox.text = "";
        TalkTextBoxPanel.SetActive(false);
        GameManager.Instance.SetMode(GameMode.Field);
    }
}