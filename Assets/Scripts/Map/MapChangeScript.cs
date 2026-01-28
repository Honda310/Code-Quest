using UnityEngine;

public class MapChangeScript : MonoBehaviour
{
    [SerializeField] private int TransId;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") == false) return;
        switch (TransId)
        {
            case 0:
                GameManager.Instance.mapManager.TransAnotherMap("ToNeto", TransId);
                break;
            case 1:
                GameManager.Instance.mapManager.TransAnotherMap("InFrontOfLamentForest", TransId);
                break;
            case 2:
                GameManager.Instance.mapManager.TransAnotherMap("LamentForest", TransId);
                break;
            case 3:
                GameManager.Instance.mapManager.TransAnotherMap("PoisonedSpring", TransId);
                break;
        }
    }
}
