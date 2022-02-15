using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class ButtonsSpawnManager : NetworkBehaviour
{
    public List<GameObject> buttons = new List<GameObject>();
    private List<Vector2> buttonSpawns = new List<Vector2>();
    public static Sprite imgToClick;
    public int round = 0;
    float x, y;
    public RoundData roundData;
    public GameObject wrongButton;
    public GameObject rightButton;
    //private RectTransform rtWrongButton, rtRightButton;
    private bool isInstantiate;
    public float tpX, tpY;

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
        imgToClick = roundData.spritesToFind[round];
        Debug.Log("Img to find : " + imgToClick);
        isInstantiate = false;
    }

    private void Update()
    {
        switch (round)
        {
            case 0:
                rightButton.GetComponent<SpriteRenderer>().sprite = imgToClick;

                /*rtWrongButton.anchorMin = new Vector2(0.5f, 0.5f);
                rtWrongButton.anchorMax = new Vector2(0.5f, 0.5f);
                rtRightButton.anchorMin = new Vector2(0.5f, 0.5f);
                rtRightButton.anchorMax = new Vector2(0.5f, 0.5f);

                rtWrongButton.pivot = new Vector2(0.5f, 0.5f);
                rtRightButton.pivot = new Vector2(0.5f, 0.5f);*/
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

        if (isInstantiate)
        {
            foreach (GameObject button in buttons)
            {
                //Debug.Log(buttons[0].GetComponent<RectTransform>().anchoredPosition);
                //Debug.Log(button.transform.position);
                //button.GetComponent<Rigidbody2D>().velocity += Vector2.right * Time.deltaTime;
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
    
    private void SpawnButtons()
    {
        int compteur = 0;
        for(int i = 0; i < roundData.buttonsNumber[round]; i++)
        {
            
            x = Random.Range(0, 1920);
            y = Random.Range(0, 1080);
            /*foreach(Vector2 position in buttonSpawns)
            {
                foreach(GameObject button in buttons)
                {
                    while (x <= (position.x - button.GetComponent<RectTransform>().rect.width / 2) && x >= (position.x + button.GetComponent<RectTransform>().rect.width / 2) &&
                        y <= (position.y - button.GetComponent<RectTransform>().rect.height / 2) && y >= (position.y + button.GetComponent<RectTransform>().rect.height / 2))
                    {
                        x = Random.Range(0, 1920);
                        y = Random.Range(0, 1080);
                    }
                }                
            }*/
            Vector2 spawnButton = new Vector2(x, y);
            buttonSpawns.Add(spawnButton);
        }

        foreach(GameObject button in buttons)
        {
            Instantiate(button, buttonSpawns[compteur], Quaternion.identity);
            isInstantiate = true;
            compteur++;
        }
        round = -1;
    }

    public void NextRound()
    {
        imgToClick = roundData.spritesToFind[round];
    }
}
