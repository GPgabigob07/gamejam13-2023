using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GameJam
{
    class BasicMovingEntity : MonoBehaviour, IMovingEntity<OmnidirectionalMovement>
    {
        [SerializeField]
        public OmnidirectionalMovement controller;

        void Start()
        {
            controller.CanMove = true;    
        }

        void Update()
        {
            Move();
        }

        public int Damage(int amount)
        {
            //throw new NotImplementedException();
            return 1;
        }

        public void Kill()
        {
           // throw new NotImplementedException();

        }

        public int Life()
        {
            return 1;
            //throw new NotImplementedException();
        }

        public bool Move()
        {
            if (controller.CanMove)
            {
                return controller.Move(this);
            }

            return false;
        }

        public OmnidirectionalMovement MovementController()
        {
            return controller;
        }

        public GameObject Entity()
        {
            return gameObject;
        }
    }
}
