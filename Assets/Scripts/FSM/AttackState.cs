using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : BaseAIState
{
    //qualifiers for state based on personality
    public static float teamwork = 0.5f;
    public static float cowardice = 0.4f;
    public static float aggro = 1f;
    public AttackState(AIController controller) : base(controller){}
    
    public override int stateType { get; protected set; } = 3;

    public AIStateMachine.AIState StateType
    {
        get { return (AIStateMachine.AIState)stateType; }
    }
    
    public override void Enter()
    {
        Debug.Log("Entering Attack State");
    }
    
    public override void Execute()
    {
        //use ProcessInputs() on AIController to Shoot()
        controller.pawn.Shoot();
        Debug.Log("Executing Attack State");
    }
    
    public override void Exit()
    {
        Debug.Log("Exiting Attack State");
    }
}
