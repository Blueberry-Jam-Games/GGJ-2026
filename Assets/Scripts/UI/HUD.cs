using System;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField]
    private Slider socialBattery;
    [SerializeField]
    private Slider stim;

    private void Start()
    {
        GameplayManager.Instance.stimChanged += UpdateStimometer;
        GameplayManager.Instance.socialBatteryChanged += UpdateSocialBattery;

        UpdateStimometer (GameplayManager.Instance.GetStim());
        UpdateSocialBattery (GameplayManager.Instance.GetEnergy());
    }

    private void OnDestroy()
    {
        GameplayManager.Instance.stimChanged -= UpdateStimometer;
        GameplayManager.Instance.socialBatteryChanged -= UpdateSocialBattery;
    }

    // input a value between 0 and 100 or so help me god
    private void UpdateSocialBattery(float percentage)
    {
        socialBattery.value = percentage / 100.0f;
    }

    // input a value between 0 and 100 or so help me god
    private void UpdateStimometer(float percentage)
    {
        stim.value = percentage / 100.0f;
    }
}
