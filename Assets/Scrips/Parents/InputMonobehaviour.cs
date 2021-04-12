using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputMonobehaviour : MonoBehaviour
{

    protected PlayerInputActions GameInput;


    protected void Awake()
    {
        GameInput = new PlayerInputActions();

    }
    protected void OnEnable()
    {
        GameInput.Enable();
    }

    protected void OnDisable()
    {
        GameInput.Disable();
    }
}
