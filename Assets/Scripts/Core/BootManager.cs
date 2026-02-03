using UnityEngine;
using UnityEngine.SceneManagement;


public class BootManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Screen.SetResolution(1280, 720, FullScreenMode.Windowed);
        GameManager.Instance.mapManager.TransAnotherMap("Title", 2);
    }
    void Update()
    {

    }
}
