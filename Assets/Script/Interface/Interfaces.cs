using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GameJam
{
    public interface IEntity
    {
        int Life();

        int Damage(int amount);

        void Kill();

        GameObject Entity();

    }

    public interface IMovingEntity<T> : IEntity where T : IMovementController {

        T MovementController();

        bool Move();

    }

    public interface IMovementController
    {
        bool Move(IEntity go);
        bool CanMove { get; set; }
    }
}
