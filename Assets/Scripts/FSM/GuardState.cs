using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardState : BaseAIState
{
    //qualifiers for state based on personality
    public static float teamwork = 0.8f;
    public static float cowardice = 1;
    public static float aggro = 0.5f;
    public GuardState(AIController controller) : base(controller){}
    
    public override int stateType { get; protected set; } = 4;

    public AIStateMachine.AIState StateType
    {
        get { return (AIStateMachine.AIState)stateType; }
    }
    
    public override void Enter()
    {
        //Debug.Log("Entering Idle State");
    }
    
    public override void Execute()
    {
        //Debug.Log("Executing Idle State");
    }
    
    public override void Exit()
    {
        //Debug.Log("Exiting Idle State");
    }
}
