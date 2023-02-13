using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using UnityEngine;

namespace CodeBase
{
    public class PlayerManager : MonoBehaviourPunCallbacks, IPunObservable
    {
        [Tooltip("The current Health of our player")]
        public float Health = 1f;
        [Space]
        [Tooltip("The Beams GameObject to control")]
        [SerializeField] private GameObject _beams;
        [SerializeField] private CameraWork _cameraWork;
        private bool _isFiring;
        
        [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
        public static GameObject LocalPlayerInstance;

        private void Awake() {
            _beams.SetActive(false);

            if (photonView.IsMine)
                LocalPlayerInstance = gameObject;
            
            DontDestroyOnLoad(gameObject);
        }

        private void Start() {
            if(photonView.IsMine)
                _cameraWork.OnStartFollowing();
            
#if UNITY54ORNEWER
// Unity 5.4 has a new scene management. register a method to call CalledOnLevelWasLoaded.
UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
#endif
        }

        private void Update() {
            if(photonView.IsMine)
                ProcessInputs();
            
            if (_beams != null && _isFiring != _beams.activeInHierarchy) 
                _beams.SetActive(_isFiring);
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

        #region IPunObservable implementation

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
            if (stream.IsWriting) {
                stream.SendNext(_isFiring);
                stream.SendNext(Health);
            }
            else {
                _isFiring = (bool)stream.ReceiveNext();
                Health = (float)stream.ReceiveNext();
            }
        }
        #endregion

#if UNITY54ORNEWER
void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode loadingMode)
{
    this.CalledOnLevelWasLoaded(scene.buildIndex);
}
#endif
        
        
#if !UNITY_5_4_OR_NEWER
/// <summary>See CalledOnLevelWasLoaded. Outdated in Unity 5.4.</summary>
void OnLevelWasLoaded(int level)
{
    this.CalledOnLevelWasLoaded(level);
}
endif

void CalledOnLevelWasLoaded(int level)
{
    // check if we are outside the Arena and if it's the case, spawn around the center of the arena in a safe zone
    if (!Physics.Raycast(transform.position, -Vector3.up, 5f))
    {
        transform.position = new Vector3(0f, 5f, 0f);
    }
}
#endif

#if UNITY54ORNEWER
public override void OnDisable()
{
    // Always call the base to remove callbacks
    base.OnDisable ();
    UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
}  
#endif
    }
}
