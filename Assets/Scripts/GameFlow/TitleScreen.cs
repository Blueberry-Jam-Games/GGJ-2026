using BJ;
using UnityEngine;
using UnityEngine.InputSystem;

public class TitleScreen : MonoBehaviour
{
    public InputAction submitAction;
    bool loading = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        BJ.LevelTransitionEffect.Templates.FadeTransition("Fade", 1.0f, Color.black, false, Color.black, Color.black);
        submitAction = InputSystem.actions.FindAction("Submit"); 
    }

    // Update is called once per frame
    void Update()
    {
        if (submitAction.WasReleasedThisFrame() && !loading)
        {
            BJ.LevelLoader.Instance.LoadLevel("Mom", "Fade");
        }
    }
}
