using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : Controller
{
    [SerializeField] private float tickRate = 0.1f;
    private float lastTickTime;
    
    
    public NoiseMaker[] AllNoiseMakers;
    private Transform[] Targets; //will be used later to store all the targets in the scene
    
    private Transform activeTarget; //stores which pawn the AI wants to chase or shoot at
        
    //viewing distance and angle for the AI
    [SerializeField] float viewDistance = 10f;
    [SerializeField] float viewAngle = 90f;
    
    //targeting distance and spherecast radius for the AI for direct line of sight
    [SerializeField] float targetingDistance = 40f;
    [SerializeField] float spherecastRadius = 0.15f;
    
    //hearing distance for the AI
    [SerializeField] private NoiseMaker _noiseMaker;
    [SerializeField] private float hearingDistance = 10;
    
    //cached values for whether the ai can see or hear the target
    private bool _isTargeting;
    private bool _seesTarget;
    private bool _hearsTarget;
    
    //times for the AI to know when to update
    private float _targetRate = 0.1f;
    private float _lastLookedTime;
    private float _lastSphereCheckTime;
    private float _lastListenedTime;
    
    //variables for AI state machine
    public  BaseAIState CurrentState;
    public EnemyTypes enemyType;
    
    public bool HearsTarget
    {
        get
        {
            //check if time is greather than the cached time + the rate
            if (_lastListenedTime + _targetRate < Time.time)
            {
                _hearsTarget = CanHear(_noiseMaker);
            }
            return _hearsTarget;
        }
        private set{ _hearsTarget = value; }
    }

    public bool SeesTarget
    {
        get
        {
            if (_lastLookedTime + _targetRate < Time.time)
            {
                _seesTarget = CanSee(activeTarget);
            }
            return _seesTarget;
        }
        private set{ _seesTarget = value; }
    }
    public bool IsTargeting
    {
        get
        {
            if (_lastSphereCheckTime + _targetRate < Time.time)
            {
                _isTargeting = SpherecastCheck();
            }
            return _isTargeting;
        }
        private set{ _isTargeting = value; }
    }
    
    public void Awake()
    {
        AIStateMachine.RegisterAgent(this);
        CurrentState = new GuardState(this);
        _noiseMaker = GetComponent<NoiseMaker>();
        
    }
    
    // Start is called before the first frame update
    public override void Start()
    {
        AllNoiseMakers = Component.FindObjectsOfType<NoiseMaker>();
        //Debug.Log(AllNoiseMakers);
        CurrentState.Enter();
        // Run the parent (base) Start
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        Debug.Log($"Current state: {(int)CurrentState.StateType}");
        //Debug.Log($"Horizontal: {CurrentState.horizontalInput}");
        //Debug.Log($"Vertical: {CurrentState.verticalInput}");
        lastTickTime += Time.deltaTime;
        if (lastTickTime > tickRate)
        {
            lastTickTime = 0;
            //might change state every 10th of a second :( 
            /* I want to update these values here to trigger state change but idk implementation
             * 
             * _hearsTarget = CanHear(_noiseMaker);
             * _seesTarget = CanSee(activeTarget);
             * _isTargeting = SpherecastCheck();
            */
            
            // These sort of senses should be separated out into their own components 
            // and raise events when their own state changes. Ai controller should add those components
            // and listen for thier events to then request a state change

            AIStateMachine.RequestStateChange(this);
        }
        
        CurrentState.Execute();
        
        ProcessInputs();
        
        // Run the parent (base) Update
        base.Update();
    }
    
    private void OnDestroy()
    {
        AIStateMachine.UnregisterAgent(this);
    }

    public override void ProcessInputs()
    {
        base.ProcessInputs();
        //inputs in ProcessInputs() are returning null exception, needs to be fixed
        
        float verticalInput = CurrentState.verticalInput;
        float horizontalInput = CurrentState.horizontalInput;
        
        if (verticalInput < 0)
        {
            horizontalInput = -CurrentState.horizontalInput;
        }
        
        pawn.Move(verticalInput);
        pawn.Rotate(horizontalInput);
        if (CurrentState.ShootInput)
        {
            pawn.Shoot();
        }
    }
    
    public bool CanHear(NoiseMaker noise)
    {
        _lastListenedTime = Time.time;
        if (Vector3.Distance(noise.transform.position, transform.position) < noise.volumeDistance + hearingDistance)
        {
            return true;
        }
        return false;
    }
    
    private bool CanSee(Transform target)
    {
        _lastLookedTime = Time.time;
        //check distance
        float DistanceToTarget = Vector3.Distance(pawn.transform.position, target.position);
        if (DistanceToTarget > viewDistance)
        {
            return false;
        }
        //check angles
        Vector3 directionToTarget = (target.position - pawn.transform.position).normalized;
        float angle = Vector3.Angle(pawn.transform.forward, directionToTarget);
        if (angle > viewAngle)
        {
            return false;
        }
        return true;
    }

    //using spherecast instead of raycast so aim doesn't need to be perfect
    private bool SpherecastCheck()
    {
        _lastSphereCheckTime = Time.time;
        RaycastHit hit;
        if (Physics.SphereCast(pawn.transform.position, spherecastRadius, pawn.transform.forward, out hit, targetingDistance))
        {
            if (hit.transform.CompareTag("Player"))
            {
                return true;
            }
            return true;
        }
        return false;
    }
    
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, hearingDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _noiseMaker.volumeDistance);
        
        //Draw the view distance and angle
        Gizmos.matrix = Matrix4x4.TRS( transform.position, transform.rotation, Vector3.one );
        Gizmos.color = Color.red;
        Gizmos.DrawFrustum(Vector3.zero, viewAngle, viewDistance, 0, 1);
        RaycastHit hit;
        if (Physics.SphereCast(pawn.transform.position, spherecastRadius, pawn.transform.forward, out hit,
                targetingDistance))
        {
            Vector3 HitPoint = transform.InverseTransformPoint(hit.point);
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(Vector3.zero, HitPoint);
            Gizmos.DrawWireSphere(HitPoint, spherecastRadius);
        }
        else
        {
            Gizmos.color = Color.grey;
            Gizmos.DrawLine(Vector3.zero, transform.forward * targetingDistance);
        }

        if (CurrentState == null)
        {
            return;
        }

        Gizmos.color = Color.white;
        string stateName = Enum.GetName(typeof(AIStateMachine.AIState), CurrentState.StateType);
        UnityEditor.Handles.Label(transform.position + Vector3.up, stateName);
    }
#endif
    
}
