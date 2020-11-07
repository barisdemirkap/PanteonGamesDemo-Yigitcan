using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Predefined vfxPrefab for preventing null errors. If anyone forgets to set vfx prefab for an obstacle it will be set automaticly in runtime 
/// </summary>
[CreateAssetMenu(fileName ="Obstacle VFX",menuName ="Scriptable Objects")]
public class ObsactleVFX : ScriptableObject
{
     public GameObject vfxPrefab;
}
