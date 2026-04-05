using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;


public class Mode2PLGenerator : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Mode2_GameManager gameManager;
    private Mode2_PLmodel cubeModel;

    public Image[] upFaceImages = new Image[9];
    public Image[] downFaceImages = new Image[9];
    public Image[] frontFaceImages = new Image[9];
    public Image[] backFaceImages = new Image[9];
    public Image[] leftFaceImages = new Image[9];
    public Image[] rightFaceImages = new Image[9];

    // GameManagerから受け取るための変数
    private Dictionary<Mode2_PLmodel.CubeColor, Color> colorMap;

    void Start()
    {
        cubeModel = gameManager.pl_Model;

    }

    // GameManagerからカラーマップを受け取るための関数
    public void Initialize(Dictionary<Mode2_PLmodel.CubeColor, Color> map)
    {
        colorMap = map;
    }

    public void UpdateView(Mode2_PLmodel model)
    {
        // 上面の9マスを更新する例
        UpdateFaceView(upFaceImages, model.State, (int)Mode2_PLmodel.Face.Up, false);

        // 他の5面も同様に呼び出す
        UpdateFaceView(downFaceImages, model.State, (int)Mode2_PLmodel.Face.Down, false);
        UpdateFaceView(frontFaceImages, model.State, (int)Mode2_PLmodel.Face.Front, false);
        UpdateFaceView(backFaceImages, model.State, (int)Mode2_PLmodel.Face.Back, false);
        UpdateFaceView(leftFaceImages, model.State, (int)Mode2_PLmodel.Face.Left, true);
        UpdateFaceView(rightFaceImages, model.State, (int)Mode2_PLmodel.Face.Right, true);


        // ...
    }

    // 1面（9マス）分の色を更新するヘルパー関数
    void UpdateFaceView(Image[] faceImages, Mode2_PLmodel.CubeColor[,,] state, int faceIndex, bool reverce)
    {

        if (!reverce)
        {
            Mode2_PLmodel.CubeColor colorEnum1 = state[faceIndex, 0, 0];
            Mode2_PLmodel.CubeColor colorEnum2 = state[faceIndex, 0, 1];
            Mode2_PLmodel.CubeColor colorEnum3 = state[faceIndex, 0, 2];
            Mode2_PLmodel.CubeColor colorEnum4 = state[faceIndex, 1, 0];
            Mode2_PLmodel.CubeColor colorEnum5 = state[faceIndex, 1, 1];
            Mode2_PLmodel.CubeColor colorEnum6 = state[faceIndex, 1, 2];
            Mode2_PLmodel.CubeColor colorEnum7 = state[faceIndex, 2, 0];
            Mode2_PLmodel.CubeColor colorEnum8 = state[faceIndex, 2, 1];
            Mode2_PLmodel.CubeColor colorEnum9 = state[faceIndex, 2, 2];

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
        else
        {

            Mode2_PLmodel.CubeColor colorEnum1 = state[faceIndex, 0, 2];
            Mode2_PLmodel.CubeColor colorEnum2 = state[faceIndex, 0, 1];
            Mode2_PLmodel.CubeColor colorEnum3 = state[faceIndex, 0, 0];
            Mode2_PLmodel.CubeColor colorEnum4 = state[faceIndex, 1, 2];
            Mode2_PLmodel.CubeColor colorEnum5 = state[faceIndex, 1, 1];
            Mode2_PLmodel.CubeColor colorEnum6 = state[faceIndex, 1, 0];
            Mode2_PLmodel.CubeColor colorEnum7 = state[faceIndex, 2, 2];
            Mode2_PLmodel.CubeColor colorEnum8 = state[faceIndex, 2, 1];
            Mode2_PLmodel.CubeColor colorEnum9 = state[faceIndex, 2, 0];

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
        

    }

    void printFaceColor(PL_Model2.Face targetFace)
    {
        Mode2_PLmodel.CubeColor targetColor = cubeModel.State[(int)targetFace, 0, 0];
        UnityEngine.Debug.Log("上面の(0,0)の色は: " + targetColor.ToString());
    }
}

