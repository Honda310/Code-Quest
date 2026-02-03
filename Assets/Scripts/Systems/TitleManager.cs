using UnityEngine;

public class TitleManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    void Update()
    {
        if (Input.anyKeyDown)
        {
            GameManager.Instance.mapManager.TransAnotherMap("GameStartScene", 2);
        }
    }
}
