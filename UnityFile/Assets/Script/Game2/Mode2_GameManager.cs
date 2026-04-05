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

public class Mode2_GameManager : MonoBehaviour
{

    private Dictionary<Mode2_PLmodel.CubeColor, Color> colorMap;
    private Dictionary<Mode2CubeModel3D.CubeColor, Color> colorMap2;

    [Header("2D描画用スクリプト")]
    public Mode2_PLmodel pl_Model;
    public Mode2PLGenerator PL_Generator;

    [Header("3D描画用スクリプト")]
    public Mode2CubeGenerator3D cube3D_generator;
    public Mode2CubeModel3D cubeModel3D;

    [Header("Plane回転用ボタン")]
    public Button Up1;
    public Button Up2;
    public Button Up3;
    public Button Down1;
    public Button Down2;
    public Button Down3;
    public Button Left1;
    public Button Left2;
    public Button Left3;
    public Button Right1;
    public Button Right2;
    public Button Right3;
    public Button ImR1;
    public Button ImR2;
    public Button ImR3;
    public Button ImL1;
    public Button ImL2;
    public Button ImL3;

    public Button ResetQuiz;
    public Button ReturnTitle;

    [Header("text")]
    public TextMeshProUGUI ResultKaisuuText;
    private int mokorikaisuu = 5;

    [Header("GameObject")]
    public CanvasGroup ButtonGroup;
    public GameObject Crear;
    public GameObject Riset;

    [Header("AudioObject")]
    public AudioSource Pinpon;
    public AudioSource BGM;
    

    private int[] RotateCounter = new int[10000];
    private int RotateCounter_NumCounter = 0;
    private int[] RotateQuiz = new int[10000];

    public int NumberOfRotate = 3;
    public int NumberOfTume = 2;

    private bool isclear = false;
    private float Dtime;
    private float TimeData_Clear;

 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        BGM.Play();
        Crear.SetActive(false);
        Riset.SetActive(true);

        NumberOfRotate = Mode2Data.kaitenkaisuuData;
        NumberOfTume = Mode2Data.TumekaisuuData;
        InitializeColorMap();

        PL_Generator.Initialize(colorMap);
        PL_Generator.UpdateView(pl_Model);

        InitializeColorMap2();
        cube3D_generator.ApplyColorsToCube();

        ButtonGroup.interactable = true;

        //以下はボタン処理
        Up1.onClick.AddListener(RotateButtonUp1);
        Up2.onClick.AddListener(RotateButtonUp2);
        Up3.onClick.AddListener(RotateButtonUp3);
        Down1.onClick.AddListener(RotateButtonDown1);
        Down2.onClick.AddListener(RotateButtonDown2);
        Down3.onClick.AddListener(RotateButtonDown3);
        Right1.onClick.AddListener(RotateButtonRight1);
        Right2.onClick.AddListener(RotateButtonRight2);
        Right3.onClick.AddListener(RotateButtonRight3);
        Left1.onClick.AddListener(RotateButtonLeft1);
        Left2.onClick.AddListener(RotateButtonLeft2);
        Left3.onClick.AddListener(RotateButtonLeft3);
        ImR1.onClick.AddListener(RotateButtonImR1);
        ImR2.onClick.AddListener(RotateButtonImR2);
        ImR3.onClick.AddListener(RotateButtonImR3);
        ImL1.onClick.AddListener(RotateButtonImL1);
        ImL2.onClick.AddListener(RotateButtonImL2);
        ImL3.onClick.AddListener(RotateButtonImL3);

        ResetQuiz.onClick.AddListener(ResetQuiz_mode2);
        ReturnTitle.onClick.AddListener(Return_Title);

