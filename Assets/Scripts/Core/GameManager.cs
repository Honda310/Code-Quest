using UnityEngine;

/// <summary>
/// 【ゲーム進行管理】
/// ゲーム全体の監督役となるクラスです。
/// シングルトンパターンを用いて、どこからでもアクセスできるようにしています。
/// </summary>
public class GameManager : MonoBehaviour
{
    // 外部から GameManager.Instance でアクセスできるようにする変数
    public static GameManager Instance { get; private set; }

    [Header("マネージャー群 (システム)")]
    // 各機能ごとの管理クラスをここに登録します
    public UIManager uiManager;             // UI（画面表示）の管理
    public DataManager dataManager;         // データ（CSVなど）の読み込み管理
    public QuestManager questManager;       // クイズ問題の管理
    public EventManager eventManager;       // 会話などのイベント管理
    public SaveLoadManager saveLoadManager; // セーブ・ロード機能
    public AudioManager audioManager;       // BGM・効果音
    public MapManager mapManager;           // シーン移動

    [Header("マネージャー群 (ゲーム独自機能)")]
    public ItemDebugManager itemDebugManager; // アイテムデバッグ機能
    public ShopManager shopManager;           // ショップ機能
    public DojoManager dojoManager;           // ネト道場機能

    [Header("プレイヤーデータ")]
    public Player player;       // 主人公
    public Neto neto;           // パートナー
    public Inventory inventory; // アイテム所持状況

    /// <summary>
    /// ゲーム起動時に最初に呼ばれる処理
    /// </summary>
    private void Awake()
    {
        // シングルトン化の処理
        // シーン移動してもこのオブジェクトが破壊されないようにします
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // すでに存在している場合は、重複しないように自分を削除します
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Awakeの後に呼ばれる初期化処理
    /// </summary>
    private void Start()
    {
        // マスタデータをCSVから読み込みます
        dataManager.LoadAllData();

        // クイズデータを読み込みます
        questManager.LoadQuests();

        // ログに起動完了を表示します
        uiManager.ShowLog("システム起動完了。");
    }

    /// <summary>
    /// ゲームを終了する処理
    /// </summary>
    public void EndGame()
    {
        // 正常終了時はオートセーブを削除して、次回の再開位置をリセットします
        saveLoadManager.DeleteAutoSave();

        // エディタ実行中と実際のアプリで終了処理を分けます
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}