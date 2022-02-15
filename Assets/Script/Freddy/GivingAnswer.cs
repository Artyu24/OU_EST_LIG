using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GivingAnswer : NetworkBehaviour
{
    public void OnMouseDown()
    {
        if (gameObject.GetComponent<SpriteRenderer>().sprite == ButtonsSpawnManager.imgToClick)
        {
            //Add score to the player
            Debug.Log("Found!");
        }
        else
        {
            Debug.Log("Img clicked : " + gameObject.GetComponent<SpriteRenderer>().sprite);
            //Lose total score
            Debug.Log("Wrong one!");
        }
        //ButtonsSpawnManager.instance.round++;
        //ButtonsSpawnManager.instance.NextRound();
    }
}
