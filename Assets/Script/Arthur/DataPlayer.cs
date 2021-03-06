using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class DataPlayer : NetworkBehaviour
{
    [SyncVar] public string username = "Player";

    [SyncVar]
    private int actualScore;
    public int GetActualScore {get {return actualScore;} }

    [SyncVar]
    private int classement = 0;
    public int Classement { get => classement; set => classement = value; }


    private void Awake()
    {
        actualScore = 0;
    }

    public void AddScore(int addPoint)
    {
        actualScore += addPoint;
    }

    public void DecreaseScore()
    {
        actualScore -= 100;
    }
}
