using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

public class QuizManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private readonly List<int> masterQuestionNumbers = new List<int> { 1, 2, 3, 4 };
    private readonly List<int> CubeShuffleNumber = new List<int> { 1, 2, 3, 4 ,5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18};

    // 外部から参照されるQueue（中身）
    // { get; private set; } にすると、外部から中身の変更はできないが参照はできる
    public Queue<int> shuffledQuestionOrder { get; private set; }

    // ゲーム開始時に、まず最初のシャッフルを実行
    void Awake()
    {
        // Queueを初期化
        shuffledQuestionOrder = new Queue<int>();
        // 最初のシャッフルを実行
        PerformShuffle();
    }

    // ★★★
    /// <summary>
    /// 【公開用】外部からシャッフルを要求する
    /// </summary>
    public void RequestShuffle()
    {
        UnityEngine.Debug.Log("外部からシャッフルのリクエストがありました。");
        PerformShuffle();
    }

    // ★★★
    /// <summary>
    /// 【内部用】シャッフルとQueueへの格納を実行する
    /// </summary>
    private void PerformShuffle()
    {
        // 1. まずQueueを空にする
        shuffledQuestionOrder.Clear();

        // 2. 「原本」リストからコピーを作成する
        //    (こうしないと、2回目以降シャッフルする中身がなくなる)
        List<int> listToShuffle = masterQuestionNumbers.ToList();

        // 3. コピーしたリストをシャッフル (Fisher-Yates)
        for (int i = listToShuffle.Count - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            int temp = listToShuffle[i];
            listToShuffle[i] = listToShuffle[j];
            listToShuffle[j] = temp;
        }

        // 4. シャッフルしたリストをQueueにPush (Enqueue)
        UnityEngine.Debug.Log("--- 新しいシャッフル結果 ---");
        foreach (int number in listToShuffle)
        {
            shuffledQuestionOrder.Enqueue(number);
            UnityEngine.Debug.Log(number + " をPushしました");
        }
    }

    public int shuffl_Cube()
    {
        int R = UnityEngine.Random.Range(1, 18);
        return R;
    }
}
