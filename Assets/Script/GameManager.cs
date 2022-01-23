using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    private static Dictionary<string, DataPlayer> players = new Dictionary<string, DataPlayer>();

    public static GameManager instance;

    private const string playerIdPrefix = "Player_";

    private int nbrRound = 1;
    public int maxNbrRound;
    public int timeMaxPerRound;

    [SerializeField] private float timeInGame;
    public float GetTimeInGame { get { return timeInGame; } }

    private bool isCoroutineOn = false;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            return;
        }
    }

    private void Update()
    {
        if (!isCoroutineOn)
            TimerActif();

        if (nbrRound > maxNbrRound)
        {
            if (timeInGame <= 0)
            {
                nbrRound++;
                timeInGame = timeMaxPerRound;
            }
        }
        else
        {
            //Fin de partie
        }
    }

    public static void RegisterPlayer(string netID, DataPlayer player)
    {
        string playerId = playerIdPrefix + netID;
        players.Add(playerId, player);
        player.transform.name = playerId;
    }

    public static void UnregisterPlayer(string playerId)
    {
        players.Remove(playerId);
    }

    public static DataPlayer GetPlayer(string playerId)
    {
        return players[playerId];
    }

    public static DataPlayer[] GetAllPlayers()
    {
        return players.Values.ToArray();
    }

    private IEnumerator TimerActif()
    {
        isCoroutineOn = true;
        yield return new WaitForSeconds(.5f);
        timeInGame -= .5f;
        isCoroutineOn = false;
    }

}