using System.Collections;
using UnityEngine;
using static Mode2_PLmodel;

public class Mode2_PLmodel
{
    public enum CubeColor { White, Yellow, Red, Orange, Green, Blue }
    public enum Face { Up, Down, Front, Back, Left, Right }
    public enum Inner_Face { X, Y, Z }
    public CubeColor[,,] State = new CubeColor[6, 3, 3];

    public Mode2_PLmodel()
    {

        Riset_Collor();


    }

    public void Riset_Collor()
    {
        CubeColor[] faceColors = new CubeColor[]
        {
            CubeColor.White,
            CubeColor.Yellow,
            CubeColor.Red,
            CubeColor.Orange,
            CubeColor.Green,
            CubeColor.Blue,
        };

        for (int face = 0; face < 6; face++)
        {
            CubeColor currentColor = faceColors[face];
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    State[face, row, col] = currentColor;
                }
            }
        }
    }


    public void RotateFace(Face face, bool isClockwise, Inner_Face Inner, bool inner_bool)
    {

        if (isClockwise)
        {
            CubeColor[,] faceCopy = new CubeColor[3, 3];
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    faceCopy[row, col] = State[(int)face, row, col];
                }
            }


            State[(int)face, 2, 0] = faceCopy[0, 0];
            State[(int)face, 1, 0] = faceCopy[0, 1];
            State[(int)face, 0, 0] = faceCopy[0, 2];
            State[(int)face, 0, 1] = faceCopy[1, 2];
            State[(int)face, 0, 2] = faceCopy[2, 2];
            State[(int)face, 1, 2] = faceCopy[2, 1];
            State[(int)face, 2, 2] = faceCopy[2, 0];
            State[(int)face, 2, 1] = faceCopy[1, 0];




            CubeColor[] tempRow = new CubeColor[3];

            switch (face)
            {
                case Face.Up:

                    for (int i = 0; i < 3; i++) tempRow[i] = State[(int)Face.Front, 0, i];


                    for (int i = 0; i < 3; i++) State[(int)Face.Front, 0, i] = State[(int)Face.Left, 0, 2 - i];


                    for (int i = 0; i < 3; i++) State[(int)Face.Left, 0, i] = State[(int)Face.Back, 0, 2 - i];


                    for (int i = 0; i < 3; i++) State[(int)Face.Back, 0, i] = State[(int)Face.Right, 0, 2 - i];


                    for (int i = 0; i < 3; i++) State[(int)Face.Right, 0, 2 - i] = tempRow[i];

                    break; // Face.Up の処理終わり

                case Face.Front:

                    for (int i = 0; i < 3; i++) tempRow[i] = State[(int)Face.Down, 0, 2 - i];


                    for (int i = 0; i < 3; i++) State[(int)Face.Down, 0, i] = State[(int)Face.Left, i, 0];


                    for (int i = 0; i < 3; i++) State[(int)Face.Left, i, 0] = State[(int)Face.Up, 2, 2 - i];


                    for (int i = 0; i < 3; i++) State[(int)Face.Up, 2, i] = State[(int)Face.Right, i, 2];


                    for (int i = 0; i < 3; i++) State[(int)Face.Right, i, 2] = tempRow[i];

                    break; // Face.Front の処理終わり

                case Face.Right:

                    for (int i = 0; i < 3; i++) tempRow[i] = State[(int)Face.Front, i, 2];


                    for (int i = 0; i < 3; i++) State[(int)Face.Front, i, 2] = State[(int)Face.Down, i, 2];


                    for (int i = 0; i < 3; i++) State[(int)Face.Down, i, 2] = State[(int)Face.Back, 2 - i, 0];


                    for (int i = 0; i < 3; i++) State[(int)Face.Back, i, 0] = State[(int)Face.Up, 2 - i, 2];


                    for (int i = 0; i < 3; i++) State[(int)Face.Up, i, 2] = tempRow[i];

                    break; // Face.Right の処理終わり

                case Face.Back:


                    for (int i = 0; i < 3; i++) tempRow[i] = State[(int)Face.Down, 2, 2 - i];


                    for (int i = 0; i < 3; i++) State[(int)Face.Down, 2, 2 - i] = State[(int)Face.Right, i, 0];


                    for (int i = 0; i < 3; i++) State[(int)Face.Right, i, 0] = State[(int)Face.Up, 0, i];


                    for (int i = 0; i < 3; i++) State[(int)Face.Up, 0, 2 - i] = State[(int)Face.Left, i, 2];


                    for (int i = 0; i < 3; i++) State[(int)Face.Left, 2 - i, 2] = tempRow[i];







                    break; // Face.Back の処理終わり

                case Face.Left:

                    for (int i = 0; i < 3; i++) tempRow[i] = State[(int)Face.Back, i, 2];


                    for (int i = 0; i < 3; i++) State[(int)Face.Back, i, 2] = State[(int)Face.Down, 2 - i, 0];


                    for (int i = 0; i < 3; i++) State[(int)Face.Down, i, 0] = State[(int)Face.Front, i, 0];


                    for (int i = 0; i < 3; i++) State[(int)Face.Front, i, 0] = State[(int)Face.Up, i, 0];


                    for (int i = 0; i < 3; i++) State[(int)Face.Up, 2 - i, 0] = tempRow[i];

                    break; // Face.Left の処理終わり

                case Face.Down:

                    for (int i = 0; i < 3; i++) tempRow[i] = State[(int)Face.Left, 2, i];


                    for (int i = 0; i < 3; i++) State[(int)Face.Left, 2, i] = State[(int)Face.Front, 2, 2 - i];


                    for (int i = 0; i < 3; i++) State[(int)Face.Front, 2, i] = State[(int)Face.Right, 2, 2 - i];


                    for (int i = 0; i < 3; i++) State[(int)Face.Right, 2, i] = State[(int)Face.Back, 2, 2 - i];


                    for (int i = 0; i < 3; i++) State[(int)Face.Back, 2, 2 - i] = tempRow[i];

                    break; // Face.Down の処理終わり




            }
        }

        if (inner_bool)
        {
            CubeColor[] tempRow = new CubeColor[3];
            switch (Inner)
            {
                case Inner_Face.X:
                    for (int i = 0; i < 3; i++) tempRow[i] = State[(int)Face.Front, i, 1];

                    for (int i = 0; i < 3; i++) State[(int)Face.Front, i, 1] = State[(int)Face.Down, i, 1];


                    for (int i = 0; i < 3; i++) State[(int)Face.Down, i, 1] = State[(int)Face.Back, 2 - i, 1];


                    for (int i = 0; i < 3; i++) State[(int)Face.Back, i, 1] = State[(int)Face.Up, 2 - i, 1];


                    for (int i = 0; i < 3; i++) State[(int)Face.Up, i, 1] = tempRow[i];

                    break; // Face真ん中 の処理終わり

                case Inner_Face.Y:
                    for (int i = 0; i < 3; i++) tempRow[i] = State[(int)Face.Front, 1, i];

                    for (int i = 0; i < 3; i++) State[(int)Face.Front, 1, i] = State[(int)Face.Right, 1, 2 - i];


                    for (int i = 0; i < 3; i++) State[(int)Face.Right, 1, i] = State[(int)Face.Back, 1, 2 - i];


                    for (int i = 0; i < 3; i++) State[(int)Face.Back, 1, i] = State[(int)Face.Left, 1, 2 - i];


                    for (int i = 0; i < 3; i++) State[(int)Face.Left, 1, 2 - i] = tempRow[i];

                    break; // Face真ん中Y の処理終わり

                case Inner_Face.Z:
                    for (int i = 0; i < 3; i++) tempRow[i] = State[(int)Face.Left, i, 1];

                    for (int i = 0; i < 3; i++) State[(int)Face.Left, i, 1] = State[(int)Face.Down, 1, i];


                    for (int i = 0; i < 3; i++) State[(int)Face.Down, 1, i] = State[(int)Face.Right, 2 - i, 1];


                    for (int i = 0; i < 3; i++) State[(int)Face.Right, i, 1] = State[(int)Face.Up, 1, i];


                    for (int i = 0; i < 3; i++) State[(int)Face.Up, 1, 2 - i] = tempRow[i];

                    break; // Face真ん中Z の処理終わり



            }

        }
    }
}