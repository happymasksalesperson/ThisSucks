using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCIdle : MonoBehaviour, INPC
{
    private NPCStateManager stateManager;

    private void OnEnable()
    {
        stateManager = GetComponent<NPCStateManager>();
    }

    public void OnPlayerEnterRange()
    {
        stateManager.ChangeStateString("death");
    }
}
