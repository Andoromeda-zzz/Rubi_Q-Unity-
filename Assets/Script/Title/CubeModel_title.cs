using System.Collections;
using UnityEngine;
using static CubeModel_title;

public class CubeModel_title
{
    public enum CubeColor { White, Yellow, Red, Orange, Green, Blue }
    public enum Face { Up, Down, Front, Back, Left, Right }
    public enum Inner_Face { X, Y, Z }
    public CubeColor[,,] State = new CubeColor[6, 3, 3];

    public CubeModel_title()
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

}
