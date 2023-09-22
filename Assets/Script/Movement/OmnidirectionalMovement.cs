using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GameJam
{
    [Serializable]
    public class OmnidirectionalMovement : IMovementController
    {
        private bool active = false;
        public Bounds bounds;
        public float speed;
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
            var go = entity.Entity();
            var h = Input.GetAxis("Horizontal");
            var v = Input.GetAxis("Vertical");

            go.GetComponent<Rigidbody2D>().MovePosition((Vector2)go.transform.position + new Vector2(h, v) * speed * Time.deltaTime);

            var pos = go.transform.position;

            go.transform.position = new Vector3(
                Mathf.Clamp(pos.x, bounds.nx, bounds.x),
                Mathf.Clamp(pos.y, bounds.ny, bounds.y),
                Mathf.Clamp(pos.z, -3, -4) + 90

            );

            return false;
        }
    }
}
