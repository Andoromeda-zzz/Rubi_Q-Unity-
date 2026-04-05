// 忘れずにUnityEngine.UIを追加します
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Button型を使うために必要
using static System.Net.Mime.MediaTypeNames;


public class GameManager : MonoBehaviour
{

    private Dictionary<RubiksCubeModel.CubeColor, Color> colorMap;
    private Dictionary<PL_Model2.CubeColor, Color> colorMap2;
    private Dictionary<PL_Model3.CubeColor, Color> colorMap3;
    private Dictionary<PL_Model4.CubeColor, Color> colorMap4;

    // --- インスペクターから設定する変数 ---

    [Header("UIの親オブジェクト")] // モード選択UIの親オブジェクト
    public GameObject unfoldViewUI; // 展開図UIの親オブジェクト
    public GameObject Plane_UI;

    [Header("UIのボタン部品")] // モード1開始ボタン
    public Button endMode1Button;
    public Button No1_QuizButton;
    public Button No2_QuizButton;
    public Button No3_QuizButton;
    public Button No4_QuizButton;


    public GameObject rubiksCubeParent;
    public GameObject Plane_Cube;
    public GameObject Plane2;
    public GameObject Plane3;
    public GameObject Plane4;

    public GameObject GoodMark;
    public GameObject BadMark;

    public GameObject FailedCounter;
    public TextMeshProUGUI FailedCounterText;

    public GameOver GameOverObject;
    

    [Header("解答中タイマー")]
    public UnityEngine.UI.Image timerImage; // Inspectorから設定
    public float timeLimit = 10.0f; // 制限時間（秒）
    private float currentTime;
    private bool isAnswering = false;
    public TextMeshProUGUI timerText;

    [Header("カウントダウン")]
    public CanvasGroup quizPanelCanvasGroup; // QuizPanel をアタッチ
    public TextMeshProUGUI countdownText;
    public TextMeshProUGUI countdownText_Display; // CountdownText をアタッチ
    // --- 内部で使うデータ ---
    // private RubiksCubeModel cubeModel; // 将来的に使うデータモデル

    [Header("Scripts")]
    public RubiksCubeModel CubeModel;
    public PL_Model2 pl_Model2;
    public PL_Model3 pl_Model3;
    public PL_Model4 pl_Model4;

    public RubiksCubeGenerator Generator;
    public Plane_RubikCubeGenerator PL_Gen;

    public PL_Gen2 pl_gen2;
    public PL_Gen3 pl_gen3;
    public PL_Gen4 pl_gen4;

    [Header("Scripts 平面図移動用")]
    public PL1_Mover pl1_Mover;
    public PL2_Mover pl2_Mover;
    public PL3_Mover pl3_Mover;
    public PL4_Mover pl4_Mover;

    [Header("Scripts 3Dcube回転用")]
    public ThreeDCubeMover ThreeDCubeMove;


    [Header("ランダム回転回数")]
    public int ninnikaisuu = 3;
    public int ninnikaisuu2 = 3;


    [Header("AudioSourse")]
    public AudioSource bgmAudioSource;
    public AudioSource goodSE;
    public AudioSource badSE;

    [Header("誤回答回数")]
    public int False_num = 5;



    private string Q_number = "A";
    private bool isPushQButton = false;
    private bool isgoodORbad = false;
    

    private int TrustNumber;
    private int NowQNumber = 0;
    private string[] ResultofQ = new string[100000];

    private int LebelUPNum = 0;
    private float TimeMaru;
    private float TimeofEnd;

    private int Rx = 0;
    private int Ry = 0;
    private int Rz = 0;


    [SerializeField] private QuizManager quizmanager;

    void Awake()
    {
        CubeModel = new RubiksCubeModel();
        pl_Model2 = new PL_Model2();
        pl_Model3 = new PL_Model3();
        pl_Model4 = new PL_Model4();
    }

    void Start()
    {
        DataObject.TFdata = null;
        False_num = DataObject.mode;
        False_num = False_num - 1;
        FailedCounter.SetActive(false);
        FailedCounterText.text = $"{False_num}";
        // --- ここでボタンのクリックイベントを登録 ---
        // startMode1Buttonがクリックされたら、OnMode1Start関数を呼び出すように設定
        InitializeColorMap();
        InitializeColorMap2();
        InitializeColorMap3();
        InitializeColorMap4();

        PL_Gen.Initialize(colorMap);
        pl_gen2.Initialize(colorMap2);
        pl_gen3.Initialize(colorMap3);
        pl_gen4.Initialize(colorMap4);

        rubiksCubeParent.SetActive(false);
        Plane_Cube.SetActive(false);
        endMode1Button.onClick.AddListener(OnEndMode1Button);
        No1_QuizButton.onClick.AddListener(PushButton01);
        No2_QuizButton.onClick.AddListener(PushButton02);
        No3_QuizButton.onClick.AddListener(PushButton03);
        No4_QuizButton.onClick.AddListener(PushButton04);



        // アプリケーション起動時のUI状態を整える
        unfoldViewUI.SetActive(false);
        Plane_UI.SetActive(false);
        GoodMark.SetActive(false);
        BadMark.SetActive(false);
        currentTime = timeLimit;
        if (timerText != null)
        {
            timerText.text = timeLimit.ToString("F0"); // "F0"は小数点なしの意味
        }

        StartCoroutine(OnMode1Start());






    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UnityEngine.Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            PushButton01();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            PushButton02();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            PushButton03();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            PushButton04();
        }

