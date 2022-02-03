using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Text scoreText;

    private DataPlayer dataPlayer;

    [SerializeField] private GameObject scoreBoard;

    public void SetDataPlayer(DataPlayer _dataPlayer)
    {
        dataPlayer = _dataPlayer;
    }

    private void Update()
    {
        UpdateScore(dataPlayer.GetActualScore);

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            scoreBoard.SetActive(true);
        }
        else if(Input.GetKeyUp(KeyCode.Tab))
        {
            scoreBoard.SetActive(false);
        }
    }

    public void UpdateScore(int score)
    {
        scoreText.text = score.ToString();
    }
}
