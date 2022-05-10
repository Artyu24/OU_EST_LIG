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

    private List<GameObject> wrongButtonsList = new List<GameObject>();

    [SyncVar]
    private int nbrButtonsInList;

    [SyncVar]
    public int nbrRound = 0;

    private int previousNbrOfWrongButtons;
    public int randSprite;
    private bool isChanging = false;
    public float timeBetweenRound;

    private bool isSpriting = true;
    public int maxNbrRound;
    public int timeMaxPerRound;
    private int scoreToTakeOff;
    [SyncVar]
    public float timeInGame;
    public float GetTimeInGame { get { return timeInGame; } }

    private Sprite imgToClick;
    public Sprite GetSetImgToClick { get { return imgToClick; } set { imgToClick = value; } }

    [SyncVar]
    private bool isCoroutineOn = true; // reset to true for Arthur

    private bool isAdminHere = false;

    private Vector3 previousPosition;


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

        if (GameObject.FindGameObjectWithTag("WrongButton") && isSpriting == false && previousNbrOfWrongButtons != GameObject.FindGameObjectsWithTag("WrongButton").Length)
        {
            wrongButtonsList.AddRange(GameObject.FindGameObjectsWithTag("WrongButton"));
            previousNbrOfWrongButtons = wrongButtonsList.Count;
            //Debug.Log("Number of WrongButtons found and added to the List : " + GameObject.FindGameObjectsWithTag("WrongButton").Length);
            ChangeSpriteWrongButton();
            isSpriting = true;
        }

        if (nbrRound < maxNbrRound)
        {
            if (timeInGame <= 0 && isChanging == false)
            {
                StartCoroutine(Changes());
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

    IEnumerator Changes()
    {
        imgToFindUI.SetActive(true);
        // Display imgtoclick "full screen" before put it in the bottom right corner
        ChangeImgToClick();
        previousPosition = imgToFindUI.transform.position;
        imgToFindUI.transform.position = Vector3.zero;
        imgToFindUI.transform.localScale = new Vector3(3, 3, 3);
        isChanging = true;
        ResetRound();
        if(GameObject.FindGameObjectWithTag("UIPlayer") != null)
            GameObject.FindGameObjectWithTag("UIPlayer").GetComponent<PlayerUI>().isGetGoodAnswer = false;
        yield return new WaitForSeconds(timeBetweenRound);
        imgToFindUI.transform.position = previousPosition;
        imgToFindUI.transform.localScale = new Vector3(1, 1, 1);
        ButtonsManagement();
    }

    private void ResetRound()
    {
        ButtonsSpawnManager.instance.inContact = false;
        ButtonsSpawnManager.choosingDirection = false;
        ButtonsSpawnManager.randMod = UnityEngine.Random.Range(0, 3);
        ButtonsSpawnManager.choosingSpeed = true;
        if (ButtonsSpawnManager.instance.buttons.Count > 0)
        {
            ButtonsSpawnManager.instance.ResetButtons();
        }
        if (ButtonsSpawnManager.instance.buttonDirections.Count > 0)
        {
            ButtonsSpawnManager.instance.buttonDirections.Clear();
        }
        ScoreManager.instance.ResetScore();
    }

    private void ButtonsManagement()
    {
        //Debug.Log(ButtonsSpawnManager.instance.inContact);
        ButtonsSpawnManager.instance.SpawnButtons();
        isSpriting = false;
        nbrButtonsInList = ButtonsSpawnManager.instance.buttons.Count;
        timeInGame = timeMaxPerRound;
        //ChangeImgToClick();
        //Debug.Log("New img to find : " + GetSetImgToClick);
        ChangerRound();
        Debug.Log("round suivant");
        isChanging = false;
    }

    [ServerCallback]
    private void ChangerRound()
    {
        nbrRound++;
    }

    [ClientCallback]
    private void ChangeSpriteWrongButton()
    {
        Debug.Log("Number of wrongButton in the List : " + wrongButtonsList.Count);
        foreach (GameObject wrongButton in wrongButtonsList)
        {
            randSprite = UnityEngine.Random.Range(0, maxNbrRound);
            while (ButtonsSpawnManager.instance.roundData.spritesToFind[randSprite] == GetSetImgToClick) 
            {
                randSprite = UnityEngine.Random.Range(0, maxNbrRound);
            }
            //Debug.Log("Rand int for Sprites : " + randSprite + "//// Image a avoir " + GetSetImgToClick);
            wrongButton.GetComponent<SpriteRenderer>().sprite = ButtonsSpawnManager.instance.roundData.spritesToFind[randSprite];
        }
        wrongButtonsList.Clear();
    }

    private void ChangeImgToClick()
    {
        GetSetImgToClick = ButtonsSpawnManager.instance.roundData.spritesToFind[nbrRound];
        imgToFindUI.GetComponent<SpriteRenderer>().sprite = GetSetImgToClick;        
        //Debug.Log("Sprite GETSET : " + GetSetImgToClick);
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
        ScoreManager.instance.ScoreDecrease(scoreToTakeOff);
        isCoroutineOn = false;
    }

    [Server]
    private IEnumerator StartGame()
    {
        yield return new WaitForSeconds(0f);
        Debug.Log("Le Serveur à lancer la partie");
        isCoroutineOn = false;
    }


    public void TEST()
    {
        Debug.Log("JE LANCE LE JEU");
        isCoroutineOn = false;
    }
}