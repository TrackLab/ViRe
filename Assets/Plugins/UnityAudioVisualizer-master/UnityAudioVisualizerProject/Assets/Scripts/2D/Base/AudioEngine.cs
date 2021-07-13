using UnityEngine;

public class AudioEngine : MonoBehaviour
{
    public static float SpectrumValue { get; private set; }
    private float[] m_audioSpectrum;
    // Start is called before the first frame update
    void Start() => m_audioSpectrum = new float[128]; //must me power of 2

    // Update is called once per frame
    void Update()
    {
        AudioListener.GetSpectrumData(m_audioSpectrum, 0, FFTWindow.Hamming);
        if (m_audioSpectrum != null && m_audioSpectrum.Length > 0)
        {
            SpectrumValue = m_audioSpectrum[0] * 100; //maybe needs to be normalized
        }
    }
    
}
