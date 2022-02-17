using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class ButtonsSpawnManager : NetworkBehaviour
{
    [SyncVar]
    private Sprite imgToClick;
    public Sprite GetSetImgToClick { get { return imgToClick; } set { imgToClick = value; } }

    public List<GameObject> buttons = new List<GameObject>();
    private List<Vector2> buttonSpawns = new List<Vector2>();
    public int round = 0;
    public RoundData roundData;
    public GameObject wrongButton;
    public GameObject rightButton;
    private bool isInstantiate;
    public float tpX, tpY;
    private float timer = 3f;
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
        GetSetImgToClick = roundData.spritesToFind[round];
        Debug.Log("Img to find : " + imgToClick);
        isInstantiate = false;
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            switch (round)
            {
                case 0:
                    rightButton.GetComponent<SpriteRenderer>().sprite = GetSetImgToClick;
                    for (int i = 0; i < roundData.buttonsNumber[round] - 1; i++)
                    {
                        buttons.Add(wrongButton);
                    }
                    buttons.Add(rightButton);
                    SpawnButtons();
                    break;
                default:
                    break;
            }
        }
        

        if (isInstantiate)
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
        }
    }
    
    [ServerCallback]
    private void SpawnButtons()
    {
        int compteur = 0;
        for(int i = 0; i < roundData.buttonsNumber[round]; i++)
        {
            
            x = Random.Range(-8, 7);
            y = Random.Range(-4, 4);
            /*foreach(Vector2 position in buttonSpawns)
            {
                foreach(GameObject button in buttons)
                {
                    while (x <= (position.x - button.GetComponent<Collider2D>().bounds.size.x / 2) || x >= (position.x + button.GetComponent<Collider2D>().bounds.size.x / 2) &&
                        y <= (position.y - button.GetComponent<Collider2D>().bounds.size.y / 2) || y >= (position.y + button.GetComponent<Collider2D>().bounds.size.y / 2))
                    {
                        x = Random.Range(-9, 9);
                        y = Random.Range(-5, 5);
                    }
                }                
            }*/
            Vector2 spawnButton = new Vector2(x, y);
            buttonSpawns.Add(spawnButton);
        }

        foreach(GameObject button in buttons)
        {
            GameObject spawnButton = (GameObject)Instantiate(button, buttonSpawns[compteur], Quaternion.identity);
            NetworkServer.Spawn(spawnButton);
            isInstantiate = true;
            compteur++;
        }
        round = -1;
    }

    public void NextRound()
    {
        GetSetImgToClick = roundData.spritesToFind[round];
    }
}
