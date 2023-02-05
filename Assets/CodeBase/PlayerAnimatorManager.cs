using UnityEngine;

namespace CodeBase
{
    public class PlayerAnimatorManager : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [Space] 
        [SerializeField] private float _directionDampTime = 0.25f; 

        private void Update() {
            Jump();
            MoveAndRotation();
        }

        private void Jump() {
            AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

            if (stateInfo.IsName("Base Layer.Run")) {
                if(Input.GetButtonDown("Fire2"))
                    _animator.SetTrigger("Jump");
            }
        }

        private void MoveAndRotation() {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            if (verticalInput < 0)
                verticalInput = 0;

            _animator.SetFloat("Speed", horizontalInput * horizontalInput + verticalInput * verticalInput);
            _animator.SetFloat("Direction", horizontalInput, _directionDampTime, Time.deltaTime);
        }
    }
}
