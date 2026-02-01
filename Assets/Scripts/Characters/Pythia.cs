using UnityEngine;

public class Pythia : NPCMovement
{
    [Header("Pythia")]
    public Dialogue conversation1;
    public Dialogue conversationRest;

    public bool interacted = false;

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
}
