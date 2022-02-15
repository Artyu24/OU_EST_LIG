using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsSpawnManager : MonoBehaviour
{
    public List<Button> buttons = new List<Button>();
    public static Sprite imgToClick;
    int round = 1;
    public RoundData roundData;
    public Button wrongButton;
    public Button rightButton;
    public Canvas canvas;

    private void Start()
    {
        imgToClick = roundData.spritesToFind[0];
        Debug.Log("Img to find : " + imgToClick);
    }
    private void Update()
    {
        switch (round)
        {
            case 1:
                rightButton.image.sprite = imgToClick;
                Debug.Log(imgToClick);
                for (int i = 0; i < roundData.buttonsNumber[0] - 1; i++)
                {
                    buttons.Add(wrongButton);
                }
                buttons.Add(rightButton);
                SpawnButtons();
                round++;
                break;
            default:
                break;
        }
    }

    private void SpawnButtons()
    {
        Vector2 spawnButtons = new Vector2(50, 100);
        foreach(Button button in buttons)
        {
            Instantiate(button, spawnButtons, Quaternion.identity, canvas.transform);
        }
    }

    public void OnClick(Button buttonClicked)
    {
        Debug.Log("Img to find : " + imgToClick);
        if (buttonClicked.image.sprite == imgToClick)
        {
            //Add score to the player
            Debug.Log("Found!");
        } else
        {
            Debug.Log("Img clicked : " + buttonClicked.image.sprite);
            //Lose total score
            Debug.Log("Wrong one!");
        }
    }
}
