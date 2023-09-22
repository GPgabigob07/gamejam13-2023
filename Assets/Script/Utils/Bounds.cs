using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameJam
{   [Serializable]
    public class Bounds 
    {
        public float x, y, nx, ny;

        public Bounds(float X, float Y, float NX, float NY)
        {
            x = X;
            y = Y;
            nx = NX;
            ny = NY;
        }
    }
}
