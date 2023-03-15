using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCStateManager : MonoBehaviour
{
    public MonoBehaviour currentState;

    public MonoBehaviour startingState;
    
    public MonoBehaviour deathState;

    public MonoBehaviour idleState;

    private string stateString;

    private void Start()
    {
        ChangeState(startingState);
    }

    //Cam's change state stuff:
    public void ChangeState(MonoBehaviour newState)
    {
        if (newState == currentState)
        {
            return;
        }

        if (currentState != null)
        {
            currentState.enabled = false;
        }

        newState.enabled = true;

        currentState = newState;
    }

    public void ChangeStateString(string state)
    {
        switch (state)
        {
            case ("idle"):
                ChangeState(idleState);
                break;

            case ("death"):
                ChangeState(deathState);
                break;
        }
    }
}
