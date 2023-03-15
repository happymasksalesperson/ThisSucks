using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    //NPCSpawner is an Instance 
    //on Start, Instantiates a pool of NPC GameObjects
    //has a List of spawnPoints
    //using SpawnNPC, spawns an NPC from the Pool at a Spawn Point
    //when an NPC dies, it shuffles itself back into the pool

    public static NPCSpawner instance;

    public GameObject NPCPrefab;
    public int poolSize;

    public List<GameObject> NPCPool = new List<GameObject>();

    public List<GameObject> spawnPoints = new List<GameObject>();

    public List<GameObject> passedPoints = new List<GameObject>();

    private GameObject parentObj;

    private void Start()
    {
        instance = this;
        
        spawnPoints.AddRange(passedPoints);
        passedPoints.Clear();

        parentObj = new GameObject("NPC Parent");

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(NPCPrefab);
            obj.SetActive(false);
            NPCPool.Add(obj);
        }
    }

    public void SpawnNPC()
    {
        GameObject NPC = GetPooledNPC();
        GameObject NPCspawn = SpawnPoint();

        NPC.transform.position = NPCspawn.transform.position;
        NPC.transform.rotation = NPCspawn.transform.rotation;

        NPC.transform.SetParent(parentObj.transform);
    }

    private GameObject SpawnPoint()
    {
        GameObject spawnPoint;
        if (spawnPoints.Count == 0)
            return null;
        
        spawnPoint = spawnPoints[0];
            spawnPoints.RemoveAt(0);
            passedPoints.Add(spawnPoint);
            
        return spawnPoint;
    }

    public GameObject GetPooledNPC()
    {
        foreach (GameObject obj in NPCPool)
        {
            if (!obj.activeInHierarchy)
            {
                Rigidbody NPCrb = obj.GetComponent<Rigidbody>();
                NPCrb.velocity = Vector3.zero;
                NPCrb.angularVelocity = Vector3.zero;
                NPCrb.rotation = Quaternion.identity;

                obj.SetActive(true);
                return obj;
            }
        }

        GameObject newObj = Instantiate(NPCPrefab);
        NPCPool.Add(newObj);
        return newObj;
    }
}
