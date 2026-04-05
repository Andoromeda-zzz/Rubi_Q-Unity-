using UnityEngine;

public class Mode2CubeController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float rotationSpeed = 0.5f;

    // 前のフレームのマウス位置を記憶するための変数
    private Vector3 lastMousePosition;

    void Update()
    {
        // マウスの左ボタンが"押された瞬間"を検知
        if (Input.GetMouseButtonDown(0))
        {
            // ドラッグ開始時のマウス位置を記録
            lastMousePosition = Input.mousePosition;
        }
        // マウスの左ボタンが"押されている間"を検知
        else if (Input.GetMouseButton(0))
        {
            // 現在のマウス位置と、前のフレームのマウス位置との差分を計算
            Vector3 delta = Input.mousePosition - lastMousePosition;

            // 差分を使って回転量を計算
            float rotY = delta.x * rotationSpeed * -1;
            float rotX = -delta.y * rotationSpeed * -1;

            // ワールド座標を基準に、オブジェクトを回転させる
            transform.Rotate(rotX, rotY, 0, Space.World);


            // 現在のマウス位置を「前の位置」として更新し、次のフレームに備える
            lastMousePosition = Input.mousePosition;
        }
    }
}
