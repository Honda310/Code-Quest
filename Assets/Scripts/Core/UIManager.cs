using UnityEngine;
using UnityEngine.TextCore.Text;
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
    [SerializeField] private Text logText;          // ゲーム内のログメッセージを表示

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
    [SerializeField] private GameObject NetoselectPanel;
    [SerializeField] private GameObject HealthDpSlidersAndCharactersPanel;
    [SerializeField] private GameObject DifficultSelectPanel;
    [SerializeField] private GameObject QuestFramePanel;
    [SerializeField] private GameObject DifficultAndCheckButtonFramePanel;
    [SerializeField] private GameObject DifficultAndSelectButtonFramePanel;

    [Header("メニュー画面の各パネル")]
    [SerializeField] private GameObject MenuPanel;
    [SerializeField] private GameObject MenuBarPanel;
    [SerializeField] private GameObject ItemPanel;
    [SerializeField] private GameObject EquipandStatusPanel;
    [SerializeField] private GameObject ConfigPanel;
    [SerializeField] private GameObject KeyBindPanel;

    [Header("装備&ステータス画面の各テキスト&ボタン")]
    [SerializeField] private Text CharaNameText;
    [SerializeField] private Text EquipWeaponName;
    [SerializeField] private Text EquipAccessoryName;
    [SerializeField] private Text currentHPText;
    [SerializeField] private Text CurrentAtkSlot;
    [SerializeField] private Text CurrentDefSlot;
    [SerializeField] private Text CurrentDebugLimitText;
    [SerializeField] private Text EquipItemSelectSlot1;
    [SerializeField] private Text EquipItemSelectSlot2;
    [SerializeField] private Text EquipItemSelectSlot3;
    [SerializeField] private Text EquipItemSelectSlot4;
    [SerializeField] private Text EquipItemSelectSlot5;

    //装備&ステータス画面の制御系
    private bool EquipCharacterSelecter;
    private bool EquipSlots;
    private bool EquipChangeSelecter;


    /// <summary>
    /// プレイヤーとネトのHP表示を更新します
    /// </summary>
    private GameManager gm;
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
    ///メインの画面を除き、すべてのパネルを閉じるメソッドです。
    ///特にデバッグ時とか、あるいはビルド後にパネルのデフォルトがTrue担ってしまっているときに回避するために作成したよ。
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
    }

    ///<summary>
    ///フィールド上などで、キャラの現在ステータスをUIに反映するためのメソッドです。
    ///戦闘中はPlayer,Neto,Enemyの3引数がある方を使ってね。
    ///</summary>
    public void UpdateStatus(Player p, Neto n)
    {
        // テキストコンポーネントが存在する場合のみ更新します
        if (PlayerStatusText != null)
        {
            PlayerStatusText.text = $"Player HP: {p.CurrentHP}/{p.MaxHP}\nATK: {p.CurrentAtk} DEF: {p.CurrentDef}";
        }

        if (NetoStatusText != null)
        {
            NetoStatusText.text = $"Neto HP: {n.CurrentHP}/{n.MaxHP}";
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

    /// <summary>
    /// 戦闘ログを更新するためのメソッド。現状、通常会話を振り返ったりできる機能はない…でいいんだよね？
    /// </summary>
    public void ShowLog(string message)
    {
        // Unityのコンソールにも出す
        Debug.Log("[Log] " + message);

        if (logText != null)
        {
            logText.text = message + "\n" + logText.text;
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
            battleQuestText.text = text+"\n"+"A:"+opts[0]+ "　B:" + opts[1] + "　C:" + opts[2] + "　D:" + opts[3] ;
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
        NetoselectPanel.SetActive(true);
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
        NetoselectPanel.SetActive(false);
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
        NetoselectPanel.SetActive(false);
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
        HealthDpSlidersAndCharactersPanel.SetActive(true);
        PlSelectPanel.SetActive(true);
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

        }
        
    }
    public void OnEquipAndStatusButtonClicked()
    {
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
    }
    
    public void MenuToggle()
    {

        if (MenuPanel.activeSelf == false)
        {
            MenuPanel.SetActive(true);
            MenuBarPanel.SetActive(true);
            gm.SetMode(GameMode.Menu);
        }
        else if (MenuPanel.activeSelf && (((ItemPanel.activeSelf || EquipandStatusPanel.activeSelf) || (ConfigPanel.activeSelf || KeyBindPanel.activeSelf))== false))
        {
            MenuPanel.SetActive(false);
            MenuBarPanel.SetActive(false);
            gm.SetMode(GameMode.Field);
        }
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
        Debug.Log("キャラアイコンが選択されたよ");
        EquipSlots = true;
        EquipCharacterSelecter = false;
    }
    public void OnNetoIconClicked()
    {
        if (EquipCharacterSelecter == false)
        {
            Debug.Log("そのアイコンはfalseだよ");
            return;
        }
        Debug.Log("キャラアイコンが選択されたよ");
        EquipSlots = true;
        EquipCharacterSelecter = false;
    }
    public void OnWeaponSlotClicked(GameObject gameObject)
    {
        if (EquipSlots == false)
        {
            Debug.Log("そのボタンはfalseだよ～");
            return;
        }
        Debug.Log("武器スロットが選択されたよ");
        gameObject.GetComponentInChildren<Text>().text = "Selected";
        EquipSlots = false;
        EquipChangeSelecter = true;
    }
    public void OnAccessorySlotClicked(GameObject gameObject)
    {
        if (EquipSlots == false)
        {
            Debug.Log("そのボタンはfalseだよ～");
            return;
        }
        Debug.Log("アクセサリスロットが選択されたよ");
        gameObject.GetComponentInChildren<Text>().text = "Selected";
        EquipSlots = false;
        EquipChangeSelecter = true;
    }
    public void OnEquipSelecterClicked(int slotID)
    {
        if (EquipChangeSelecter == false) return;
        Debug.Log(slotID+"個目のスロットが選択されたよ～");
        EquipSlots = true;
        EquipChangeSelecter = false;
    }
}