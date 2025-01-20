using System;
using UnityEngine;

public class MoveOptPanel : MonoBehaviour
{
    public IKFootSolver leftLeg, rightLeg;
    public TMPro.TextMeshProUGUI STval, SDval;
    private float threshold, distance;

    //Load the IK settings or set them to defaults
    void Start()
    {
        threshold = PlayerPrefs.GetFloat("IKthreshold", leftLeg.stepDistance);
        distance = PlayerPrefs.GetFloat("IKdistance", leftLeg.stepLength);
        updateText();
    }

    public void updateDistance(bool incr)
    {
        if (incr)
        {
            distance = (float)Math.Round((0.05f + distance) * 100f) / 100f;
        }
        else
        {
            distance = (float)Math.Round((distance - 0.05f) * 100f) / 100f;
        }
        updateText();
        writeSettings();
    }

    public void updateThreshold(bool incr)
    {
        if (incr)
        {
            threshold = (float)Math.Round((0.05f + threshold) * 100f) / 100f;
        }
        else
        {
            threshold = (float)Math.Round((threshold - 0.05f) * 100f) / 100f;
        }
        updateText();
        writeSettings();
    }

    private void updateText()
    {
        SDval.text = distance.ToString();
        STval.text = threshold.ToString();
    }

    //Write changes to IK scripts and Player Preferences
    private void writeSettings()
    {
        leftLeg.stepLength = rightLeg.stepLength = distance;
        leftLeg.stepDistance = rightLeg.stepDistance = threshold;

        PlayerPrefs.SetFloat("IKthreshold", threshold);
        PlayerPrefs.SetFloat("IKdistance", distance);
        PlayerPrefs.Save();
    }
}
