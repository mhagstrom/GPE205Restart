using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : Controller
{
    public NoiseMaker[] AllNoiseMakers;
    
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
    
}
