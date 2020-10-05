using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;

namespace Platformer
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var window = new RenderWindow(new VideoMode(800, 600), "Platformer"))
            {
                window.Closed += (s, e) => window.Close();

                Clock frameclock = new Clock();
                Scene scene = new Scene(window);
                scene.Load("Level1.txt");
                scene.Spawn(new Player() { Position = new Vector2f(200, 150) });
                Block block = new Block();
                block.Position = new Vector2f(100, 100);
                while (window.IsOpen)
                {
                    window.DispatchEvents();
                    window.Clear(new Color(131, 197, 235));
                    float deltaTime = frameclock.Restart().AsSeconds();
                    scene.RenderAll();
                    scene.UpdateAll(deltaTime);

                    window.Display();
                }
            }
        }
    }
}