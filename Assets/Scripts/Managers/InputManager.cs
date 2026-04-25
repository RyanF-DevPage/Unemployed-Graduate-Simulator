using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Simulator_Game
{
    [DefaultExecutionOrder(-100)]
    public class InputManager : MonoBehaviour, IInputManager
    {
        [SerializeField] private InputActionAsset inputActions;

        public static InputManager Instance { get; private set; }

        // Fires with the world-space position of every left click
        public event Action<Vector2> OnWorldClick;

        private InputAction _click;
        private InputAction _point;

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            DontDestroyOnLoad(gameObject);
            ServiceLocator.Register<IInputManager>(this);

            var ui = inputActions.FindActionMap("UI", throwIfNotFound: true);
            _click = ui.FindAction("Click", throwIfNotFound: true);
            _point = ui.FindAction("Point", throwIfNotFound: true);
            ui.Enable();
        }

        private void OnEnable()  { if (_click != null) _click.performed += HandleClick; }
        private void OnDisable() { if (_click != null) _click.performed -= HandleClick; }

        private void OnDestroy()
        {
            if (Instance == this) ServiceLocator.Unregister<IInputManager>();
        }

        private void HandleClick(InputAction.CallbackContext ctx)
        {
            Vector2 screenPos = _point.ReadValue<Vector2>();
            Vector2 worldPos  = Camera.main.ScreenToWorldPoint(screenPos);
            OnWorldClick?.Invoke(worldPos);
        }
    }
}
