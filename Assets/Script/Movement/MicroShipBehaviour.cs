using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameJam
{
    public class MicroShipBehaviour : MonoBehaviour, IMovingEntity<OmnidirectionalMovement>  {

        public OmnidirectionalMovement controller;
        public Camera camera;
        public Transform sprite;
        public Transform dir;

        public CombatScene combat;

        public int maxImpulses;
        public int usedImpulses;
        public float impulseDelay;
        public bool onImpulse;
        public float impulseSpeed;
        public Rigidbody2D rb;

        public float potato;

        private Animator anim;


        public int Damage(int amount)
        {
            Kill();
            return 0;
        }

        public GameObject Entity()
        {
            return this.gameObject;
        }

        public void Kill()
        {
            Destroy(gameObject);
        }

        public int Life()
        {
            return 1;
        }

        public bool Move()
        {
            if (controller.CanMove)
            {
                var h = Input.GetAxis("Horizontal");
                var v = Input.GetAxis("Vertical");
                anim.SetBool("moving", h != 0 || v != 0);
                return controller.Move(this);
            }
            return false;
        }

        public OmnidirectionalMovement MovementController()
        {
            return controller;
        }

        // Use this for initialization
        void Start() {
            anim = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
            controller.CanMove = true;
        }

        // Update is called once per frame
        void FixedUpdate() {
            Move();
            Impulse();
        }

        public float lastImpulse;
        private void Impulse()
        {
            potato = Time.time;
        
            if (Input.GetMouseButtonDown(0))
            {
                anim.SetTrigger("attack");

                if (!onImpulse)
                {
                    onImpulse = true;
                    StartCoroutine(DePulse());
                    lastImpulse = Time.time + 1;
                    var tx = Mathf.Cos((sprite.rotation.eulerAngles.z + 90)* Mathf.Deg2Rad);
                    var ty = Mathf.Sin((sprite.rotation.eulerAngles.z + 90)* Mathf.Deg2Rad);
                    rb.AddForce(/*(Vector2)transform.position +*/ new Vector2(tx, ty) * impulseSpeed, ForceMode2D.Impulse);
                }
            }
        }

        private IEnumerator DePulse()
        {
            yield return new WaitForSeconds(.3f);
            onImpulse = false;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            
            if(onImpulse)
            {
                Debug.Log("SKJDFGBÇSDLKGFJB");
                Destroy(collision.gameObject);
            } else
            {
                combat.EndGame();
            }
            
        }
    }
}