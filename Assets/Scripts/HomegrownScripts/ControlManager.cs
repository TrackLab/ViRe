using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

public class ControlManager : MonoBehaviour
{   
    //This script holds references to the Input Maps we use. There might be a better way to do this

    public InputActionAsset xrControls, keyboard;
    private InputActionManager inputManager;

    void Awake(){inputManager = GetComponent<InputActionManager>();}

    public InputActionAsset getXRcontrols(){return xrControls;}

    public InputActionAsset getKeyboard(){return keyboard;}
}
