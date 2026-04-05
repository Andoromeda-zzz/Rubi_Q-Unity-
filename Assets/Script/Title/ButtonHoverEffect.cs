using UnityEngine;
using UnityEngine.EventSystems; // マウスイベントを取得するために必要
using System.Collections;       // コルーチンを使用するために必要

public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Tooltip("カーソルが乗った時の拡大率")]
    public float hoverScale = 1.1f;

    [Tooltip("アニメーションの速度（秒）")]
    public float animationDuration = 0.1f;

    private Vector3 originalScale;
    private Coroutine runningCoroutine;

    void Awake()
    {
        // 起動時に元の大きさを記憶しておく
        originalScale = transform.localScale;
    }

    // マウスカーソルがボタンに乗った時に呼び出される
    public void OnPointerEnter(PointerEventData eventData)
    {
        // 実行中のアニメーションがあれば停止
        if (runningCoroutine != null)
        {
            StopCoroutine(runningCoroutine);
        }
        // 拡大アニメーションを開始
        runningCoroutine = StartCoroutine(AnimateScale(new Vector3(hoverScale, hoverScale, hoverScale)));
    }

    // マウスカーソルがボタンから離れた時に呼び出される
    public void OnPointerExit(PointerEventData eventData)
    {
        // 実行中のアニメーションがあれば停止
        if (runningCoroutine != null)
        {
            StopCoroutine(runningCoroutine);
        }
        // 元の大きさに戻すアニメーションを開始
        runningCoroutine = StartCoroutine(AnimateScale(originalScale));
    }

    // 指定したスケールまで滑らかに変化させるコルーチン
    private IEnumerator AnimateScale(Vector3 targetScale)
    {
        float elapsedTime = 0f;
        Vector3 startingScale = transform.localScale;

        while (elapsedTime < animationDuration)
        {
            // 経過時間に応じて線形補間でスケールを計算
            transform.localScale = Vector3.Lerp(startingScale, targetScale, elapsedTime / animationDuration);
            elapsedTime += Time.deltaTime;
            yield return null; // 1フレーム待つ
        }

        // 最終的にきっちり目標のスケールに設定
        transform.localScale = targetScale;
        runningCoroutine = null;
    }
}