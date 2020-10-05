using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Platformer
{
    public class Block : Entity
    {
        private static readonly Vector2f BlockSize = new Vector2f(32, 32);

        public Block() : base(new RectangleShape(BlockSize))
        {

        }
        public override bool Obstacle => true;
    }
}
