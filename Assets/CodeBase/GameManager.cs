using System;
using Photon.Pun;
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

        public override void OnLeftRoom() => SceneManager.LoadScene(0);

        private void LeaveRoom() => PhotonNetwork.LeaveRoom();
    }
}
