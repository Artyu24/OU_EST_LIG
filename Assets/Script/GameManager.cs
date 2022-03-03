using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using System.Linq;
using Mirror;
using Mirror.Examples.Basic;

public class GameManager : NetworkBehaviour
{
    private static Dictionary<string, DataPlayer> players = new Dictionary<string, DataPlayer>();

    public static GameManager instance;
    public GameObject imgToFindUI;

    public const string playerIdPrefix = "Player_";

    public int nbrRound = 0;

    public int maxNbrRound;
    public int timeMaxPerRound;
    private int scoreToTakeOff;
    public float timeInGame;
    //public float GetTimeInGame { get { return timeInGame; } }

    private Sprite imgToClick;
    public Sprite GetSetImgToClick { get { return imgToClick; } set { imgToClick = value; } }

    [SyncVar]
    private bool isCoroutineOn = false; // reset to true for Arthur

    private bool isAdminHere = false;


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
        GetSetImgToClick = ButtonsSpawnManager.instance.roundData.spritesToFind[nbrRound];
        ButtonsSpawnManager.instance.rightButton.GetComponent<SpriteRenderer>().sprite = GetSetImgToClick;
        imgToFindUI.GetComponent<SpriteRenderer>().sprite = GetSetImgToClick;

        timeInGame = timeMaxPerRound;
        scoreToTakeOff = (ScoreManager.instance.GetMaxScore * 3 / 4) / (timeMaxPerRound * 2);
    }

    private void Update()
    {
        if (!isAdminHere)
        {
            foreach (DataPlayer player in GetAllPlayers())
            {
                if (player.username == "Admin")
                {
                    isAdminHere = true;
                    if(StartGame() != null)
                        StartCoroutine(StartGame());
                    return;
                }
            }
        }

        if (!isCoroutineOn && TimerActif() != null)
        {
            isCoroutineOn = true;
            StartCoroutine(TimerActif());
        }

        if (nbrRound <= maxNbrRound)
        {
            if (timeInGame <= 0)
            {
                ButtonsSpawnManager.choosingDirection = false;
                if (ButtonsSpawnManager.instance.buttons.Count > 0)
                {
                    ButtonsSpawnManager.instance.ResetButtons();
                }
                imgToFindUI.SetActive(true);
                GetSetImgToClick = ButtonsSpawnManager.instance.roundData.spritesToFind[nbrRound];
                Debug.Log("Img to find : " + GetSetImgToClick);
                imgToFindUI.GetComponent<SpriteRenderer>().sprite = GetSetImgToClick;

                ButtonsSpawnManager.instance.rightButton.GetComponent<SpriteRenderer>().sprite = GetSetImgToClick;

                ButtonsSpawnManager.instance.SpawnButtons();
                timeInGame = timeMaxPerRound;
                nbrRound++;
                ScoreManager.instance.ResetScore();
                Debug.Log("round suivant");
            }
        }
        else
        {
            Debug.Log("FIN DE LA PARTIE");
            StopAllCoroutines();
            isCoroutineOn = true;
        }

        //PUR DEBUG
        if (Input.GetKeyDown(KeyCode.K))
        {
            foreach (DataPlayer player in GetAllPlayers())
            {
                Debug.Log(player.name + " : " + player.GetActualScore);
            }
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

    [ServerCallback]
    private IEnumerator TimerActif()
    {
        //isCoroutineOn = true;
        yield return new WaitForSeconds(.5f);
        DecreaseTime();
        ScoreManager.instance.ScoreDecrease(scoreToTakeOff);
        isCoroutineOn = false;
    }

    [ClientRpc]
    private void DecreaseTime()
    {
        timeInGame -= .5f;
    }

    [Server]
    private IEnumerator StartGame()
    {
        Debug.Log("La partie démarre dans 5 sec !");
        yield return new WaitForSeconds(5f);
        Debug.Log("Le Serveur à lancer la partie");
        isCoroutineOn = false;
    }


    public void TEST()
    {
        Debug.Log("JE LANCE LE JEU");
        isCoroutineOn = false;
    }

}