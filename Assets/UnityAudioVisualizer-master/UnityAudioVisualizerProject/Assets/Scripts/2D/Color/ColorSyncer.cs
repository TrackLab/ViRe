using System.Collections;
using UnityEngine;

[RequireComponent(typeof(GameObject))]
public class ColorSyncer : BaseSyncer
{

    public Color[] beatColors;
    public Color restColor;

    private int m_randomIndx;
    private GameObject target;

    private IEnumerator MoveToColor(Color _target)
    {
        var _curr = GetComponent<Renderer>().material.color;
        var _initial = _curr;
        float _timer = 0;

        while (_curr != _target)
        {
            _curr = Color.Lerp(_initial, _target, _timer / timeToBeat);
            _timer += Time.deltaTime;

            GetComponent<Renderer>().material.color = _curr;

            yield return null;
        }

        m_isBeat = false;
    }

    private Color RandomColor()
    {
        if (beatColors == null || beatColors.Length == 0)
        {
            return Color.white;
        }

        m_randomIndx = Random.Range(0, beatColors.Length);
        return beatColors[m_randomIndx];
    }

    public override void OnUpdate() => base.OnUpdate();

    public override void OnBeat()
    {
        base.OnBeat();

        // pick a random color
        var newColor = new Color(Random.value, Random.value, Random.value, 1.0f);
        // apply it on current object's material
        GetComponent<Renderer>().material.color = newColor;
    }

    private void Start() => target = GetComponent<GameObject>();

}