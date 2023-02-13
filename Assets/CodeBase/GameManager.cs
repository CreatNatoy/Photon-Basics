using System;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CodeBase
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        public static GameManager Instance;
        
        [SerializeField] private Button _buttonLeave;
        [Tooltip("The prefab to use for representing the player")]
        [SerializeField] private GameObject _playerPrefab;

        private void Start() {
            Instance = this; 
            
            _buttonLeave.onClick.AddListener(LeaveRoom);
            
            if (_playerPrefab == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'",this);
            }
            else
            {
                if (PlayerManager.LocalPlayerInstance == null) {
                    Debug.LogFormat("We are Instantiating LocalPlayer from {0}", Application.loadedLevelName);
                    PhotonNetwork.Instantiate(_playerPrefab.name, new Vector3(0f,5f,0f), Quaternion.identity, 0);
                }
                else
                {
                    Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
                }
            }
        }

        #region Photon Callbacks
        
        public override void OnLeftRoom() => SceneManager.LoadScene(0);

        public override void OnPlayerEnteredRoom(Player other) {
            Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName);

            if (PhotonNetwork.IsMasterClient) {
                Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient);
                LoadArena();
            }
        }

        public override void OnPlayerLeftRoom(Player other) {
            Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName);

            if (PhotonNetwork.IsMasterClient) {
                Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); 

                LoadArena();
            }
        }
        
        #endregion

        public void LeaveRoom() => PhotonNetwork.LeaveRoom();

        private void LoadArena() {
            if (!PhotonNetwork.IsMasterClient) {
                Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
                return;
            }

            var playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
            Debug.LogFormat("PhotonNetwork : Loading Level : {0}", playerCount);
            PhotonNetwork.LoadLevel("RoomFor" + playerCount);
        }
    }
}
