using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class DataPlayer : NetworkBehaviour
{
    [SyncVar]
    private int actualScore;

    public int GetActualScore {get {return actualScore;} }

    private void Awake()
    {
        actualScore = 0;
    }

    public void AddScore(int addPoint)
    {
        actualScore += addPoint;
        Debug.Log(actualScore);
        //Afficher le score sur UI
    }

    public void DecreaseScore()
    {
        actualScore -= 100;
        Debug.Log(actualScore);
        //Afficher le score sur UI
    }
}
