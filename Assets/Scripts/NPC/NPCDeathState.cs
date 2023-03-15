using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class NPCDeathState : MonoBehaviour
{
    private Rigidbody rb;

    //how much rb spins
    [SerializeField] private float torque;
    
    //how far NPC travels horizontally
    //tie to hitDir
    [SerializeField] private float horizontalDist;

    //how much NPC "jumps" up on Death
    [SerializeField] private float verticalDist;

    //change direction depending on facing direction
    //change to direction of incoming hit?
    [SerializeField] private int hitDir;

    private Gravity gravity;

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        gravity = GetComponent<Gravity>();

        if (Random.value < 0.5f)
        {
            hitDir = -1;
            horizontalDist = -horizontalDist;
        }

        else
            hitDir = 1;

        rb.AddForce(new Vector3(horizontalDist, verticalDist, horizontalDist), ForceMode.Impulse);

        //unlocks faster spin
        rb.maxAngularVelocity = 100000f;

        rb.AddTorque(transform.forward * torque * hitDir);

        gravity.enabled = true;

        StartCoroutine(Die());
    }

    //destroys gameobject once renderer is out of sight
    private IEnumerator Die()
    {
        yield return new WaitForSeconds(5);
            Destroy(gameObject);
    }
}