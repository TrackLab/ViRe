using System;
using UnityEngine;

public class MoveOptPanel : MonoBehaviour
{
    private IKFootSolver leftLeg, rightLeg;
    private TMPro.TextMeshProUGUI STval, SDval;
    private float threshold, distance;

    void Start(){
        leftLeg = GameObject.Find("LeftAnkleController").GetComponent<IKFootSolver>();
        rightLeg = GameObject.Find("RightAnkleController").GetComponent<IKFootSolver>();
        STval = GameObject.Find("STVal").GetComponentInChildren<TMPro.TextMeshProUGUI>();
        SDval = GameObject.Find("SDVal").GetComponentInChildren<TMPro.TextMeshProUGUI>();

        threshold = PlayerPrefs.GetFloat("IKthreshold", leftLeg.stepDistance);
        STval.text = threshold.ToString();
        distance = PlayerPrefs.GetFloat("IKdistance", leftLeg.stepLength);
        SDval.text = distance.ToString();
    }

    public void updateDistance(bool incr){
        if (incr){
            distance = (float) Math.Round((0.05f+distance)*100f)/100f;
        } else {
            distance = (float) Math.Round((distance-0.05f)*100f)/100f;
        }
        SDval.text = distance.ToString();
        writeSettings();
    }

    public void updateThreshold(bool incr){
        if (incr){
            threshold = (float) Math.Round((0.05f+threshold)*100f)/100f;
        } else {
            threshold = (float) Math.Round((threshold-0.05f)*100f)/100f;
        }
        STval.text = threshold.ToString();
        writeSettings();
    }

    private void writeSettings(){
        leftLeg.stepLength = distance;
        leftLeg.stepDistance = threshold;
        rightLeg.stepLength = distance;
        rightLeg.stepDistance = threshold;

        PlayerPrefs.SetFloat("IKthreshold", threshold);
        PlayerPrefs.SetFloat("IKdistance", distance);
        PlayerPrefs.Save();
    }
}
