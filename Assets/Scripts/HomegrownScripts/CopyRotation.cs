using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyRotation : MonoBehaviour
{

    public Transform CopyFrom;
    public Transform PasteTo;

    void Update()
    {
        float Rotation = CopyFrom.eulerAngles.y;

        PasteTo.rotation = Quaternion.Euler(0, Rotation, 0);
    }
}
