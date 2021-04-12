using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;




public class MovementComponent : MonoBehaviour
{

    


    [SerializeField] private float WalkSpeed;
    [SerializeField] private float RunSpeed;
    [SerializeField] private float JumpForce;

    [SerializeField] private LayerMask JumpLayerMask;
    [SerializeField] private float JumpThreshold = 0.1f;
    [SerializeField] private float JumpLandingCheckDelay = 0.1f;
    [SerializeField] private float MoveDirectionBuffer = 10.0f;

    //components
    private PlayerController PlayerController;
    private Animator PlayerAnimator;
    private Rigidbody PlayerRigidbody;
    private NavMeshAgent PlayerNavMeshAgent;

    //references
    private Transform PlayerTransform;


    private Vector3 NextPositionCheck;

    private Vector2 InputVector = Vector2.zero;
    private Vector3 MoveDirection = Vector3.zero;


    //Animator Hashes
    public readonly int MovementXHash = Animator.StringToHash("MovementX");
    public readonly int MovementYHash = Animator.StringToHash("MovementY");
    public readonly int IsJumpingHash = Animator.StringToHash("IsJumping");
    public readonly int IsRunningHash = Animator.StringToHash("IsRunning");


    private void Awake()
    {
        PlayerTransform = transform;
        PlayerController = GetComponent<PlayerController>();
        PlayerAnimator = GetComponent<Animator>();
        PlayerRigidbody = GetComponent<Rigidbody>();
        PlayerNavMeshAgent = GetComponent<NavMeshAgent>();
    }

    public void OnMovement(InputValue value)
    {
        //Debug.Log(value.Get());
        InputVector = value.Get<Vector2>();
        PlayerAnimator.SetFloat(MovementXHash, InputVector.x);
        PlayerAnimator.SetFloat(MovementYHash, InputVector.y);
    }

    public void OnRun(InputValue value)
    {
        PlayerController.IsRunning = value.isPressed;
        PlayerAnimator.SetBool(IsRunningHash, value.isPressed);
    }


    public void OnJump(InputValue value)
    {
        if (PlayerController.IsJumping) return;

        //PlayerNavMeshAgent.isStopped = true;
        //PlayerNavMeshAgent.enabled = false;

        //jump
        PlayerController.IsJumping = value.isPressed;
        PlayerAnimator.SetBool(IsJumpingHash, value.isPressed);
        PlayerRigidbody.AddForce((PlayerTransform.up + MoveDirection) * JumpForce , ForceMode.Impulse);

       // InvokeRepeating(nameof(LandingCheck), JumpLandingCheckDelay, 0.1f);

    }


    private void LandingCheck()
    {
        //Debug.DrawLine(transform.position, transform.position + (-transform.up* 100.0f), Color.red, 0.1f);


        if (!Physics.Raycast(transform.position, -transform.up, 
            out RaycastHit hit, 100.0f, JumpLayerMask)) return;
        
        Debug.Log(hit.distance);

        if (!(hit.distance < JumpThreshold) || !PlayerController.IsJumping) return;
            
        PlayerNavMeshAgent.enabled = true;
        PlayerNavMeshAgent.isStopped = false;

        PlayerController.IsJumping = false;
        PlayerAnimator.SetBool(IsJumpingHash, false);

        CancelInvoke(nameof(LandingCheck));
            
        
    }

    private void Update()
    {
        if (PlayerController.IsJumping) return;

        //if (!(InputVector.magnitude > 0)) MoveDirection = Vector3.zero;

        
        MoveDirection = transform.forward * InputVector.y + transform.right * InputVector.x;

        float currentSpeed = PlayerController.IsRunning ? RunSpeed : WalkSpeed;

        Vector3 movementDirection = MoveDirection * (currentSpeed * Time.deltaTime);


        //Vector3 newPosition = transform.position + MoveDirection;


        NextPositionCheck = transform.position + MoveDirection * MoveDirectionBuffer;

        if (NavMesh.SamplePosition(NextPositionCheck, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
        {
            transform.position += movementDirection;
        }

        //PlayerNavMeshAgent.Move(movementDirection);
        
        //old way without nav mesh
        //PlayerTransform.position += movementDirection;



    }

   

    private void OnDrawGizmos()
    {
        if (NextPositionCheck != Vector3.zero)
        {
            Gizmos.DrawWireSphere(NextPositionCheck, 0.5f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //bool ground = collision.collider.CompareTag("Ground");
        if (!collision.collider.CompareTag("Ground") && !PlayerController.IsJumping) return;

        //Debug.Log(ground);
        PlayerController.IsJumping = false;
        PlayerAnimator.SetBool(IsJumpingHash, false);

    }

    //PlayerInputActions PlayerActions;

    //private void Awake()
    //{
    //    PlayerActions = new PlayerInputActions();
    //}

    //private void OnEnable()
    //{
    //    PlayerActions.Enable();
    //    PlayerActions.PlayerActionMap.Movement.performed += Movement;
    //}
    //public void Movement(InputAction.CallbackContext value)
    //{
    //    Debug.Log(value.ReadValue<Vector2>());
    //}

    //private void OnDisable()
    //{
    //    PlayerActions.Disable();
    //    PlayerActions.PlayerActionMap.Movement.performed -= Movement;
    //}
}
