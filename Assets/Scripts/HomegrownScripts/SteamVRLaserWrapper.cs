using UnityEngine;
using UnityEngine.EventSystems;
using Valve.VR.Extras;

public class SteamVRLaserWrapper : MonoBehaviour
{
    /**
       This script lets the SteamVR laser interact with Unity Buttons using Box Colliders
       Offered up by a gracious soul on the Unity Forums
       TODO: Find original author
    **/

    private SteamVR_LaserPointer steamVrLaserPointer;

    private void Awake()
    {
        steamVrLaserPointer = gameObject.GetComponent<SteamVR_LaserPointer>();
        steamVrLaserPointer.PointerIn += OnPointerIn;
        steamVrLaserPointer.PointerOut += OnPointerOut;
        steamVrLaserPointer.PointerClick += OnPointerClick;
    }

    private void OnPointerClick(object sender, PointerEventArgs e)
    {
        if (e.target.TryGetComponent<IPointerClickHandler>(out var clickHandler))
        {
            if (clickHandler == null)
            {
                return;
            }
            clickHandler.OnPointerClick(new PointerEventData(EventSystem.current));
        }
    }

    private void OnPointerOut(object sender, PointerEventArgs e)
    {
        if (e.target.TryGetComponent<IPointerExitHandler>(out var pointerExitHandler))
        {


            if (pointerExitHandler == null)
            {
                return;
            }

            pointerExitHandler.OnPointerExit(new PointerEventData(EventSystem.current));
        }
    }

    private void OnPointerIn(object sender, PointerEventArgs e)
    {
        if (e.target.TryGetComponent<IPointerEnterHandler>(out var pointerEnterHandler))
        {
            if (pointerEnterHandler == null)
            {
                return;
            }

            pointerEnterHandler.OnPointerEnter(new PointerEventData(EventSystem.current));
        }
    }
}