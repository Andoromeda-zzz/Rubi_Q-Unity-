using System.Diagnostics;
using UnityEngine;


public class PL1_Mover : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void MoveTo(int i)
    {
        // 1. 現在のローカルZ座標を取得
        float currentZ = transform.localPosition.z;
        Vector3 newPosition;

        switch (i)
        {
            case 0:
                newPosition = new Vector3(-481, 190, currentZ);
                break;
            case 1:
                newPosition = new Vector3(258, 192, currentZ);
                break;
            case 2:
                newPosition = new Vector3(-476, -259, currentZ);
                break;
            case 3:
                newPosition = new Vector3(257, -253, currentZ);
                break;
            default:
                UnityEngine.Debug.LogWarning("MoveToに想定外のindexが来ました: " + i);
                newPosition = transform.localPosition; // 現在のローカル座標
                break;
        }

        // 3. transform.localPosition に代入する (transform.position ではない)
        transform.localPosition = newPosition;
    }

}
