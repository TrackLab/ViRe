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
    public bool ApplyXOffset = false;
    public bool ApplyYOffset = false;
    public bool ApplyZOffset = false;

    private float Xpos = 0;
    private float Ypos = 0;
    private float Zpos = 0;
    private Quaternion rotate;
    private Vector3 position;

    public bool CopyRotation = false;
    //public bool OverwriteHeightForChest = false;

    void Update()
    {
        if (ApplyXOffset) {Xpos = CopyFrom.position.x-XOffset;}
        else {Xpos = CopyFrom.position.x;}

        if (ApplyYOffset) {Ypos = CopyFrom.position.y-YOffset;}
        else {Ypos = CopyFrom.position.y;}

        if (ApplyZOffset) {Zpos = CopyFrom.position.z-ZOffset;}
        else {Zpos = CopyFrom.position.z;}
        
        position = new Vector3(Xpos,Ypos,Zpos);

        if (CopyRotation){rotate = new Quaternion(CopyFrom.rotation.x,CopyFrom.rotation.y,CopyFrom.rotation.z,CopyFrom.rotation.w);} 
        else {rotate = new Quaternion(PasteTo.rotation.x,PasteTo.rotation.y,PasteTo.rotation.z,PasteTo.rotation.w);}
        
        PasteTo.SetPositionAndRotation(position,rotate);
    }
}
