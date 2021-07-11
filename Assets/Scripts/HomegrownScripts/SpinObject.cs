using UnityEngine;

public class SpinObject : MonoBehaviour
{
    private Transform trans;
    public float XRotate = 0;
    public float YRotate = 0;
    public float ZRotate = 0;

    void Start(){
        trans = GetComponent<Transform>();    
    }

    // Update is called once per frame
    void Update()
    {
        trans.Rotate(XRotate,YRotate,ZRotate);
    }
}
