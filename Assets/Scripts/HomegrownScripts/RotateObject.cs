using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public new Transform transform;
    public float XRotate = 0;
    public float YRotate = 0;
    public float ZRotate = 0;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(XRotate,YRotate,ZRotate);
    }
}
