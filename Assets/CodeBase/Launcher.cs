using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
        [SerializeField] private byte _maxPlayersPerRoom = 4;
        [Tooltip("The Ui Panel to let the user enter name, connect and play")]
        [SerializeField] private GameObject _controlPanel;
        [Tooltip("The UI Label to inform the user that the connection is in progress")]
        [SerializeField] private GameObject _progressLabel;

        [Header("Button")] 
        [SerializeField] private Button _buttonPlay;
        
        private readonly string _gameVersion = "1";

        private bool _isConnecting; 

        private void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true; 
        }

        private void Start()
        {
            StatusConnected(false);
            _buttonPlay.onClick.AddListener(Connect);
        }

        private void StatusConnected(bool isConnected)
        {
            _progressLabel.SetActive(isConnected);
            _controlPanel.SetActive(!isConnected);
        }

        private void Connect()
        {
            StatusConnected(true);

            
            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                _isConnecting = PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = _gameVersion;
            }
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");

            if (_isConnecting) {
                PhotonNetwork.JoinRandomRoom();
                _isConnecting = false;
            }
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
            _isConnecting = false;
            StatusConnected(false);
        }

        public override void OnJoinRandomFailed(short returnCode, string massage)
        {
            Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");

            PhotonNetwork.CreateRoom(null, new RoomOptions{ MaxPlayers = _maxPlayersPerRoom}); 
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");
            
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                Debug.Log("We load the 'Room for 1' ");
                PhotonNetwork.LoadLevel("RoomFor1");
            }
        }
    }
}
