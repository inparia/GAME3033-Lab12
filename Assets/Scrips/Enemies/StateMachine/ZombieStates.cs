public class ZombieStates : State
{
    protected ZombieComponent OwnerZombie;
    public ZombieStates(ZombieComponent zombie, StateMachine statemachine) : base(statemachine)
    {
        OwnerZombie = zombie;
    }
}


public enum ZombieStateType
{
    Idle,
    Attack,
    Follow,
    Dead
}