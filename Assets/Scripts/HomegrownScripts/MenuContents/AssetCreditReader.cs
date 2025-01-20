using UnityEngine;

public class AssetCreditReader : MonoBehaviour
{
    public TextAsset OpenSourceCredits;
    public TMPro.TextMeshProUGUI creditText;

    void OnEnable()
    {
        creditText.text = OpenSourceCredits.text;
    }
}
