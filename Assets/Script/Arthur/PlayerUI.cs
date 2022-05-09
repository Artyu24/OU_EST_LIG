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
    [SerializeField] private GameObject wrongOneBox, goodOneBox;
    [SerializeField] private Text timerText;

    public bool isGetGoodAnswer;

    public float delay = 3;

    public void SetDataPlayer(DataPlayer _dataPlayer)
    {
        dataPlayer = _dataPlayer;
    }

    private void Update()
    {
        if(isGetGoodAnswer)
            goodOneBox.SetActive(true);
        else
            goodOneBox.SetActive(false);
            

        if (delay < 2)
        {
            delay += Time.deltaTime;
            wrongOneBox.SetActive(true);
        }
        else
        {
            delay = 3;
            wrongOneBox.SetActive(false);
        }

        timerText.text = ((int)GameManager.instance.timeInGame).ToString();

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
