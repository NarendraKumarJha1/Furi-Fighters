using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.MultiplayerModels;
using TMPro;


public class PlayfabMatchmaking : MonoBehaviour
{

    public string queueName = "test_queue";
    public string lobbyName = "MyLobby";

    public GameObject _lobbyInputField;
    public GameObject _lobbyDisplayHolder;
    public TMP_Text _lobbyDisplaytxt;
    void Start()
    {

    }

    #region Create Lobby

    public void CreateLobby()
    {
        var request = new CreateLobbyRequest
        {
            Owner = new EntityKey
            {
                Id = PlayFabSettings.staticPlayer.EntityId,
                Type = PlayFabSettings.staticPlayer.EntityType
            },
            MaxPlayers = 10,
            AccessPolicy = AccessPolicy.Public,
            CustomTags = new Dictionary<string, string>
            {
                {"LobbyName", lobbyName}
            },
            Members = new List<Member>
            {
                new Member
                {
                    MemberEntity = new EntityKey
                    {
                        Id= PlayFabSettings.staticPlayer.EntityId,
                        Type= PlayFabSettings.staticPlayer.EntityType   
                    }
                }
            }
        };
        PlayFabMultiplayerAPI.CreateLobby(request, OnLobbyCreated, OnLobbyError);
    }
    void OnLobbyCreated(CreateLobbyResult result)
    {
        Debug.Log("Lobby created successfully! LobbyId: " + result.ConnectionString);
        _lobbyInputField.SetActive(false);
        _lobbyDisplayHolder.SetActive(true);
        _lobbyDisplaytxt.text = result.LobbyId;
    }

    void OnLobbyError(PlayFabError error)
    {
        Debug.LogError("Error creating lobby: " + error.GenerateErrorReport());
    }
    #endregion

    #region JoinLobby

    public void JoinLobby(TMP_InputField _lobbyInputField)
    {
        string lobbyId = _lobbyInputField.text;
        Debug.LogError($" Lobby Id {_lobbyInputField.text}");
        var request = new JoinLobbyRequest
        {
            ConnectionString = lobbyId,
            MemberEntity = new EntityKey
            {
                Id = PlayFabSettings.staticPlayer.EntityId,
                Type = PlayFabSettings.staticPlayer.EntityType
            }
        };

        PlayFabMultiplayerAPI.JoinLobby(request, OnLobbyJoined, OnLobbyError);
    }
    void OnLobbyJoined(JoinLobbyResult result)
    {
        Debug.Log("Joined lobby: " + result.LobbyId);
    }
    #endregion

    #region MatchMaking

    private void StartMatchMaking()
    {
        var request = new CreateMatchmakingTicketRequest 
        {
            QueueName = queueName,
            Creator = new MatchmakingPlayer
            {
                Entity = new EntityKey
                {
                    Id = PlayFabSettings.staticPlayer.EntityId,
                    Type = PlayFabSettings.staticPlayer.EntityType
                },
                Attributes = new MatchmakingPlayerAttributes
                {
                    DataObject = new {skill = 50}
                }
            }
        };

        PlayFabMultiplayerAPI.CreateMatchmakingTicket(request, OnMatchmakingSuccess, OnMatchmakingError);
    }

    void OnMatchmakingSuccess(CreateMatchmakingTicketResult result)
    {
        Debug.Log("Matchmaking ticket created: " + result.TicketId);
        PollMatchmakingTicket(result.TicketId);
    }

    void PollMatchmakingTicket(string ticketId)
    {
        InvokeRepeating(nameof(CheckMatchmakingStatus), 2.0f, 5.0f);
    }
    void CheckMatchmakingStatus(string ticketId)
    {
        var request = new GetMatchmakingTicketRequest
        {
            TicketId = ticketId,
            QueueName = queueName
        };

        PlayFabMultiplayerAPI.GetMatchmakingTicket(request, OnCheckMatchmakingStatusSuccess, OnMatchmakingError);
    }

    void OnCheckMatchmakingStatusSuccess(GetMatchmakingTicketResult result)
    {
        if (result.Status == "Matched")
        {
            Debug.Log("Match found! MatchId: " + result.MatchId);
            // You can now proceed to connect to the game server
        }
        else if (result.Status == "WaitingForMatch")
        {
            Debug.Log("Still waiting for a match...");
        }
    }

    void OnMatchmakingError(PlayFabError error)
    {
        Debug.LogError("Error during matchmaking: " + error.GenerateErrorReport());
    }
    #endregion
}
