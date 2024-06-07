using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace KnightOfLight.Contoller
{
    public class Input
    {
        public Keys Up {
            get { return Keys.W; }
        }
        public Keys Down
        {
            get { return Keys.S; }
        }
        public Keys Right
        {
            get { return Keys.D; }
        }
        public Keys Left
        {
            get { return Keys.A; }
        }
        public Keys Attack
        {
            get { return Keys.Z; }
        }
        public Keys Jump
        {
            get { return Keys.Space; }
        }

        public Keys Run
        {
            get { return Keys.LeftShift; }
        }

        public Keys LeftGame
        {
            get { return Keys.Escape; }
        }
    }
}
