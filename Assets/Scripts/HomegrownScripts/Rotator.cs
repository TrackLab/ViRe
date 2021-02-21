using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{   
    public Transform ObjectTransform;

    public int X = 0;
    public int Y = 0;
    public int Z = 0;

    void Update()
    {
        ObjectTransform.Rotate(X, Y, Z);
    }
}
