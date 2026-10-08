using System;
using UnityEngine;

namespace Framework.Core.Sensors
{
    public class EnvironmentDetector2D : MonoBehaviour
    {
        [Header("Collision Layers")] [SerializeField]
        private LayerMask groundLayer;

        [SerializeField] private LayerMask wallLayer;

        [Header("Detection Points & Sizes")] [SerializeField]
        private Transform groundCheckPoint;

        [SerializeField] private Vector2 groundCheckSize = new Vector2(0.6f, 0.1f);

        [SerializeField] private Transform wallCheckPoint;
        [SerializeField] private float wallCheckDistance = 0.35f;

        public bool IsGrounded { get; private set; }
        public bool IsOnWall { get; private set; }
        public int WallDirection { get; private set; } // -1 = Left, 1 = Right, 0 = None
        public Vector2 GroundNormal { get; private set; } = Vector2.up;

        public event Action OnGrounded;
        public event Action OnLeftGround;

        private void Update()
        {
            PerformGroundCheck();
            PerformWallCheck();
        }

        private void PerformGroundCheck()
        {
            if (groundCheckPoint == null) return;

            Collider2D hit = Physics2D.OverlapBox(groundCheckPoint.position, groundCheckSize, 0f, groundLayer);
            bool wasGrounded = IsGrounded;
            IsGrounded = hit != null;

            if (IsGrounded)
            {
                RaycastHit2D slopeHit = Physics2D.Raycast(groundCheckPoint.position, Vector2.down, 0.5f, groundLayer);
                if (slopeHit.collider != null)
                {
                    GroundNormal = slopeHit.normal;
                }
            }
            else
            {
                GroundNormal = Vector2.up;
            }

            if (!wasGrounded && IsGrounded) OnGrounded?.Invoke();
            if (wasGrounded && !IsGrounded) OnLeftGround?.Invoke();
        }

        private void PerformWallCheck()
        {
            if (wallCheckPoint == null) return;

            RaycastHit2D hitRight =
                Physics2D.Raycast(wallCheckPoint.position, Vector2.right, wallCheckDistance, wallLayer);
            if (hitRight.collider != null)
            {
                IsOnWall = true;
                WallDirection = 1;
                return;
            }

            RaycastHit2D hitLeft =
                Physics2D.Raycast(wallCheckPoint.position, Vector2.left, wallCheckDistance, wallLayer);
            if (hitLeft.collider != null)
            {
                IsOnWall = true;
                WallDirection = -1;
                return;
            }

            IsOnWall = false;
            WallDirection = 0;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = IsGrounded ? Color.green : Color.red;
            if (groundCheckPoint != null)
            {
                Gizmos.DrawWireCube(groundCheckPoint.position, groundCheckSize);
            }

            Gizmos.color = IsOnWall ? Color.cyan : Color.yellow;
            if (wallCheckPoint != null)
            {
                Gizmos.DrawLine(wallCheckPoint.position, wallCheckPoint.position + Vector3.right * wallCheckDistance);
                Gizmos.DrawLine(wallCheckPoint.position, wallCheckPoint.position + Vector3.left * wallCheckDistance);
            }
        }
    }
}