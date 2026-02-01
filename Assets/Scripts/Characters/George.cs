using UnityEngine;

public class George : NPCMovement
{
    [Header("George")]
    public Dialogue conversation1;
    public Dialogue conversation2;

    public int interactTimes = 0;

    public override void InteractWithPlayer()
    {
        if (interactTimes == 0)
        {
            PlayDialogue(conversation1);
            interactTimes++;
        }
        else if(interactTimes == 1)
        {
            PlayDialogue(conversation2);
            interactTimes++;
        }
    }
}
