using UnityEngine;
using UnityEngine.UI;

public class ScrollBarButtons : MonoBehaviour
{
    public GameObject ScrollParent;
    public bool ScrollDown;
    private ScrollRect scrollbar;
    
    void Start()
    {
        scrollbar = ScrollParent.GetComponent<ScrollRect>();
    }

    public void scroll(){
        if (ScrollDown){
            scrollbar.velocity = new Vector2(0,100);
        } else {
            scrollbar.velocity = new Vector2(0,-100);
        }
    }
}
