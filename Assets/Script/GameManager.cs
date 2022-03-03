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

    [SyncVar]
    public int nbrRound = 0;

    public int maxNbrRound;
    public int timeMaxPerRound;
    private int scoreToTakeOff;
    [SyncVar]
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

        if (GameObject.FindGameObjectWithTag("RightButton"))
        {
            GameObject.FindGameObjectWithTag("RightButton").GetComponent<SpriteRenderer>().sprite = GetSetImgToClick;
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
                if(ButtonsSpawnManager.instance.buttonDirections.Count > 0)
                {
                    ButtonsSpawnManager.instance.buttonDirections.Clear();
                }
                imgToFindUI.SetActive(true);
                ButtonsSpawnManager.instance.SpawnButtons();
                ChangeImgToClick();
                Debug.Log("New img to find : " + GetSetImgToClick);
                timeInGame = timeMaxPerRound;
                ChangerRound();
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

    [ServerCallback]
    private void ChangerRound()
    {
        nbrRound++;
    }

    private void ChangeImgToClick()
    {
        GetSetImgToClick = ButtonsSpawnManager.instance.roundData.spritesToFind[nbrRound];
        imgToFindUI.GetComponent<SpriteRenderer>().sprite = GetSetImgToClick;
        Debug.Log("Sprite GETSET : " + GetSetImgToClick);
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
        timeInGame -= .5f;
        DecreaseTime();
        ScoreManager.instance.ScoreDecrease(scoreToTakeOff);
        isCoroutineOn = false;
    }

    [ClientRpc]
    private void DecreaseTime()
    {
    }

    [Server]
    private IEnumerator StartGame()
    {
        Debug.Log("La partie d�marre dans 5 sec !");
        yield return new WaitForSeconds(5f);
        Debug.Log("Le Serveur � lancer la partie");
        isCoroutineOn = false;
    }


    public void TEST()
    {
        Debug.Log("JE LANCE LE JEU");
        isCoroutineOn = false;
    }

}