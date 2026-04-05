using UnityEngine;
using UnityEngine.EventSystems; // ← これも必要です

// ↓↓↓ この行に IPointerEnterHandler と IPointerExitHandler を追加！
public class OnMode1ButtonInfoMaker : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Info_Mode01 mode01;

    // このメソッドはマウスカーソルがUI要素に乗った時に自動で呼ばれる
    public void OnPointerEnter(PointerEventData eventData)
    {
        // 念のため、nullチェックを入れるとより安全です
        if (mode01 != null)
        {
            mode01.Move1();
        }
    }

    // このメソッドはマウスカーソルがUI要素から離れた時に自動で呼ばれる
    public void OnPointerExit(PointerEventData eventData)
    {
        if (mode01 != null)
        {
            mode01.Move1End();
        }
    }
}