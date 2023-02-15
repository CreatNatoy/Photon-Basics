using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase
{
    public class PlayerUI : MonoBehaviour
    {
        #region Private Fields

        [Tooltip("Pixel offset from the player target")]
        [SerializeField] private Vector3 _screenOffset = new Vector3(0f,30f,0f);
        
        [Tooltip("UI Text to display Player's Name")]
        [SerializeField] private TextMeshProUGUI _playerNameText;

        [Tooltip("UI Slider to display Player's Health")]
        [SerializeField] private Slider _playerHealthSlider;

        private PlayerManager _target;
        
        private float _characterControllerHeight = 0f;
        private Transform _targetTransform;
        private Renderer _targetRenderer;
        private CanvasGroup _canvasGroup;
        private Vector3 _targetPosition;
        private Camera _camera;

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake() {
            transform.SetParent(GameObject.Find("Canvas").GetComponent<Transform>(), false);
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        private void Start() {
            _camera = Camera.main;
        }

        private void Update() {
            if (_target == null) {
                Destroy(gameObject);
                return;
            }
            
            _playerHealthSlider.value = _target.Health;
        }
        
        void LateUpdate()
        {
            if (_targetRenderer != null)
            {
                _canvasGroup.alpha = _targetRenderer.isVisible ? 1f : 0f;
            }
            
            if (_targetTransform != null)
            {
                _targetPosition = _targetTransform.position;
                _targetPosition.y += _characterControllerHeight;
                transform.position = _camera.WorldToScreenPoint (_targetPosition) + _screenOffset;
            }
        }

        #endregion

        #region Public Methods

        public void SetTarget(PlayerManager target) {
            _target = target;
            _playerNameText.text = _target.photonView.Owner.NickName;
            
            _targetTransform = _target.GetComponent<Transform>();
            _targetRenderer = _target.GetComponent<Renderer>();
            CharacterController characterController = _target.GetComponent<CharacterController> ();
            if (characterController != null)
            {
                _characterControllerHeight = characterController.height;
            }
        }
        
        #endregion
    }
}
