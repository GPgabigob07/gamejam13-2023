using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GameJam
{
    [Serializable]
    public class PlataformerMovement : IMovementController
    {

        private bool active;
        //newer movement
        public bool isJump, isSecondJump;
        public float speed;
        public Bounds bounds;
        public float jumpPowLevel = 1.4f;
        public float extraJumpForce = 1.4f;
        public float wallJumpFactor = 4;
        public bool walllock = false;
        public float walllockFactor = 0.2f;
        public float firstJumpFromWallForce;
        public float raycastH = 0f;
        public int layer = LayerMask.NameToLayer("Ground");

        public float initialGravity = 3;

        public int maxJumps, jumped;

        public bool walljump = false;

        public Action<bool> onMove; 
        public Action<bool> onWall;
        public Action<bool> onJump; 
        public Action<bool> onExtraJump; 
        public Action<bool> onFall;

        public PlataformerMovement(int maxJumps, float speed, Bounds bounds)
        {
            this.maxJumps = maxJumps;
            this.speed = speed;
            this.bounds = bounds;
        }

        public bool CanMove
        {
            get
            {
                return active;
            }

            set
            {
                active = value;
            }
        }

        public void ClearJumps()
        {
            isJump = false;
            isSecondJump = false;
        }

        internal void AddWallJumpForce()
        {
            walljump = true;
            ClearJumps();
        }

        public bool Move(IEntity entity)
        {
            var go = entity.Entity();

            if (go.transform.position.y < bounds.ny)
                entity.Kill();

            var h = Input.GetAxis("Horizontal");

            //Change sprite direction
            var oldScale = go.transform.localScale;
            if (h > 0)
                go.transform.localScale = new Vector2(Mathf.Abs(oldScale.x), oldScale.y);
            else if (h < 0 && oldScale.x > 0)
                go.transform.localScale = new Vector2(oldScale.x * -1, oldScale.y);

            //Idle cycle
            if (h == 0 && !isJump) onMove(false);
            else onMove(true);


            RaycastHit2D hit = Physics2D.Raycast(go.transform.position, -Vector2.left * h, raycastH);

            Debug.DrawLine(go.transform.position, hit.point, Color.red);
            Debug.Log(hit.point);
            Debug.Log(go.transform.position);

            if (hit.collider == null)
            {
                go.transform.Translate(new Vector2(h, 0) * speed * Time.deltaTime);
            }

            ValidateJump(entity);

            if (isJump && isSecondJump && walljump)
            {
                walljump = false;
            }

            return false;
        }

        private void ValidateJump(IEntity entity)
        {
            var go = entity.Entity();
            var rb = go.GetComponent<Rigidbody2D>();
            var transform = go.transform;
            var jump = Input.GetKeyDown(KeyCode.Space);

            var ground = Physics2D.Raycast(transform.position, -Vector2.up, .5f);
            var wall = Physics2D.Raycast(transform.position, -Vector2.left, 1);
            var onGround = ground.collider != null;
            var input = Input.GetAxis("Horizontal") != 0;

            Debug.DrawLine(transform.position, ground.point, Color.yellow);
            Debug.DrawLine(transform.position, wall.point, Color.cyan);

            if (onGround && ground.collider.gameObject.CompareTag("Ground"))
            {
                isJump = false;
                isSecondJump = false;
                onMove(false);
            }

            if (!onGround && input && wall.collider != null)
            {
                walllock = true;
                rb.gravityScale = initialGravity / 30;
                onWall(true);
            }
            else
            {
                rb.gravityScale = initialGravity;
                walllock = false;
                onWall(false);
            };

            if (!isSecondJump && jump)
            {
                if (isJump || ground.collider != null)
                {
                    go.GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpPowLevel * speed * ExtraJumpForce(), ForceMode2D.Impulse);
                    isJump = true;
                    onJump(isJump && !isSecondJump);
                    onExtraJump(isJump && isSecondJump);
                }
            }
        }

        private float ExtraJumpForce()
        {
            if (walllock)
            {
                walllock = false;
                return walllockFactor;
            }

            if (isJump || jumped >= 1)
            {
                isSecondJump = true;
                return walljump ? extraJumpForce * wallJumpFactor : extraJumpForce;
            }            
            return walljump ? firstJumpFromWallForce : 1;
        }
    }
}
