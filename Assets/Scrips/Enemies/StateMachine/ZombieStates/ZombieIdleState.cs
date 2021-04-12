using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieIdleState : ZombieStates
{
    public ZombieIdleState(ZombieComponent zombie, StateMachine statemachine) : base(zombie, statemachine)
    {
    }

    public override void Start()
    {
        base.Start();
        OwnerZombie.ZombieNavMesh.isStopped = true;
        OwnerZombie.ZombieNavMesh.ResetPath();
        OwnerZombie.ZombieAnimator.SetFloat("MovementZ", 0.0f);
    }
}
