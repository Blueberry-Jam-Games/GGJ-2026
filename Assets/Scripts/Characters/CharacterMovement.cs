using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
        moveAction = InputSystem.actions.FindAction("Move");
        interactAction = InputSystem.actions.FindAction("Interact");
        InputSystem.actions.FindActionMap("Player").Enable();
        InputSystem.actions.FindActionMap("UI").Disable();
        activeInputMap = "Player";
        thoughtBubble.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        moveAmount = moveAction.ReadValue<Vector2>();
        Vector3 movement = new Vector3(moveAmount.x,0,moveAmount.y);
        controller.Move(movement * moveSpeed * Time.deltaTime);

        if (interactAction.IsPressed())
        {
            print("egg");
            ThinkThought("I wonder if they would listen to me talk about my game jam game...");
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

    public void toggleInputSystem()
    {
        if (activeInputMap == "Player")
        {
            InputSystem.actions.FindActionMap("Player").Disable();
            InputSystem.actions.FindActionMap("UI").Enable();
        } else if (activeInputMap == "UI") {
            InputSystem.actions.FindActionMap("UI").Disable();
            InputSystem.actions.FindActionMap("Player").Enable();
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
}
