using BJ;
using UnityEngine;

public class Lacy : NPCMovement
{
    [Header("Lacy")]
    public Dialogue conversation1;
    public Dialogue conversationRest;

    public bool interacted = false;

    bool loading = false;

    public override void InteractWithPlayer()
    {
        if (!interacted)
        {
            PlayDialogue(conversation1);
            interacted = true;
        }
        else
        {
            PlayDialogue(conversationRest);
        }
    }

    private void Update()
    {
        if (GameplayManager.Instance.EvaluateCondition("LaceyEnding") && loading)
        {
            loading = true;
            LevelLoader.Instance.LoadLevel("ThanksForPlaying", "Fade");
        }
    }
}
