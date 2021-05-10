using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class MusicVisulizer : MonoBehaviour
{

    public SpriteRenderer background;
    public SpriteRenderer upperPanel;
    public SpriteRenderer lowerPanel;
    public SpriteRenderer bigCircle;
    public SpriteRenderer smallCircle;
    public SpriteRenderer bg;
    public Transform bigCircleTransform;
    public Transform smallCircleTransform;
    public float variation;
    public float speed = 1;
    float r;
    float g;
    float b;
    public float h;
    public float s;
    public float v;
    public Slider colorSlider;
    public Slider sValueSlider;
    public Slider vValueSlider;
    public float size = 1;

    //	private float[] musicData;


    void Start()
    {


    }

    void Update()
    {

        Visulization();
        variation = colorSlider.value;
        s = sValueSlider.value;
        v = vValueSlider.value;
        //Demo();
    }

    void Demo()
    {
        float[] spectrum = GetComponent<AudioSource>().GetSpectrumData(1024, 0, FFTWindow.BlackmanHarris);
        int i = 1;
        while (i < 1023)
        {
            Debug.DrawLine(new Vector3(i - 1, spectrum[i], 0), new Vector3(i, spectrum[i + 1], 0), Color.red);
            Debug.DrawLine(new Vector3(i - 1, Mathf.Log(spectrum[i - 1]) + 20, 2), new Vector3(i, Mathf.Log(spectrum[i]) + 20, 2), Color.cyan);
            Debug.DrawLine(new Vector3(Mathf.Log(i - 1), spectrum[i - 1] - 10, 1), new Vector3(Mathf.Log(i), spectrum[i] - 10, 1), Color.green);
            Debug.DrawLine(new Vector3(i - 1, Mathf.Log(spectrum[i - 1]), 4), new Vector3(i, Mathf.Log(spectrum[i]), 4), Color.yellow);
            i++;
        }


    }

    void Visulization()
    {
        int i = 2;
        float[] musicData = GetComponent<AudioSource>().GetSpectrumData(64, 0, FFTWindow.Triangle);

        while (i < 63)
        {
            h = musicData[i] * speed;

            bg.color = HSVtoRGB(variation, s, v - 0.03f, 1);
            bigCircle.color = HSVtoRGB(h - variation, s, v, 1);
            smallCircle.color = HSVtoRGB(h + variation, s, v, 1);

            bigCircleTransform.localScale = new Vector3(size + h * 2, size - h * 4, 1);
            smallCircleTransform.localScale = new Vector3(size - h * 4, size + h * 4, 1);
            i++;
        }
    }

    public static Color HSVtoRGB(float hue, float saturation, float value, float alpha)
    {
        while (hue > 1f)
        {
            hue -= 1f;
        }
        while (hue < 0f)
        {
            hue += 1f;
        }
        while (saturation > 1f)
        {
            saturation -= 1f;
        }
        while (saturation < 0f)
        {
            saturation += 1f;
        }
        while (value > 1f)
        {
            value -= 1f;
        }
        while (value < 0f)
        {
            value += 1f;
        }
        if (hue > 0.999f)
        {
            hue = 0.999f;
        }
        if (hue < 0.001f)
        {
            hue = 0.001f;
        }
        if (saturation > 0.999f)
        {
            saturation = 0.999f;
        }
        if (saturation < 0.001f)
        {
            return new Color(value * 255f, value * 255f, value * 255f);
        }
        if (value > 0.999f)
        {
            value = 0.999f;
        }
        if (value < 0.001f)
        {
            value = 0.001f;
        }

        float h6 = hue * 6f;
        if (h6 == 6f)
        {
            h6 = 0f;
        }
        int ihue = (int)(h6);
        float p = value * (1f - saturation);
        float q = value * (1f - (saturation * (h6 - (float)ihue)));
        float t = value * (1f - (saturation * (1f - (h6 - (float)ihue))));
        switch (ihue)
        {
            case 0:
                return new Color(value, t, p, alpha);
            case 1:
                return new Color(q, value, p, alpha);
            case 2:
                return new Color(p, value, t, alpha);
            case 3:
                return new Color(p, q, value, alpha);
            case 4:
                return new Color(t, p, value, alpha);
            default:
                return new Color(value, p, q, alpha);
        }
    }

}
