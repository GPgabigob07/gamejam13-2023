using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GameJam
{
    class NetEnemyMovement : MonoBehaviour, IMovingEntity<NetEnemyMovement>, IMovementController
    {
        public float baseSpeed, speedIncreasePerSecond;

        private float lastIncrease;

        public Transform target;

        public bool CanMove
        {
            get
            {
                return true;
            }
            set
            {
                value = true;
            }
        }

        private void FixedUpdate()
        {
            Move();
        }

        public int Damage(int amount)
        {
            Kill();
            return 0;
        }

        public GameObject Entity()
        {
            return gameObject;
        }

        public void Kill()
        {
            Destroy(Entity());
        }

        public int Life()
        {
            return 1;
        }

        public bool Move(IEntity go)
        {
            if (lastIncrease == 0 || Time.time > lastIncrease + 1)
            {
                lastIncrease = Time.time;
                baseSpeed += speedIncreasePerSecond;
            }

            if (target != null)
            {
                Entity().transform.Translate((target.position - transform.position) * Time.deltaTime * baseSpeed);
            }
            return true;
        }

        public bool Move()
        {
            return this.Move(this);
        }

        public NetEnemyMovement MovementController()
        {
            return this;
        }
    }
}
