using Framework.Animation;
using UnityEngine;

namespace Extensions
{
    [RequireComponent(typeof(Animator))]
    public class PlayerMovementSound : MonoBehaviour
    {
        [SerializeField] private AudioClip jumpClip;
        [SerializeField] private AudioClip[] footstepClips;
        [SerializeField] private float footstepInterval = 0.3f;
        [SerializeField] private float volume = 0.6f;

        private Animator _animator;
        private bool _wasGrounded;
        private float _footstepTimer;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            bool isGrounded = _animator.GetBool(AnimationHashes.IsGrounded);
            float speed = _animator.GetFloat(AnimationHashes.Speed);

            // Left the ground this frame -> jump (covers both intentional jumps and walking off ledges)
            if (_wasGrounded && !isGrounded && jumpClip != null)
            {
                AudioSource.PlayClipAtPoint(jumpClip, transform.position, volume);
            }

            if (isGrounded && speed > 0.1f)
            {
                _footstepTimer -= Time.deltaTime;
                if (_footstepTimer <= 0f)
                {
                    _footstepTimer = footstepInterval;
                    PlayFootstep();
                }
            }
            else
            {
                _footstepTimer = 0f;
            }

            _wasGrounded = isGrounded;
        }

        private void PlayFootstep()
        {
            if (footstepClips == null || footstepClips.Length == 0) return;
            var clip = footstepClips[Random.Range(0, footstepClips.Length)];
            if (clip != null) AudioSource.PlayClipAtPoint(clip, transform.position, volume);
        }
    }
}
