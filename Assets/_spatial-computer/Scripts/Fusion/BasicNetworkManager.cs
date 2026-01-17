using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Fusion;
using Fusion.Sockets;
using System;

public class BasicNetworkManager : MonoBehaviour
{
    public static BasicNetworkManager Instance;

    public DrawingManager LocalDrawingManager;

    public GameObject WhiteboardObject;   // Assign the cube object in the Inspector
    public GameObject linePrefab;    // Prefab that has a LineRenderer component

    public int RemotePlayerCount;

    private NetworkRunner _runner;
    private NetworkSceneManagerDefault SceneManager;
    private Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();


    [SerializeField] private NetworkPrefabRef _playerPrefab;
    //[SerializeField] private NetworkPrefabRef _PCPrefab;
    //[SerializeField] private NetworkPrefabRef _QuestPrefab;

    public NetworkObject LocalPlayer;

    bool isSpawned = false;

    [HideInInspector]
    public GameObject RemotePlayer;

    //public GameObject SelfSenders;
    //public GameObject EnvSenders;

    //public GameObject Annotation2D;
    //public GameObject Annotation3D;

    //public RawImage RemoteView;

    //public GameObject RemotePlayerFrustum;

    public GameObject EnvObjects;

    public Toggle RecordToggle;


    public bool IsVideo = false;
    public bool IsRemote = false;


    private Vector3 velocity = Vector3.zero;

    async void StartGame(GameMode mode)
    {
        // Create the Fusion runner and let it know that we will be providing user input
        _runner = gameObject.AddComponent<NetworkRunner>();
        _runner.ProvideInput = true;

        // Start or join (depends on gamemode) a session with a specific name
        await _runner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = "spatialcomputer",
            // Scene = SceneManager.GetActiveScene().buildIndex,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });
    }

    private void Awake()
    {
        if (Instance is null)
            Instance = this;
        else if (Instance != this)
            Destroy(this);
    }

    private void Start()
    {
        RemotePlayerCount = 0;
        isSpawned = false;
        IsVideo = false;
        IsRemote = false;
        StartGame(GameMode.Shared);

        // for (int i = 0; i < EnvObjects.transform.childCount; i++)
        // {
        //     EnvObjects.transform.GetChild(i).gameObject.SetActive(false);
        // }

    }
  
    public void SetModel(int i, bool state)
    {
        EnvObjects.transform.GetChild(i).gameObject.SetActive(state);
    }

    public void SetEnvRender(bool state)
    {
        for (int i = 0; i < EnvObjects.transform.childCount; i++)
        {
            EnvObjects.transform.GetChild(i).gameObject.SetActive(state);
        }
    }
    public void SetColor(Color color)
    {
        LocalPlayer.gameObject.GetComponent<NewFusionPlayer>().RPC_SetColor(color);
    }

    public void ClearLines()
    {
        LocalPlayer.gameObject.GetComponent<NewFusionPlayer>().RPC_ClearLine();
    }


    // public void SendModelMessage0(bool state)
    // {
    //     LocalPlayer.gameObject.GetComponent<NewFusionPlayer>().RPC_SetModel0(state);
    // }

    // public void SendModelMessage1(bool state)
    // {
    //     LocalPlayer.gameObject.GetComponent<NewFusionPlayer>().RPC_SetModel1(state);
    // }

    // public void SendModelMessage2(bool state)
    // {
    //     LocalPlayer.gameObject.GetComponent<NewFusionPlayer>().RPC_SetModel2(state);
    // }

    // public void SendModelMessage3(bool state)
    // {
    //     LocalPlayer.gameObject.GetComponent<NewFusionPlayer>().RPC_SetModel3(state);
    // }

    // public void SendModelMessage4(bool state)
    // {
    //     LocalPlayer.gameObject.GetComponent<NewFusionPlayer>().RPC_SetModel4(state);
    // }

    // public void SendModelMessage5(bool state)
    // {
    //     LocalPlayer.gameObject.GetComponent<NewFusionPlayer>().RPC_SetModel5(state);
    // }




    // UI

    //public void Toggle2DAnnotation(bool value)
    //{
    //    Annotation2D.SetActive(value);
    //}

    //public void Toggle3DAnnotation(bool value)
    //{
    //    Annotation3D.SetActive(value);
    //}

    //public void ChangeAvatar(bool value)
    //{
    //    if(value)
    //    {
    //        for(int i = 0; i<SelfSenders.transform.childCount; i++)
    //        {
    //            if(SelfSenders.transform.GetChild(i).GetComponent<Toggle>().isOn)
    //            {
    //                LocalPlayer.gameObject.GetComponent<AvatarQuadHandler>().RPC_ChangeAvatar(i);
    //            }
    //        }


    //    }
    //}

    //public void ChangeEnv(bool value)
    //{
    //    if (value)
    //    {
    //        for (int i = 0; i < 4; i++)
    //        {
    //            if (EnvSenders.transform.GetChild(i).GetComponent<Toggle>().isOn)
    //            {
    //                LocalPlayer.gameObject.GetComponent<AvatarQuadHandler>().RPC_ChangeEnv(i);
    //            }
    //        }


    //    }
    //}

    //


    #region FusionCallbacks
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {

            Debug.Log("Spawn");


        if (runner.LocalPlayer == player)
        {

            LocalPlayer = runner.Spawn(_playerPrefab, Vector3.zero, Quaternion.identity, player);
            Debug.Log("Spawn");
            //if (BasicDeviceManager.Instance.Mode == BasicDeviceManager.DeviceModes.Android)
            //{
            //    LocalPlayer = runner.Spawn(_playerPrefab, Vector3.zero, Quaternion.identity, player);

            //}
            //else
            //{
            //    LocalPlayer = runner.Spawn(_PCPrefab, Vector3.zero, Quaternion.identity, player);
            //}
            //
            
            LocalPlayer.transform.localPosition = Vector3.zero;
            LocalPlayer.transform.localRotation = Quaternion.identity;
            isSpawned = true;
            //PlayerCount++;
        }
        else
        {
            RemotePlayerCount++;
        }




    }
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {

        RemotePlayerCount--;
    }


    // public void OnInput(NetworkRunner runner, NetworkInput input)
    // {
    //     NetworkInputData networkInput = PrepareNetworkInput();
    //     input.Set(networkInput);
    // }

    // protected virtual NetworkInputData PrepareNetworkInput()
    // {
    //     NetworkInputData input = new NetworkInputData();

    //     input.relativePosition = DeviceManager.Instance.relativePosition;
    //     input.relativeRotation = DeviceManager.Instance.relativeRotation;



    //     if (DeviceManager.Instance.Camera.transform.childCount == 1)
    //     {
    //         GameObject LocalPlayer = DeviceManager.Instance.Camera.transform.GetChild(0).gameObject;
    //         //input.isPointerActive = LocalPlayer.GetComponent<BasicPlayer>().isPointerActive;
    //         //input.pointerPosition = LocalPlayer.GetComponent<BasicPlayer>().pointerPosition;
    //     }

    //     return input;
    // }


    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    public void OnConnectedToServer(NetworkRunner runner) { }
    public void OnDisconnectedFromServer(NetworkRunner runner) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }

    #endregion



}
