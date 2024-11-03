using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class StartNetwrok : MonoBehaviour
{
    [SerializeField] private Button startHostButton;
    [SerializeField] private Button startServerButton;
    [SerializeField] private Button startClientButton;
    
    private void Awake()
    {
        startHostButton.onClick.AddListener(StartHost);
        startServerButton.onClick.AddListener(StartServer);
        startClientButton.onClick.AddListener(StartClient);
    }

    private void StartHost()
    {
        NetworkManager.Singleton.StartHost();
        DisableButtons();
    }
    
    private void StartServer()
    {
        NetworkManager.Singleton.StartServer();
        DisableButtons();
    }
    
    private void StartClient()
    {
        NetworkManager.Singleton.StartClient();
        DisableButtons();
    }

    private void DisableButtons()
    {
        startServerButton.gameObject.SetActive(false);
        startHostButton.gameObject.SetActive(false);
        startClientButton.gameObject.SetActive(false);
    }
}
