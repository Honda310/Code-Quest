using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{
    public void CharacterSpawn(int SpawnID)
    {
        switch (SpawnID)
        {
            case 0:
                GameManager.Instance.player.transform.position = new Vector3(90,50,0);
                GameManager.Instance.neto.transform.position = new Vector3(60,50,0);
                break;
            case 1:
                GameManager.Instance.player.transform.position = new Vector3();
                GameManager.Instance.neto.transform.position = new Vector3();
                break;
        }
    }
    
}
