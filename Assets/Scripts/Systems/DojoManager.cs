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
    [SerializeField] private GameObject BattleAskPanel;
    [SerializeField] private GameObject DojoPanel;
    [SerializeField] private Text NetoTellText;
    [SerializeField] private Text IndexLabel;
    [SerializeField] private Text BattleAskText;
    //[SerializeField] private Text 

    private int IdxKeeper;
    private int BookMarker;
    string[] VariableContent = { "■ プログラミングとJavaの世界へようこそ！\n\nプログラミングっていうのは、コンピュータに「こう動いて！」っていう命令書を書くことだよ。\nその中でも「Java（ジャバ）」は、ルールがしっかりしていて、世界中の銀行システムやスマホアプリ、Webサイトの裏側で大活躍している言語なんだ。\n\n最初にこの基本をマスターしておけば、他のプログラミング言語も楽に覚えられるようになるから、一緒に頑張ろう！"
                                ,"■ まずは「箱」を用意しよう（変数）\n\nJavaで何かを作るときは、まずデータを一時的に保存しておく「箱」を作ることから始まるよ。これを「変数（へんすう）」と呼ぶんだ。でもJavaはちょっと厳しくて、「この箱には数字しか入れちゃダメ！」「こっちは文字専用！」って、最初に箱の種類（型）を決めてあげる必要があるんだ。"
                                ,"【よく使う箱の種類（型）】\n\n・int（イント）：整数を入れる箱。\n（例：1, 100, -5, 0）※小数は入れられないよ！\n\n・double（ダブル）  ：小数を入れる箱。\n（例：3.14, 0.5, -1.2）\n\n・String（ストリング）：文字を入れる箱。\n（例：\"こんにちは\"）\n※必ず「\"\"（ダブルクォーテーション）」で囲むのがルール！"
                                ,"★超重要！\nプログラミングの「＝」の意味\n\n数学だと「＝」は「同じ」って意味だよね。\nでも、プログラミングでは「右のものを左の箱に入れる（代入）」という動きを表すんだ。\n\nint a = 10;\n（意味：整数を入れる箱 a を作って、そこに 10 を入れる！）\n「←」こういう矢印のイメージを持つと、コードがスラスラ読めるようになるよ！"
                                ,"■ 画面に表示してみよう\n\n箱を作って計算しても、画面に見えなかったら意味がないよね。\n結果を表示するための魔法の呪文がこれ！\n\nSystem.out.println\n(中身);\n\nこれを書くと、カッコの中身を画面にポン！と表示してくれるよ。"
                                ,"★忘れちゃダメな「;（セミコロン）」\nJavaでは、命令の最後に必ず「;」を付けるルールがあるよ。\n\nこれは日本語の「。」（句点）と同じで、「ここで一つの命令がおしまい！」っていう合図なんだ。これを付け忘れると、コンピュータが「どこで終わるの！？」ってパニックになってエラーになっちゃうから気をつけてね。"
                                ,"■ 計算のやり方（四則演算）\n\n計算には、キーボードにある専用の記号（演算子）を使うよ。\n「×」や「÷」の記号はキーボードにないから、別の記号で代用するんだ。\n\n【基本の計算】\n・ + ： 足し算\n・ - ： 引き算\n・ * ： かけ算（アスタリスクって読むよ）\n・ / ： わり算（スラッシュって読むよ）"
                                ,"★初心者がハマる罠「整数のわり算」！\nここだけは絶対に覚えておいて！\n\nint（整数）同士でわり算をすると、小数点以下はバッサリ切り捨てられて、答えも整数になっちゃうんだ。\n\nint d = 7 / 2;\n（普通は3.5だけど...Javaのint型だと「3」になる！）\n\n四捨五入じゃなくて「切り捨て」だから注意してね。"
                                ,"■ 「あまり」を出す計算（剰余算）\n\nJavaには、わり算の答え（商）じゃなくて、「あまり」だけを計算する便利な記号があるんだ。・ % （パーセント）：わり算の「あまり」を出す\n\nint result = 7 % 3;\n（7 ÷ 3 ＝ 2 あまり 1なので、箱には「1」が入る！）\n★いつ使うの？\n「%」はプログラミングでめちゃくちゃ使うよ！\n・「2で割ったあまりが0なら偶数、1なら奇数」って判定するよ。"
                                ,"■計算の順番（優先順位）\n\n数学と同じで、Javaの計算にも「優先順位」があるよ。\n\n1. 「*」（かけ算）「/」（わり算）「%」（あまり）\n2. 「+」（たし算）「-」（ひき算）\nint x = 10 + 2 * 3;\n（2 * 3 = 6 が先。そのあと10+6=16になるよ！）\n※もし足し算を先にやりたいときは、(10 + 2) * 3 みたいにカッコをつければOK！"
                                ,"■ ベテランっぽい書き方（複合代入・インクリメント）\n\nプログラマーは面倒くさがり屋だから、よく使う計算を短く書くテクニックがあるんだ。\n\n★値を1だけ増やしたいとき（インクリメント）\nx = x + 1;\nって書くのが面倒なときは、こう書けるよ。\n・ x++;"
                                ,"★計算と代入をまとめちゃう複合代入\n\n・x += 3;\n意味：x = x + 3; xに3を足して、xに入れ直す\n\n・ x *= 2;\n意味：x = x* 2;\nxを2倍にして、xに入れ直す"
                                ,"どうだった？\nこの「計算の優先順位」と「切り捨て」、そして「変数の箱のイメージ」さえ掴めていれば、プログラミングの第一歩はバッチリだよ！" };
    string[] IfContent = { "■プログラミングに「判断」させよう（if文）\r\n\r\nプログラミングが面白いのは、ただ計算するだけ\r\nじゃなくて「もし〇〇なら、こう動いて！」\r\nっていう判断ができるところなんだ。\r\nこれを実現するのが「if（イフ）文」だよ。\r\n\r\nゲームで「HPが0になったらゲームオーバー」\r\nとか「スコアが100点を超えたらクリア」って\r\nなるのも、全部この if文 のおかげなんだ！"
                            ,"■ if文の書き方\r\n\r\n基本の形はすごくシンプル！\r\n\r\nif (条件式) {\r\n    // 条件に一致する時だけ動く命令\r\n}\r\n\r\n【書き方のルール】\r\n・( ) の中 ： 「x は 5 より大きい？」\r\n\t　　　みたいな「条件」を書く場所。\r\n・{ } の中 ： 条件が合ったときだけ実行させたい\r\n\t　　「命令」を書く場所。"
                            ,"★絶対に気をつけて！「魔のセミコロン」\r\nif (条件式) のすぐ後ろには\r\n絶対に「;（セミコロン）」を付けないで！\r\nもし `if (x > 5);` って書くと\r\nそこで「判断終了！」ってなって、下の `{ }`\r\nの中身が条件に関係なく勝手に動いちゃうんだ。"
                            ,"■ 比べるための記号（比較演算子）\r\n\r\n「AとBは同じ？」「どっちが大きい？」\r\nって比べるときは、専用の記号を使うよ。\r\n\r\n【左右を比べる】\r\n・a == b   ： a と b は同じ\r\n   → 超重要！ 「=」を2つ並べるのがルール。\r\n　　「=」1つだと「代入」になっちゃうから注意！\r\n・a != b   ： a と b は違う\r\n（「!」は「否定」の意味だよ）"
                            ,"【大きさを比べる】\r\n・a >  b   ： a は b より大きい\r\n・a <  b   ： a は b より小さい\r\n・a >= b   ： a は b 以上（同じか、大きい）\r\n・a <= b   ： a は b 以下（同じか、小さい）"
                            ,"■ 複数の条件を組み合わせる（論理演算子）\r\n\r\n「テストが80点以上」かつ「宿題を出した人」\r\nみたいに、条件を欲張りたいときはこれを使おう。\r\n\r\n・a && b   ： a かつ b （AND）\r\n   → 両方の条件がクリアできたときだけOK！\r\n   \r\n・a || b   ： a または b （OR）\r\n   → どっちか片方でもクリアできてればOK！"
                            ,"■ 「そうじゃなかったら」の道を作る\r\n　　（else / else if）\r\n\r\n条件に合わなかったときに、別の動きをさせたいとき\r\nは else（エルス）をつなげるんだ。\r\n\r\n【2つの道に分ける】\r\nif (score >= 60) {\r\n    System.out.println(\"合格！\");\r\n} else {\r\n　　 // 60点未満ならこっちが動く\r\n    System.out.println(\"不合格...\");\r\n}"
                            ,"【3つ以上の道に分ける】\r\n「A評価、B評価、それ以外」みたいに\r\n分けたいときは、else if を間に挟もう。\r\n\r\nif (score >= 80) {\r\n    System.out.println(\"最高！\");\r\n} else if (score >= 60) {\r\n　　// 80未満、かつ60以上のとき\r\n    System.out.println(\"惜しい！\"); \r\n} else {\r\n　　// どの条件にも合わなかったとき\r\n    System.out.println(\"頑張ろう\");\r\n} "
                            ,"★ポイント\r\n上から順番にチェックしていって、\r\n一度どれかの条件に合ったら、その下の\r\nelse if や else は全部無視されるよ。\r\n\r\n一番最初に「当たり」を引いたら、\r\nそこでおしまい！って動きをするんだ。\r\n\r\nどう？「=」と「==」の違いや「&&」と「||」\r\nの使い分け、バッチリ掴めたかな？ "
                            };
    string[] ForWhileContent = { "" };
    string[] ArrayContent = {"【学習文章：配列 × for文】\n配列を使うときに、よく一緒に使うのが for文ネト。\nfor文は「決まった回数だけ繰り返す処理」を書くときに使うネト。"
                                ,"基本形はこれネト：\n\nfor (int i = 0; i < 5; i++) {\n    //繰り返す処理\n}\nint i = 0 … ループ用の変数iを宣言、0でスタート\ni < 5 … iが4までループ(5回繰り返す)\ni++ … ループのたびにiを1増やす\n\n配列と組み合わせると、配列の全要素を順番に処理できるネト。"
                                , "例えば：\n\nint[] nums = { 5, 8, 12 };\n\nfor (int i = 0; i < nums.length; i++) {\n    System.out.println(nums[i]);\n}\n\nこれで 配列の中身を0番目から順に表示できるネト。"
                                ,"次のコードを実行すると、最後に表示される値は何になるネト？\n\npublic class Main {\n    public static void main(String[] args) {\n        int[] nums = { 4, 9, 2, 7 };\n        for (int i = 0; i < nums.length; i++) {\n            System.out.println(nums[i]);\n        }\n    }\n}\n A:4 B:2 C:7 D:9"
                                ,"【解説】ネト\n配列 nums の中身は次ネト。\nインデックス：0 1 2 3\n値　　　　　：4 9 2 7\n\nfor文はこう動くネト：\ni = 0 → nums[0] = 4 を表示\ni = 1 → nums[1] = 9 を表示\ni = 2 → nums[2] = 2 を表示\ni = 3 → nums[3] = 7 を表示\n\nつまり、最後に出力されるのは 7 、正解は C ネト！"};
    int[] ArrayLengths = new int[4];
    string[] BattleAsk = {"変数の学習内容を実践しますか？\n（戦闘が始まります）",
                            "条件分岐の学習内容を実践しますか？\n（戦闘が始まります）",
                            "ループ文の学習内容を実践しますか？\n（戦闘が始まります）",
                            "配列の学習内容を実践しますか？\n（戦闘が始まります）"};  
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
                DojoPanel.SetActive(false);
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
        DojoPanel.SetActive(true);
        DojoSelectPanel.SetActive(true);
        DojoTellPanel.SetActive(false);
    }
    public void TutorialDoll(int DollId)
    {
        DojoPanel.SetActive(true);
        BattleAskPanel.SetActive(true);
        BattleAskText.text = BattleAsk[DollId];
    }
    public void TutorialbattleStart()
    {

    }
    public void TutorialbattleCancel()
    {
        DojoPanel.SetActive(false);
        BattleAskPanel.SetActive(false);
    }
}