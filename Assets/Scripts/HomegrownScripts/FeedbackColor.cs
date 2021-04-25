using UnityEngine;

public class FeedbackColor : MonoBehaviour
{
    public Material onMaterial;
    public Material offMaterial;
    private Renderer render;

    void Awake(){render = GetComponent<Renderer>();}

    public void OnAir(){render.material = onMaterial;}
    public void OffAir(){render.material = offMaterial;}
}
