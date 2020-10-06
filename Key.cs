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
        private static Texture texture = new Texture("Key.png");
        private static Sprite sprite = new Sprite(texture);
        private static int circleRadius = 32;
        public Key() : base(new CircleShape(circleRadius)
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
            sprite.Origin = new Vector2f(circleRadius / 2, circleRadius / 2);
            sprite.Position = Position;
        }
        public override void Render(RenderTarget target)
        {
            target.Draw(sprite);
        }
    }
}
