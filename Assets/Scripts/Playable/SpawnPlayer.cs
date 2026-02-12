using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{
    public void CharacterSpawn(int SpawnID)
    {
        switch (SpawnID)
        {
            //ƒlƒg‚Æ‰ï‚¤‚Ü‚Å‚Ì“¹(“üŒû)‚ÉˆÚ“®
            case 0:
                GameManager.Instance.player.transform.position = new Vector3(-110,30,0);
                GameManager.Instance.neto.transform.position = new Vector3(-140,50,0);
                break;
            //ƒlƒg‚Æ‰ï‚¤‚Ü‚Å‚Ì“¹(oŒû)‚ÉˆÚ“®
            case 1:
                //GameManager.Instance.player.transform.position = new Vector3(2350, -500, 0);
                //GameManager.Instance.neto.transform.position = new Vector3(2350, -500, 0);
                GameManager.Instance.player.transform.position = new Vector3(1000, -500, 0);
                GameManager.Instance.neto.transform.position = new Vector3(1000, -500, 0);
                break;
            //’Q‚«‚ÌX‘O(“üŒû)‚ÉˆÚ“®
            case 2:
                GameManager.Instance.player.transform.position = new Vector3(-100, 60, 0);
                GameManager.Instance.neto.transform.position = new Vector3(-130, 60, 0);
                break;
            //’Q‚«‚ÌX‘O(oŒû)‚ÉˆÚ“®
            case 3:
                GameManager.Instance.player.transform.position = new Vector3(400, 70, 0);
                GameManager.Instance.neto.transform.position = new Vector3(400, 70, 0);
                break;
            //’Q‚«‚ÌX(“üŒû)‚ÉˆÚ“®
            case 4:
                GameManager.Instance.player.transform.position = new Vector3(150, 50, 0);
                GameManager.Instance.neto.transform.position = new Vector3(150, 50, 0);
                break;
            //’Q‚«‚ÌX(oŒû)‚ÉˆÚ“®
            case 5:
                GameManager.Instance.player.transform.position = new Vector3(240, -1230, 0);
                GameManager.Instance.neto.transform.position = new Vector3(240, -1230, 0);
                break;
            //“ÅŠQòŒ¹(“üŒû)‚ÉˆÚ“®
            case 6:
                GameManager.Instance.player.transform.position = new Vector3(180, 50, 0);
                GameManager.Instance.neto.transform.position = new Vector3(150, 50, 0);
                break;
            case 99:
                GameManager.Instance.player.transform.position = new Vector3(0, 60, 0);
                GameManager.Instance.neto.transform.position = new Vector3(0, 100, 0);
                break;
            
            }
    }
    
}
