using System;
using UnityEngine;

public class HUD : MonoBehaviour
{
    float HEIGHT = 257f;
    float WIDTH = 206f;
    float MAX_X = 800f;
    float MAX_Y = 325f;

    public GameObject StimometerBar;
    public GameObject SocialBatteryBar;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // input a value between 0 and 100 or so help me god
    public void UpdateSocialBattery(float percentage)
    {
        RectTransform rect = SocialBatteryBar.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(rect.sizeDelta.x * percentage/100f, rect.sizeDelta.y * percentage/100f);
        rect.transform.position = new Vector3(-MAX_X, -MAX_Y - (percentage/100f * HEIGHT/2), 0);
    }

    // input a value between 0 and 100 or so help me god
    public void UpdateStimometer(float percentage)
    {
        RectTransform rect = StimometerBar.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(rect.sizeDelta.x * percentage/100f, rect.sizeDelta.y * percentage/100f);
        rect.transform.position = new Vector3(MAX_X, -MAX_Y - (percentage/100f * HEIGHT/2), 0);
    }
}
