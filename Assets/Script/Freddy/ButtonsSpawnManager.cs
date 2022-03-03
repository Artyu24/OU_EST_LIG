using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ButtonsSpawnManager : NetworkBehaviour
{ 
    public List<GameObject> buttons = new List<GameObject>();

    private List<Vector2> buttonSpawns = new List<Vector2>();

    private List<Vector3> buttonDirections = new List<Vector3>();

    public RoundData roundData;
    public GameObject wrongButton;
    public GameObject rightButton;
    private bool isInstantiate;
    public float limitX, limitY;
    public float speed;
    private float x, y;

    private int randMod, randDir, randSpeed;
    private int k = 0;
    public static bool choosingDirection;

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
        choosingDirection = false;
        randMod = Random.Range(0, 3);
    }

    private void Update()
    { 
        if(buttons.Count != 0)
        {
            switch (randMod)
            {
                // Direction
                case 0:
                    if (!choosingDirection)
                    {
                        for(int i = 0; i < buttons.Count; i++)
                        {
                            Vector3 buttonDirection = new Vector3(Random.Range(-10, 10), Random.Range(-10, 10));
                            buttonDirections.Add(buttonDirection);
                        }
                        choosingDirection = true;
                    }

                    foreach (GameObject button in buttons)
                    {
                        button.transform.position += buttonDirections[k] * Time.deltaTime * speed;
                        if(k >= buttonDirections.Count - 1)
                        {
                            k = 0;
                        } else
                        {
                            k++;
                        }
                    }

                    foreach (GameObject button in buttons)
                    {
                        if (button.transform.position.x >= limitX)
                        {
                            button.transform.position = new Vector3(-limitX, button.transform.position.y, button.transform.position.z);
                        }

                        if (button.transform.position.x <= -limitX)
                        {
                            button.transform.position = new Vector3(limitX, button.transform.position.y, button.transform.position.z);
                        }

                        if (button.transform.position.y >= limitY)
                        {
                            button.transform.position = new Vector3(button.transform.position.x, -limitY, button.transform.position.z);
                        }

                        if (button.transform.position.y <= -limitY)
                        {
                            button.transform.position = new Vector3(button.transform.position.x, limitY, button.transform.position.z);
                        }
                    }
                    break;
                // Speed
                case 1:
                    if (!choosingDirection)
                    {
                        Vector3 buttonDirection = new Vector3(Random.Range(-10, 10), Random.Range(-10, 10));
                        buttonDirections.Add(buttonDirection);
                        choosingDirection = true;
                    }
                    foreach (GameObject button in buttons)
                    {
                        button.transform.position += buttonDirections[k] * Time.deltaTime * Random.Range(1, 7);
                        if (k >= buttonDirections.Count - 1)
                        {
                            k = 0;
                        }
                        else
                        {
                            k++;
                        }
                    }

                    foreach (GameObject button in buttons)
                    {
                        if (button.transform.position.x >= limitX)
                        {
                            button.transform.position = new Vector3(-limitX, button.transform.position.y, button.transform.position.z);
                        }

                        if (button.transform.position.x <= -limitX)
                        {
                            button.transform.position = new Vector3(limitX, button.transform.position.y, button.transform.position.z);
                        }

                        if (button.transform.position.y >= limitY)
                        {
                            button.transform.position = new Vector3(button.transform.position.x, -limitY, button.transform.position.z);
                        }

                        if (button.transform.position.y <= -limitY)
                        {
                            button.transform.position = new Vector3(button.transform.position.x, limitY, button.transform.position.z);
                        }
                    }
                    break;
                // Both
                case 2:
                    if (!choosingDirection)
                    {
                        for (int i = 0; i < buttons.Count; i++)
                        {
                            Vector3 buttonDirection = new Vector3(Random.Range(-10, 10), Random.Range(-10, 10));
                            buttonDirections.Add(buttonDirection);
                        }
                        choosingDirection = true;
                    }

                    foreach (GameObject button in buttons)
                    {
                        button.transform.position += buttonDirections[k] * Time.deltaTime * Random.Range(1, 7);
                        if (k >= buttonDirections.Count - 1)
                        {
                            k = 0;
                        }
                        else
                        {
                            k++;
                        }
                    }

                    foreach (GameObject button in buttons)
                    {
                        if (button.transform.position.x >= limitX)
                        {
                            button.transform.position = new Vector3(-limitX, button.transform.position.y, button.transform.position.z);
                        }

                        if (button.transform.position.x <= -limitX)
                        {
                            button.transform.position = new Vector3(limitX, button.transform.position.y, button.transform.position.z);
                        }

                        if (button.transform.position.y >= limitY)
                        {
                            button.transform.position = new Vector3(button.transform.position.x, -limitY, button.transform.position.z);
                        }

                        if (button.transform.position.y <= -limitY)
                        {
                            button.transform.position = new Vector3(button.transform.position.x, limitY, button.transform.position.z);
                        }
                    }
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
