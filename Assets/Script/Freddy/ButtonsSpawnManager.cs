using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsSpawnManager : MonoBehaviour
{
    public List<Button> buttons = new List<Button>();
    private Sprite imgToClick;
    int round = 1;
    public RoundData roundData;
    public Button wrongButton;
    public Button rightButton;
    public Canvas canvas;

    private void Update()
    {
        switch (round)
        {
            case 1:
                imgToClick = roundData.spritesToFind[0];
                rightButton.image.sprite = imgToClick;
                Debug.Log(imgToClick);
                for (int i = 0; i < roundData.buttonsNumber[0]; i++)
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
        Debug.Log(buttonClicked.image.sprite);
        if (buttonClicked.image.sprite == imgToClick)
        {
            //Add score to the player
            Debug.Log("Found!");
        } else
        {
            //Lose total score
            Debug.Log("Wrong one!");
        }
    }
}
