using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class ButtonsSpawnManager : NetworkBehaviour
{ 
    public List<GameObject> buttons = new List<GameObject>();
    private List<Vector2> buttonSpawns = new List<Vector2>();
    public RoundData roundData;
    public GameObject wrongButton;
    public GameObject rightButton;
    private bool isInstantiate;
    public float tpX, tpY;
    float x, y;

    public static ButtonsSpawnManager instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            return;
        }
    }


    private void Start()
    {
        isInstantiate = false;
    }

    private void Update()
    { 
        /*if (isInstantiate)
        {
            foreach (GameObject button in buttons)
            {
                button.GetComponent<Rigidbody2D>().velocity += Vector2.right * Time.deltaTime * 0.2f;
                if(button.transform.position.x <= -tpX)
                {
                    button.transform.position = new Vector3(tpX, button.transform.position.y, button.transform.position.z);
                } 
                if(button.transform.position.x >= tpX)
                {
                    button.transform.position = new Vector3(-tpX, button.transform.position.y, button.transform.position.z);
                }
                if (button.transform.position.y <= -tpY)
                {                    
                    button.transform.position = new Vector3(button.transform.position.x, tpY, button.transform.position.z);
                }
                if (button.transform.position.y >= tpY)
                {
                    button.transform.position = new Vector3(button.transform.position.x, -tpY, button.transform.position.z);
                }
            }
        }*/
    }

    [ServerCallback]
    public void SpawnButtons()
    {
        int compteur = 0;
        for (int i = 0; i < roundData.buttonsNumber[GameManager.instance.nbrRound] - 1; i++)
        {
            x = Random.Range(-8, 7);
            y = Random.Range(-4, 4);
            Vector2 spawnButtonVector = new Vector2(x, y);
            buttonSpawns.Add(spawnButtonVector);

            GameObject spawnWrongButton = (GameObject)Instantiate(wrongButton, buttonSpawns[compteur], Quaternion.identity);
            buttons.Add(spawnWrongButton);
            NetworkServer.Spawn(spawnWrongButton);
            //isInstantiate = true;
            compteur++;
        }
        x = Random.Range(-8, 7);
        y = Random.Range(-4, 4);
        Vector2 spawnRightButtonVector = new Vector2(x, y);
        buttonSpawns.Add(spawnRightButtonVector);
        GameObject spawnRightButton = (GameObject)Instantiate(rightButton, buttonSpawns[compteur], Quaternion.identity);
        buttons.Add(spawnRightButton);
        NetworkServer.Spawn(spawnRightButton);
    }

    [ServerCallback]
    public void ResetButtons()
    {
        foreach (GameObject button in buttons)
        {
            Destroy(button);
        }
        buttons.Clear();
        buttonSpawns.Clear();
    }
}
