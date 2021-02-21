using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackColor : MonoBehaviour
{
    public Material onMaterial;
    public Material offMaterial;

    public void OnAir(){GetComponent<Renderer>().material = onMaterial;}
    public void OffAir(){GetComponent<Renderer>().material = offMaterial;}
}
