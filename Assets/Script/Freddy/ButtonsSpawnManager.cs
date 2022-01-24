using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsSpawnManager : MonoBehaviour
{
    public List<Button> buttons = new List<Button>();
    public Sprite imgToClick;
    int round = 1;
    private RoundData roundData;
    public Button wrongButton;
    public Button rightButton;
    public Canvas canvas;

    private void Update()
    {
        switch (round)
        {
            case 1:
                for(int i = 0; i < 50; i++)
                {
                    buttons.Add(wrongButton);
                }
                buttons.Add(rightButton);
                SpawnButtons();
                round = -1;
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

    public void OnClick()
    {
        foreach (Button button in buttons)
        {
            if (button.image.sprite == imgToClick)
            {
                Debug.Log("Nice one !");
                break;
            }
            else
            {
                Debug.Log("Nul !");
            }
        }
    }
}
