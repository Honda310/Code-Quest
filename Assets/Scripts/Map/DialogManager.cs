using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    Queue<string> currentLines;
    [SerializeField] private TalkEventList talkEventList;
    private bool inputAble=false;
    public void StartDialogue(int npcId)
    {
        currentLines = new Queue<string>(talkEventList.TalkEventTable[npcId]);
        ShowNext();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && GameManager.Instance.CurrentMode == GameManager.GameMode.Talk && inputAble)
        {
            ShowNext();
        }
    }

    void ShowNext()
    {
        StartCoroutine(InputCooldown());
        if (currentLines.Count == 0)
        {
            UIManager.Active?.TalkingEventEnd();
            inputAble = false;
            return;
        }
        string line = currentLines.Dequeue();
        UIManager.Active?.TalkingFowarded(line);
    }
    IEnumerator InputCooldown()
    {
        inputAble = false;
        yield return new WaitForSeconds(0.15f);
        inputAble = true;
    }
}
