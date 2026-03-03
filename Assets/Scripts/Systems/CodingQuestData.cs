using UnityEngine;
[System.Serializable]
public class CodingQuestData
{
    public int ID;
    public string QuestionText;
    public string[] CorrectAnswer;
    public string QuestionHint;
    public string SampleCode;
    public CodingQuestData(int id, string q, string[] a, string h,string s)
    {
        ID = id;
        QuestionText = q;
        CorrectAnswer = a;
        QuestionHint = h;
        SampleCode = s;
    }
}
