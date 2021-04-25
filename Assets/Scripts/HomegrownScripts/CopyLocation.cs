using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyLocation : MonoBehaviour
{

    public Transform CopyFrom;
    public Transform PasteTo;
    public bool CopyRotation = false;

    [Header("Positional Offsets")]
    public bool ApplyXOffset = false;
    public float XOffset = 0f;
    public bool ApplyYOffset = false;
    public float YOffset = 0f;
    public bool ApplyZOffset = false;
    public float ZOffset = 0f;

    [Header("Rotational Offsets")]
    public bool ApplyAngles = false;
    public float XAngle = 0f;
    public float YAngle = 0f;
    public float ZAngle = 0f;

    private float Xpos, Ypos, Zpos;
    private Quaternion rotate;
    private Vector3 position;

    
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
        
        if(ApplyAngles){rotate*=Quaternion.Euler(XAngle,YAngle,ZAngle);}

        PasteTo.SetPositionAndRotation(position,rotate);
    }
}
