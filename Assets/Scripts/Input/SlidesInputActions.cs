using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class SlidesInputActions : ScriptableObject, PlayerInput.ISlidesControlActions
{
    private PlayerInput _playerInput;
    
    public event UnityAction onFirstPage;
    public event UnityAction onLastPage;
    public event UnityAction onPreviousPage;
    public event UnityAction onNextPage;
    public event UnityAction onLoad;
    
    #region generalMethods
    void OnEnable()
    {
        _playerInput = new PlayerInput();
        _playerInput.SlidesControl.SetCallbacks(this);
    }
    void OnDisable()
    {
        DisableSlidesInput();
    }
    public void DisableSlidesInput()
    {
        _playerInput.SlidesControl.Disable();
    }
    
    public void EnableSlidesInput()
    {
        _playerInput.SlidesControl.Enable();
    }
    #endregion

    #region Input Actions

    public void OnFirstPage(InputAction.CallbackContext context)
    {
        if (context.phase is InputActionPhase.Performed)
        {
            onFirstPage?.Invoke();
        }
    }

    public void OnLastPage(InputAction.CallbackContext context)
    {
        if (context.phase is InputActionPhase.Performed)
        {
            onLastPage?.Invoke();
        }
    }

    public void OnPreviousPage(InputAction.CallbackContext context)
    {
        if (context.phase is InputActionPhase.Performed)
        {
            onPreviousPage?.Invoke();
        }
    }

    public void OnNextPage(InputAction.CallbackContext context)
    {
        if (context.phase is InputActionPhase.Performed)
        {
            onNextPage?.Invoke();
        }
    }

    public void OnLoad(InputAction.CallbackContext context)
    {
        if (context.phase is InputActionPhase.Performed)
        {
            onLoad?.Invoke();
        }
    }

    #endregion
    
}
