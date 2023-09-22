using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace  GameJam
{
    public class EnemyPlataformMovement : MonoBehaviour, IMovingEntity<EnemyPlataformerLoopMovement>
    {
        public EnemyPlataformerLoopMovement controller;

        public int timeToFight;
        public float speed = 2;


        private void FixedUpdate()
        {
            Move();
        }

        public int Damage(int amount)
        {
            throw new NotImplementedException();
        }

        public GameObject Entity()
        {
            return gameObject;
        }

        public void Kill()
        {
            Destroy(gameObject);
        }

        public int Life()
        {
            throw new NotImplementedException();
        }

        public bool Move()
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);

            var hit = Physics2D.Raycast(transform.position, Vector2.down, 1);
            var hit1 = Physics2D.Raycast(transform.position, Vector2.right, .7f);
            var hit2 = Physics2D.Raycast(transform.position, Vector2.left, .7f);

            Debug.DrawLine(transform.position, hit.point, Color.green);
            Debug.DrawLine(transform.position, hit1.point, Color.red);
            Debug.DrawLine(transform.position, hit2.point, Color.red);

            /*if (hit.collider == null || hit1.collider != null || hit2.collider != null)
                speed *= -1;
                **/
            return false;
        }

        public EnemyPlataformerLoopMovement MovementController()
        {
            return controller;
        }
    }
}
