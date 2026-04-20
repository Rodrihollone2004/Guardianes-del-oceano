using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    private InputSystem_Actions inputActions;

    [Header("Input Values")]
    public Vector2 MoveInput { get; private set; }

    public bool IsDraging { get; private set; } 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            inputActions = new InputSystem_Actions();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
    }

    private void OnDisable()
    {
        inputActions.Player.Disable();
    }

    public bool WasClickPressedThisFrame()
    {
        return inputActions.Player.MouseClick.WasPressedThisFrame();
    }

    private void Update()
    {
        MoveInput = inputActions.Player.MousePosition.ReadValue<Vector2>();
        IsDraging = inputActions.Player.MouseClick.IsPressed();
    }
}
