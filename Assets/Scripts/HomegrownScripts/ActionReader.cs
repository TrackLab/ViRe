using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ActionReader
{
    //This is a helper class that gets and returns InputAction(s) from either the XRcontroller or Keyboard Action Maps
    
    private static readonly InputActionAsset xrControls;
    private static readonly InputActionAsset keyboard; 
    private static readonly ControlManager controlRef;
    
    static ActionReader(){
        controlRef = GameObject.Find("ControlReferencer").GetComponent<ControlManager>();
        xrControls = controlRef.getXRcontrols();
        keyboard = controlRef.getKeyboard();
    }
    
    public InputAction getInputAction(string mapName, string actionName){
        InputActionMap map = xrControls.FindActionMap(mapName,false);
        if (map!=null){
            try{return map.FindAction(actionName, false);}
            catch (ArgumentException){}
        }
        map = keyboard.FindActionMap(mapName,false);
        if (map!=null){
            try{return map.FindAction(actionName, false);}
            catch (ArgumentException){throw new ArgumentException("Input Action name invalid");}
        }
        throw new ArgumentException("Input Map name invalid");
    }

    public ControlManager getControlManager(){return controlRef;}
}
