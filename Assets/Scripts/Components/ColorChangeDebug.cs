using System;
using System.Collections.Generic;
using UnityEngine;

public class ColorChangeDebug : MonoBehaviour
{
    private AiVision Vision;

    private void Start()
    {
        Vision = GetComponent<AiVision>();
        if (Vision == null) return;

        Vision.OnStatusChanged += Vision_OnStatusChanged;
    }

    private void Vision_OnStatusChanged(bool seesTarget, HashSet<TankPawn> pawns)
    {
        GetComponentInChildren<Renderer>().materials[0].color = seesTarget ? Color.red : Color.white;
    }
}
