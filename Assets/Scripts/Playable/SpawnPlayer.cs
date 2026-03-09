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
            //“ÅŠQòŒ¹(oŒû‚ÉˆÚ“®)
            case 7:
                GameManager.Instance.player.transform.position = new Vector3(1920, -540, 0);
                GameManager.Instance.neto.transform.position = new Vector3(1920, -540, 0);
                break;
            //ƒGƒ‰[W—(“üŒû‚ÉˆÚ“®)
            case 8:
                GameManager.Instance.player.transform.position = new Vector3(320, -300, 0);
                GameManager.Instance.neto.transform.position = new Vector3(320, -300, 0);
                break;
            //ƒGƒ‰[W—(oŒû‚ÉˆÚ“®)
            case 9:
                GameManager.Instance.player.transform.position = new Vector3(320, 80, 0);
                GameManager.Instance.neto.transform.position = new Vector3(320, 110, 0);
                break;
            //Š”÷sŠX(“üŒû‚ÉˆÚ“®)
            case 10:
                GameManager.Instance.player.transform.position = new Vector3(270, -1150, 0);
                GameManager.Instance.neto.transform.position = new Vector3(240, -1150, 0);
                break;
            //Š”÷sŠX(oŒû‚ÉˆÚ“®)
            case 11:
                GameManager.Instance.player.transform.position = new Vector3(1350, 50, 0);
                GameManager.Instance.neto.transform.position = new Vector3(1320, 50, 0);
                break;
            //???(“üŒû‚ÉˆÚ“®)
            case 12:
                GameManager.Instance.player.transform.position = new Vector3(90, 50, 0);
                GameManager.Instance.neto.transform.position = new Vector3(60, 50, 0);
                break;
            //???(oŒû‚ÉˆÚ“®)
            case 13:
                GameManager.Instance.player.transform.position = new Vector3(1600, 50, 0);
                GameManager.Instance.neto.transform.position = new Vector3(1630, 50, 0);
                break; 
            //_“a(“üŒû‚ÉˆÚ“®)
            case 14:
                GameManager.Instance.player.transform.position = new Vector3(1570, -230, 0);
                GameManager.Instance.neto.transform.position = new Vector3(1570, -200, 0);
                break; 
            //_“a(oŒû‚ÉˆÚ“®)
            case 15:
                GameManager.Instance.player.transform.position = new Vector3(0, -950, 0);
                GameManager.Instance.neto.transform.position = new Vector3(0, -930, 0);
                break;
            //_“aB2F(“üŒû‚ÉˆÚ“®)
            case 16:
                GameManager.Instance.player.transform.position = new Vector3(-30, -2530, 0);
                GameManager.Instance.neto.transform.position = new Vector3(-30, -2530, 0);
                break;
            //ƒRƒ“ƒJƒŒ‚É‘JˆÚ
            case 17:
                GameManager.Instance.player.transform.position = new Vector3(-80, 105, 0);
                break;
            //ƒQ[ƒ€ƒXƒ^[ƒg’¼ŒãA‰Æ‚É‘JˆÚ
            case 18:
                GameManager.Instance.player.transform.position = new Vector3(100, -120, 0);
                break;
            case 99:
                GameManager.Instance.player.transform.position = new Vector3(0, 60, 0);
                GameManager.Instance.neto.transform.position = new Vector3(0, 100, 0);
                break;
            }
    }
}
