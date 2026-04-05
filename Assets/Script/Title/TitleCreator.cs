using UnityEngine;
using System.Collections;

public class TitleCreator : MonoBehaviour
{
    [Header("アニメーションさせるUI")]
    public RectTransform menuSetRectTransform; // InspectorでMenuSetを割り当てる

    [Header("移動アニメーションの設定")]
    public Vector2 startPosition;  // アニメーションの開始位置 (画面外)
    public Vector2 endPosition;    // アニメーションの終了位置 (画面内)
    public float animationDuration = 0.5f; // アニメーションにかかる時間（秒）

    [Header("その他のUI")]
    public GameObject blinkingText; // InspectorでBlinkingTextを割り当てる
    public GameObject Display_TouchText;
    public GameObject AfterTouchText;

    [Header("Videos")]
    public GameObject Video;
    public GameObject Video_Back_Image;

    private bool isMenuShown = false;
    private float currentTime = 0f;

    

    void Update()
    {
        // まだメニューが表示されておらず、何かのキーが押されたら
        if (!isMenuShown && Input.anyKeyDown)
        {

            isMenuShown = true;

            // 点滅テキストを非表示にする
            blinkingText.SetActive(false);
            Display_TouchText.SetActive(false);
            Video.SetActive(false);
            Video_Back_Image.SetActive(false);

            AfterTouchText.SetActive(true);

            // アニメーションを開始する
            StartCoroutine(SlideMenuCoroutine());
        }

        if (isMenuShown)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= 20)
            {
                StartCoroutine(SlideMenuModoru());
                blinkingText.SetActive(true);
                AfterTouchText.SetActive(false);
                Display_TouchText.SetActive(true);
                isMenuShown = false;
                currentTime = 0;
                Video.SetActive(true);
                Video_Back_Image.SetActive(true);
            }
        }
    }

    // UIを滑らかに動かすコルーチン
    IEnumerator SlideMenuCoroutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < animationDuration)
        {
            // 経過時間に基づいて現在位置を計算 (Lerp)
            float t = elapsedTime / animationDuration;
            menuSetRectTransform.anchoredPosition = Vector2.Lerp(startPosition, endPosition, t);

            // 経過時間を加算
            elapsedTime += Time.deltaTime;

            // 1フレーム待つ
            yield return null;
        }

        // アニメーション終了後、正確に終了位置に設定する
        menuSetRectTransform.anchoredPosition = endPosition;
    }

    IEnumerator SlideMenuModoru()
    {
        float elapsedTime = 0f;

        while (elapsedTime < animationDuration)
        {
            // 経過時間に基づいて現在位置を計算 (Lerp)
            float t = elapsedTime / animationDuration;
            menuSetRectTransform.anchoredPosition = Vector2.Lerp(endPosition, startPosition, t);

            // 経過時間を加算
            elapsedTime += Time.deltaTime;

            // 1フレーム待つ
            yield return null;
        }

        // アニメーション終了後、正確に終了位置に設定する
        menuSetRectTransform.anchoredPosition = startPosition;
    }
}