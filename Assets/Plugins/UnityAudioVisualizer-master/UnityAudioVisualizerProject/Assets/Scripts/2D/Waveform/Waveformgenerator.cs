using UnityEngine;

public class Waveformgenerator : MonoBehaviour
{
    public Color c2 = Color.red;
    public AudioSource audioSource;

    void Start()
    {
        var lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.widthMultiplier = 0.2f;
        float[] spectrum = new float[256];
        lineRenderer.positionCount = spectrum.Length;
 
        lineRenderer.startColor = c2;
    }

    void Update()
    {
        var lineRenderer = GetComponent<LineRenderer>();
        float[] spectrum = new float[256];
        audioSource.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);
        for (int i = 1; i < spectrum.Length - 1; i++)
        {
            lineRenderer.SetPosition(i, new Vector3(i - 1, spectrum[i] * 100, 0.0f));
        }
    }
}