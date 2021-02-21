using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyLocation : MonoBehaviour
{

    public Transform CopyFrom;
    public Transform PasteTo;

    public float XOffset = 0f;
    public float YOffset = 0f;
    public float ZOffset = 0f;

    public bool ApplyX = false;
    public bool ApplyY = false;
    public bool ApplyZ = false;

    private float X = 0;
    private float Y = 0;
    private float Z = 0;

    public bool OverwriteHeightForChest = false;

    void Update()
    {
        
        if (ApplyX) {
            X = CopyFrom.position.x-XOffset;
        }
        else {
            X = CopyFrom.position.x;
        }

        if (ApplyY) {
            Y = CopyFrom.position.y-YOffset;
        }
        else {
            Y = CopyFrom.position.y;
        }

        if (ApplyZ) {
            Z = CopyFrom.position.z-ZOffset;
        }
        else {
            Z = CopyFrom.position.z;
        }
        
        if (OverwriteHeightForChest) {
            Y = 1.041f;
        }

        PasteTo.position = new Vector3(X,Y,Z);
    }
}