        if (isAnswering)
        {
            currentTime -= Time.deltaTime; // 毎フレームの経過時間を引く

            // fillAmountは 0 (空) ~ 1 (満タン) の値
            timerImage.fillAmount = currentTime / timeLimit;
            if (timerText != null)
            {
                // Mathf.CeilToInt は小数点以下を切り上げ（9.1 -> 10）
                timerText.text = Mathf.CeilToInt(currentTime).ToString();
            }

            if (currentTime <= 0)
            {
                // 時間切れ
                currentTime = 0;
                timerText.text = "0";
                quizPanelCanvasGroup.interactable = false;
                isAnswering = false;
                DataObject.TFdata = ResultofQ;
                DataObject.isClea = true;
                StartCoroutine(EndGame());
                //ここに時間切れ時の処理を加える

            }
        }

        if (isgoodORbad)
        {
            TimeMaru += Time.deltaTime;
            if (TimeMaru >= 1.0)
            {
                GoodMark.SetActive(false);
                BadMark.SetActive(false);
                TimeMaru = 0;
                isgoodORbad = false;
            }
            
        }

        


        if (isPushQButton)
        {
            Riset_Cube();
            UnityEngine.Debug.Log(Q_number);
            GoodMark.SetActive(false);
            BadMark.SetActive(false);

            

            if (TrustNumber == 0 && Q_number == "Q1")
            {
                ResultofQ[NowQNumber] = "T";
                GoodMark.SetActive(true);
                goodSE.Play();
            }
            else if (TrustNumber == 1 && Q_number == "Q2")
            {
                ResultofQ[NowQNumber] = "T";
                GoodMark.SetActive(true);
                goodSE.Play();
            }
            else if (TrustNumber == 2 && Q_number == "Q3")
            {
                ResultofQ[NowQNumber] = "T";
                GoodMark.SetActive(true);
                goodSE.Play();
            }
            else if (TrustNumber == 3 && Q_number == "Q4")
            {
                ResultofQ[NowQNumber] = "T";
                GoodMark.SetActive(true);
                goodSE.Play();
            }
            else 
            {
                ResultofQ[NowQNumber] = "F";
                BadMark.SetActive(true);
                badSE.Play();
            }

            isgoodORbad = true;

            UnityEngine.Debug.Log(ResultofQ[NowQNumber]);

            int i = 0;
            for (int j = 0;j<= NowQNumber; j++)
            {
                if (ResultofQ[j] == "T") i += 1;
            }
            if (i%2 == 0 && ResultofQ[NowQNumber] != "F")
            {
                LebelUPNum += 1;
                
                UnityEngine.Debug.Log("レベルアップ！");
            }


            if (LebelUPNum < 1)
            {
                ninnikaisuu = ninnikaisuu + LebelUPNum * 3;
                quizmanager.RequestShuffle();
                Position_Maker();
                quizmakerOfPlanes(ninnikaisuu);

            }
            if (LebelUPNum >= 1 && LebelUPNum < 3)
            {
                if (LebelUPNum == 1)
                {
                    ninnikaisuu = 2;
                }

                ninnikaisuu = ninnikaisuu + (LebelUPNum - 1) * 2;

                quizmanager.RequestShuffle();
                Position_Maker();
                quizmakerOfPlanes(ninnikaisuu);

                Rx = UnityEngine.Random.Range(0, 361);
                Ry = UnityEngine.Random.Range(0, 361);
                Rz = UnityEngine.Random.Range(0, 361);

                ThreeDCubeMove.RMoveRotate(Rx, Ry, Rz);

            }

            if (LebelUPNum >= 3 && LebelUPNum < 5)
            {

                if (LebelUPNum == 3) ninnikaisuu = 3;

                ninnikaisuu = ninnikaisuu + (LebelUPNum - 3) * 2;

                quizmanager.RequestShuffle();
                Position_Maker();
                quizmakerOfPlanes2(ninnikaisuu);
                ImaginalMaker();
            }
            if (LebelUPNum >= 5 )
            {

                if (LebelUPNum == 5) ninnikaisuu = 4;

                ninnikaisuu = ninnikaisuu + (LebelUPNum - 5) * 2;

                quizmanager.RequestShuffle();
                Position_Maker();
                quizmakerOfPlanes2(ninnikaisuu);
                ImaginalMaker();

                Rx = UnityEngine.Random.Range(0, 361);
                Ry = UnityEngine.Random.Range(0, 361);
                Rz = UnityEngine.Random.Range(0, 361);

                ThreeDCubeMove.RMoveRotate(Rx, Ry, Rz);
            }


            int n = 0;
            for (int k = 0; k <= NowQNumber; k++)
            {
                if (ResultofQ[k] == "F") { n++; }

                FailedCounterText.text = $"{False_num - n}";
                if (n > False_num)
                {
                   
                    UnityEngine.Debug.Log("ゲームオーバー");
                    quizPanelCanvasGroup.interactable = false;
                    isPushQButton = false;
                    Riset_Cube();
                    GameOverObject.Move1();
                    bgmAudioSource.Stop();

                    if(currentTime >= 4)
                    {
                        DataObject.TFdata = ResultofQ;
                        DataObject.isClea = false;
                    }
                    else if(currentTime > 0)
                    {
                        currentTime += 100;
                        DataObject.TFdata = ResultofQ;
                        DataObject.isClea = false;
                    }


                    StartCoroutine(EndGame()); 


                }
            }



            NowQNumber += 1;

            

            

            isPushQButton = false;
        }

    }

    IEnumerator EndGame()
    {
        string sceneName = "ResultOfGame";
        yield return new WaitForSeconds(6.0f);

        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            UnityEngine.Debug.Log("Scene name is empty!");
        }
    }

    void Riset_Cube()
    {
        //color_Riset
        CubeModel.Riset_Collor();
        pl_Model2.Riset_Collor();
        pl_Model3.Riset_Collor();
        pl_Model4.Riset_Collor();
        //3Dキューブを壊してリセット
        Generator.DestroyCube();
        Generator.GenerateCube();
        Generator.ApplyColorsToCube();

        //以下は平面のアップデート処理
        PL_Gen.UpdateView(CubeModel);
        pl_gen2.UpdateView(pl_Model2);
        pl_gen3.UpdateView(pl_Model3);
        pl_gen4.UpdateView(pl_Model4);

    }

    void InitializeColorMap()
    {
        colorMap = new Dictionary<RubiksCubeModel.CubeColor, Color>()
        {
            { RubiksCubeModel.CubeColor.White, Color.white },
            { RubiksCubeModel.CubeColor.Yellow, Color.yellow },
            { RubiksCubeModel.CubeColor.Red, Color.red },
            { RubiksCubeModel.CubeColor.Orange, new Color(1, 0.5f, 0) }, // Orange
            { RubiksCubeModel.CubeColor.Green, Color.green },
            { RubiksCubeModel.CubeColor.Blue, Color.blue }
        };
    }

    void InitializeColorMap2()
    {
        colorMap2 = new Dictionary<PL_Model2.CubeColor, Color>()
        {
            { PL_Model2.CubeColor.White, Color.white },
            { PL_Model2.CubeColor.Yellow, Color.yellow },
            { PL_Model2.CubeColor.Red, Color.red },
            { PL_Model2.CubeColor.Orange, new Color(1, 0.5f, 0) }, // Orange
            { PL_Model2.CubeColor.Green, Color.green },
            { PL_Model2.CubeColor.Blue, Color.blue }
        };
    }

    void InitializeColorMap3()
    {
        colorMap3 = new Dictionary<PL_Model3.CubeColor, Color>()
        {
            { PL_Model3.CubeColor.White, Color.white },
            { PL_Model3.CubeColor.Yellow, Color.yellow },
            { PL_Model3.CubeColor.Red, Color.red },
            { PL_Model3.CubeColor.Orange, new Color(1, 0.5f, 0) }, // Orange
            { PL_Model3.CubeColor.Green, Color.green },
            { PL_Model3.CubeColor.Blue, Color.blue }
        };
    }

    void InitializeColorMap4()
    {
        colorMap4 = new Dictionary<PL_Model4.CubeColor, Color>()
        {
            { PL_Model4.CubeColor.White, Color.white },
            { PL_Model4.CubeColor.Yellow, Color.yellow },
            { PL_Model4.CubeColor.Red, Color.red },
            { PL_Model4.CubeColor.Orange, new Color(1, 0.5f, 0) }, // Orange
            { PL_Model4.CubeColor.Green, Color.green },
            { PL_Model4.CubeColor.Blue, Color.blue }
        };
    }

    void Position_Maker()
    {
        int i = 0;
        
        while (quizmanager.shuffledQuestionOrder.Count > 0)
        {
            // Pop (Dequeue) する
            int positionNumber = quizmanager.shuffledQuestionOrder.Dequeue();

            UnityEngine.Debug.Log("Pop: " + positionNumber + "。この位置に問題を配置します。");

            if(positionNumber == 1)
            {
                pl1_Mover.MoveTo(i);
                TrustNumber = i;
            }
            if (positionNumber == 2)
            {
                pl2_Mover.MoveTo(i);
            }
            if (positionNumber == 3)
            {
                pl3_Mover.MoveTo(i);
            }
            if (positionNumber == 4)
            {
                pl4_Mover.MoveTo(i);
            }

            i += 1;

        }
    }


    void  PushButton01()
    {
        Q_number =  "Q1";
        isPushQButton = true;
    }
    void PushButton02()
    {
        Q_number = "Q2";
        isPushQButton = true;
    }
    void PushButton03()
    {
        Q_number = "Q3";
        isPushQButton = true;
    }
    void PushButton04()
    {
        Q_number = "Q4";
        isPushQButton = true;
    }


    int[] QuizMakerOfPlane_peace(int kaisuu)
    {
        // 問題点4修正: 配列のサイズを固定(20)ではなく、引数の 'kaisuu' に合わせる
        int[] PL_kaiten = new int[kaisuu];

        // 'i' は for ループの中で宣言するのが一般的です
        for (int i = 0; i < kaisuu; i++)
        {
            // 問題点3修正: 1～18 (18を含む) を生成するため、(1, 19) にする
            PL_kaiten[i] = UnityEngine.Random.Range(1, 19);
            

            // 最初の動き(i=0)は、比較対象の「直前の動き」がないのでチェック不要
            if (i == 0)
            {
                continue; // 次のループ (i=1) へ進む
            }

            // 
            // 
            int currentMove = PL_kaiten[i];
            // 問題点2修正: タイポを PL1_kaiten (大文字L) に直す
            int prevMove = PL_kaiten[i - 1];

            // 問題点1修正: 冗長な 'if' 文を 'switch' 文に置き換え
            switch (currentMove)
            {
                // グループ1: 1, 2, 3, 7, 8, 9, 13, 14, 15
                // (反対の動きは +3 した値)
                case 1:
                case 2:
                case 3:
                case 7:
                case 8:
                case 9:
                case 13:
                case 14:
                case 15:
                    if (prevMove == currentMove + 3)
                    {
                        // 直前の動き(例: 4)が、今回の動き(例: 1)の反対だった
                        // やり直し
                        i = i - 1;
                    }
                    break;

                // グループ2: 4, 5, 6, 10, 11, 12, 16, 17, 18
                // (反対の動きは -3 した値)
                case 4:
                case 5:
                case 6:
                case 10:
                case 11:
                case 12:
                case 16:
                case 17:
                case 18:
                    if (prevMove == currentMove - 3)
                    {
                        // 直前の動き(例: 1)が、今回の動き(例: 4)の反対だった
                        // やり直し
                        i = i - 1;
                    }
                    break;
            }

        }
        
        return PL_kaiten;
    }

    void quizmakerOfPlanes(int kaisuu)
    {
        int[] PL1_kaiten = new int[kaisuu];
        int[] PL2_kaiten = new int[kaisuu];
        int[] PL3_kaiten = new int[kaisuu];
        int[] PL4_kaiten = new int[kaisuu];

        PL1_kaiten = QuizMakerOfPlane_peace(kaisuu);
        PL2_kaiten = QuizMakerOfPlane_peace(kaisuu);
        PL3_kaiten = QuizMakerOfPlane_peace(kaisuu); 
        PL4_kaiten = QuizMakerOfPlane_peace(kaisuu);

        for (int i = 0; i > 0;)
        {
            if (PL1_kaiten.SequenceEqual(PL2_kaiten))
            {
                PL2_kaiten = QuizMakerOfPlane_peace(kaisuu);
            }
            else if (PL1_kaiten.SequenceEqual(PL3_kaiten))
            {
                PL3_kaiten = QuizMakerOfPlane_peace(kaisuu);
            }
            else if (PL1_kaiten.SequenceEqual(PL4_kaiten))
            {
                PL4_kaiten = QuizMakerOfPlane_peace(kaisuu);
            }
            else if (PL2_kaiten.SequenceEqual(PL3_kaiten))
            {
                PL3_kaiten = QuizMakerOfPlane_peace(kaisuu);
            }
            else if (PL2_kaiten.SequenceEqual(PL4_kaiten))
            {
                PL4_kaiten = QuizMakerOfPlane_peace(kaisuu);
            }
            else if (PL3_kaiten.SequenceEqual(PL4_kaiten))
            {
                PL4_kaiten = QuizMakerOfPlane_peace(kaisuu);
            }
            else
            {
                break;
            }

        }

        for (int j = 0;j< kaisuu;j++)
        {
            Rotate(PL1_kaiten[j]);
            Rotate2(PL2_kaiten[j]);
            Rotate3(PL3_kaiten[j]);
            Rotate4(PL4_kaiten[j]);

        }

    }

    void quizmakerOfPlanes2(int kaisuu)
    {
        int[] PL_kaiten = new int[kaisuu];
        

        PL_kaiten = QuizMakerOfPlane_peace(kaisuu);
        for (int j = 0; j < kaisuu; j++)
        {
            Rotate(PL_kaiten[j]);
            Rotate2(PL_kaiten[j]);
            Rotate3(PL_kaiten[j]);
            Rotate4(PL_kaiten[j]);

        }

    }

    void ImaginalMaker()
    {

        int RandomChikan1 = 1;
        int RandomChikan2 = 1;
        int RandomChikan3 = 1;

        for (int i = 0; ; i++)
        {
            RandomChikan1 = UnityEngine.Random.Range(1, 10);
            RandomChikan2 = UnityEngine.Random.Range(1, 10);
            RandomChikan3 = UnityEngine.Random.Range(1, 10);

            if (RandomChikan1 != RandomChikan2 && RandomChikan1 != RandomChikan3 && RandomChikan3 != RandomChikan2)
            {
                break;
            }
        }

        pl_Model2.ImFaceMaker(RandomChikan1);
        pl_Model3.ImFaceMaker(RandomChikan2);
        pl_Model4.ImFaceMaker(RandomChikan3);

        pl_gen2.UpdateView(pl_Model2);
        pl_gen3.UpdateView(pl_Model3);
        pl_gen4.UpdateView(pl_Model4);
    }


    // --- ボタンが押された時に実行される関数 ---

    IEnumerator OnMode1Start()
    {
        UnityEngine.Debug.Log("モード1開始！");
        countdownText.gameObject.SetActive(true);

        quizPanelCanvasGroup.alpha = 0.0f;
        quizPanelCanvasGroup.interactable = false;

        quizmanager.RequestShuffle();
        Position_Maker();
        quizmakerOfPlanes(ninnikaisuu);
        // quizPanelCanvasGroup.alpha = 0.5f; // (任意) 半透明にして操作不可を分かりやすくする

        // 3. カウントダウン
        countdownText.text = "3";
        countdownText_Display.text = "3";
        yield return new WaitForSeconds(1.0f); // 1秒待つ

        countdownText.text = "2";
        countdownText_Display.text = "2";
        yield return new WaitForSeconds(1.0f);

        countdownText.text = "1";
        countdownText_Display.text = "1";
        yield return new WaitForSeconds(1.0f);

        countdownText.text = "START!";
        countdownText_Display.text = "READY?";
        yield return new WaitForSeconds(1.0f);


        // モード選択UIを非表示にし、展開図UIを表示する
        countdownText.gameObject.SetActive(false);
        countdownText_Display.gameObject.SetActive(false);// カウントダウン文字を非表示
        quizPanelCanvasGroup.interactable = true;
        quizPanelCanvasGroup.alpha = 1.0f;
        FailedCounter.SetActive(true);

        unfoldViewUI.SetActive(true);

        if (rubiksCubeParent != null)
            rubiksCubeParent.SetActive(true);
            Plane_Cube.SetActive(true);
            Plane_UI.SetActive(true);
            PL_Gen.UpdateView(CubeModel);

            pl_gen2.UpdateView(pl_Model2);
            pl_gen3.UpdateView(pl_Model3);
            pl_gen4.UpdateView(pl_Model4);


        if (bgmAudioSource != null)
        {
            bgmAudioSource.Play();
        }
        isAnswering = true;
        



    }
    void OnEndMode1Button()
    {
        UnityEngine.Debug.Log("モード1を終了します");
        GameOverObject.Move1();
        

        if (rubiksCubeParent != null)
            
            Riset_Cube();

        Return_Title();
            


    }

    void Return_Title()
    {
        string sceneName = "Title";

        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            UnityEngine.Debug.Log("Scene name is empty!");
        }
    }

    void Rotate(int Whitch)
    {
        if (Whitch == 1)
        {
            CubeModel.RotateFace(RubiksCubeModel.Face.Left, true, RubiksCubeModel.Inner_Face.X, false);
            CubeModel.RotateFace(RubiksCubeModel.Face.Left, true, RubiksCubeModel.Inner_Face.X, false);
            CubeModel.RotateFace(RubiksCubeModel.Face.Left, true, RubiksCubeModel.Inner_Face.X, false);
        }

        if (Whitch == 2)
            CubeModel.RotateFace(RubiksCubeModel.Face.Up, false, RubiksCubeModel.Inner_Face.X, true);

        if (Whitch == 3)
            CubeModel.RotateFace(RubiksCubeModel.Face.Right, true, RubiksCubeModel.Inner_Face.X, false);

        if (Whitch == 4)
            CubeModel.RotateFace(RubiksCubeModel.Face.Left, true, RubiksCubeModel.Inner_Face.X, false);

        if (Whitch == 5)
        {
            CubeModel.RotateFace(RubiksCubeModel.Face.Up, false, RubiksCubeModel.Inner_Face.X, true);
            CubeModel.RotateFace(RubiksCubeModel.Face.Up, false, RubiksCubeModel.Inner_Face.X, true);
            CubeModel.RotateFace(RubiksCubeModel.Face.Up, false, RubiksCubeModel.Inner_Face.X, true);
        }
        if (Whitch == 6)
        {
            CubeModel.RotateFace(RubiksCubeModel.Face.Right, true, RubiksCubeModel.Inner_Face.X, false);
            CubeModel.RotateFace(RubiksCubeModel.Face.Right, true, RubiksCubeModel.Inner_Face.X, false);
            CubeModel.RotateFace(RubiksCubeModel.Face.Right, true, RubiksCubeModel.Inner_Face.X, false);
        }
        if (Whitch == 7)
        {
            CubeModel.RotateFace(RubiksCubeModel.Face.Up, true, RubiksCubeModel.Inner_Face.X, false);
        }
        if (Whitch == 8)
        {
            CubeModel.RotateFace(RubiksCubeModel.Face.Up, false, RubiksCubeModel.Inner_Face.Y, true);
            CubeModel.RotateFace(RubiksCubeModel.Face.Up, false, RubiksCubeModel.Inner_Face.Y, true);
            CubeModel.RotateFace(RubiksCubeModel.Face.Up, false, RubiksCubeModel.Inner_Face.Y, true);
        }
        if (Whitch == 9)
        {
            CubeModel.RotateFace(RubiksCubeModel.Face.Down, true, RubiksCubeModel.Inner_Face.X, false);
            CubeModel.RotateFace(RubiksCubeModel.Face.Down, true, RubiksCubeModel.Inner_Face.X, false);
            CubeModel.RotateFace(RubiksCubeModel.Face.Down, true, RubiksCubeModel.Inner_Face.X, false);
        }

        if (Whitch == 10)
        {
            CubeModel.RotateFace(RubiksCubeModel.Face.Up, true, RubiksCubeModel.Inner_Face.X, false);
            CubeModel.RotateFace(RubiksCubeModel.Face.Up, true, RubiksCubeModel.Inner_Face.X, false);
            CubeModel.RotateFace(RubiksCubeModel.Face.Up, true, RubiksCubeModel.Inner_Face.X, false);
        }
        if (Whitch == 11)
        {
            CubeModel.RotateFace(RubiksCubeModel.Face.Up, false, RubiksCubeModel.Inner_Face.Y, true);
            
        }
        if(Whitch == 12)
        {
            CubeModel.RotateFace(RubiksCubeModel.Face.Down, true, RubiksCubeModel.Inner_Face.X, false);
            
        }


        if (Whitch == 13)
        {
            CubeModel.RotateFace(RubiksCubeModel.Face.Front, true, RubiksCubeModel.Inner_Face.X, false);
            CubeModel.RotateFace(RubiksCubeModel.Face.Front, true, RubiksCubeModel.Inner_Face.X, false);
            CubeModel.RotateFace(RubiksCubeModel.Face.Front, true, RubiksCubeModel.Inner_Face.X, false);
        }

        if (Whitch == 14)
            CubeModel.RotateFace(RubiksCubeModel.Face.Up, false, RubiksCubeModel.Inner_Face.Z, true);
        
        if (Whitch == 15)
            CubeModel.RotateFace(RubiksCubeModel.Face.Back, true, RubiksCubeModel.Inner_Face.X, false);

        if (Whitch == 16) CubeModel.RotateFace(RubiksCubeModel.Face.Front, true, RubiksCubeModel.Inner_Face.X, false);

        if(Whitch == 17)
        {
            CubeModel.RotateFace(RubiksCubeModel.Face.Up, false, RubiksCubeModel.Inner_Face.Z, true);
            CubeModel.RotateFace(RubiksCubeModel.Face.Up, false, RubiksCubeModel.Inner_Face.Z, true);
            CubeModel.RotateFace(RubiksCubeModel.Face.Up, false, RubiksCubeModel.Inner_Face.Z, true);
        }

        if (Whitch == 18)
        {
            CubeModel.RotateFace(RubiksCubeModel.Face.Back, true, RubiksCubeModel.Inner_Face.X, false);
            CubeModel.RotateFace(RubiksCubeModel.Face.Back, true, RubiksCubeModel.Inner_Face.X, false);
            CubeModel.RotateFace(RubiksCubeModel.Face.Back, true, RubiksCubeModel.Inner_Face.X, false);
        }








        Generator.ApplyColorsToCube();
        PL_Gen.UpdateView(CubeModel);

    }

    void Rotate2(int Whitch)
    {
        if (Whitch == 1)
        {
            pl_Model2.RotateFace(PL_Model2.Face.Left, true, PL_Model2.Inner_Face.X, false);
            pl_Model2.RotateFace(PL_Model2.Face.Left, true, PL_Model2.Inner_Face.X, false);
            pl_Model2.RotateFace(PL_Model2.Face.Left, true, PL_Model2.Inner_Face.X, false);
        }

        if (Whitch == 2)
            pl_Model2.RotateFace(PL_Model2.Face.Up, false, PL_Model2.Inner_Face.X, true);

        if (Whitch == 3)
            pl_Model2.RotateFace(PL_Model2.Face.Right, true, PL_Model2.Inner_Face.X, false);

        if (Whitch == 4)
            pl_Model2.RotateFace(PL_Model2.Face.Left, true, PL_Model2.Inner_Face.X, false);

        if (Whitch == 5)
        {
            pl_Model2.RotateFace(PL_Model2.Face.Up, false, PL_Model2.Inner_Face.X, true);
            pl_Model2.RotateFace(PL_Model2.Face.Up, false, PL_Model2.Inner_Face.X, true);
            pl_Model2.RotateFace(PL_Model2.Face.Up, false, PL_Model2.Inner_Face.X, true);
        }
        if (Whitch == 6)
        {
            pl_Model2.RotateFace(PL_Model2.Face.Right, true, PL_Model2.Inner_Face.X, false);
            pl_Model2.RotateFace(PL_Model2.Face.Right, true, PL_Model2.Inner_Face.X, false);
            pl_Model2.RotateFace(PL_Model2.Face.Right, true, PL_Model2.Inner_Face.X, false);
        }
        if (Whitch == 7)
        {
            pl_Model2.RotateFace(PL_Model2.Face.Up, true, PL_Model2.Inner_Face.X, false);
        }
        if (Whitch == 8)
        {
            pl_Model2.RotateFace(PL_Model2.Face.Up, false, PL_Model2.Inner_Face.Y, true);
            pl_Model2.RotateFace(PL_Model2.Face.Up, false, PL_Model2.Inner_Face.Y, true);
            pl_Model2.RotateFace(PL_Model2.Face.Up, false, PL_Model2.Inner_Face.Y, true);
        }
        if (Whitch == 9)
        {
            pl_Model2.RotateFace(PL_Model2.Face.Down, true, PL_Model2.Inner_Face.X, false);
            pl_Model2.RotateFace(PL_Model2.Face.Down, true, PL_Model2.Inner_Face.X, false);
            pl_Model2.RotateFace(PL_Model2.Face.Down, true, PL_Model2.Inner_Face.X, false);
        }

        if (Whitch == 10)
        {
            pl_Model2.RotateFace(PL_Model2.Face.Up, true, PL_Model2.Inner_Face.X, false);
            pl_Model2.RotateFace(PL_Model2.Face.Up, true, PL_Model2.Inner_Face.X, false);
            pl_Model2.RotateFace(PL_Model2.Face.Up, true, PL_Model2.Inner_Face.X, false);
        }
        if (Whitch == 11)
        {
            pl_Model2.RotateFace(PL_Model2.Face.Up, false, PL_Model2.Inner_Face.Y, true);

        }
        if (Whitch == 12)
        {
            pl_Model2.RotateFace(PL_Model2.Face.Down, true, PL_Model2.Inner_Face.X, false);

        }


        if (Whitch == 13)
        {
            pl_Model2.RotateFace(PL_Model2.Face.Front, true, PL_Model2.Inner_Face.X, false);
            pl_Model2.RotateFace(PL_Model2.Face.Front, true, PL_Model2.Inner_Face.X, false);
            pl_Model2.RotateFace(PL_Model2.Face.Front, true, PL_Model2.Inner_Face.X, false);
        }

        if (Whitch == 14)
            pl_Model2.RotateFace(PL_Model2.Face.Up, false, PL_Model2.Inner_Face.Z, true);

        if (Whitch == 15)
            pl_Model2.RotateFace(PL_Model2.Face.Back, true, PL_Model2.Inner_Face.X, false);

        if (Whitch == 16) pl_Model2.RotateFace(PL_Model2.Face.Front, true, PL_Model2.Inner_Face.X, false);

        if (Whitch == 17)
        {
            pl_Model2.RotateFace(PL_Model2.Face.Up, false, PL_Model2.Inner_Face.Z, true);
            pl_Model2.RotateFace(PL_Model2.Face.Up, false, PL_Model2.Inner_Face.Z, true);
            pl_Model2.RotateFace(PL_Model2.Face.Up, false, PL_Model2.Inner_Face.Z, true);
        }

        if (Whitch == 18)
        {
            pl_Model2.RotateFace(PL_Model2.Face.Back, true, PL_Model2.Inner_Face.X, false);
            pl_Model2.RotateFace(PL_Model2.Face.Back, true, PL_Model2.Inner_Face.X, false);
            pl_Model2.RotateFace(PL_Model2.Face.Back, true, PL_Model2.Inner_Face.X, false);
        }









        pl_gen2.UpdateView(pl_Model2);

    }

    void Rotate3(int Whitch)
    {
        if (Whitch == 1)
        {
            pl_Model3.RotateFace(PL_Model3.Face.Left, true, PL_Model3.Inner_Face.X, false);
            pl_Model3.RotateFace(PL_Model3.Face.Left, true, PL_Model3.Inner_Face.X, false);
            pl_Model3.RotateFace(PL_Model3.Face.Left, true, PL_Model3.Inner_Face.X, false);
        }

        if (Whitch == 2)
            pl_Model3.RotateFace(PL_Model3.Face.Up, false, PL_Model3.Inner_Face.X, true);

        if (Whitch == 3)
            pl_Model3.RotateFace(PL_Model3.Face.Right, true, PL_Model3.Inner_Face.X, false);

        if (Whitch == 4)
            pl_Model3.RotateFace(PL_Model3.Face.Left, true, PL_Model3.Inner_Face.X, false);

        if (Whitch == 5)
        {
            pl_Model3.RotateFace(PL_Model3.Face.Up, false, PL_Model3.Inner_Face.X, true);
            pl_Model3.RotateFace(PL_Model3.Face.Up, false, PL_Model3.Inner_Face.X, true);
            pl_Model3.RotateFace(PL_Model3.Face.Up, false, PL_Model3.Inner_Face.X, true);
        }
        if (Whitch == 6)
        {
            pl_Model3.RotateFace(PL_Model3.Face.Right, true, PL_Model3.Inner_Face.X, false);
            pl_Model3.RotateFace(PL_Model3.Face.Right, true, PL_Model3.Inner_Face.X, false);
            pl_Model3.RotateFace(PL_Model3.Face.Right, true, PL_Model3.Inner_Face.X, false);
        }
        if (Whitch == 7)
        {
            pl_Model3.RotateFace(PL_Model3.Face.Up, true, PL_Model3.Inner_Face.X, false);
        }
        if (Whitch == 8)
        {
            pl_Model3.RotateFace(PL_Model3.Face.Up, false, PL_Model3.Inner_Face.Y, true);
            pl_Model3.RotateFace(PL_Model3.Face.Up, false, PL_Model3.Inner_Face.Y, true);
            pl_Model3.RotateFace(PL_Model3.Face.Up, false, PL_Model3.Inner_Face.Y, true);
        }
        if (Whitch == 9)
        {
            pl_Model3.RotateFace(PL_Model3.Face.Down, true, PL_Model3.Inner_Face.X, false);
            pl_Model3.RotateFace(PL_Model3.Face.Down, true, PL_Model3.Inner_Face.X, false);
            pl_Model3.RotateFace(PL_Model3.Face.Down, true, PL_Model3.Inner_Face.X, false);
        }

        if (Whitch == 10)
        {
            pl_Model3.RotateFace(PL_Model3.Face.Up, true, PL_Model3.Inner_Face.X, false);
            pl_Model3.RotateFace(PL_Model3.Face.Up, true, PL_Model3.Inner_Face.X, false);
            pl_Model3.RotateFace(PL_Model3.Face.Up, true, PL_Model3.Inner_Face.X, false);
        }
        if (Whitch ==11)
        {
            pl_Model3.RotateFace(PL_Model3.Face.Up, false, PL_Model3.Inner_Face.Y, true);

        }
        if (Whitch == 12)
        {
            pl_Model3.RotateFace(PL_Model3.Face.Down, true, PL_Model3.Inner_Face.X, false);

        }


        if (Whitch == 13)
        {
            pl_Model3.RotateFace(PL_Model3.Face.Front, true, PL_Model3.Inner_Face.X, false);
            pl_Model3.RotateFace(PL_Model3.Face.Front, true, PL_Model3.Inner_Face.X, false);
            pl_Model3.RotateFace(PL_Model3.Face.Front, true, PL_Model3.Inner_Face.X, false);
        }

        if (Whitch == 14)
            pl_Model3.RotateFace(PL_Model3.Face.Up, false, PL_Model3.Inner_Face.Z, true);

        if (Whitch == 15)
            pl_Model3.RotateFace(PL_Model3.Face.Back, true, PL_Model3.Inner_Face.X, false);

        if (Whitch == 16) pl_Model3.RotateFace(PL_Model3.Face.Front, true, PL_Model3.Inner_Face.X, false);

        if (Whitch == 17)
        {
            pl_Model3.RotateFace(PL_Model3.Face.Up, false, PL_Model3.Inner_Face.Z, true);
            pl_Model3.RotateFace(PL_Model3.Face.Up, false, PL_Model3.Inner_Face.Z, true);
            pl_Model3.RotateFace(PL_Model3.Face.Up, false, PL_Model3.Inner_Face.Z, true);
        }

        if (Whitch == 18)
        {
            pl_Model3.RotateFace(PL_Model3.Face.Back, true, PL_Model3.Inner_Face.X, false);
            pl_Model3.RotateFace(PL_Model3.Face.Back, true, PL_Model3.Inner_Face.X, false);
            pl_Model3.RotateFace(PL_Model3.Face.Back, true, PL_Model3.Inner_Face.X, false);
        }

        pl_gen3.UpdateView(pl_Model3);

    }

    void Rotate4(int Whitch)
    {
        if (Whitch == 1)
        {
            pl_Model4.RotateFace(PL_Model4.Face.Left, true, PL_Model4.Inner_Face.X, false);
            pl_Model4.RotateFace(PL_Model4.Face.Left, true, PL_Model4.Inner_Face.X, false);
            pl_Model4.RotateFace(PL_Model4.Face.Left, true, PL_Model4.Inner_Face.X, false);
        }

        if (Whitch == 2)
            pl_Model4.RotateFace(PL_Model4.Face.Up, false, PL_Model4.Inner_Face.X, true);

        if (Whitch == 3)
            pl_Model4.RotateFace(PL_Model4.Face.Right, true, PL_Model4.Inner_Face.X, false);

        if (Whitch == 4)
            pl_Model4.RotateFace(PL_Model4.Face.Left, true, PL_Model4.Inner_Face.X, false);

        if (Whitch == 5)
        {
            pl_Model4.RotateFace(PL_Model4.Face.Up, false, PL_Model4.Inner_Face.X, true);
            pl_Model4.RotateFace(PL_Model4.Face.Up, false, PL_Model4.Inner_Face.X, true);
            pl_Model4.RotateFace(PL_Model4.Face.Up, false, PL_Model4.Inner_Face.X, true);
        }
        if (Whitch == 6)
        {
            pl_Model4.RotateFace(PL_Model4.Face.Right, true, PL_Model4.Inner_Face.X, false);
            pl_Model4.RotateFace(PL_Model4.Face.Right, true, PL_Model4.Inner_Face.X, false);
            pl_Model4.RotateFace(PL_Model4.Face.Right, true, PL_Model4.Inner_Face.X, false);
        }
        if (Whitch == 7)
        {
            pl_Model4.RotateFace(PL_Model4.Face.Up, true, PL_Model4.Inner_Face.X, false);
        }
        if (Whitch == 8)
        {
            pl_Model4.RotateFace(PL_Model4.Face.Up, false, PL_Model4.Inner_Face.Y, true);
            pl_Model4.RotateFace(PL_Model4.Face.Up, false, PL_Model4.Inner_Face.Y, true);
            pl_Model4.RotateFace(PL_Model4.Face.Up, false, PL_Model4.Inner_Face.Y, true);
        }
        if (Whitch == 9)
        {
            pl_Model4.RotateFace(PL_Model4.Face.Down, true, PL_Model4.Inner_Face.X, false);
            pl_Model4.RotateFace(PL_Model4.Face.Down, true, PL_Model4.Inner_Face.X, false);
            pl_Model4.RotateFace(PL_Model4.Face.Down, true, PL_Model4.Inner_Face.X, false);
        }

        if (Whitch == 10)
        {
            pl_Model4.RotateFace(PL_Model4.Face.Up, true, PL_Model4.Inner_Face.X, false);
            pl_Model4.RotateFace(PL_Model4.Face.Up, true, PL_Model4.Inner_Face.X, false);
            pl_Model4.RotateFace(PL_Model4.Face.Up, true, PL_Model4.Inner_Face.X, false);
        }
        if (Whitch == 11)
        {
            pl_Model4.RotateFace(PL_Model4.Face.Up, false, PL_Model4.Inner_Face.Y, true);

        }
        if (Whitch == 12)
        {
            pl_Model4.RotateFace(PL_Model4.Face.Down, true, PL_Model4.Inner_Face.X, false);

        }


        if (Whitch == 13)
        {
            pl_Model4.RotateFace(PL_Model4.Face.Front, true, PL_Model4.Inner_Face.X, false);
            pl_Model4.RotateFace(PL_Model4.Face.Front, true, PL_Model4.Inner_Face.X, false);
            pl_Model4.RotateFace(PL_Model4.Face.Front, true, PL_Model4.Inner_Face.X, false);
        }

        if (Whitch == 14)
            pl_Model4.RotateFace(PL_Model4.Face.Up, false, PL_Model4.Inner_Face.Z, true);

        if (Whitch == 15)
            pl_Model4.RotateFace(PL_Model4.Face.Back, true, PL_Model4.Inner_Face.X, false);

        if (Whitch == 16) pl_Model4.RotateFace(PL_Model4.Face.Front, true, PL_Model4.Inner_Face.X, false);

        if (Whitch == 17)
        {
            pl_Model4.RotateFace(PL_Model4.Face.Up, false, PL_Model4.Inner_Face.Z, true);
            pl_Model4.RotateFace(PL_Model4.Face.Up, false, PL_Model4.Inner_Face.Z, true);
            pl_Model4.RotateFace(PL_Model4.Face.Up, false, PL_Model4.Inner_Face.Z, true);
        }

        if (Whitch == 18)
        {
            pl_Model4.RotateFace(PL_Model4.Face.Back, true, PL_Model4.Inner_Face.X, false);
            pl_Model4.RotateFace(PL_Model4.Face.Back, true, PL_Model4.Inner_Face.X, false);
            pl_Model4.RotateFace(PL_Model4.Face.Back, true, PL_Model4.Inner_Face.X, false);
        }

        pl_gen4.UpdateView(pl_Model4);

    }


    // 他に必要な関数をここに追加していく...
}