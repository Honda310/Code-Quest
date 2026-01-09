using UnityEngine;
using UnityEngine.SceneManagement;


public class BootManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.Instance.mapManager.TransAnotherMap("LamentForest",2);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
