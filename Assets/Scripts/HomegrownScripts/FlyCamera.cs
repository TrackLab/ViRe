using UnityEngine;
using UnityEngine.InputSystem;
 
public class FlyCamera : MonoBehaviour {
 
    private float mainSpeed = 5.0f; //regular speed
    private float camSens = 0.25f; //How sensitive it with mouse
    private Vector2 p;
    private Vector3 lastMouse;
    private bool activateCam = false;

    public GameObject DroneObject;

    public void onMove(InputAction.CallbackContext value){
        p = -value.ReadValue<Vector2>();
        p = p * mainSpeed * Time.deltaTime;
    }

    // TODO: Camera flips when the X angle is either max or min. Needs to be clipped
    public void onMouse(InputAction.CallbackContext value){
        lastMouse = value.ReadValue<Vector2>();
        lastMouse = new Vector2(lastMouse.y * camSens, lastMouse.x * camSens);
        lastMouse = new Vector2(transform.eulerAngles.x + lastMouse.x, transform.eulerAngles.y + lastMouse.y);
    }

    public void onCameraActivate(InputAction.CallbackContext value){
        activateCam = !value.canceled;
    }

    void LateUpdate(){
        DroneObject.transform.Translate(new Vector3(p.x,0,p.y));
        if (activateCam){transform.eulerAngles = lastMouse;}
    }
}