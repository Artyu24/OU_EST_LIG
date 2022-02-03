using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UserAccountManager : MonoBehaviour
{
    public static UserAccountManager instance;

    public static string LoggedInUsername;

    public string lobbySceneName = "Lobby";

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this);
    }

    public void LogIn(Text usernameText)
    {
        LoggedInUsername = usernameText.text;
        SceneManager.LoadScene(lobbySceneName);
    }
}
