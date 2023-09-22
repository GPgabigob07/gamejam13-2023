using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GameJam
{
    [Serializable]
    class GravityController : IMovementController
    {
        public float mass, gravityScalar;
        public bool active = false;

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

        public bool Move(IEntity entity)
        {
            if (active)
            {
                var go = entity.Entity();
                var accel = mass * gravityScalar * Time.deltaTime;

                go.transform.Translate(Vector2.down * accel);
            }
            return true;
        }
    }
}
