using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    private void Start()
    {
        StartPanel.SetActive(true);
        LoadPanel.SetActive(false);
    }
    //public void OnStartClicked()
    //{
    //    nameInputPanel.SetActive(true);
    //}
    //public void OnNameDecided()
    //{
    //    string name = nameInputField.text;

    //    // 名前が空でなければ保存してゲーム開始
    //    if (!string.IsNullOrEmpty(name))
    //    {
    //        PlayerPrefs.SetString("PlayerName", name);
    //        SceneManager.LoadScene("GameScene");
    //    }
    //}
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

    }
    public void SampleLoadButtonClicked()
    {
        GameManager.Instance.mapManager.TransAnotherMap("LamentForest", 2);
    }
}