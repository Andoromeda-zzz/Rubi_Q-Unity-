using System.Collections;
using System.Diagnostics;
using System.Linq;
using TMPro;                // 1. TextMeshPro を使うため
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class GameManager_Risult : MonoBehaviour
{


    private string[] Risult_Data = new string[10000];
    private bool isCrear;
    private int risultMode;


    [Header("表示するテキスト (TMP)")]
    public TextMeshProUGUI correctCountText;  // 正解数
    public TextMeshProUGUI passCountText;     // パス回数
    public TextMeshProUGUI avgTimeText;       // 平均解答時間
    public TextMeshProUGUI scoreText;         // スコア
    public TextMeshProUGUI rankText;          // ランク
    public TextMeshProUGUI selectedMode;

    [Header("アニメーション設定")]
    public float countUpDuration = 1.0f;     // 1つの項目をカウントアップする時間 (秒)
    public float delayBetweenItems = 0.5f;   // 次の項目を表示するまでの待機時間 (秒)
    public float delayBeforeRank = 1.0f;     // スコア表示後、ランクを表示するまでの時間

    [Header("BGM")]
    public AudioSource ARankBGM;
    public AudioSource SRankBGM;
    public AudioSource RetireRankBGM;

    
    // --- 外部から与えられる（想定の）データ ---
    // (実際には、前のシーンやGameManagerからこれらの値を受け取ってください)
    private int targetCorrectCount = 8;
    private int targetPassCount = 2;
    private float targetAvgTime = 12.5f;
    private int targetScore = 8500;
    private string targetRank = "A";

    private bool isback = false;
    private float time = 0.0f;
   


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        
       
        Risult_Data = DataObject.TFdata;
        risultMode = DataObject.mode;

        isCrear = DataObject.isClea;
        int i = 0;
        int j = 0;

        
        if (isCrear)
        {
            for(int k = 0; k >= 0; k++)
            {
                if (Risult_Data[k] == "T") i++;
                else if (Risult_Data[k] == "F") j++;
                else break;
            }

            targetCorrectCount = i;
            targetPassCount = j;
            if (i != 0 || j!=0)
            {
                targetAvgTime = 120 / (i + j);
            }
            else
            {
                targetAvgTime = 0;
            }
                targetScore = (int)((targetCorrectCount * 1000 - targetPassCount * 1000 + (120 - targetAvgTime) * 10) * 10 + (11 - risultMode) * 2000);
            if (targetScore <= 0)
            {
                targetScore = 0;
            }

            if (targetScore <= 30000) targetRank = "D";
            else if (targetScore <= 60000) targetRank = "C";
            else if (targetScore <= 80000) targetRank = "B";
            else if (targetScore <= 120000) targetRank = "A";
            else if (targetScore <= 130000)
            {
                targetRank = "S";
                rankText.color = Color.yellow;
            }
            else 
            {
                targetRank = "U";
                rankText.color = Color.cyan;
            }
        }

        if (!isCrear)
        {
            for (int k = 0; ; k++)
            {
                if (Risult_Data[k] == "T") i++;
                else if (Risult_Data[k] == "F") j++;
                else break;
            }

            targetCorrectCount = i;
            targetPassCount = j;
            targetAvgTime = 0.00f;


            targetRank = "N";
        }

        if(targetRank == "S" || targetRank == "U")
        {
            SRankBGM.Play();
        }
        else if (targetRank == "N")
        {
            RetireRankBGM.Play();
        }
        else
        {
             ARankBGM.Play();
        }

        

        StartCoroutine(ShowResultsSequence());

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            TitleBack_Script();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UnityEngine.Application.Quit();
        }


        if (isback)
        {
            time += Time.deltaTime;
            if(time >= 60)
            {
                TitleBack_Script();
                isback = false;
            }


        }

    }

    void TitleBack_Script()
    {
        string sceneName = "Title";
        DataObject.TFdata = null;
        DataObject.isClea = false;

        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            UnityEngine.Debug.Log("Scene name is empty!");
        }
    }

    private IEnumerator ShowResultsSequence()
    {
        // 1. まず全てのテキストを非表示にする (または空にする)
        correctCountText.gameObject.SetActive(false);
        passCountText.gameObject.SetActive(false);
        avgTimeText.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(false);
        rankText.gameObject.SetActive(false);
        selectedMode.gameObject.SetActive(false);

        // 少し待ってから開始
        //yield return new WaitForSeconds(0.5f);

        // --- 2. 正解数をカウントアップ ---
        // (UIを表示状態にしてからカウントアップを開始)
        correctCountText.gameObject.SetActive(true);
        // CountUpCoroutine が終わるまで待つ
        yield return StartCoroutine(
            CountUpCoroutine(correctCountText, targetCorrectCount, countUpDuration)
        );

        // 項目間の待機
        yield return new WaitForSeconds(delayBetweenItems);

        // --- 3. パス回数をカウントアップ ---
        passCountText.gameObject.SetActive(true);
        yield return StartCoroutine(
            CountUpCoroutine(passCountText, targetPassCount, countUpDuration)
        );

        yield return new WaitForSeconds(delayBetweenItems);

        // --- 4. 平均解答時間をカウントアップ ---
        // (平均時間は小数第2位 (F2) まで表示する例)
        avgTimeText.gameObject.SetActive(true);
        
        yield return StartCoroutine(
                CountUpCoroutine(avgTimeText, targetAvgTime, countUpDuration, "F1") // "F2" = 小数第2位まで
            );

        yield return new WaitForSeconds(delayBetweenItems);

        selectedMode.gameObject.SetActive(true);

        if (risultMode >= 9)
        {
            selectedMode.color = Color.green;
            selectedMode.text = "Easy";
        }
        else if (risultMode >= 4)
        {
            selectedMode.text = "Normal";
            selectedMode.color = Color.blue;
        }
        else if (risultMode >= 2)
        {
            selectedMode.text = "Hard";
            selectedMode.color = Color.red;
        }
        else 
        { 
            selectedMode.text = "VeryHard";
            selectedMode.color = Color.black;
        }




        yield return new WaitForSeconds(delayBetweenItems + 0.6f);

        // --- 5. スコアをカウントアップ ---
        // (スコアは少しゆっくりカウントアップする例)
        scoreText.gameObject.SetActive(true);

        if (isCrear)
        {
            yield return StartCoroutine(
                CountUpCoroutine(scoreText, targetScore, countUpDuration * 1.5f) // 1.5倍の時間をかける
            );
        }

        else
        {
            scoreText.text = "Retired";
        }
        // ランク表示前の「タメ」
        yield return new WaitForSeconds(delayBeforeRank);

        // --- 6. ランクを表示 ---
        // (ランクはカウントアップしない)
        rankText.gameObject.SetActive(true);
        rankText.text = targetRank; // 書式は適宜調整してください

        isback = true;

        // (ランク表示時に効果音やアニメーションを入れても良い)
    }

    /// <summary>
    /// 数値を0からターゲット値までカウントアップする汎用コルーチン
    /// </summary>
    /// <param name="textUI">対象のTextMeshProUGUI</param>
    /// <param name="targetValue">最終的な値</param>
    /// <param name="duration">カウントアップにかかる時間</param>
    /// <param name="format">数値の書式 (F0=整数, F1=小数第1位, F2=小数第2位...)</param>
    private IEnumerator CountUpCoroutine(TextMeshProUGUI textUI, float targetValue, float duration, string format = "F0")
    {
        // (例: "Score: {0}" のように {0} を含むテキストを初期設定しておくと、
        //  {0} の部分だけが数値に置き換わります。
        //  ここでは単純に数値だけを表示する前提で進めます)

        float timer = 0f;
        float startValue = 0f;

        while (timer < duration)
        {
            // 経過時間に合わせて 0 -> duration の値を 0 -> 1 の割合(t)に変換
            timer += Time.deltaTime;
            float t = timer / duration;

            // t の割合を使って、開始値と目標値の間を補間 (Lerp)
            float currentValue = Mathf.Lerp(startValue, targetValue, t);

            // テキストに反映 (format で指定された書式で)
            textUI.text = currentValue.ToString(format);

            // 1フレーム待つ
            yield return null;
        }

        // ループ終了後、必ず最終的な値を正確に設定する
        textUI.text = targetValue.ToString(format);
    }
}
