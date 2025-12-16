using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

/// <summary>
/// 【UI管理】
/// 画面の表示切り替えや、テキストの更新を行うクラスです。
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
    [SerializeField] private Text battleQuestText;       // 問題表示
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
    [SerializeField] private GameObject ItemPanel;
    [SerializeField] private GameObject EquipandStatusPanel;
    [SerializeField] private GameObject ConfigPanel;
    [SerializeField] private GameObject KeyBindPanel;
    /// <summary>
    /// プレイヤーとネトのHP表示を更新します
    /// </summary>
    public void Start()
    {

    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.X))
        {
            MenuToggle ();
        }
    }
    public void UpdateStatus(Player p, Neto n,Enemy e)
    {
        // テキストコンポーネントが存在する場合のみ更新します
        if (PlayerStatusText != null)
        {
            PlayerStatusText.text = $"{p.CurrentHP}/{p.MaxHP}"; ;
            PlayerStatusSlider.maxValue = p.MaxHP;
            PlayerStatusSlider.minValue = 0;
            PlayerStatusSlider.value = p.CurrentHP;
        }
        if (NetoStatusText != null)
        {
            NetoStatusText.text = $"{n.CurrentHP}/{n.MaxHP}"; ;
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

    /// <summary>
    /// 画面上のログウィンドウにメッセージを追加します
    /// </summary>
    public void ShowLog(string message)
    {
        // Unityのコンソールにも出す
        Debug.Log("[Log] " + message);

        if (logText != null)
        {
            // 新しいメッセージを一番上に追加して、履歴を残します
            logText.text = message + "\n" + logText.text;
        }
    }

    /// <summary>
    /// 戦闘画面のメッセージ（問題文など）を更新します
    /// </summary>
    public void UpdateBattleMessage(string text)
    {
        if (battleQuestText != null)
        {
            battleQuestText.text = text;
        }
    }
    public void UpdateBattleMessage(string text, string[] opts)
    {
        if (battleQuestText != null)
        {
            battleQuestText.text = text+"\n"+"A:"+opts[0]+ "　B:" + opts[1] + "　C:" + opts[2] + "　D:" + opts[3] ;
        }
    }

    // --- パネルの表示/非表示を切り替えるメソッド群 ---

    public void ToggleMenu(bool show)
    {
        MenuPanel.SetActive(show);
    }

    public void ToggleBattle(bool show)
    {
        battlePanel.SetActive(show);
    }
    public void Turnstart()
    {
        PlSelectPanel.SetActive(true);
        NetoselectPanel.SetActive(false);
        HealthDpSlidersAndCharactersPanel.SetActive(true);
        DifficultSelectPanel.SetActive(false);
        DifficultAndCheckButtonFramePanel.SetActive(false);
        DifficultAndSelectButtonFramePanel.SetActive(false);
        QuestFramePanel.SetActive(false);
    }

    public void ToggleShop(bool show)
    {
        shopPanel.SetActive(show);
    }

    public void ToggleDojo(bool show)
    {
        dojoPanel.SetActive(show);
    }

    public void ToggleItemDebug(bool show)
    {
        itemDebugPanel.SetActive(show);
    }

    /// <summary>
    /// 画面上の「解答送信」ボタンが押されたときに呼ばれます
    /// </summary>
    public void OnSubmitButtonClicked(Button clickedButton)
    {

        string buttonText = clickedButton.GetComponentInChildren<Text>().text;
        string answer = buttonText;
        GameManager.Instance.GetComponent<BattleManager>().OnSubmitAnswer(answer);
        battleInfoText.text = answer;
        // BattleManagerに入力されたテキストを渡します
        if ((answer=="A" || answer == "B" || answer == "C" || answer == "D")==false)
        {
            answer = answerInput.text;
            GameManager.Instance.GetComponent<BattleManager>().OnSubmitAnswer(answer);
            Debug.Log("fill");
        }
            // 入力欄を空にします
            answerInput.text = "";
    }
    //テンプレ用
    //public void OnButtonClicked(Button clickedButton)
    //{

    //}
    public void OnSelectNormalButtonClicked(Button clickedButton)
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
    public void OnSelectHardButtonClicked(Button clickedButton)
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
    public void OnReselectButtonClicked(Button clickedButton)
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
    public void OnAcceptButtonClicked(Button clickedButton)
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
    public void OnCancelButtonClicked(Button clickedButton)
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
    public void OnPlayerDebugButtonClicked(Button clickedButton)
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
    public void OnPlayerItemButtonClicked(Button clickedButton)
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
    public void OnNetoSearchButtonClicked(Button clickedButton)
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
    public void OnNetoItemButtonClicked(Button clickedButton)
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
    public void OnStatusButtonClicked()
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
        }
        else if (MenuPanel.activeSelf && (((ItemPanel.activeSelf || EquipandStatusPanel.activeSelf) || (ConfigPanel.activeSelf || KeyBindPanel.activeSelf))== false))
        {
            MenuPanel.SetActive(false);
        }
        else
        {
            ItemPanel.SetActive(false);
            EquipandStatusPanel.SetActive(false);
            ConfigPanel.SetActive(false);
            KeyBindPanel.SetActive(false);
        }
    }
}