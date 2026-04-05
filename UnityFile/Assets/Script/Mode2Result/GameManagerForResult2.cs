using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections; // コルーチン (IEnumerator) のために追加します
using UnityEngine.SceneManagement;

public class GameManagerForResult2 : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource BGM;
    public AudioSource BGM2;

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI stageText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI rankText;

    [Header("Button")]
    public Button ReturnTitle1;
    public Button ReturnTitle2;
    public Button ReturnChoose;
    public Button NextStage;
    public Button Retry;

    [Header("GameObject")]
    public GameObject NextStageObject;


    private float time = 0;
    private int stage_num = 0;
    private string Rank = "D";

    private float timer = 0;
    private int Return_kaitenkaisuu;
    private int Return_Tumekaisuu;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        time = Mode2Data.TimeData;
        stage_num = Mode2Data.StageData;
        Return_kaitenkaisuu = Mode2Data.kaitenkaisuuData;
        Return_Tumekaisuu = Mode2Data.TumekaisuuData;

        NextStageObject.SetActive(true);

        if (stage_num == 6)
        {
            NextStageObject.SetActive(false);
        }
        else
        {
            Mode2Data.StageData = stage_num + 1;

            switch(stage_num + 1)
            {
                case 2:
                    Mode2Data.kaitenkaisuuData = 5;
                    Mode2Data.TumekaisuuData = 1;
                    break;
                case 3:
                    Mode2Data.kaitenkaisuuData = 5;
                    Mode2Data.TumekaisuuData = 2;
                    break;
                case 4:
                    Mode2Data.kaitenkaisuuData = 7;
                    Mode2Data.TumekaisuuData = 2;
                    break;
                case 5:
                    Mode2Data.kaitenkaisuuData = 10;
                    Mode2Data.TumekaisuuData = 2;
                    break;
                case 6:
                    Mode2Data.kaitenkaisuuData = 10;
                    Mode2Data.TumekaisuuData = 3;
                    break;
            }
        }




        // ランク判定
        if (time > 180)
        {
            Rank = "D";
        }
        else if (time > 120)
        {
            Rank = "C";
        }
        else if (time > 60)
        {
            Rank = "B";
        }
        else if (time > 30)
        {
            Rank = "A";
        }
        else
        {
            Rank = "S";
        }
        if (Rank == "S")
        {
            BGM2.Play();
        }
        else
        {
            BGM.Play();
        }

        // --- ここから変更 ---
        // 最初にテキストを空にしておく (任意)
        // これをしないと、Unityエディタで設定した "New Text" などが最初から表示されてしまいます
        if (stageText != null) stageText.text = "";
        if (timeText != null) timeText.text = "";
        if (rankText != null) rankText.text = "";

        // 順番に表示するコルーチンを開始
        StartCoroutine(ShowResultsSequentially());
        // --- ここまで変更 ---
        ReturnTitle1.onClick.AddListener(ReturnTitle);
        ReturnTitle2.onClick.AddListener(ReturnTitle);
        ReturnChoose.onClick.AddListener(ReturnChoose_Go);
        NextStage.onClick.AddListener(NextStage_Go);

        Retry.onClick.AddListener(() => Retry_Stage(Return_kaitenkaisuu, Return_Tumekaisuu));






    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UnityEngine.Application.Quit();
        }

        timer += Time.deltaTime;
        if (timer > 60)
        {
            ReturnTitle();
        }
    }

    // --- ここから追加 ---
    /// <summary>
    /// 結果を1秒ごとに順番に表示するコルーチン
    /// </summary>
    private IEnumerator ShowResultsSequentially()
    {
        // 1. Stage を表示
        if (stageText != null)
        {
            stageText.text =  stage_num.ToString();
        }

        // 1秒待機
        yield return new WaitForSeconds(0.7f);

        // 2. Time を表示
        if (timeText != null)
        {
            timeText.text = time.ToString("F2") + "秒" ;
        }

        // 1秒待機
        yield return new WaitForSeconds(0.7f);

        // 3. Rank を表示
        if (rankText != null)
        {
            rankText.text =  Rank;
            rankText.color = Color.yellow;
        }

        // これでコルーチンは終了します
    }

    void Retry_Stage(int kasiuu,int tumekaisuu)
    {
        Mode2Data.kaitenkaisuuData = kasiuu;
        Mode2Data.TumekaisuuData = tumekaisuu;

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
    // --- ここまで追加 ---

    void ReturnTitle()
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

    void ReturnChoose_Go()
    {
        string sceneName = "ModeSelect";

        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            UnityEngine.Debug.Log("Scene name is empty!");
        }
    }

    void NextStage_Go()
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