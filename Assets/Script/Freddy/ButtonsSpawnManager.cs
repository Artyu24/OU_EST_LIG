using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ButtonsSpawnManager : NetworkBehaviour
{ 
    public List<GameObject> buttons = new List<GameObject>();

    private List<Vector2> buttonSpawns = new List<Vector2>();

    public List<Vector3> buttonDirections = new List<Vector3>();
    public List<float> buttonsSpeed = new List<float>();

    public RoundData roundData;
    public GameObject wrongButton;
    public GameObject rightButton;
    public GameObject spawnRightButton;
    public GameObject spawnWrongButton;
    private GameObject spawnTpButton;
    private Vector3 newPosSpawnTpButton;
    public float limitX, limitY;
    public float speed;
    private float x, y;

    public bool inContact = false;

    public static int randMod;
    private int k = 0;
    public static bool choosingDirection;
    public static bool choosingSpeed = true;

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
        choosingDirection = false;
        randMod = Random.Range(0, 3);
    }
    
    private void Update()
    { 
        if(buttons.Count != 0)
        {
            Debug.Log(randMod);
            switch (randMod)
            {
                // Direction
                case 0:
                    SetDirection();
                    break;
                // Speed
                case 1:
                    SetSpeed();
                    break;
                // Both
                case 2:
                    SetDirectionAndSpeed();
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

            spawnWrongButton = (GameObject)Instantiate(wrongButton, buttonSpawns[compteur], Quaternion.identity);
            buttons.Add(spawnWrongButton);
            NetworkServer.Spawn(spawnWrongButton);         
            
            compteur++;
        }

        x = Random.Range(-8, 7);
        y = Random.Range(-4, 4);
        Vector2 spawnRightButtonVector = new Vector2(x, y);
        buttonSpawns.Add(spawnRightButtonVector);
        spawnRightButton = (GameObject)Instantiate(rightButton, buttonSpawns[compteur], Quaternion.identity);
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

    [ServerCallback]
    private void SetDirection()
    {
        if (!choosingDirection)
        {
            speed = Random.Range(2, 5);
            for (int i = 0; i < buttons.Count; i++)
            {
                Vector3 buttonDirection = new Vector3(Random.Range(-10, 10), Random.Range(-10, 10));
                buttonDirections.Add(buttonDirection);
            }
            choosingDirection = true;
        }

        foreach (GameObject button in buttons)
        {
            if (inContact == false) 
            {
                button.GetComponent<GivingAnswer>().direction = buttonDirections[k].normalized;
                button.transform.position += buttonDirections[k].normalized * Time.deltaTime * speed;                
            }

            if (k >= buttonDirections.Count - 1)
            {
                k = 0;
            }
            else
            {
                k++;
            }
        }
    }

    [ServerCallback]
    private void SetSpeed()
    {
        if (choosingSpeed)
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                speed = Random.Range(2f, 5f);
                buttonsSpeed.Add(speed);
            }
            choosingSpeed = false;
        }

        if (!choosingDirection)
        {
            Vector3 buttonDirection = new Vector3(Random.Range(-10, 10), Random.Range(-10, 10));
            for (int i = 0; i < buttons.Count; i++)
            {
                buttonDirections.Add(buttonDirection);
            }
            choosingDirection = true;
        }
        foreach (GameObject button in buttons)
        {
            if (inContact == false)
            {
                button.GetComponent<GivingAnswer>().direction = buttonDirections[k].normalized;
                button.transform.position += buttonDirections[k].normalized * Time.deltaTime * buttonsSpeed[k];

            }
            if (k >= buttonDirections.Count - 1)
            {
                k = 0;
            }
            else
            {
                k++;
            }
        }        
    }

    [ServerCallback]
    private void SetDirectionAndSpeed()
    {
        if (choosingSpeed)
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                speed = Random.Range(2f, 5f);
                buttonsSpeed.Add(speed);
            }
            choosingSpeed = false;
        }

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
            if(inContact == false)
            {
                button.GetComponent<GivingAnswer>().direction = buttonDirections[k].normalized;
                button.transform.position += buttonDirections[k].normalized * Time.deltaTime * buttonsSpeed[k];
            }
            if (k >= buttonDirections.Count - 1)
            {
                k = 0;
            }
            else
            {
                k++;
            }
        }
    }
}