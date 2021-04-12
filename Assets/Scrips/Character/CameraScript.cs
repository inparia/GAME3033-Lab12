using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraScript : MonoBehaviour
{
    [SerializeField]
    private float RotationPower = 10;

    [SerializeField]
    private float HorizontalDamping = 1;

    [SerializeField]
    private GameObject FollowTarget;

    private Transform FollowTargetTransform;
    private Vector2 PreviousMouseDelta = Vector2.zero;


    private void Awake()
    {
        FollowTargetTransform = FollowTarget.transform;
    }

    private void OnLook(InputValue delta)
    {
        //Debug.Log("Camera Rotate");

        Vector2 aimValue = delta.Get<Vector2>();

        Quaternion addedRotation = Quaternion.AngleAxis(Mathf.Lerp(PreviousMouseDelta.x, aimValue.x, 1.0f/HorizontalDamping) * RotationPower, Vector3.up);

        FollowTargetTransform.rotation *= addedRotation;

        PreviousMouseDelta = aimValue;

        transform.rotation = Quaternion.Euler(0, FollowTargetTransform.rotation.eulerAngles.y, 0);


        FollowTargetTransform.localEulerAngles = Vector3.zero;

    }

}
