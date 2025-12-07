using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 【UI管理】
/// 画面の表示切り替えや、テキストの更新を行うクラスです。
/// </summary>
public class UIManager : MonoBehaviour
{
    [Header("HUD (常時表示するもの)")]
    [SerializeField] private Text playerStatusText; // プレイヤーのHPなどを表示
    [SerializeField] private Text netoStatusText;   // ネトのHPを表示
    [SerializeField] private Text logText;          // ゲーム内のログメッセージを表示
    [SerializeField] private GameObject inventoryPanel; // アイテム一覧画面

    [Header("各モードの画面パネル")]
    [SerializeField] private GameObject battlePanel;    // 戦闘画面
    [SerializeField] private Text battleQuestText;       // 問題表示
    [SerializeField] private Text battleInfoText;
    [SerializeField] private InputField answerInput;    // 記述式回答の入力欄
    [SerializeField] private GameObject shopPanel;      // お店画面
    [SerializeField] private GameObject dojoPanel;      // 道場画面
    [SerializeField] private GameObject itemDebugPanel; // アイテムデバッグ画面

    /// <summary>
    /// プレイヤーとネトのHP表示を更新します
    /// </summary>
    public void Start()
    {
        battleQuestText.text = "test";
    }
    public void UpdateStatus(Player p, Neto n)
    {
        // テキストコンポーネントが存在する場合のみ更新します
        if (playerStatusText != null)
        {
            playerStatusText.text = $"Player HP: {p.CurrentHP}/{p.MaxHP}\nATK: {p.CurrentAtk} DEF: {p.CurrentDef}";
        }

        if (netoStatusText != null)
        {
            netoStatusText.text = $"Neto HP: {n.CurrentHP}/{n.MaxHP}";
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
            battleQuestText.text = text+"\n"+"A:"+opts[0]+ "B:" + opts[1] + "C:" + opts[2] + "D:" + opts[3] ;
        }
    }

    // --- パネルの表示/非表示を切り替えるメソッド群 ---

    public void ToggleInventory(bool show)
    {
        inventoryPanel.SetActive(show);
    }

    public void ToggleBattle(bool show)
    {
        battlePanel.SetActive(show);
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
        Debug.Log(answer);
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
}