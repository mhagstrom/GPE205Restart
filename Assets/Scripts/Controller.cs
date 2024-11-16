using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public Pawn pawn;
    public virtual void Start()
    {
        
    }
    
    public virtual void Update()
    {
        
    }
    
    public virtual void ProcessInputs()
    {
        
    }
}

    public enum ControllerType
    {
        Player,
        AI
    }