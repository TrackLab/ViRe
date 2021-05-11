using UnityEngine;

public class BaseSyncer : MonoBehaviour
{
    public float bias; // Spectrum Value that Triggers Event
    public float timeStep; //Time between 2 Events
    public float timeToBeat; //How fast will it transform
    public float restSmoothTime; //Resttime

    private float m_previousAudioValue;
    private float m_audioValue;
    private float m_timer;

    protected bool m_isBeat;

    public virtual void OnBeat()
    {
        Debug.Log("beat");
        m_timer = 0;
        m_isBeat = true;
    }

    public virtual void OnUpdate()
    {
        // update audio value
        m_previousAudioValue = m_audioValue;
        m_audioValue = AudioEngine.SpectrumValue;

        // if audio value went below the bias during this frame
        if (m_previousAudioValue > bias && m_audioValue <= bias)
        {
            // if minimum beat interval is reached
            if (m_timer > timeStep)
            {
                OnBeat();
            }
        }

        // if audio value went above the bias during this frame
        if (m_previousAudioValue <= bias && m_audioValue > bias)
        {
            // if minimum beat interval is reached
            if (m_timer > timeStep)
            {
                OnBeat();
            }
        }

        m_timer += Time.deltaTime;
    }

    private void Update() => OnUpdate();
}