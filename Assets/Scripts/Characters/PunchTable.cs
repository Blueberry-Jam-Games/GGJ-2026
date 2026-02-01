using UnityEngine;

public class PunchTable : NPCMovement
{
    [Header("PunchTable")]
    public Dialogue conversation1;

    public override void InteractWithPlayer()
    {
        PlayDialogue(conversation1);
    }
}
