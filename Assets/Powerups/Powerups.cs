using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Powerups", menuName = "Create new powerup", order = 1)]
public class SpawnManagerScriptableObject : ScriptableObject
{
    public string powerupName;

    public Vector3[] spawnPoints;
}