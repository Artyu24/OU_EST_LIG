using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScoreboardItem : MonoBehaviour
{
    [SerializeField] private Text usernameText;

    [SerializeField] private Text scoreText;

    private DataPlayer playerStock;

    public void Setup(DataPlayer player)
    {
        usernameText.text = player.username;
        scoreText.text = player.GetActualScore.ToString();
        playerStock = player;
    }

    private void Update()
    {
        scoreText.text = playerStock.GetActualScore.ToString();
    }
}
