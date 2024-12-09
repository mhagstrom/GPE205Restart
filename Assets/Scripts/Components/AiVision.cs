using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AiVision : AiSense
{
    //variable to store the pawn that this component is attached to
    private Pawn pawn => GetComponent<Pawn>();
    
    [SerializeField] private float tickRate = 0.1f;
    private float timeSinceLastTick = 0;
    [Space(10)]
    
    //viewing distance and angle for the AI
    [SerializeField] float viewDistance = 20f;
    [SerializeField] float viewAngle = 90f;
    private bool _seesTarget;
    //event that gets raise when istargeting changes
    public delegate void TargetingChanged(bool seesTarget, HashSet<TankPawn> pawns);
    public event TargetingChanged OnStatusChanged;

    public HashSet<TankPawn> Targets = new HashSet<TankPawn>();
    
    public bool SeesTarget
    {
        get
        {
            return _seesTarget;
        }
        private set
        {
            _seesTarget = value;
        }
    }
    
    private bool CanSee(Transform target)
    {
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

    private void Update()
    {
        timeSinceLastTick += Time.deltaTime;
        if (timeSinceLastTick > tickRate)
        {
            timeSinceLastTick = 0;

            CheckForTargets();
        }
        var names = string.Join("\n", Targets);
        DebugPlus.LogOnScreen(names);
        DebugPlus.LogOnScreen("SeesTarget: " + SeesTarget);
    }

    private void CheckForTargets()
    {
        var newTargets = GameManager.Instance.AllPawns.Where(p => p != MyPawn && CanSee(p.transform)).ToHashSet();

        SeesTarget = newTargets.Count > 0;

        var shouldAlert = !Targets.SetEquals(newTargets);

        Targets = newTargets;

        if(shouldAlert)
            OnStatusChanged?.Invoke(SeesTarget, Targets);

        //Debug.Log($"Sees Target: {SeesTarget}");
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        //Draw the view distance and angle
        Gizmos.matrix = Matrix4x4.TRS( transform.position, transform.rotation, Vector3.one );
        Gizmos.color = Color.red;
        Gizmos.DrawFrustum(Vector3.zero, viewAngle, viewDistance, 0, 1);
    }
    #endif
}
