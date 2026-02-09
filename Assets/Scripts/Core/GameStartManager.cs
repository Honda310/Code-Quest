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
    //        //プレイヤーネーム決定場所
    //        SceneManager.LoadScene("");
    //    }
    //    enemyList.EnemyListTake(Gamemanager.Instance.DataManager.EnemyMaster);
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
        GameManager.Instance.saveLoadManager.LoadGame(i);
    }
    public void SampleLoadButtonClicked()
    {
        GameManager.Instance.mapManager.TransAnotherMap("LamentForest", 2);
    }
}