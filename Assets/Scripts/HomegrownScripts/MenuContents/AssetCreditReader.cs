using UnityEngine;

public class AssetCreditReader : MonoBehaviour
{
    public TextAsset OpenSourceCredits;
    void OnEnable()
    {
        TMPro.TextMeshProUGUI creditText = GetComponent<TMPro.TextMeshProUGUI>();
        creditText.text = OpenSourceCredits.text;
    }
}
