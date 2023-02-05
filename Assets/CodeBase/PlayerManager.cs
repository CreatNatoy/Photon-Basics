using Photon.Pun;
using UnityEngine;

namespace CodeBase
{
    public class PlayerManager : MonoBehaviourPunCallbacks
    {
        [Tooltip("The current Health of our player")]
        public float Health = 1f;
        [Space]
        [Tooltip("The Beams GameObject to control")]
        [SerializeField] private GameObject _beams;
        
        private void Awake() {
            _beams.SetActive(false);
        }

        private void Update() {
            ProcessInputs();
        }

        private void ProcessInputs() {
            if (Input.GetButtonDown("Fire1"))  
                _beams.SetActive(true);
            if (Input.GetButtonUp("Fire1")) 
                _beams.SetActive(false);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (!photonView.IsMine)
            {
                return;
            }

            if (!other.name.Contains("Beam"))
            {
                return;
            }
            Health -= 0.1f;
        }
        
        private void OnTriggerStay(Collider other)
        {
            if (!photonView.IsMine)
            {
                return;
            }

            if (!other.name.Contains("Beam"))
            {
                return;
            }
            
            Health -= 0.1f * Time.deltaTime;
            
            if (Health <= 0f)
            {
                GameManager.Instance.LeaveRoom();
            }
            
        }
    }
}
