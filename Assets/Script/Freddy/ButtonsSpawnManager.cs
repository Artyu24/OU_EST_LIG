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
    public float limitX, limitY;
    public float speed;
    private float x, y;

    private int randMod, randDir, randSpeed;
    public static bool buttonsMoving;

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
        buttonsMoving = false;
        randMod = Random.Range(0, 3);
    }

    private void Update()
    { 
        if(buttons.Count != 0)
        {
            randMod = 0;
            switch (randMod)
            {
                // Direction
                case 0:
                    if (!buttonsMoving)
                    {
                        randDir = Random.Range(0, 4);
                        buttonsMoving = true;
                    }
                    foreach(GameObject button in buttons)
                    {
                        switch (randDir)
                        {
                            case 0: button.transform.position += Vector3.right * Time.deltaTime * speed;
                                if(button.transform.position.x >= limitX) { button.transform.position = new Vector3(-limitX, button.transform.position.y, button.transform.position.z); }
                                break; // Right
                            case 1: button.transform.position += Vector3.left * Time.deltaTime * speed;
                                if(button.transform.position.x <= -limitX) { button.transform.position = new Vector3(limitX, button.transform.position.y, button.transform.position.z); }
                                break;  // Left
                            case 2: button.transform.position += Vector3.up * Time.deltaTime * speed; 
                                if(button.transform.position.y >= limitY) { button.transform.position = new Vector3(button.transform.position.x, -limitY, button.transform.position.z); }
                                break;    // Up
                            case 3: button.transform.position += Vector3.down * Time.deltaTime * speed;
                                if (button.transform.position.y <= -limitY) { button.transform.position = new Vector3(button.transform.position.x, limitY, button.transform.position.z); }
                                break;  // Down
                            default: break;
                        }
                    }
                    break;
                // Speed
                case 1: 
                    break;
                // Both
                case 2: 
                    break;
                default: break;
            }
        }
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
