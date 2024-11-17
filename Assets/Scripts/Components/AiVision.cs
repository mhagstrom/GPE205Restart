using System.Collections.Generic;
using UnityEngine;

public class AiVision : MonoBehaviour
{
    //variable to store the pawn that this component is attached to
    private Pawn pawn => GetComponent<Pawn>();
    
    [SerializeField] private float tickRate = 0.1f;
    private float timeSinceLastTick = 0;
    [Space(10)]
    
    //viewing distance and angle for the AI
    [SerializeField] float viewDistance = 10f;
    [SerializeField] float viewAngle = 90f;
    private float _lastLookedTime;
    private bool _seesTarget;
    private List<TankPawn> playerPawns = GameManager.Instance.PlayerPawns;
    private TankPawn checkTarget;
    //event that gets raise when istargeting changes
    public delegate void TargetingChanged(bool isTargeting);
    public event TargetingChanged OnTargetingChanged;
    
    
    public bool SeesTarget
    {
        get
        {
            if (_lastLookedTime + tickRate < Time.time)
            {
                _seesTarget = CanSee(checkTarget.transform);
            }
            return _seesTarget;
        }
        private set
        {
            if (_seesTarget != value) OnTargetingChanged?.Invoke(value);
            _seesTarget = value;
        }
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

    private void Update()
    {
        timeSinceLastTick += Time.deltaTime;
        if (timeSinceLastTick > tickRate)
        {
            timeSinceLastTick = 0;
            foreach (var playerPawn in playerPawns)
            {
                checkTarget = playerPawn;
                _seesTarget = CanSee(checkTarget.transform);
            }
            
        }
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
