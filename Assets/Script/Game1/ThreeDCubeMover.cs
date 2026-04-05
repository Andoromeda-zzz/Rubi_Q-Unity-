using UnityEngine;

public class ThreeDCubeMover : MonoBehaviour
{
    

    public void RMoveRotate(int x,int y,int z)
    {
        transform.eulerAngles = new Vector3(x,y,z);
    }
}
