using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.ComponentModel;
using TMPro;



public class GameManagerForModeSelect : MonoBehaviour
{
    [Header("Button")]
    public Button BackButton;
    public Button Mode1;
    public Button Mode1_Easy;
    public Button Mode1_Nomal;
    public Button Mode1_Hard;
    public Button Mode1_VeryHard;
    public Button BacktoChoose;
    public Button Mode2;
    public Button BacktoChooseMode2;
    public Button BacktoChooseMode2_2;
    public Button Mode2_Move;
    public Button Mode2_Stage2;
    public Button Mode2_Stage3;
    public Button Mode2_Stage4;
    public Button Mode2_Stage5;
    public Button Mode2_Stage6;
    public Button Mode2_Stage7;
    public Button Mode2_Stage8;
    public Button Mode2_Stage9;
    public Button Mode2_Stage10;
    public Button Mode2_Stage11;
    public Button Mode2_Stage12;


    public Button RemoveGo;
    public Button StartMode2;

    public Button SlideStageSelect1;
    public Button SlideStageSelect7;


    [Header("GameObject")]

    public Mode1Diffrence Mode1_Selector;
    public MoveStageSelect StageSelect;
    public GameObject FinalAnswerMode2;
    public MoveModeSelectStage7 stage7;
    public MoveStage1 stage1;

    [Header("Audio")]
    public AudioSource ClickAudio;

    [Header("Text")]
    public TextMeshProUGUI Shuffle_Num;
    public TextMeshProUGUI Tume_Num;
    public TextMeshProUGUI sougou_Num;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        FinalAnswerMode2.SetActive(false);

        BackButton.onClick.AddListener(Back_to_Title);
        Mode1.onClick.AddListener(SlideManu_Mode1);
        Mode1_Easy.onClick.AddListener(mode1_Nanido_Easy);
        Mode1_Nomal.onClick.AddListener(mode1_Nanido_Normal);
        Mode1_Hard.onClick.AddListener(mode1_Nanido_Hard);
        Mode1_VeryHard.onClick.AddListener(mode1_Nanido_VeryHard);
        BacktoChoose.onClick.AddListener(SlideManu_Mode1_Modoru);

        SlideStageSelect1.onClick.AddListener(SlideStageSelect);
        SlideStageSelect7.onClick.AddListener(SlideStageSelectModoru);

        Mode2.onClick.AddListener(StageSelectMode2);
        BacktoChooseMode2.onClick.AddListener(StageSelectMode2_modoru);
        BacktoChooseMode2_2.onClick.AddListener(StageSelectMode2_modoru);

        Mode2_Move.onClick.AddListener(() => NextStep_Mode2(3, 1, 1));
        Mode2_Stage2.onClick.AddListener(() => NextStep_Mode2(5, 1, 2));
        Mode2_Stage3.onClick.AddListener(() => NextStep_Mode2(5, 2, 3));
        Mode2_Stage4.onClick.AddListener(() => NextStep_Mode2(7, 2, 4));
        Mode2_Stage5.onClick.AddListener(() => NextStep_Mode2(10, 2, 5));
        Mode2_Stage6.onClick.AddListener(() => NextStep_Mode2(15, 2, 6));
        Mode2_Stage7.onClick.AddListener(() => NextStep_Mode2(5, 3, 7));
        Mode2_Stage8.onClick.AddListener(() => NextStep_Mode2(7, 3, 8));
        Mode2_Stage9.onClick.AddListener(() => NextStep_Mode2(10, 3, 9));
        Mode2_Stage10.onClick.AddListener(() => NextStep_Mode2(10, 4, 10));
        Mode2_Stage11.onClick.AddListener(() => NextStep_Mode2(10, 5, 11));
        Mode2_Stage12.onClick.AddListener(() => NextStep_Mode2(10, 6, 12));
        StartMode2.onClick.AddListener(NextStep_Mode2_Changescenes);
        RemoveGo.onClick.AddListener(RemoveGoUI);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UnityEngine.Application.Quit();
        }
        if (Input.GetMouseButtonDown(0))
        {
            ClickAudio.Play();
        }
    }

    void Back_to_Title()
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

    void SlideStageSelect()
    {
        stage7.Move1();
        stage1.Move1();
    }

    void SlideStageSelectModoru()
    {
        stage7.Move1End();
        stage1.Move1End();
    }

    void SlideManu_Mode1()
    {
        Mode1_Selector.Move1();
    }

    void SlideManu_Mode1_Modoru()
    {
        Mode1_Selector.Move1End();
    }

    void StageSelectMode2()
    {
        StageSelect.Move1();
    }

    void StageSelectMode2_modoru()
    {
        StageSelect.Move1End();
    }


    void mode1_Nanido_Easy()
    {

        DataObject.mode = 10;
        NextStep_Mode1();
    }
    void mode1_Nanido_Normal()
    {
        DataObject.mode = 5;
        NextStep_Mode1();
    }
    void mode1_Nanido_Hard()
    {
        DataObject.mode = 3;
        NextStep_Mode1();
    }
    void mode1_Nanido_VeryHard()
    {
        DataObject.mode = 1;
        NextStep_Mode1();
    }


    void NextStep_Mode1()
    {
        string sceneName = "main_game_Scene";

        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            UnityEngine.Debug.Log("Scene name is empty!");
        }
    }

    void NextStep_Mode2(int kaisuu,int tumekaisuu,int ModeNumber)
    {
        Mode2Data.kaitenkaisuuData = kaisuu;
        Mode2Data.TumekaisuuData = tumekaisuu;
        Mode2Data.StageData = ModeNumber;
        Shuffle_Num.text = $"{kaisuu}";
        Tume_Num.text = $"{tumekaisuu}";
        sougou_Num.text = $"Lv.{kaisuu + tumekaisuu * 7 - 9}";
        FinalAnswerMode2.SetActive(true);

    }

    void RemoveGoUI()
    {
        FinalAnswerMode2.SetActive(false);
    }

    void NextStep_Mode2_Changescenes()
    {
        string sceneName = "Mode2";
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            UnityEngine.Debug.Log("Scene name is empty!");
        }
    }
}
