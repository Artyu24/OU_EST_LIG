using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GivingAnswer : NetworkBehaviour
{
    public void OnMouseDown()
    {
        if (gameObject.GetComponent<SpriteRenderer>().sprite == GameManager.instance.GetSetImgToClick)
        {
            ScoreManager.instance.AddScorePlayer();
            Debug.Log("Found!");
        }
        else
        {
            ScoreManager.instance.DecreaseScorePlayer();
            Debug.Log("Wrong one!");
        }
    }
}
