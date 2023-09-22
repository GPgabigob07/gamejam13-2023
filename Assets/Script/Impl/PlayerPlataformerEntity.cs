using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GameJam
{
    public class PlayerPlataformerEntity : MonoBehaviour, IMovingEntity<PlataformerMovement>
    {

        public CombatScene combatScene;
        public PlataformerMovement controller;

        public Vector2 startPosition;
        private Rigidbody2D rb;

        public Camera camera;

        public Animator animator;
        public bool isJump, isSecondJump;
        public float speed;
        public Bounds bounds;
        public float jumpPowLevel = 1.4f;
        public float extraJumpForce = 1.4f;
        public float wallJumpFactor = 4;
        public float walllock = 0;
        public float walllockFactor = 0.2f;
        public float firstJumpFromWallForce;
        public float groundRaycastLenght = 0f;
        public float wallRaycastLenght = 0f;
        public int layer = 0;

        public float initialGravity = 3;

        public int maxJumps, jumped;

        public bool walljump = false;

        public Action<bool> onMove;
        public Action<bool> onWall;
        public Action<bool> onJump;
        public Action<bool> onExtraJump;
        public Action<bool> onFall;
        public Action onAttack;

        public bool CanMove;
        public float attackRange;

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


            RaycastHit2D hit = Physics2D.Raycast(go.transform.position, -Vector2.left * h, wallRaycastLenght);

            Debug.DrawLine(go.transform.position, hit.point, Color.red);
            if (hit.collider == null)
            {
                go.transform.Translate(new Vector2(h, 0) * speed * Time.deltaTime);
            }

            ValidateState();

            if (isJump && isSecondJump && walljump)
            {
                walljump = false;
            }

            if (Input.GetMouseButtonDown(0))
            {
                onAttack();
            }

            return false;
        }

        private void ValidateState()
        {
            var go = gameObject;
            var jump = Input.GetKeyDown(KeyCode.Space);
            var h = Input.GetAxis("Horizontal");

            if (jump)
                Debug.Log("Jump pressed!");

            var ground = Physics2D.Raycast(transform.position, -Vector2.up, groundRaycastLenght);
            var wall = Physics2D.Raycast(transform.position, Vector2.right * h, wallRaycastLenght);

            var onGround = ground.collider != null && ground.collider.gameObject.CompareTag("Ground");
            var input = h != 0;

            Debug.DrawLine(transform.position, ground.point, Color.yellow);
            Debug.DrawLine(transform.position, wall.point, Color.cyan);

            if (!onGround && input && wall.collider != null)
            {
                Debug.Log("ON WALL");
                walllock = h;
                rb.gravityScale = initialGravity / 30;
                onWall(true);
            }
            else
            {
                rb.gravityScale = initialGravity;
                walllock = 0;
                onWall(false);
            };

            if (jump && (((onGround || wall.collider != null) && !isJump) || !isSecondJump))
            {
                Debug.Log("Attempting Jump");
                if (isJump || ground.collider != null)
                {
                    go.GetComponent<Rigidbody2D>().AddForce(
                        (Vector2.up * jumpPowLevel * speed * ExtraJumpForce()),
                        ForceMode2D.Impulse
                    );

                    if (walllock != 0)
                    {
                        rb.AddForce(((walllock > 0 ? 1 : (walllock < 0 ? -1 : 0)) * Vector2.right * firstJumpFromWallForce));
                    }
                    isJump = true;
                    onJump(isJump || isSecondJump);
                    onFall(false);
                    //onExtraJump(isJump && isSecondJump);
                }
            }

            if (isSecondJump)
            {
                StartCoroutine(DelayOnFall());
            }

            if (onGround)
            {
                isJump = false;
                isSecondJump = false;
                onMove(input);
                onFall(false);
            }
            else
            {
                onFall(true);
            }
        }

        private float ExtraJumpForce()
        {
            if (walllock != 0)
            {
                walllock = 0;
                return walllockFactor;
            }

            if (isJump || jumped >= 1)
            {
                isSecondJump = true;
                return walljump ? extraJumpForce * wallJumpFactor : extraJumpForce;
            }
            return walljump ? firstJumpFromWallForce : 1;
        }


        private void Awake()
        {
            CanMove = true;
            startPosition = gameObject.transform.position;
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
        }

        private void Start()
        {
            onMove = (v) => animator.SetBool("running", v);
            onJump = (v) =>
            {
                animator.SetTrigger("jumped");
                //Delay( () => animator.SetBool("jump", false));
            };
            onExtraJump = (v) =>
            {
                animator.SetBool("jump_extra", v);
                animator.SetBool("jump_extra", false);
            };
            onFall = (v) => animator.SetBool("fall", v);
            onWall = (v) => animator.SetBool("wall_lock", v);
            onAttack = () =>
            {
                animator.SetTrigger("attack");
                doAttack();
            };

        }

        private void doAttack()
        {
            Delay(() =>
            {
                var direction = transform.localScale.x > 0 ? 1 : -1;
                var ray = Physics2D.Raycast(transform.position, Vector2.right * direction, attackRange);

                if (ray.collider != null && ray.collider.gameObject.CompareTag("Enemy"))
                {
                    combatScene.StartCombat(this, ray.collider.gameObject.GetComponent<EnemyPlataformMovement>());
                }
            });
        }

        private void Update()
        {
            if (CanMove)
            {
                Move(this);
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            rb.gravityScale = initialGravity;
            walllock = 0;

            if (collision.gameObject.CompareTag("Ground"))
            {
                if (!isJump)
                    isJump = true;

            }
        }

        IEnumerator DelayOnFall()
        {
            yield return new WaitForSeconds(.2f);
            onFall(true);
        }

        float Width()
        {
            var s = GetComponent<SpriteRenderer>();
            return s.bounds.size.x;
        }

        float Height()
        {
            var s = GetComponent<SpriteRenderer>();
            return s.bounds.size.y;
        }

        private void Delay(Action todo)
        {
            StartCoroutine(ToDelay(todo));
        }

        IEnumerator ToDelay(Action todo)
        {
            yield return new WaitForSeconds(.2f);
            todo();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("DeadlyEnvi"))
            {
                Kill();
            }
        }

        public int Damage(int amount)
        {
            Kill();
            return 0;
        }

        public void Kill()
        {
            transform.position = startPosition + new Vector2(0, 2);
        }

        public int Life()
        {
            throw new NotImplementedException();
        }

        public bool Move()
        {
            throw new NotImplementedException();
        }

        public PlataformerMovement MovementController()
        {
            return controller;
        }
        public GameObject Entity()
        {
            return gameObject;
        }
    }
}
