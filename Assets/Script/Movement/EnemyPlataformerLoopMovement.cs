using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GameJam
{ 
    [Serializable]
    public class EnemyPlataformerLoopMovement : IMovementController
    {

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

        public bool Move(IEntity entity)
        {
            var go = entity.Entity();
            

            return true;
        }

       
    }
}
