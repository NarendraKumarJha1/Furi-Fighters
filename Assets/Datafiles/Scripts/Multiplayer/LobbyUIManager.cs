using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Unity.Services.Lobbies.Models;

public class LobbyUIManager : MonoBehaviour
{
    //[SerializeField] private Button mainMenuButton;
    [SerializeField] private Button createLobbyButton;
    [SerializeField] private Button quickJoinButton;
    [SerializeField] private Button joinCodeButton;
    [SerializeField] private TMP_InputField joinCodeInputField;
    [SerializeField] private TMP_InputField playerNameInputField;
    [SerializeField] private LobbyCreateUI lobbyCreateUI;
    [SerializeField] private Transform lobbyContainer;
    [SerializeField] private Transform lobbyTemplate;


    private void Awake()
    {
       /* mainMenuButton.onClick.AddListener(() => {
            GameLobby.Instance.LeaveLobby();
            Loader.Load(Loader.Scene.MainMenuScene);
        });*/
        createLobbyButton.onClick.AddListener(() => {
            lobbyCreateUI.Show();
            //GameLobby.Instance.CreateLobby("Lobbyname", false);
        });
        quickJoinButton.onClick.AddListener(() => {
            GameLobby.Instance.QuickJoin();
        });
        joinCodeButton.onClick.AddListener(() => {
            GameLobby.Instance.JoinWithCode(joinCodeInputField.text);
        });

        //lobbyTemplate.gameObject.SetActive(false);
    }

    private void Start()
    {
        Debug.LogError($"{CustomNetworkManager.Instance.GetPlayerName()}");
        playerNameInputField.text = CustomNetworkManager.Instance.GetPlayerName();
        playerNameInputField.onValueChanged.AddListener((string newText) => {
            CustomNetworkManager.Instance.SetPlayerName(newText);
        });

        GameLobby.Instance.OnLobbyListChanged += KitchenGameLobby_OnLobbyListChanged;
        UpdateLobbyList(new List<Lobby>());
    }

    private void KitchenGameLobby_OnLobbyListChanged(object sender, GameLobby.OnLobbyListChangedEventArgs e)
    {
        UpdateLobbyList(e.lobbyList);
    }

    private void UpdateLobbyList(List<Lobby> lobbyList)
    {
        foreach (Transform child in lobbyContainer)
        {
            if (child == lobbyTemplate) continue;
            Destroy(child.gameObject);
        }

        foreach (Lobby lobby in lobbyList)
        {
            Transform lobbyTransform = Instantiate(lobbyTemplate, lobbyContainer);
            lobbyTransform.gameObject.SetActive(true);
            lobbyTransform.GetComponent<LobbyListSingleUI>().SetLobby(lobby);
        }
    }

    private void OnDestroy()
    {
        GameLobby.Instance.OnLobbyListChanged -= KitchenGameLobby_OnLobbyListChanged;
    }
}
