using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupScript : MonoBehaviour
{

    public enum PowerUpType
    {
        HealthUp,
        SpeedUp,
        DoubleJump,
        Invincibility
    };
    public PowerUpType myPowerType;

    private int powerUpInt =  (int)PowerUpType.HealthUp;
    
    private Rigidbody rb;

    private ThisSucksPlayer playerScript;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.transform.Rotate(90.0f, 0.0f, 0.0f, Space.Self);
    }

    public int MyTypeInt()
    {
        return powerUpInt;
    }
    
    
}
