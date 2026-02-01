using UnityEngine;

public class Lacy : NPCMovement
{
    [Header("Lacy")]
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

    private void Update()
    {
        // TODO!!!!!
        if (GameplayManager.Instance.EvaluateCondition("LaceyEnding"))
        {
            // END GAME
        }
    }
}
