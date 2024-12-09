using UnityEngine;

public class GuardState : BaseAIState
{
    //qualifiers for state based on personality
    public new static float teamwork = 0.6f;
    public new static float cowardice = 0.1f;
    public new static float aggro = 0.6f;
    public GuardState(AIController controller) : base(controller){}
    
    public override int stateType { get; protected set; } = 3;

    public float spinInterval = 5f;
    public float spinIntervalTimer = 5f;

    public float spinSpeed = 1f;

    public Vector3 targetDir = Vector3.forward;
    public new AIStateMachine.AIState StateType
    {
        get { return (AIStateMachine.AIState)stateType; }
    }
    
    public override void Enter()
    {
        //Debug.Log("Entering Idle State");
    }
    
    public override void Execute()
    {
        spinIntervalTimer -= Time.deltaTime;
        if (spinIntervalTimer < 0)
        {
            PointRandomDirection();
            spinIntervalTimer = spinInterval;
        }

        var angle = Vector3.Angle(controller.transform.forward, targetDir);
        if (angle > 3)
        {
            horizontalInput = 1;
        }

        if (angle < -3)
        {
            horizontalInput = -1;
        }
        if (angle > -3 && angle < 3)
        {
            horizontalInput = 0;
        }
    }

    private void PointRandomDirection()
    {
        targetDir = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
    }

    public override void Exit()
    {
        //Debug.Log("Exiting Idle State");
    }
}
