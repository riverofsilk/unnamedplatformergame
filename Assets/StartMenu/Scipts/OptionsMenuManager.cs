using TMPro;
using UnityEngine;

public class OptionsMenuManager : MonoBehaviour
{
    private TextMeshPro lashControlText;
    void Start()
    {
        lashControlText = transform.GetChild(1).GetChild(0).GetComponent<TextMeshPro>();
    }
    void Update()
    {
        lashControlText.text = $"Lash With : {Options.lashControl}";
    }
    public void toggleLashControl()
    {
        if (Options.lashControl == "R3")
        {
            Options.lashControl = "R2 + L3";
        }
        else
        {
            Options.lashControl = "R3";
        }
    }
}
