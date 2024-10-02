using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswerButton : MonoBehaviour
{
    public GameObject MenuHide;
    public void RevealMenu()
    {
        MenuHide.SetActive(false);
    }
}
