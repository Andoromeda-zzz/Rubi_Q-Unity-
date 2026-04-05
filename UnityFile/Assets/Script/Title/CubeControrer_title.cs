using UnityEngine;

public class CubeControrer_title : MonoBehaviour
{
    public float rotX = 1;
    public float rotY = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        
        transform.Rotate(rotX, rotY, 0, Space.World);
    }
}
