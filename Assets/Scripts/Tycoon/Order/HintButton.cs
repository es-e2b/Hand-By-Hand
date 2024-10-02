using TMPro;
using UnityEngine;

public class HintButton : MonoBehaviour
{
    public string hintMessage;
    public TextMeshProUGUI hintGUI;
    public bool onoff;
    public void OutputMessage()
    {
        if(!onoff)
            hintGUI.text = hintMessage;
        else
            hintGUI.text = "";
        onoff=!onoff;
    }
}
