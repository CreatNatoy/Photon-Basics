using Photon.Pun;
using TMPro;
using UnityEngine;

namespace CodeBase
{
    public class PlayerNameInputField : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _inputField;
        
        private const string PlayerNamePrefKey = "PlayerName";

        private void Start()
        {
            SetupName();
            _inputField.onValueChanged.AddListener(SetPlayerName);
        }

        private void SetPlayerName(string arg0)
        {
            if (string.IsNullOrEmpty(arg0))
            {
                Debug.LogError("Player Name is null or empty");
                return;
            }

            PhotonNetwork.NickName = arg0;
            PlayerPrefs.SetString(PlayerNamePrefKey, arg0);
        }

        private void SetupName()
        {
            var defaultName = PlayerPrefs.GetString(PlayerNamePrefKey, "Player" + Random.Range(1000, 10000));
            _inputField.text = defaultName;
            PhotonNetwork.NickName = defaultName;
        }
        
        
    }
}
