using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBoard : MonoBehaviour
{
    [SerializeField] private GameObject playerScoreboardItem;

    [SerializeField] private Transform playerScoreboardList;

    private List<DataPlayer> listPlayerPrint = new List<DataPlayer>();

    private bool isActive = false;

    private void OnEnable()
    {
        isActive = true;

        DataPlayer[] dataPlayers = GameManager.GetAllPlayers();

        while (listPlayerPrint.Count < dataPlayers.Length)
        {
            DataPlayer playerChoose = null;
            int bestScore = 0;

            foreach (DataPlayer player in dataPlayers)
            {
                if (!listPlayerPrint.Contains(player) && bestScore <= player.GetActualScore)
                {
                    bestScore = player.GetActualScore;
                    playerChoose = player;
                }
            }
            GameObject itemGO = Instantiate(playerScoreboardItem, playerScoreboardList);
            listPlayerPrint.Add(playerChoose);
            PlayerScoreboardItem item = itemGO.GetComponent<PlayerScoreboardItem>();
            if (item != null)
            {
                item.Setup(playerChoose);
            }
        }
    }

    private void Update()
    {
        if (isActive)
        {
            
            foreach (Transform child in playerScoreboardList)
            {
                Destroy(child.gameObject);
            }

            listPlayerPrint.Clear();

            DataPlayer[] dataPlayers = GameManager.GetAllPlayers();

            while (listPlayerPrint.Count < dataPlayers.Length)
            {
                DataPlayer playerChoose = null;
                int bestScore = 0;

                foreach (DataPlayer player in dataPlayers)
                {
                    if (!listPlayerPrint.Contains(player) && bestScore <= player.GetActualScore)
                    {
                        bestScore = player.GetActualScore;
                        playerChoose = player;
                    }
                }
                GameObject itemGO = Instantiate(playerScoreboardItem, playerScoreboardList);
                listPlayerPrint.Add(playerChoose);
                playerChoose.Classement = listPlayerPrint.Count;
                PlayerScoreboardItem item = itemGO.GetComponent<PlayerScoreboardItem>();
                if (item != null)
                {
                    item.Setup(playerChoose);
                }
            }
        }
    }

    private void OnDisable()
    {
        isActive = false;

        foreach (Transform child in playerScoreboardList)
        {
            Destroy(child.gameObject);
        }

        listPlayerPrint.Clear();
    }
}
