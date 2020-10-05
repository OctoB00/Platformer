using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System.Linq;

namespace Platformer
{
    public class Key : Entity
    {
        public Key() : base(new CircleShape(16)
        {
            FillColor = Color.Green
        })
        { }
        public override void Update(Scene scene, float deltaTime)
        {
            if (scene.FindIntersects(this).Any(other => other is Player))
            {
                foreach (Door door in scene.FindAll().OfType<Door>())
                {
                    door.OpenDoor();
                    Dead = true;
                    break;
                }
            }
        }
    }
}
