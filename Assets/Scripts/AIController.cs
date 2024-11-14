using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : Controller
{
    public NoiseMaker[] AllNoiseMakers;
    private Transform[] Targets; //will be used later to store all the targets in the scene
    public float viewDistance = 10f;
    public float viewAngle = 90f;
    
    // Start is called before the first frame update
    public override void Start()
    {
        AllNoiseMakers = Component.FindObjectsOfType<NoiseMaker>();
        Debug.Log(AllNoiseMakers);
        
        // Run the parent (base) Start
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        // Make decisions
        MakeDecisions();
        // Run the parent (base) Update
        base.Update();
    }

    public void MakeDecisions()
    {
        Debug.Log("Making Decisions");
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

    private bool RaycastCheck()
    {
        
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
