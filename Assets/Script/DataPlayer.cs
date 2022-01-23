using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class DataPlayer : NetworkBehaviour
{
    private int actualScore = 0;

    public void AddScore(int addPoint)
    {
        if (isLocalPlayer)
        {
            actualScore += addPoint;
            //Afficher le score sur UI
        }
    }
}
