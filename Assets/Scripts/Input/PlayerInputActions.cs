using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;


[CreateAssetMenu(menuName = "Player Input")]
public class PlayerInputActions : ScriptableObject, PlayerInput.IGameplayActions
{
    private PlayerInput _playerInput;
    public event UnityAction<Vector2> onMovement;
    public event UnityAction onStopMove;

    public event UnityAction onToggleSpeak;

    public event UnityAction onReleaseSpeak;
    // general methods
    void OnEnable()
    {
        _playerInput = new PlayerInput();
        _playerInput.Gameplay.SetCallbacks(this);
    }
    void OnDisable()
    {
        DisableAllInputs();
    }
    public void DisableAllInputs()
    {
        _playerInput.Gameplay.Disable();
    }
    
    public void EnableGameplayInput()
    {
        _playerInput.Gameplay.Enable();
    }
    // movement action invoke
    public void OnMovement(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            onMovement?.Invoke(context.ReadValue<Vector2>());
        }

        if (context.phase == InputActionPhase.Canceled)
        {
            onStopMove?.Invoke();
        }
    }

    public void OnSpeak(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            onToggleSpeak?.Invoke();
        }
        if (context.phase == InputActionPhase.Canceled)
        {
            onReleaseSpeak?.Invoke();
        }
    }
}
