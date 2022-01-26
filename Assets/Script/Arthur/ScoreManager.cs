using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScoreManager : NetworkBehaviour
{
    public static ScoreManager instance;

    [SerializeField] private int maxScore;
    public int GetMaxScore { get { return maxScore; } }

    [SyncVar]
    private int actualScore;
    public int GetActualScore { get { return actualScore; } }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            return;
        }
    }

    private void Start()
    {
        ResetScore();
    }

    [Server]
    public void ScoreDecrease(int scoreToTakeOff)
    {
        actualScore -= scoreToTakeOff;
        //Debug.Log(actualScore);
    }

    [Client]
    public void AddScorePlayer()
    {
        GameObject localPlayer = NetworkClient.localPlayer.gameObject;

        CmdUpdateScore(GameManager.GetPlayer(localPlayer.name));
    }

    [Command(requiresAuthority = false)]
    public void CmdUpdateScore(DataPlayer player)
    {
        player.AddScore(actualScore);
    }


    public void ResetScore()
    {
        actualScore = maxScore;
    }
}
