using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// シーン遷移（マップ移動）を管理するクラスです。
/// </summary>
public class MapManager : MonoBehaviour
{
    public void TransAnotherMap(string sceneName)
    {
        Debug.Log($"マップ移動: {sceneName}");
        SceneManager.LoadScene(sceneName);
    }
}