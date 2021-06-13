using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateFloor : MonoBehaviour
{
    public int rowNum = 50;
    public int columnNum = 50;
    public float widthOffset = 5;
    public float lengthOffset = 5;
    public Vector3 startPos;
    
    public GameObject floorPrefab;
    // Start is called before the first frame update
    void Awake()
    {
        if (floorPrefab)
        {
            for (int i = 0; i < rowNum; i++)
            {
                for (int j = 0; j < columnNum; j++)
                {
                    Instantiate(floorPrefab, new Vector3(i * widthOffset + startPos.x, startPos.y, j * lengthOffset + startPos.z), Quaternion.Euler(-90, 0, 0));
                }
            }
        }
    }

}
