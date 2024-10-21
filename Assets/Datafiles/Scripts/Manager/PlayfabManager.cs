using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class PlayfabManager : MonoBehaviour
{
    public TMP_InputField usernameInputField;
    public GameObject _loginPanel;
    public string _username;

    private void Start()
    {
        _username = PlayerPrefs.GetString("username");

        if (!string.IsNullOrEmpty(_username))
        {
            Debug.Log($"_username {_username}");
            Login();
        }
    }

    public void Login(string val = null)
    {
        var request = new LoginWithCustomIDRequest {
            /*CustomId = SystemInfo.deviceUniqueIdentifier,*/
            CustomId = string.IsNullOrEmpty(val) ? usernameInputField.text : _username,
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnSuccess, OnError);
    }

    void OnSuccess(LoginResult result)
    {
        if (result.NewlyCreated)
        {
            // Newly created account, let's set the username
            string username = usernameInputField.text;
            if (!string.IsNullOrEmpty(username))
            {
                SetPlayerUsername(username);  // Set the username
                _loginPanel.SetActive(false);
                PlayerPrefs.SetString("username", username);
            }
        }
        else
        {
            Debug.Log("Returning user.");
            // Optionally, handle returning users or display their username
        }
    }
    private void SetPlayerUsername(string username)
    {
        var request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = username // This is the username for the player
        };

        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnUsernameSetSuccess, OnUsernameSetError);
    }
    private void OnUsernameSetSuccess(UpdateUserTitleDisplayNameResult result)
    {
        Debug.Log("Username successfully set: " + result.DisplayName);
    }

    // Error handler for setting username (e.g., duplicate username)
    private void OnUsernameSetError(PlayFabError error)
    {
        Debug.LogError("Failed to set username: " + error.GenerateErrorReport());
    }


    void OnError(PlayFabError error)
    {
        Debug.Log("Failed");
        Debug.LogError(error.GenerateErrorReport());

    }
}
