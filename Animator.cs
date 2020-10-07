using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Platformer
{
    public class Animator
    {
        private static Texture texture = new Texture("SimpleRobot.png");
        private static Sprite AnimatedSprite = new Sprite(texture);
        int width;
        int height;
        int currentFrame = 0;
        int maxFrame = 1;
        public Animator(int width, int height, int frames)
        {
            this.width = width;
            this.height = height;
            this.maxFrame = frames - 1;
        }
        public void Animate(Sprite sprite)
        {
            sprite.TextureRect = new IntRect(width * currentFrame, 0, width, height);
            currentFrame++;
            if (currentFrame > maxFrame)
            {
                currentFrame = 0;
            }
        }
        public Sprite GetSprite()
        {
            return AnimatedSprite;
        }
    }
}
