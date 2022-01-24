using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsSpawnManager : MonoBehaviour
{
    public List<Button> buttons = new List<Button>();
    public Sprite imgToClick;

    public void OnClick()
    {
        foreach (Button button in buttons)
        {
            if (button.image.sprite == imgToClick)
            {
                Debug.Log("Nice one !");
                break;
            }
        }
    }
}
