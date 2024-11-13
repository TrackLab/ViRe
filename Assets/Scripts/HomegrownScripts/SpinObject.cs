using UnityEngine;

public class SpinObject : MonoBehaviour
{
    public float XRotate = 0;
    public float YRotate = 0;
    public float ZRotate = 0;

    void LateUpdate()
    {
        transform.Rotate(XRotate, YRotate, ZRotate);
    }
}
