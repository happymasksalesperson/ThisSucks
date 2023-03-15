using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ThisSucksUI : MonoBehaviour
{
    public PlayerModelView modelView;

    [SerializeField] TextMeshProUGUI HPtext;
    private int HP;

    [SerializeField] TextMeshProUGUI uselessPointsText;
    private int uselessPoints;

    //on enable subscribes to the ChangeHP/ChangeUP Events
    //starts the clock
    private void OnEnable()
    {
        modelView = GetComponent<PlayerModelView>();
        modelView.ChangeHP += ChangeHP;
        modelView.ChangeUP += ChangePoints;
    }
    
    void Update()
    {
        HPtext.text = HP.ToString();
        uselessPointsText.text = (uselessPoints.ToString());
    }

    private void ChangeHP(int x)
    {
        HP += x;
    }

    private void ChangePoints(int x)
    {
        uselessPoints += x;
    }
}