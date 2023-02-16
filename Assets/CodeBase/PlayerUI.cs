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
        [SerializeField] private PlayerManager _target;
        
        private float _characterControllerHeight = 0f;
        private Renderer _targetRenderer;
        private CanvasGroup _canvasGroup;
        private Vector3 _targetPosition;
        
        #endregion

        #region MonoBehaviour Callbacks

        private void Start() {
            _playerNameText.text = _target.photonView.Owner.NickName;
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
            transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
        }

        #endregion

        #region Public Methods
        

        #endregion
    }
}
