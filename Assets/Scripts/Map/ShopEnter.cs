using UnityEngine;

public class ShopEnter : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb2D;
    [SerializeField] private UIManager uiManager;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            uiManager.ShopPanelOpen();
        }
    }
}
