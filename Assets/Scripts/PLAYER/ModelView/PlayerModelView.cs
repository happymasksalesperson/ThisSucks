using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModelView : MonoBehaviour
{
    public event Action<int> ChangeHP;

    public void OnChangeHP(int x)
    {
        ChangeHP?.Invoke(x);
    }
    
    public event Action<int> ChangeUP;

    public void OnChangeUP(int x)
    {
        ChangeUP?.Invoke(x);
    }
    
}
