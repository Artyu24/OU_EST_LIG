using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GivingAnswer : NetworkBehaviour
{
    public Vector3 direction;
   
    private Vector3 newDir;

    [ServerCallback]
    private void Update()
    {
        foreach(GameObject button in GameObject.FindGameObjectsWithTag("WrongButton"))
        {
            Physics2D.IgnoreCollision(button.GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>(), true);
        }

        Physics2D.IgnoreCollision(GameObject.FindGameObjectWithTag("RightButton").GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>(), true);


        if (ButtonsSpawnManager.instance.inContact == true)
        {
            // recupérer list de speed
            transform.position += direction * Time.deltaTime * ButtonsSpawnManager.instance.speed;
        }
    }

    [ServerCallback]
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //it's working when the direction is from one direction but not the other one
        if (collision.gameObject.CompareTag("Down"))
        {
            //Debug.Log("Previous dir : " + direction);
            direction = new Vector3(direction.x, -direction.y, 0);
            ButtonsSpawnManager.instance.inContact = true;
           // Debug.Log("New dir : " + direction);

        }
        if (collision.gameObject.CompareTag("Up"))
        {
            //Debug.Log("Previous dir : " + direction);
            direction = new Vector3(direction.x, -direction.y, 0);
            ButtonsSpawnManager.instance.inContact = true;

            //Debug.Log("New dir : " + direction);
            /* Debug.Log(direction);
             Debug.Log(collision.gameObject.tag);
             newDir = Vector2.Reflect(direction, Vector2.down);
             Debug.Log(newDir);*/
        }

        if (collision.gameObject.CompareTag("Right"))
        {
            //Debug.Log("Previous dir : " + direction);
            direction = new Vector3(-direction.x, direction.y, 0);
            ButtonsSpawnManager.instance.inContact = true;

            //Debug.Log("New dir : " + direction);
        }

        if (collision.gameObject.CompareTag("Left"))
        {
            //Debug.Log("Previous dir : " + direction);
            direction = new Vector3(-direction.x, direction.y, 0);
            ButtonsSpawnManager.instance.inContact = true;

            //Debug.Log("New dir : " + direction);
        }
    }

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
