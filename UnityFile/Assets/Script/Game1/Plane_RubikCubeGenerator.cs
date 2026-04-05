using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;


public class Plane_RubikCubeGenerator : MonoBehaviour
{

    public GameManager gameManager;
    private RubiksCubeModel cubeModel;

    public Image[] upFaceImages = new Image[9];
    public Image[] downFaceImages = new Image[9];
    public Image[] frontFaceImages = new Image[9];
    public Image[] backFaceImages = new Image[9];
    public Image[] leftFaceImages = new Image[9];
    public Image[] rightFaceImages = new Image[9];

    // GameManagerから受け取るための変数
    private Dictionary<RubiksCubeModel.CubeColor, Color> colorMap;

    void Start()
    {
        cubeModel = gameManager.CubeModel;

    }

    // GameManagerからカラーマップを受け取るための関数
    public void Initialize(Dictionary<RubiksCubeModel.CubeColor, Color> map)
    {
        colorMap = map;
    }

    public void UpdateView(RubiksCubeModel model)
    {
        // 上面の9マスを更新する例
        UpdateFaceView(upFaceImages, model.State, (int)RubiksCubeModel.Face.Up,false);

        // 他の5面も同様に呼び出す
        UpdateFaceView(downFaceImages, model.State, (int)RubiksCubeModel.Face.Down,false);
        UpdateFaceView(frontFaceImages, model.State, (int)RubiksCubeModel.Face.Front,false);
        UpdateFaceView(backFaceImages, model.State, (int)RubiksCubeModel.Face.Back,false);
        UpdateFaceView(leftFaceImages, model.State, (int)RubiksCubeModel.Face.Left,true);
        UpdateFaceView(rightFaceImages, model.State, (int)RubiksCubeModel.Face.Right,true);


        // ...
    }

    // 1面（9マス）分の色を更新するヘルパー関数
    void UpdateFaceView(Image[] faceImages, RubiksCubeModel.CubeColor[,,] state, int faceIndex,bool reverce)
    {

        if (!reverce)
        {
            RubiksCubeModel.CubeColor colorEnum1 = state[faceIndex, 0, 0];
            RubiksCubeModel.CubeColor colorEnum2 = state[faceIndex, 0, 1];
            RubiksCubeModel.CubeColor colorEnum3 = state[faceIndex, 0, 2];
            RubiksCubeModel.CubeColor colorEnum4 = state[faceIndex, 1, 0];
            RubiksCubeModel.CubeColor colorEnum5 = state[faceIndex, 1, 1];
            RubiksCubeModel.CubeColor colorEnum6 = state[faceIndex, 1, 2];
            RubiksCubeModel.CubeColor colorEnum7 = state[faceIndex, 2, 0];
            RubiksCubeModel.CubeColor colorEnum8 = state[faceIndex, 2, 1];
            RubiksCubeModel.CubeColor colorEnum9 = state[faceIndex, 2, 2];

            faceImages[0].color = colorMap[colorEnum1];
            faceImages[1].color = colorMap[colorEnum2];
            faceImages[2].color = colorMap[colorEnum3];
            faceImages[3].color = colorMap[colorEnum4];
            faceImages[4].color = colorMap[colorEnum5];
            faceImages[5].color = colorMap[colorEnum6];
            faceImages[6].color = colorMap[colorEnum7];
            faceImages[7].color = colorMap[colorEnum8];
            faceImages[8].color = colorMap[colorEnum9];
        }
        else{

            RubiksCubeModel.CubeColor colorEnum1 = state[faceIndex, 0, 2];
            RubiksCubeModel.CubeColor colorEnum2 = state[faceIndex, 0, 1];
            RubiksCubeModel.CubeColor colorEnum3 = state[faceIndex, 0, 0];
            RubiksCubeModel.CubeColor colorEnum4 = state[faceIndex, 1, 2];
            RubiksCubeModel.CubeColor colorEnum5 = state[faceIndex, 1, 1];
            RubiksCubeModel.CubeColor colorEnum6 = state[faceIndex, 1, 0];
            RubiksCubeModel.CubeColor colorEnum7 = state[faceIndex, 2, 2];
            RubiksCubeModel.CubeColor colorEnum8 = state[faceIndex, 2, 1];
            RubiksCubeModel.CubeColor colorEnum9 = state[faceIndex, 2, 0];

            faceImages[0].color = colorMap[colorEnum1];
            faceImages[1].color = colorMap[colorEnum2];
            faceImages[2].color = colorMap[colorEnum3];
            faceImages[3].color = colorMap[colorEnum4];
            faceImages[4].color = colorMap[colorEnum5];
            faceImages[5].color = colorMap[colorEnum6];
            faceImages[6].color = colorMap[colorEnum7];
            faceImages[7].color = colorMap[colorEnum8];
            faceImages[8].color = colorMap[colorEnum9];

        }


    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            // フラグをtrueにする
            RubiksCubeModel.Face targetFace = RubiksCubeModel.Face.Up;
            printFaceColor(targetFace);
        }

    }

    void printFaceColor(RubiksCubeModel.Face targetFace)
    {
        RubiksCubeModel.CubeColor targetColor = cubeModel.State[(int)targetFace, 0, 0];
        UnityEngine.Debug.Log("上面の(0,0)の色は: " + targetColor.ToString());
    }
}

// Start is called once before the first execution of Update after the MonoBehaviour is created


    // Update is called once per frame
