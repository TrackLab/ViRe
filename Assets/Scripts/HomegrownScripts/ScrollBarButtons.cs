using UnityEngine;
using UnityEngine.UI;

public class ScrollBarButtons : MonoBehaviour
{
    public ScrollRect scrollbar;

    public void Scroll(bool down)
    {
        scrollbar.velocity = new Vector2(0, down ? 100 : -100);
    }
}
