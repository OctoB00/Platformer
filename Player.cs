using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Platformer
{
    public class Player : Entity
    {
        private static readonly Texture texture = new Texture("Robot.png");
        private static Sprite sprite = new Sprite(texture);
        private static readonly Vector2f PlayerSize = new Vector2f(36, 64);
        private bool moveLeft, moveRight, jump, movingRight, movingLeft;
        private float verticalVelocity;
        Animator animator = new Animator(36, 64, 8);
        public Player() : base(new RectangleShape(PlayerSize)
        {

        })
        { }
        private void OnKeyPressed(object sender, KeyEventArgs ev)
        {
            switch (ev.Code)
            {
                case Keyboard.Key.Left: moveLeft = true;  movingRight = false; movingLeft = true; break;
                case Keyboard.Key.Right: moveRight = true; movingLeft = false; movingRight = true; break;
                case Keyboard.Key.Up: jump = true; break;
            }
        }
        private void OnKeyReleased(object sender, KeyEventArgs ev)
        {
            switch (ev.Code)
            {
                case Keyboard.Key.Left: moveLeft = false; break;
                case Keyboard.Key.Right: moveRight = false; break;
                case Keyboard.Key.Up: jump = false; break;
            }
        }
        public override void OnSpawn(Window window)
        {
            window.KeyPressed += OnKeyPressed;
            window.KeyReleased += OnKeyReleased;
        }
        public override void OnDespawn(Window window)
        {
            window.KeyPressed -= OnKeyPressed;
            window.KeyReleased -= OnKeyReleased;
        }
        public override void Update(Scene scene, float deltaTime)
        {
            Vector2f amount = new Vector2f(0, 0);
            if (moveLeft)
            {
                amount.X -= 200.0f;
            }
            if (moveRight)
            {
                amount.X += 200.0f;
            }
            
            if (scene.IsGrounded(this))
            {
                verticalVelocity = 0.0f;
                if (jump)
                {
                    verticalVelocity = -600.0f;
                }
            }
            if (verticalVelocity < 0.0f && scene.IsTouchingCeiling(this))
            {
                verticalVelocity = 0.0f;
            }
            amount.Y += verticalVelocity;
            verticalVelocity += deltaTime * 1000.0f;
            scene.Move(this, amount * deltaTime);
            if (Position.Y > 1000.0f)
            {
                scene.ShouldRestart = true;
            }
            if (movingLeft)
            {
                sprite.Scale = new Vector2f(-1f, 1f);
            }
            else
            {
                sprite.Scale = new Vector2f(1f, 1f);
            }
            sprite.Origin = new Vector2f(PlayerSize.X / 2, PlayerSize.Y / 2);
            sprite.Position = Position;
        }
        
        public override void Render(RenderTarget target)
        {
            if (moveRight || moveLeft)
            {
                sprite = animator.GetSprite();
                animator.Animate(sprite);
                target.Draw(sprite);
            }
            else
            {
                target.Draw(sprite);
            }
            
        }
    }
}
