using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RubiksCubeGenerator : MonoBehaviour
{
    public GameObject cubePrefab; // 小キューブのプレハブ
    public float cubeSpacing = 1.05f; // キューブ間の隙間
    public Button Rotation;

    private GameObject[,,] cubes = new GameObject[3, 3, 3];

    private RubiksCubeModel cubeModel;
    public GameManager gameManager;

    // ★追加：CubeColor enumとUnityのColorを対応させる辞書
    private Dictionary<RubiksCubeModel.CubeColor, Color> colorMap;

    void Start()
    {
        
        cubeModel = gameManager.CubeModel;
        InitializeColorMap();

        GenerateCube();
        // ★追加：生成後に色を適用する
        ApplyColorsToCube();
    }

    void Update()
    {
        // --- 更新フェーズ（テスト用） ---
        
    }

    void InitializeColorMap()
    {
        colorMap = new Dictionary<RubiksCubeModel.CubeColor, Color>()
        {
            { RubiksCubeModel.CubeColor.White, Color.white },
            { RubiksCubeModel.CubeColor.Yellow, Color.yellow },
            { RubiksCubeModel.CubeColor.Red, Color.red },
            { RubiksCubeModel.CubeColor.Orange, new Color(1, 0.5f, 0) }, // Orange
            { RubiksCubeModel.CubeColor.Green, Color.green },
            { RubiksCubeModel.CubeColor.Blue, Color.blue }
        };
    }

    public void GenerateCube()
    {
        // 中心を原点にするため、-1～1までループ
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                for (int z = -1; z <= 1; z++)
                {
                    Vector3 position = new Vector3(
                        x * cubeSpacing,
                        y * cubeSpacing,
                        z * cubeSpacing
                    );
                    GameObject smallCube = Instantiate(cubePrefab, position, Quaternion.identity, transform);
                    cubes[x + 1, y + 1, z + 1] = smallCube;
                }
            }
        }
    }

    public void DestroyCube()
    {
        // このオブジェクト（親オブジェクト）の子になっている全ての小キューブをループ
        foreach (Transform child in transform)
        {
            // 子オブジェクトをシーンから削除
            Destroy(child.gameObject);
        }

        // 配列の参照もクリアしておく
        cubes = new GameObject[3, 3, 3];
    }


    public void ApplyColorsToCube()
    {
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                for (int z = 0; z < 3; z++)
                {
                    GameObject smallCube = cubes[x, y, z];
                    if (smallCube == null) continue;

                    // 各面の色を設定するためにヘルパー関数を呼び出す
                    // 面の表示/非表示は、プレハブの時点で対応するPlaneの有効/無効で設定しておく

                    // 上面/下面
                    SetStickerColor(smallCube, RubiksCubeModel.Face.Up, cubeModel.State[0, 2 - z, x]);
                    SetStickerColor(smallCube, RubiksCubeModel.Face.Down, cubeModel.State[1, z, x]);
                    // 前面/後面
                    SetStickerColor(smallCube, RubiksCubeModel.Face.Front, cubeModel.State[2, 2 - y, x]);
                    SetStickerColor(smallCube, RubiksCubeModel.Face.Back, cubeModel.State[3, 2 - y, 2 - x]);
                    // 左面/右面
                    SetStickerColor(smallCube, RubiksCubeModel.Face.Left, cubeModel.State[4, 2 - y, z]);
                    SetStickerColor(smallCube, RubiksCubeModel.Face.Right, cubeModel.State[5, 2 - y, 2 - z]);
                }
            }
        }
    }

    void SetStickerColor(GameObject targetCube, RubiksCubeModel.Face face, RubiksCubeModel.CubeColor color)
    {
        // 1. "Up" + "Face" -> "UpFace" のように、探す子オブジェクトの名前を作る
        string faceName = face.ToString();

        // 2. targetCubeの子オブジェクトの中から、その名前のものを探す
        Transform faceTransform = targetCube.transform.Find(faceName);

        // 3. もし見つかったら、その色を変更する
        if (faceTransform != null)
        {
            Renderer faceRenderer = faceTransform.GetComponent<Renderer>();
            faceRenderer.material.color = colorMap[color];
        }
    }
}
