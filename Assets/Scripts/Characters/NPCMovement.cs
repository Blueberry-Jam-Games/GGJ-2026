using System.Runtime.InteropServices;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    public CharacterMovement player;
    public string name;
    public Canvas interactabilitySymbol;
    public Canvas yapBox;

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
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InteractWithPlayer()
    {
        print("Hello Caleb. My name is " + name + ". Prepare to d-d-d-d-dddd-duel!");
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
