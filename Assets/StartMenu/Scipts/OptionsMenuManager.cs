using TMPro;
using UnityEngine;

public class OptionsMenuManager : MonoBehaviour
{
    private string lashWith = "R3";
    public GameObject lashToggleText;
    void Start()
    {
    }
    void Update()
    {
        lashToggleText.GetComponent<TextMeshProUGUI>().text = $"Lash With : {lashWith}";
    }
    public void toggleLashControl()
    {
        if (lashWith == "R3")
        {
            lashWith = "R2 + L3";
        }
        else
        {
            lashWith = "R3";
        }
    }
}
