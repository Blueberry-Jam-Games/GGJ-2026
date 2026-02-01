using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections.Generic;

public class CharacterMovement : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    private CharacterController controller;
    public string activeInputMap;
    Vector2 moveAmount;
    InputAction moveAction;
    InputAction interactAction;
    bool isThinking = false;
    public Canvas thoughtBubble;
    public TextMeshProUGUI playerThought;
    public float thinkingTimer = 0f;

    public NPCMovement[] npcs;
    private Dictionary<string, NPCMovement> namedNPCs;
    private Dictionary<string, bool> iAmNear;
    private string nearestNPC = "";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        namedNPCs = new Dictionary<string, NPCMovement>();
        iAmNear = new Dictionary<string, bool>();

        controller = GetComponent<CharacterController>();

        moveAction = InputSystem.actions.FindAction("Move");
        interactAction = InputSystem.actions.FindAction("Interact");

        InputSystem.actions.FindActionMap("Player").Enable();
        InputSystem.actions.FindActionMap("UI").Disable();

        activeInputMap = "Player";
        thoughtBubble.enabled = false;

        foreach (var npc in npcs)
        {
            namedNPCs.Add(npc.GetName(), npc);
            iAmNear.Add(npc.GetName(), false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        moveAmount = moveAction.ReadValue<Vector2>();
        Vector3 movement = new Vector3(moveAmount.x,0,moveAmount.y);
        controller.Move(movement * moveSpeed * Time.deltaTime);

        if (interactAction.WasReleasedThisFrame())
        {
            ThinkThought("I wonder if they would listen to me talk about my game jam game...");
            TriggerDialogueWithNearestNPC();
        }

        if (isThinking)
        {
            thinkingTimer -= Time.deltaTime;
            if (thinkingTimer <= 0)
            {
                thoughtBubble.enabled = false;
                isThinking = false;
            }
        }
    }

    void FixedUpdate()
    {
        foreach (var npc in npcs)
        {
            npc.HideSmiley();
        }
        NearestNPC();
        if (nearestNPC != "")
        {
            print("fuckme");
            namedNPCs[nearestNPC].ShowSmiley();
        }
    }

    public void toggleInputSystem()
    {
        if (activeInputMap == "Player")
        {
            InputSystem.actions.FindActionMap("Player").Disable();
            InputSystem.actions.FindActionMap("UI").Enable();
            activeInputMap = "UI";
        } else if (activeInputMap == "UI") {
            InputSystem.actions.FindActionMap("UI").Disable();
            InputSystem.actions.FindActionMap("Player").Enable();
            activeInputMap = "Player";
        }
    }

    public void ThinkThought(string thoughttoThink)
    {
        if (!isThinking) {
            thoughtBubble.enabled = true;
            thinkingTimer = 2.0f;
            isThinking = true;
            playerThought.text = thoughttoThink;
        }
    }

    public void EntersSpeakingDistanceWith(string name)
    {
        iAmNear[name] = true;
        print(name + " sees you approach");
    }

    public void ExitsSpeakingDistanceWith(string name)
    {
        iAmNear[name] = false;
        print(name + " watches you leave");
    }

    public void TriggerDialogueWithNearestNPC()
    {
        namedNPCs[nearestNPC].InteractWithPlayer();
    }

    public void NearestNPC()
    {
        bool iAmNearAnyone = false;
        nearestNPC = "";

        foreach (var name in namedNPCs.Keys)
        {
            if (iAmNear[name])
            {
                iAmNearAnyone = true;
            }
        }

        if (iAmNearAnyone)
        {
            float minDist = float.MaxValue;
            foreach (var name in namedNPCs.Keys)
            {
                if (iAmNear[name] && Vector3.Distance(transform.position, namedNPCs[name].GetPos()) < minDist)
                {
                    nearestNPC = name;
                    minDist = Vector3.Distance(transform.position, namedNPCs[name].GetPos());
                }
            }
        }
    }
}
