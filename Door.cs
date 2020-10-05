using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Platformer
{
    public class Door : Entity
    {
        private static readonly Vector2f DoorSize = new Vector2f(40, 64);
        private static readonly Color Gray = new Color(128, 128, 128);
        private bool open;

        public Door() : base(new RectangleShape(DoorSize)
        {
            FillColor = Gray
        })
        { }
        public void OpenDoor()
        {
            Color = Color.Green;
            open = true;
        }
        public override void Update(Scene scene, float deltaTime)
        {
            if (open && scene.FindIntersects(this).Any(other => other is Player))
            {
                scene.ShouldRestart = true;
            }
        }
    }
}
