using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyTypes", menuName = "Enemy/EnemyTypes")]
public class EnemyTypes : ScriptableObject
{
    [Range(0,1)] public float teamwork;
    [Range(0,1)] public float cowardice;
    [Range(0,1)] public float aggro;

}