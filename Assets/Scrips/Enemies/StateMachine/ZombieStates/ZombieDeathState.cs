using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieDeathState : ZombieStates
{
    public ZombieDeathState(ZombieComponent zombie, StateMachine statemachine) : base(zombie, statemachine)
    {
    }

    public override void Start()
    {
        base.Start();
        OwnerZombie.ZombieNavMesh.isStopped = true;
        OwnerZombie.ZombieNavMesh.ResetPath();

        OwnerZombie.ZombieAnimator.SetFloat("MovementZ", 0f);
        OwnerZombie.ZombieAnimator.SetBool("IsDead", true);
    }


    //if we do objetc pooling with the zombie to let us reset it
    public override void Exit()
    {
        base.Exit();
        OwnerZombie.ZombieNavMesh.isStopped = false;

        OwnerZombie.ZombieAnimator.SetBool("IsDead", false);
    }
}
