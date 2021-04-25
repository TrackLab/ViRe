using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

public class ControlManager : MonoBehaviour
{   
    //This script holds references to the Input Maps we use and can also toggle them (for something like pausing)

    public InputActionAsset xrControls, keyboard;
    private InputActionManager inputManager;

    void Awake(){inputManager = GetComponent<InputActionManager>();}

    public InputActionAsset getXRcontrols(){return xrControls;}

    public InputActionAsset getKeyboard(){return keyboard;}

    public void disableControls(){inputManager.DisableInput();}

    public void enableControls(){inputManager.EnableInput();}
}
