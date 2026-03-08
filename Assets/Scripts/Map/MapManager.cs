using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// シーン遷移（マップ移動）を管理するクラスです。
/// </summary>
public class MapManager : MonoBehaviour
{
    Dictionary<string,string> MapNameConvert;
    private void Start()
    {
        MapNameConvert = new Dictionary<string,string>();
        MapNameConvert["ToNeto"] = "はじまりの道";
        MapNameConvert["InFrontOfLamentForest"] = "嘆きの森前";
        MapNameConvert["LamentForest"] = "嘆きの森";
        MapNameConvert["PoisonedSpring"] = "毒害泉源";
        MapNameConvert["ErrorVillage"] = "エラー集落";
        MapNameConvert["CorrupedTown"] = "衰微市街";
        MapNameConvert["Unknown"] = "???";
        MapNameConvert["Temple"] = "神殿";
    }
    public void TransAnotherMap(string sceneName,int spawnID)
    {
        SceneManager.LoadScene(sceneName);
        GameManager.Instance.spawnPlayer.CharacterSpawn(spawnID);
    }
    public string MapNameConvertor(string sceneName)
    {
        return MapNameConvert[sceneName];
    }
}