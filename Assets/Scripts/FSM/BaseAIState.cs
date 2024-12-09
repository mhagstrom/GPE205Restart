using UnityEngine;

//TODO: Change into ScriptableObject
public abstract class BaseAIState //no monobehavior complexity here
{
    //constructor runs when object is created
    public BaseAIState(AIController controller)
    {
        this.controller = controller;
    }
    
    public static float teamwork;
    public static float cowardice;
    public static float aggro;
    
    protected AIController controller;
    public float verticalInput = 0;
    public float horizontalInput = 0;
    public bool ShootInput = false;

    public AIStateMachine.AIState StateType
    {
        get { return (AIStateMachine.AIState)stateType; }
    }

    public abstract int stateType { get; protected set; }
    public virtual void Enter()
    {
        
    }
    
    public virtual void Execute()
    {
        
    }
    
    public virtual void Exit()
    {
        
    }

    public void TurnTowards(TankPawn target)
    {
        if (target == null) return;

        float angle = Vector3.SignedAngle(controller.transform.forward, (target.transform.position - controller.transform.position).normalized, Vector3.up);
        if (angle > 0)
        {
            horizontalInput = 1;
        }

        if (angle < 0)
        {
            horizontalInput = -1;
        }
        if (angle > -3 && angle < 3)
        {
            horizontalInput = 0;
        }
    }
}
