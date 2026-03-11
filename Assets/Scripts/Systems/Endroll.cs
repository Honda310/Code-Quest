using UnityEngine;
using UnityEngine.SceneManagement;

public class EndRoll : MonoBehaviour
{
    public float scrollSpeed = 50f;
    private float scrollTotal = 0f;
    void Update()
    {
        transform.Translate(Vector3.up * scrollSpeed * Time.deltaTime);
        scrollTotal = scrollSpeed * Time.deltaTime;
        if(scrollTotal > 400f)
        {
            SceneManager.LoadScene("GameStartScene");
        }
    }
}