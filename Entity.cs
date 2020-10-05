using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Platformer
{
    public class Entity
    {
        private Shape shape;
        public Entity(Shape shape)
        {
            this.shape = shape;
            this.shape.Origin = Size * 0.5f;
        }
        public Vector2f Position
        {
            get => shape.Position;
            set => shape.Position = value;
        }
        public Color Color
        {
            get => shape.FillColor;
            set => shape.FillColor = value;
        }
        public Vector2f Size =>
            new Vector2f(
                shape.GetLocalBounds().Width,
                shape.GetLocalBounds().Height);
        public float Left => Position.X - Size.X * 0.5f;
        public float Top => Position.Y - Size.Y * 0.5f;
        public float Right => Position.X + Size.X * 0.5f;
        public float Bottom => Position.Y + Size.Y * 0.5f;
        public virtual bool Obstacle => false;
        public bool Dead { get; set; }
        public virtual void OnSpawn(Window w)
        {

        }
        public virtual void OnDespawn(Window w)
        {

        }
        public virtual void Update(Scene scene, float deltaTime)
        {

        }
        public virtual void Render(RenderTarget target)
        {
            target.Draw(shape);
        }
    }
}
