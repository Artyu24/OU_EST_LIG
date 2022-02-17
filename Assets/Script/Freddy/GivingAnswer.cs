using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GivingAnswer : NetworkBehaviour
{
    public void OnMouseDown()
    {
        /*Debug.Log(ButtonsSpawnManager.imgToClick);*/
        if (gameObject.GetComponent<SpriteRenderer>().sprite == ButtonsSpawnManager.instance.GetSetImgToClick)
        {
            ScoreManager.instance.AddScorePlayer();
            Debug.Log("Found!");
        }
        else
        {
            ScoreManager.instance.DecreaseScorePlayer();
            Debug.Log("Wrong one!");
        }
        //ButtonsSpawnManager.instance.round++;
        //ButtonsSpawnManager.instance.NextRound();
    }
}
