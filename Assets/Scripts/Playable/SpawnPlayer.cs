using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{
    public void CharacterSpawn(int SpawnID)
    {
        switch (SpawnID)
        {
            case 0:
                GameManager.Instance.player.transform.position = new Vector3(-110,30,0);
                GameManager.Instance.neto.transform.position = new Vector3(-140,50,0);
                break;
            case 1:
                GameManager.Instance.player.transform.position = new Vector3(-100,60,0);
                GameManager.Instance.neto.transform.position = new Vector3(-130,60,0);
                break;
            case 2:
                GameManager.Instance.player.transform.position = new Vector3(90, 50, 0);
                GameManager.Instance.neto.transform.position = new Vector3(60, 50, 0);
                break;
            case 3:
                GameManager.Instance.player.transform.position = new Vector3(180,50,0);
                GameManager.Instance.neto.transform.position = new Vector3(150,50,0);
                break;
            case 99:
                GameManager.Instance.player.transform.position = new Vector3(0, 60, 0);
                GameManager.Instance.neto.transform.position = new Vector3(0, 100, 0);
                break;
        }
    }
    
}
