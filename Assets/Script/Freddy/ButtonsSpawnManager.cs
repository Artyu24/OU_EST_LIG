using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class ButtonsSpawnManager : NetworkBehaviour
{
    int c = 0;
    [SyncVar]
    private Sprite imgToClick;
    public Sprite GetSetImgToClick { get { return imgToClick; } set { imgToClick = value; } }

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
        GetSetImgToClick = roundData.spritesToFind[GameManager.instance.nbrRound];
        Debug.Log("Img to find : " + GetSetImgToClick);
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

    [Server]
    public void NewRound(int round, Sprite newSpriteToFound)
    {
        GameManager.instance.timeInGame = GameManager.instance.timeMaxPerRound;
        Debug.Log("Called : " + c);
        GetSetImgToClick = newSpriteToFound;
        Debug.Log(newSpriteToFound + " = " + GetSetImgToClick + " ?");
        rightButton.GetComponent<SpriteRenderer>().sprite = GetSetImgToClick;
        for (int i = 0; i < roundData.buttonsNumber[round] - 1; i++)
        {
            buttons.Add(wrongButton);
        }
        buttons.Add(rightButton);
        SpawnButtons();
        c++;
    }

    [Server]
    private void SpawnButtons()
    {
        int compteur = 0;
        for (int i = 0; i < roundData.buttonsNumber[GameManager.instance.nbrRound]; i++)
        {

            x = Random.Range(-8, 7);
            y = Random.Range(-4, 4);
            /*foreach (Vector2 position in buttonSpawns)
            {
                foreach (GameObject button in buttons)
                {
                    *//*while (x <= (position.x - button.GetComponent<Collider2D>().bounds.size.x / 2) || x >= (position.x + button.GetComponent<Collider2D>().bounds.size.x / 2) &&
                        y <= (position.y - button.GetComponent<Collider2D>().bounds.size.y / 2) || y >= (position.y + button.GetComponent<Collider2D>().bounds.size.y / 2))
                    {
                        x = Random.Range(-9, 9);
                        y = Random.Range(-5, 5);
                    }*//*
                }
            }*/
            Vector2 spawnButton = new Vector2(x, y);
            buttonSpawns.Add(spawnButton);
        }

        foreach (GameObject button in buttons)
        {
            Debug.Log("Next Spawn");
            GameObject spawnButton = (GameObject)Instantiate(button, buttonSpawns[compteur], Quaternion.identity);
            NetworkServer.Spawn(spawnButton);
            //isInstantiate = true;
            compteur++;
        }
    }

    [Server]
    public void ResetButtons()
    {
        foreach (GameObject button in buttons)
        {
            button.SetActive(false);
        }
        buttons.Clear();
    }
}
