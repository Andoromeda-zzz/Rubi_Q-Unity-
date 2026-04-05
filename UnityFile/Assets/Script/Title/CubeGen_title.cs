using System.Collections.Generic;
using UnityEngine;

public class CubeGen_title : MonoBehaviour
{

    public GameObject cubePrefab; // 小キューブのプレハブ
    public float cubeSpacing = 1.05f; // キューブ間の隙間

    private GameObject[,,] cubes = new GameObject[3, 3, 3];

    private CubeModel_title cubeModel;
    

    private Dictionary<CubeModel_title.CubeColor, Color> colorMap;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        cubeModel = new CubeModel_title();
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
        colorMap = new Dictionary<CubeModel_title.CubeColor, Color>()
        {
            { CubeModel_title.CubeColor.White, Color.white },
            { CubeModel_title.CubeColor.Yellow, Color.yellow },
            { CubeModel_title.CubeColor.Red, Color.red },
            { CubeModel_title.CubeColor.Orange, new Color(1, 0.5f, 0) }, // Orange
            { CubeModel_title.CubeColor.Green, Color.green },
            { CubeModel_title.CubeColor.Blue, Color.blue }
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
                    // ★変更点：Instantiateの引数を変更し、localPositionで座標を設定

                    // 1. まず親オブジェクトの子として生成する
                    GameObject smallCube = Instantiate(cubePrefab, transform);

                    // 2. 親から見た相対的な位置（ローカル座標）を設定する
                    smallCube.transform.localPosition = new Vector3(
                        x * cubeSpacing,
                        y * cubeSpacing,
                        z * cubeSpacing
                    );

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
                    SetStickerColor(smallCube, CubeModel_title.Face.Up, cubeModel.State[0, 2 - z, x]);
                    SetStickerColor(smallCube, CubeModel_title.Face.Down, cubeModel.State[1, z, x]);
                    // 前面/後面
                    SetStickerColor(smallCube, CubeModel_title.Face.Front, cubeModel.State[2, 2 - y, x]);
                    SetStickerColor(smallCube, CubeModel_title.Face.Back, cubeModel.State[3, 2 - y, 2 - x]);
                    // 左面/右面
                    SetStickerColor(smallCube, CubeModel_title.Face.Left, cubeModel.State[4, 2 - y, z]);
                    SetStickerColor(smallCube, CubeModel_title.Face.Right, cubeModel.State[5, 2 - y, 2 - z]);
                }
            }
        }
    }

    void SetStickerColor(GameObject targetCube, CubeModel_title.Face face, CubeModel_title.CubeColor color)
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