        CreateQuiz(Mode2Data.kaitenkaisuuData, Mode2Data.TumekaisuuData);
    }

    void Awake()
    {
        pl_Model = new Mode2_PLmodel();
        cubeModel3D = new Mode2CubeModel3D();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UnityEngine.Application.Quit();
        }

        if((AreCubeStatesEqual(pl_Model.State, cubeModel3D.State))&&isclear == false)
        {
            UnityEngine.Debug.Log("そろったよーーーー");
            Crear.SetActive(true);
            Pinpon.Play();
            BGM.Stop();
            Riset.SetActive(false);
            ButtonGroup.interactable = false;
            isclear = true;

            Mode2Data.TimeData = TimeData_Clear;
        }
        if (!isclear)
        {
            TimeData_Clear += Time.deltaTime;
        }
        if (isclear)
        {
            Dtime  += Time.deltaTime;
            if(Dtime > 3)
            {
                isclear = false;
                Go_to_Result();
            }
        }

        if(mokorikaisuu <= 0)
        {
            ButtonGroup.interactable = false;
        }
    }

    bool AreCubeStatesEqual(Mode2_PLmodel.CubeColor[,,] state1, Mode2CubeModel3D.CubeColor[,,] state2)
    {
        // 6面、3行、3列でループ
        for (int face = 0; face < 6; face++)
        {
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    // 1つでも色が違ったら、その時点で false を返す
                    // (int)にキャストして、enumの型が違っても数値で比較します
                    if ((int)state1[face, row, col] != (int)state2[face, row, col])
                    {
                        return false;
                    }
                }
            }
        }

        // すべてのループを抜けた（＝1つも違うところがなかった）
        return true;
    }

    void CreateQuiz(int kaisuu,int tumekaisuu)
    {

        RotateQuiz = null;

        Riset_Cube();
        int[] kaitenkaisuu = new int[kaisuu];

        kaitenkaisuu = MakeKaiten(kaisuu);
        RotateQuiz = kaitenkaisuu;
        

        if(kaisuu < tumekaisuu)
        {
            tumekaisuu  =  0;
        }
        NumberOfRotate = kaisuu;
        NumberOfTume = tumekaisuu;

        mokorikaisuu = tumekaisuu;

        ResultKaisuuText.text = $"{mokorikaisuu}";

        for (int i = 0;i < kaisuu; i++)
        {
            Rotate_3DCube(kaitenkaisuu[i]);
        }
        for(int i = 0; i< kaisuu - tumekaisuu; i++)
        {
            Rotate_PLCube(kaitenkaisuu[i]);
        }




    }

    void ResetQuiz_mode2()
    {
        Riset_Cube();
        if(!(RotateQuiz == null))
        {
            for (int i = 0; i < NumberOfRotate; i++)
            {
                Rotate_3DCube(RotateQuiz[i]);
            }
            for (int i = 0; i < NumberOfRotate - NumberOfTume; i++)
            {
                Rotate_PLCube(RotateQuiz[i]);
            }
            mokorikaisuu = NumberOfTume;
            ResultKaisuuText.text = $"{mokorikaisuu}";
            ButtonGroup.interactable = true;
        }



    }


    int[] MakeKaiten(int kaisuu)
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

    void Riset_Cube()
    {
        //color_Riset
        cubeModel3D.Riset_Collor();
        pl_Model.Riset_Collor();

        //3Dキューブを壊してリセット
        cube3D_generator.DestroyCube();
        cube3D_generator.GenerateCube();
        cube3D_generator.ApplyColorsToCube();

        //以下は平面のアップデート処理
        PL_Generator.UpdateView(pl_Model);
        

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

    void Go_to_Result()
    {
        string sceneName = "Mode2Result";

        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            UnityEngine.Debug.Log("Scene name is empty!");
        }
    }

    void RotateButtonUp1()
    {
        Rotate_PLCube(1);
        RotateCounter[RotateCounter_NumCounter] = 1;
        RotateCounter_NumCounter += 1;
        mokorikaisuu -= 1;
        ResultKaisuuText.text = $"{mokorikaisuu}";
    }
    void RotateButtonUp2()
    {
        Rotate_PLCube(2);
        RotateCounter[RotateCounter_NumCounter] = 2;
        RotateCounter_NumCounter += 1;
        mokorikaisuu -= 1;
        ResultKaisuuText.text = $"{mokorikaisuu}";

    }
    void RotateButtonUp3()
    {
        Rotate_PLCube(3);
        RotateCounter[RotateCounter_NumCounter] = 3;
        RotateCounter_NumCounter += 1;
        mokorikaisuu -= 1;
        ResultKaisuuText.text = $"{mokorikaisuu}";
    }
    void RotateButtonDown1()
    {
        Rotate_PLCube(4);
        RotateCounter[RotateCounter_NumCounter] = 4;
        RotateCounter_NumCounter += 1;
        mokorikaisuu -= 1;
        ResultKaisuuText.text = $"{mokorikaisuu}";
    }
    void RotateButtonDown2()
    {
        Rotate_PLCube(5);
        RotateCounter[RotateCounter_NumCounter] = 5;
        RotateCounter_NumCounter += 1;
        mokorikaisuu -= 1;
        ResultKaisuuText.text = $"{mokorikaisuu}";
    }
    void RotateButtonDown3()
    {
        Rotate_PLCube(6);
        RotateCounter[RotateCounter_NumCounter] = 6;
        RotateCounter_NumCounter += 1;
        mokorikaisuu -= 1;
        ResultKaisuuText.text = $"{mokorikaisuu}";
    }
    void RotateButtonLeft1()
    {
        Rotate_PLCube(10);
        RotateCounter[RotateCounter_NumCounter] = 10;
        RotateCounter_NumCounter += 1;
        mokorikaisuu -= 1;
        ResultKaisuuText.text = $"{mokorikaisuu}";
    }
    void RotateButtonLeft2()
    {
        Rotate_PLCube(11);
        RotateCounter[RotateCounter_NumCounter] = 11;
        RotateCounter_NumCounter += 1;
        mokorikaisuu -= 1;
        ResultKaisuuText.text = $"{mokorikaisuu}";
    }
    void RotateButtonLeft3()
    {
        Rotate_PLCube(12);
        RotateCounter[RotateCounter_NumCounter] = 12;
        RotateCounter_NumCounter += 1;
        mokorikaisuu -= 1;
        ResultKaisuuText.text = $"{mokorikaisuu}";
    }
    void RotateButtonRight1()
    {
        Rotate_PLCube(7);
        RotateCounter[RotateCounter_NumCounter] = 7;
        RotateCounter_NumCounter += 1;
        mokorikaisuu -= 1;
        ResultKaisuuText.text = $"{mokorikaisuu}";
    }
    void RotateButtonRight2()
    {
        Rotate_PLCube(8);
        RotateCounter[RotateCounter_NumCounter] = 8;
        RotateCounter_NumCounter += 1;
        mokorikaisuu -= 1;
        ResultKaisuuText.text = $"{mokorikaisuu}";
    }
    void RotateButtonRight3()
    {
        Rotate_PLCube(9);
        RotateCounter[RotateCounter_NumCounter] = 9;
        RotateCounter_NumCounter += 1;
        mokorikaisuu -= 1;
        ResultKaisuuText.text = $"{mokorikaisuu}";
    }
    void RotateButtonImR1()
    {
        Rotate_PLCube(13);
        RotateCounter[RotateCounter_NumCounter] = 13;
        RotateCounter_NumCounter += 1;
        mokorikaisuu -= 1;
        ResultKaisuuText.text = $"{mokorikaisuu}";
    }
    void RotateButtonImR2()
    {
        Rotate_PLCube(14);
        RotateCounter[RotateCounter_NumCounter] = 14;
        RotateCounter_NumCounter += 1;
        mokorikaisuu -= 1;
        ResultKaisuuText.text = $"{mokorikaisuu}";
    }
    void RotateButtonImR3()
    {
        Rotate_PLCube(15);
        RotateCounter[RotateCounter_NumCounter] = 15;
        RotateCounter_NumCounter += 1;
        mokorikaisuu -= 1;
        ResultKaisuuText.text = $"{mokorikaisuu}";
    }
    void RotateButtonImL1()
    {
        Rotate_PLCube(16);
        RotateCounter[RotateCounter_NumCounter] = 16;
        RotateCounter_NumCounter += 1;
        mokorikaisuu -= 1;
        ResultKaisuuText.text = $"{mokorikaisuu}";
    }
    void RotateButtonImL2()
    {
        Rotate_PLCube(17);
        RotateCounter[RotateCounter_NumCounter] = 17;
        RotateCounter_NumCounter += 1;
        mokorikaisuu -= 1;
        ResultKaisuuText.text = $"{mokorikaisuu}";
    }
    void RotateButtonImL3()
    {
        Rotate_PLCube(18);
        RotateCounter[RotateCounter_NumCounter] = 18;
        RotateCounter_NumCounter += 1;
        mokorikaisuu -= 1;
        ResultKaisuuText.text = $"{mokorikaisuu}";
    }






    void InitializeColorMap()
    {
        colorMap = new Dictionary<Mode2_PLmodel.CubeColor, Color>()
        {
            { Mode2_PLmodel.CubeColor.White, Color.white },
            { Mode2_PLmodel.CubeColor.Yellow, Color.yellow },
            { Mode2_PLmodel.CubeColor.Red, Color.red },
            { Mode2_PLmodel.CubeColor.Orange, new Color(1, 0.5f, 0) }, // Orange
            { Mode2_PLmodel.CubeColor.Green, Color.green },
            { Mode2_PLmodel.CubeColor.Blue, Color.blue }
        };
    }

    void InitializeColorMap2()
    {
        colorMap2 = new Dictionary<Mode2CubeModel3D.CubeColor, Color>()
        {
            { Mode2CubeModel3D.CubeColor.White, Color.white },
            { Mode2CubeModel3D.CubeColor.Yellow, Color.yellow },
            { Mode2CubeModel3D.CubeColor.Red, Color.red },
            { Mode2CubeModel3D.CubeColor.Orange, new Color(1, 0.5f, 0) }, // Orange
            { Mode2CubeModel3D.CubeColor.Green, Color.green },
            { Mode2CubeModel3D.CubeColor.Blue, Color.blue }
        };
    }

    void Rotate_3DCube(int Whitch)
    {
        if (Whitch == 1)
        {
            cubeModel3D.RotateFace(Mode2CubeModel3D.Face.Left, true, Mode2CubeModel3D.Inner_Face.X, false);
            cubeModel3D.RotateFace(Mode2CubeModel3D.Face.Left, true, Mode2CubeModel3D.Inner_Face.X, false);
            cubeModel3D.RotateFace(Mode2CubeModel3D.Face.Left, true, Mode2CubeModel3D.Inner_Face.X, false);
        }

        if (Whitch == 2)
            cubeModel3D.RotateFace(Mode2CubeModel3D.Face.Up, false, Mode2CubeModel3D.Inner_Face.X, true);

        if (Whitch == 3)
            cubeModel3D.RotateFace(Mode2CubeModel3D.Face.Right, true, Mode2CubeModel3D.Inner_Face.X, false);

        if (Whitch == 4)
            cubeModel3D.RotateFace(Mode2CubeModel3D.Face.Left, true, Mode2CubeModel3D.Inner_Face.X, false);

        if (Whitch == 5)
        {
            cubeModel3D.RotateFace(Mode2CubeModel3D.Face.Up, false, Mode2CubeModel3D.Inner_Face.X, true);
            cubeModel3D.RotateFace(Mode2CubeModel3D.Face.Up, false, Mode2CubeModel3D.Inner_Face.X, true);
            cubeModel3D.RotateFace(Mode2CubeModel3D.Face.Up, false, Mode2CubeModel3D.Inner_Face.X, true);
        }
        if (Whitch == 6)
        {
            cubeModel3D.RotateFace(Mode2CubeModel3D.Face.Right, true, Mode2CubeModel3D.Inner_Face.X, false);
            cubeModel3D.RotateFace(Mode2CubeModel3D.Face.Right, true, Mode2CubeModel3D.Inner_Face.X, false);
            cubeModel3D.RotateFace(Mode2CubeModel3D.Face.Right, true, Mode2CubeModel3D.Inner_Face.X, false);
        }
        if (Whitch == 7)
        {
            cubeModel3D.RotateFace(Mode2CubeModel3D.Face.Up, true, Mode2CubeModel3D.Inner_Face.X, false);
        }
        if (Whitch == 8)
        {
            cubeModel3D.RotateFace(Mode2CubeModel3D.Face.Up, false, Mode2CubeModel3D.Inner_Face.Y, true);
            cubeModel3D.RotateFace(Mode2CubeModel3D.Face.Up, false, Mode2CubeModel3D.Inner_Face.Y, true);
            cubeModel3D.RotateFace(Mode2CubeModel3D.Face.Up, false, Mode2CubeModel3D.Inner_Face.Y, true);
        }
        if (Whitch == 9)
        {
            cubeModel3D.RotateFace(Mode2CubeModel3D.Face.Down, true, Mode2CubeModel3D.Inner_Face.X, false);
            cubeModel3D.RotateFace(Mode2CubeModel3D.Face.Down, true, Mode2CubeModel3D.Inner_Face.X, false);
            cubeModel3D.RotateFace(Mode2CubeModel3D.Face.Down, true, Mode2CubeModel3D.Inner_Face.X, false);
        }

        if (Whitch == 10)
        {
            cubeModel3D.RotateFace(Mode2CubeModel3D.Face.Up, true, Mode2CubeModel3D.Inner_Face.X, false);
            cubeModel3D.RotateFace(Mode2CubeModel3D.Face.Up, true, Mode2CubeModel3D.Inner_Face.X, false);
            cubeModel3D.RotateFace(Mode2CubeModel3D.Face.Up, true, Mode2CubeModel3D.Inner_Face.X, false);
        }
        if (Whitch == 11)
        {
            cubeModel3D.RotateFace(Mode2CubeModel3D.Face.Up, false, Mode2CubeModel3D.Inner_Face.Y, true);

        }
        if (Whitch == 12)
        {
            cubeModel3D.RotateFace(Mode2CubeModel3D.Face.Down, true, Mode2CubeModel3D.Inner_Face.X, false);

        }


        if (Whitch == 13)
        {
            cubeModel3D.RotateFace(Mode2CubeModel3D.Face.Front, true, Mode2CubeModel3D.Inner_Face.X, false);
            cubeModel3D.RotateFace(Mode2CubeModel3D.Face.Front, true, Mode2CubeModel3D.Inner_Face.X, false);
            cubeModel3D.RotateFace(Mode2CubeModel3D.Face.Front, true, Mode2CubeModel3D.Inner_Face.X, false);
        }

        if (Whitch == 14)
            cubeModel3D.RotateFace(Mode2CubeModel3D.Face.Up, false, Mode2CubeModel3D.Inner_Face.Z, true);

        if (Whitch == 15)
            cubeModel3D.RotateFace(Mode2CubeModel3D.Face.Back, true, Mode2CubeModel3D.Inner_Face.X, false);

        if (Whitch == 16) cubeModel3D.RotateFace(Mode2CubeModel3D.Face.Front, true, Mode2CubeModel3D.Inner_Face.X, false);

        if (Whitch == 17)
        {
            cubeModel3D.RotateFace(Mode2CubeModel3D.Face.Up, false, Mode2CubeModel3D.Inner_Face.Z, true);
            cubeModel3D.RotateFace(Mode2CubeModel3D.Face.Up, false, Mode2CubeModel3D.Inner_Face.Z, true);
            cubeModel3D.RotateFace(Mode2CubeModel3D.Face.Up, false, Mode2CubeModel3D.Inner_Face.Z, true);
        }

        if (Whitch == 18)
        {
            cubeModel3D.RotateFace(Mode2CubeModel3D.Face.Back, true, Mode2CubeModel3D.Inner_Face.X, false);
            cubeModel3D.RotateFace(Mode2CubeModel3D.Face.Back, true, Mode2CubeModel3D.Inner_Face.X, false);
            cubeModel3D.RotateFace(Mode2CubeModel3D.Face.Back, true, Mode2CubeModel3D.Inner_Face.X, false);
        }
        cube3D_generator.ApplyColorsToCube();

    }

    void Rotate_PLCube(int Whitch)
    {
        if (Whitch == 1)
        {
            pl_Model.RotateFace(Mode2_PLmodel.Face.Left, true, Mode2_PLmodel.Inner_Face.X, false);
            pl_Model.RotateFace(Mode2_PLmodel.Face.Left, true, Mode2_PLmodel.Inner_Face.X, false);
            pl_Model.RotateFace(Mode2_PLmodel.Face.Left, true, Mode2_PLmodel.Inner_Face.X, false);
        }

        if (Whitch == 2)
            pl_Model.RotateFace(Mode2_PLmodel.Face.Up, false, Mode2_PLmodel.Inner_Face.X, true);

        if (Whitch == 3)
            pl_Model.RotateFace(Mode2_PLmodel.Face.Right, true, Mode2_PLmodel.Inner_Face.X, false);

        if (Whitch == 4)
            pl_Model.RotateFace(Mode2_PLmodel.Face.Left, true, Mode2_PLmodel.Inner_Face.X, false);

        if (Whitch == 5)
        {
            pl_Model.RotateFace(Mode2_PLmodel.Face.Up, false, Mode2_PLmodel.Inner_Face.X, true);
            pl_Model.RotateFace(Mode2_PLmodel.Face.Up, false, Mode2_PLmodel.Inner_Face.X, true);
            pl_Model.RotateFace(Mode2_PLmodel.Face.Up, false, Mode2_PLmodel.Inner_Face.X, true);
        }
        if (Whitch == 6)
        {
            pl_Model.RotateFace(Mode2_PLmodel.Face.Right, true, Mode2_PLmodel.Inner_Face.X, false);
            pl_Model.RotateFace(Mode2_PLmodel.Face.Right, true, Mode2_PLmodel.Inner_Face.X, false);
            pl_Model.RotateFace(Mode2_PLmodel.Face.Right, true, Mode2_PLmodel.Inner_Face.X, false);
        }
        if (Whitch == 7)
        {
            pl_Model.RotateFace(Mode2_PLmodel.Face.Up, true, Mode2_PLmodel.Inner_Face.X, false);
        }
        if (Whitch == 8)
        {
            pl_Model.RotateFace(Mode2_PLmodel.Face.Up, false, Mode2_PLmodel.Inner_Face.Y, true);
            pl_Model.RotateFace(Mode2_PLmodel.Face.Up, false, Mode2_PLmodel.Inner_Face.Y, true);
            pl_Model.RotateFace(Mode2_PLmodel.Face.Up, false, Mode2_PLmodel.Inner_Face.Y, true);
        }
        if (Whitch == 9)
        {
            pl_Model.RotateFace(Mode2_PLmodel.Face.Down, true, Mode2_PLmodel.Inner_Face.X, false);
            pl_Model.RotateFace(Mode2_PLmodel.Face.Down, true, Mode2_PLmodel.Inner_Face.X, false);
            pl_Model.RotateFace(Mode2_PLmodel.Face.Down, true, Mode2_PLmodel.Inner_Face.X, false);
        }

        if (Whitch == 10)
        {
            pl_Model.RotateFace(Mode2_PLmodel.Face.Up, true, Mode2_PLmodel.Inner_Face.X, false);
            pl_Model.RotateFace(Mode2_PLmodel.Face.Up, true, Mode2_PLmodel.Inner_Face.X, false);
            pl_Model.RotateFace(Mode2_PLmodel.Face.Up, true, Mode2_PLmodel.Inner_Face.X, false);
        }
        if (Whitch == 11)
        {
            pl_Model.RotateFace(Mode2_PLmodel.Face.Up, false, Mode2_PLmodel.Inner_Face.Y, true);

        }
        if (Whitch == 12)
        {
            pl_Model.RotateFace(Mode2_PLmodel.Face.Down, true, Mode2_PLmodel.Inner_Face.X, false);

        }


        if (Whitch == 13)
        {
            pl_Model.RotateFace(Mode2_PLmodel.Face.Front, true, Mode2_PLmodel.Inner_Face.X, false);
            pl_Model.RotateFace(Mode2_PLmodel.Face.Front, true, Mode2_PLmodel.Inner_Face.X, false);
            pl_Model.RotateFace(Mode2_PLmodel.Face.Front, true, Mode2_PLmodel.Inner_Face.X, false);
        }

        if (Whitch == 14)
            pl_Model.RotateFace(Mode2_PLmodel.Face.Up, false, Mode2_PLmodel.Inner_Face.Z, true);

        if (Whitch == 15)
            pl_Model.RotateFace(Mode2_PLmodel.Face.Back, true, Mode2_PLmodel.Inner_Face.X, false);

        if (Whitch == 16) pl_Model.RotateFace(Mode2_PLmodel.Face.Front, true, Mode2_PLmodel.Inner_Face.X, false);

        if (Whitch == 17)
        {
            pl_Model.RotateFace(Mode2_PLmodel.Face.Up, false, Mode2_PLmodel.Inner_Face.Z, true);
            pl_Model.RotateFace(Mode2_PLmodel.Face.Up, false, Mode2_PLmodel.Inner_Face.Z, true);
            pl_Model.RotateFace(Mode2_PLmodel.Face.Up, false, Mode2_PLmodel.Inner_Face.Z, true);
        }

        if (Whitch == 18)
        {
            pl_Model.RotateFace(Mode2_PLmodel.Face.Back, true, Mode2_PLmodel.Inner_Face.X, false);
            pl_Model.RotateFace(Mode2_PLmodel.Face.Back, true, Mode2_PLmodel.Inner_Face.X, false);
            pl_Model.RotateFace(Mode2_PLmodel.Face.Back, true, Mode2_PLmodel.Inner_Face.X, false);
        }

        PL_Generator.UpdateView(pl_Model);

    }
}
