using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CodeBase
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        [SerializeField] private Button _buttonLeave;

        private void Start() => 
            _buttonLeave.onClick.AddListener(LeaveRoom);

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

        private void LeaveRoom() => PhotonNetwork.LeaveRoom();

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
