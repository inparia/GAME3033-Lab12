using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CrossHairScript : InputMonobehaviour
{
    public Vector2 MouseSensitivity;

    public bool Inverted = false;
    public Vector2 CurrentAimPosition { get; private set; }

    [SerializeField, Range(0, 1)]
    private float CrosshairHorizontalPercentage = 0.25f;


    private float HorizontalOffset;
    private float MaxHorizontalDeltaContstrain;
    private float MinHorizontalDeltaContstrain;

    [SerializeField, Range(0, 1)]
    private float CrosshairVerticalPercentage = 0.25f;
    private float VerticalOffset;
    private float MaxVerticalDeltaContstrain;
    private float MinVerticalDeltaContstrain;

    private Vector2 CrossHairStartingPosition;
    private Vector2 CurrentLookDeltas;




    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.Instance.CursorActive)
        {
            AppEvents.Invoke_OnMouseCursorEnable(false);
        }

        CrossHairStartingPosition = new Vector2(Screen.width / 2.0f, Screen.height / 2.0f);

        HorizontalOffset = (Screen.width * CrosshairHorizontalPercentage) / 2.0f;
        MinHorizontalDeltaContstrain = -(Screen.width / 2.0f) + HorizontalOffset;
        MaxHorizontalDeltaContstrain = (Screen.width / 2.0f) - HorizontalOffset;

        VerticalOffset = Screen.height * CrosshairVerticalPercentage/ 2.0f;
        MinVerticalDeltaContstrain = -(Screen.height / 2.0f) + VerticalOffset;
        MaxVerticalDeltaContstrain = (Screen.height / 2.0f) - VerticalOffset;
    }

    private new void OnEnable()
    {
        base.OnEnable();
        GameInput.PlayerActionMap.Look.performed += OnLook;
    }

    private void OnLook(InputAction.CallbackContext delta)
    {
        Vector2 mouseDelta = delta.ReadValue<Vector2>();

        CurrentLookDeltas.x += mouseDelta.x * MouseSensitivity.x;

        if (CurrentLookDeltas.x >= MaxHorizontalDeltaContstrain ||
            CurrentLookDeltas.x <= MinHorizontalDeltaContstrain)
        {
            CurrentLookDeltas.x -= mouseDelta.x * MouseSensitivity.x;
        }

        CurrentLookDeltas.y += mouseDelta.y * MouseSensitivity.y;

        if (CurrentLookDeltas.y >= MaxVerticalDeltaContstrain ||
            CurrentLookDeltas.y <= MinVerticalDeltaContstrain)
        {
            CurrentLookDeltas.y -= mouseDelta.y * MouseSensitivity.y;
        }
    }

    private void Update()
    {
        float crosshairXPosition = CrossHairStartingPosition.x + CurrentLookDeltas.x;
        float crosshairYPosition = Inverted ? CrossHairStartingPosition.y - CurrentLookDeltas.y : CrossHairStartingPosition.y + CurrentLookDeltas.y;

        CurrentAimPosition = new Vector2(crosshairXPosition, crosshairYPosition);

        transform.position = CurrentAimPosition;

    }

    private new void OnDisable()
    {
        base.OnDisable();
    }

}
