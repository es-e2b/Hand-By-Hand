using System;
using UnityEngine;

public class ReplayButton : MonoBehaviour
{
    public int count;
    public GameObject Hands;
    public RectTransform HandsRect;
    void Awake()
    {
        HandsRect=Hands.GetComponent<RectTransform>();
    }
    void Start()
    {
        Replay();
    }
    void Update()
    {
        Play();
    }
    public void Replay()
    {
        Hands.SetActive(true);
        count=0;
    }

    void Play()
    {
        if(count>=2)
        {
            return;
        }
        HandsRect.anchoredPosition+=new Vector2(0, 300f*Time.deltaTime);
        if(HandsRect.anchoredPosition.y>=250f)
        {
            HandsRect.anchoredPosition=Vector2.zero;
            count++;
            if(count>=2)
            {
                Hands.SetActive(false);
            }
        }
    }
}
