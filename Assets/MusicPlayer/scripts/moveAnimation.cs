using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class moveAnimation : MonoBehaviour {
    private float width = 110.0f;
    private float height = 43.0f;
    public Text headerText;
    private string myText;
    private GameObject textObject;
    private RectTransform rTransform,rTransform2;
    public Transform target1,target2;
    private float speed = 200.0f;
    private Vector3 initial;

    //public Vector3 startP, endP;
    // Use this for initialization
    void Start () {
        textObject = GameObject.Find("textObj");
        rTransform = textObject.transform as RectTransform;
		rTransform2 = target2 as RectTransform;

        rTransform.sizeDelta = new Vector2(width, height);
		rTransform2.sizeDelta = new Vector2(width, height);
    }


	Transform getLoaderFromCanvas( Transform canvas, string nameOfImage ) {
		Transform[] trans = canvas.GetComponentsInChildren<Transform>(true);
		foreach( Transform t in trans ) {
			if( t.gameObject.name.Equals(nameOfImage)) {
				return t;
			}
		}
		return null;
	}
    IEnumerator ResetPosition()
    {
        yield return new WaitForSeconds(5);
    }
	// Update is called once per frame
	void Update () {
        myText = headerText.text;
        
		rTransform.sizeDelta = new Vector2(myText.Length *11, height);
		setSizeofTarget2 ();

        transform.position = Vector3.MoveTowards(transform.position, target1.position, speed * Time.deltaTime);

        if(transform.position == target1.position)
        {
			transform.position = target2.position;
			rTransform2.pivot = new Vector2 (0, 0);
        }

        //StartCoroutine(ResetPosition());
    }

	private void setSizeofTarget2(){
		rTransform2.pivot = new Vector2 (1, 0);
		rTransform2.sizeDelta = new Vector2(myText.Length *11, height);
	}
}