using UnityEngine;
using UnityEngine.EventSystems;

public class OnMode2SelectButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Info_Mode2 mode2;
    public Mode2SelectMover Button2Mover;
    public Mode1ButtonMover Button1Mover;

    // このメソッドはマウスカーソルがUI要素に乗った時に自動で呼ばれる
    public void OnPointerEnter(PointerEventData eventData)
    {
        // 念のため、nullチェックを入れるとより安全です
        if (mode2 != null)
        {
            mode2.Move1();
       
        }
        if (Button1Mover != null)
        {
            Button1Mover.Move1();

        }
        if (Button2Mover != null)
        {
            Button2Mover.Move1();

        }
    }

    // このメソッドはマウスカーソルがUI要素から離れた時に自動で呼ばれる
    public void OnPointerExit(PointerEventData eventData)
    {
        if (mode2 != null)
        {
            mode2.Move1End();
        }
        if (Button1Mover != null)
        {
            Button1Mover.Move1End();
        }
        if (Button2Mover != null)
        {
            Button2Mover.Move1End();
        }

    }
}
