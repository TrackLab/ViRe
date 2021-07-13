using UnityEngine;

public class CopyTransform : MonoBehaviour
{

    /**
        IMPORTANT: This is a patented mess

        This script copies transform information from an object to the one this script is on
        You can also apply a specific offset to a parameter or lock it to a value using the Unity Editor
    **/
    
    public Transform CopyFrom;
    private Transform PasteTo;
    public bool EnableCopyLocation = true;
    public bool EnableCopyRotation = false;

    [Header("Position Copy Parameters")]
    public bool ApplyXOffset = false;
    public bool LockX = false;
    public float X = 0f;
    public bool ApplyYOffset = false;
    public bool LockY = false;
    public float Y = 0f;
    public bool ApplyZOffset = false;
    public bool LockZ = false;
    public float Z = 0f;

    [Header("Rotation Copy Parameters")]
    public bool ApplyXAngleOffset = false;
    public bool LockXAngle = false;
    public float XAngle = 0f;
    public bool ApplyYAngleOffset = false;
    public bool LockYAngle = false;
    public float YAngle = 0f;
    public bool ApplyZAngleOffset = false;
    public bool LockZAngle = false;
    public float ZAngle = 0f;

    private float Xpos, Ypos, Zpos, Xquat, Yquat, Zquat;
    private Quaternion rotate = new Quaternion();
    private Vector3 position;

    void Start(){
        paramCheck();
        PasteTo = GetComponent<Transform>();
    }
    
    void Update()
    {
        if (EnableCopyLocation){
            if (ApplyXOffset) {Xpos = CopyFrom.position.x-X;}
            else if (LockX) {Xpos = X;}
            else {Xpos = CopyFrom.position.x;}

            if (ApplyYOffset) {Ypos = CopyFrom.position.y-Y;}
            else if (LockY) {Ypos = Y;}
            else {Ypos = CopyFrom.position.y;}

            if (ApplyZOffset) {Zpos = CopyFrom.position.z-Z;}
            else if (LockZ) {Zpos = Z;}
            else{Zpos = CopyFrom.position.z;}
        } else {
            Xpos = PasteTo.position.x;
            Ypos = PasteTo.position.y;
            Zpos = PasteTo.position.z;
        }
        
        position = new Vector3(Xpos,Ypos,Zpos);


        if (EnableCopyRotation){
            if (ApplyXAngleOffset){Xquat = CopyFrom.eulerAngles.x-XAngle;}
            else if (LockXAngle){Xquat = XAngle;}
            else {Xquat = CopyFrom.eulerAngles.x;}

            if (ApplyYAngleOffset){Yquat = CopyFrom.eulerAngles.y-YAngle;}
            else if (LockYAngle){Yquat = YAngle;}
            else {Yquat = CopyFrom.eulerAngles.y;}

            if (ApplyZAngleOffset){Zquat = CopyFrom.eulerAngles.z-ZAngle;}
            else if (LockZAngle){Zquat = ZAngle;}
            else {Zquat = CopyFrom.eulerAngles.z;}


            rotate = Quaternion.Euler(Xquat,Yquat,Zquat);
        } 
        else {rotate = new Quaternion(CopyFrom.rotation.x,CopyFrom.rotation.y,CopyFrom.rotation.z,CopyFrom.rotation.w);}

        PasteTo.SetPositionAndRotation(position,rotate);
    }

    //Radio buttons are stupid hard to set up, so this is here to "make checkboxes be radio buttons" by getting in your way
    void paramCheck(){
        if (ApplyXOffset && LockX){
            Debug.LogWarning("Both Lock X and Apply X Offset are set, only one can be used! Lock X is ignored.");
            LockX = false;
        }
        if (ApplyYOffset && LockY){
            Debug.LogWarning("Both Lock Y and Apply Y Offset are set, only one can be used! Lock Y is ignored.");
            LockY = false;
        }
        if (ApplyZOffset && LockZ){
            Debug.LogWarning("Both Lock Z and Apply Z Offset are set, only one can be used! Lock Z is ignored.");
            LockZ = false;
        }
        if (ApplyXAngleOffset && LockXAngle){
            Debug.LogWarning("Both Lock X Angle and Apply X Angle Offset are set, only one can be used! Lock X Angle is ignored.");
            LockXAngle = false;
        }
        if (ApplyYAngleOffset && LockYAngle){
            Debug.LogWarning("Both Lock Y Angle and Apply Y Angle Offset are set, only one can be used! Lock Y Angle is ignored.");
            LockYAngle = false;
        }
        if (ApplyZAngleOffset && LockZAngle){
            Debug.LogWarning("Both Lock Z Angle and Apply Z Angle Offset are set, only one can be used! Lock Z Angle is ignored.");
            LockZAngle = false;
        }
    }
}
