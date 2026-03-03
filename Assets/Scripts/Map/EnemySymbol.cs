using UnityEngine;
using static GameManager;

public class EnemySymbol : MonoBehaviour
{
    [SerializeField] private int EnemyID;
    [SerializeField] private  Enemy enemy;
    private void Start()
    {
        // ‚·‚Å‚É“|‚³‚ê‚Ä‚¢‚é“G‚È‚çÁ‚·
        if (GameManager.Instance.enemyList.enemyDefeated[EnemyID])
        {
            Destroy(gameObject);
        }
    }
    
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (!col.gameObject.CompareTag("Player")) return;

        GameManager.Instance.RequestBattle(EnemyID,enemy);
    }
}