using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// 【タイトル画面】
/// ゲームの開始処理や、プレイヤー名の入力を担当します。
/// </summary>
public class GameStartManager : MonoBehaviour
{
    //public GameObject nameInputPanel; 
    //public InputField nameInputField; 
    [SerializeField] private GameObject StartPanel;
    [SerializeField] private GameObject LoadPanel;
    [SerializeField] private Text SaveSlotText0;
    [SerializeField] private Text SaveSlotText1;
    [SerializeField] private Text SaveSlotText2;
    [SerializeField] private GameObject NameInputPanel;
    [SerializeField] private Text NameInputText;
    [SerializeField] private Text OverLengthText;
    private SaveLoadManager.SaveData loadedData1;
    private SaveLoadManager.SaveData loadedData2;
    private void Start()
    {
        StartPanel.SetActive(true);
        LoadPanel.SetActive(false);
        string path1 = Path.Combine(Application.persistentDataPath, "save1.json");
        string path2 = Path.Combine(Application.persistentDataPath, "save2.json");
        string json1 = File.ReadAllText(path1);
        string json2 = File.ReadAllText(path2);
        loadedData1 = JsonUtility.FromJson<SaveLoadManager.SaveData>(json1);
        loadedData2 = JsonUtility.FromJson<SaveLoadManager.SaveData>(json2);
        SaveSlotText1.text = $"{loadedData1.playername}\n{loadedData1.currentMapName}  {loadedData1.saveDate}\nLv{loadedData1.currentlv}  Exp:{loadedData1.exp}/{loadedData1.currentlv * 100}";
        SaveSlotText2.text = $"{loadedData2.playername}\n{loadedData2.currentMapName}  {loadedData2.saveDate}\nLv{loadedData2.currentlv}  Exp:{loadedData2.exp}/{loadedData1.currentlv * 100}";
    }
    public void OnStartClicked()
    {
        NameInputPanel.SetActive(true);
    }
    public void OnNameDecided()
    {
        string name = NameInputText.text;
        int NameLength = GetTextLength(name);

        if (!string.IsNullOrEmpty(name)&& NameLength <= 12)
        {
            NameInputPanel.SetActive(false);
            GameManager.Instance.player.PlayerName = name;
            GameManager.Instance.mapManager.TransAnotherMap("ToNeto", 0);
        }
        else
        {
            if (NameLength%2==0)
            {
                OverLengthText.text = $"この名前は{NameLength/2}文字扱いです。\n意図しない全角やスペースが入っていませんか？";
            }
            else
            {
                OverLengthText.text = $"この名前は{NameLength / 2}.5文字扱いです。\n意図しない全角やスペースが入っていませんか？";
            }
        }
    }
    private int GetTextLength(string text)
    {
        int length = 0;

        foreach (char c in text)
        {
            length += (c <= 0x7F) ? 1 : 2;
        }

        return length;
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)|| Input.GetKeyDown(KeyCode.X))
        {
            StartPanel.SetActive(true);
            LoadPanel.SetActive(false);
        }
    }
    public void OnGameStartButtonClicked()
    {
        NameInputPanel.SetActive(true);
    }
    public void OnGameContinueButtonClicked()
    {
        StartPanel.SetActive(false);
        LoadPanel.SetActive(true);
    }
    public void OnEndButtonClicked()
    {

    }
    public void OnLoadingButtonClicked(int i)
    {
        GameManager.Instance.saveLoadManager.LoadGame(i);
    }
    public void SampleLoadButtonClicked()
    {
        GameManager.Instance.mapManager.TransAnotherMap("LamentForest", 4);
    }
}