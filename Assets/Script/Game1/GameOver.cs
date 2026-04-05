using UnityEngine;
using System.Collections;

public class GameOver : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("アニメーションさせるUI")]
    public RectTransform gameoverUI; // InspectorでMenuSetを割り当てる

    [Header("移動アニメーションの設定")]
    public Vector2 startPosition;  // アニメーションの開始位置 (画面外)
    public Vector2 endPosition;    // アニメーションの終了位置 (画面内)
    public float animationDuration = 0.5f; // アニメーションにかかる時間（秒）

    private bool isMenuShown = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Move1()
    {
        if (!isMenuShown)
        {
            StartCoroutine(SlideMenuCoroutine());
            isMenuShown = true;
        }
    }

    



    IEnumerator SlideMenuCoroutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < animationDuration)
        {
            // 経過時間に基づいて現在位置を計算 (Lerp)
            float t = elapsedTime / animationDuration;
            gameoverUI.anchoredPosition = Vector2.Lerp(startPosition, endPosition, t);

            // 経過時間を加算
            elapsedTime += Time.deltaTime;

            // 1フレーム待つ
            yield return null;
        }

        // アニメーション終了後、正確に終了位置に設定する
        gameoverUI.anchoredPosition = endPosition;
    }

    
}
