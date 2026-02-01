using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 【道場機能】
/// ネト先生による学習モードを管理します。
/// </summary>
public class DojoManager : MonoBehaviour
{
    [SerializeField] private GameObject DojoSelectPanel;
    [SerializeField] private GameObject DojoTellPanel;
    [SerializeField] private Text NetoTellText;
    [SerializeField] private Text IndexLabel;

    private int IdxKeeper;
    private int BookMarker;
    string[] VariableContent = { "" };
    string[] IfContent = { "" };
    string[] ForWhileContent = { "" };
    string[] ArrayContent = { "【学習文章：配列 × for文】\n配列を使うときに、よく一緒に使うのが for文ネト。\nfor文は「決まった回数だけ繰り返す処理」を書くときに使うネト。"
                                ,"基本形はこれネト：\n\nfor (int i = 0; i < 5; i++) {\n    //繰り返す処理\n}\nint i = 0 … ループ用の変数iを宣言、0でスタート\ni < 5 … iが4までループ(5回繰り返す)\ni++ … ループのたびにiを1増やす\n\n配列と組み合わせると、配列の全要素を順番に処理できるネト。"
                                , "例えば：\n\nint[] nums = { 5, 8, 12 };\n\nfor (int i = 0; i < nums.length; i++) {\n    System.out.println(nums[i]);\n}\n\nこれで 配列の中身を0番目から順に表示できるネト。"
                                ,"次のコードを実行すると、最後に表示される値は何になるネト？\n\npublic class Main {\n    public static void main(String[] args) {\n        int[] nums = { 4, 9, 2, 7 };\n        for (int i = 0; i < nums.length; i++) {\n            System.out.println(nums[i]);\n        }\n    }\n}\n A:4 B:2 C:7 D:9"
                                ,"【解説】ネト\n配列 nums の中身は次ネト。\nインデックス：0 1 2 3\n値　　　　　：4 9 2 7\n\nfor文はこう動くネト：\ni = 0 → nums[0] = 4 を表示\ni = 1 → nums[1] = 9 を表示\ni = 2 → nums[2] = 2 を表示\ni = 3 → nums[3] = 7 を表示\n\nつまり、最後に出力されるのは 7 、正解は C ネト！"};
    int[] ArrayLengths = new int[4];
    public static DojoManager Active { get; private set; }
    private void Awake()
    {
        Active = this;
    }
    private void Start()
    {
        ArrayLengths[0] = VariableContent.Length;
        ArrayLengths[1] = IfContent.Length;
        ArrayLengths[2] = ForWhileContent.Length;
        ArrayLengths[3] = ArrayContent.Length;
    }
    private void Update()
    {
        if (GameManager.Instance.CurrentMode == GameManager.GameMode.Dojo)
        {
            if(Input.GetKeyDown(KeyCode.Escape)|| Input.GetKeyDown(KeyCode.X))
            {
                GameManager.Instance.SetMode(GameManager.GameMode.Field);
                DojoSelectPanel.SetActive(false);
                DojoTellPanel.SetActive(false);
            }
        }
    }
    public void SelectTopic(int idx)
    {
        BookMarker = 0;
        DojoSelectPanel.SetActive(false);
        DojoTellPanel.SetActive(true);
        IdxKeeper = idx;
        PageDisplay();
    }
    public void PageDisplay()
    {
        IndexLabel.text = $"{BookMarker + 1}/{ArrayLengths[IdxKeeper]}";
        switch (IdxKeeper)
        {
            case 0:
                NetoTellText.text = VariableContent[BookMarker];
                break;
            case 1:
                NetoTellText.text = IfContent[BookMarker];
                break;
            case 2:
                NetoTellText.text = ForWhileContent[BookMarker];
                break;
            case 3:
                NetoTellText.text = ArrayContent[BookMarker];
                break;
        }
    }
    public void PageSpinner(int spinIdx)
    {
        switch (spinIdx)
        {
            case 1:
                BookMarker = Mathf.Max(--BookMarker, 0);
                PageDisplay();
                break;
            case 2:
                BookMarker++;
                if (BookMarker == ArrayLengths[IdxKeeper])
                {
                    DojoSelectPanel.SetActive(true);
                    DojoTellPanel.SetActive(false);
                }
                else
                {
                    BookMarker = Mathf.Min(BookMarker, ArrayLengths[IdxKeeper] - 1);
                    PageDisplay();
                }
                break;
        }
    }
    public void OpenNetoDojoPanel()
    {
        GameManager.Instance.SetMode(GameManager.GameMode.Dojo);
        DojoSelectPanel.SetActive(true);
        DojoTellPanel.SetActive(false);
    }
}