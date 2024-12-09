using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AttackState : BaseAIState
{
    //qualifiers for state based on personality
    public new static float teamwork = 0.5f;
    public new static float cowardice = 0.4f;
    public new static float aggro = 1f;
    public AttackState(AIController controller) : base(controller){}
    
    public override int stateType { get; protected set; } = 3;

    public new AIStateMachine.AIState StateType
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

        var target = controller._aiVision.Targets.FirstOrDefault();

        if (target == null)
        {
            return;
        }

        controller.pawn.Shoot();
        //if angle is more than 5 degrees set the horizontal input to 1

        TurnTowards(target);
        verticalInput = 0.33f;

        //Debug.Log("Executing Attack State");
    }

    public override void Exit()
    {
        Debug.Log("Exiting Attack State");
    }
}
