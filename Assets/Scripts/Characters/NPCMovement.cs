using System.Runtime.InteropServices;
using NUnit.Framework.Internal;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    public CharacterMovement player;
    public string characterName;
    public Canvas interactabilitySymbol;
    public Canvas yapBox;

    public Dialogue testdialogue;

    void OnTriggerEnter (Collider other)
    {
        player.EntersSpeakingDistanceWith(name);
        yapBox.enabled = true;
    }

    void OnTriggerExit (Collider other)
    {
        player.ExitsSpeakingDistanceWith(name);
        yapBox.enabled = false;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        interactabilitySymbol.enabled = false;
        yapBox.enabled = false;

        GameObject playerObj = GameObject.FindWithTag("Player");
        player = playerObj.GetComponent<CharacterMovement>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public virtual void InteractWithPlayer()
    {
        PlayDialogue(testdialogue);
        Debug.LogWarning($"You forgot to override npc {characterName}");
    }

    protected void PlayDialogue(Dialogue dlg)
    {
        GameObject ui = GameObject.FindWithTag("UI");
        DialogueManager dm = ui.GetComponent<DialogueManager>();

        player.toggleInputSystem();

        dm.StartDialogue(dlg, player.toggleInputSystem);
    }

    public void MoveAlongSpline()
    {
        
    }

    public string GetName()
    {
        return name;
    }

    public Vector3 GetPos()
    {
        return transform.position;
    }

    public void HideSmiley()
    {
        interactabilitySymbol.enabled = false;
    }

    public void ShowSmiley()
    {
        interactabilitySymbol.enabled = true;
    }
}
