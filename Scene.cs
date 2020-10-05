using SFML.Graphics;
using SFML.Window;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace Platformer
{
    public class Scene
    {
        private readonly List<Entity> entities;
        private readonly RenderWindow window;
        private string filename;
        public bool ShouldRestart { get; set; }
        public Scene(RenderWindow window)
        {
            entities = new List<Entity>();
            this.window = window;
        }
        public void RenderAll()
        {
            foreach (Entity entity in entities)
            {
                entity.Render(window);
            }
        }
        public void Load(string filename)
        {
            this.filename = filename;
            entities.Clear();
            foreach (string line in File.ReadLines(filename))
            {
                if (line.Length == 0)
                {
                    continue;
                }
                string[] words = line.Split(",");
                int x = int.Parse(words[0]);
                int y = int.Parse(words[1]);
                int type = int.Parse(words[2]);

                Entity entity = null;
                if (type == 0)
                {
                    entity = new Block();
                }
                else if (type == 1)
                {
                    entity = new Door();
                }
                else if (type == 2)
                {
                    entity = new Key();
                }
                else continue;

                entity.Position = new Vector2f(x, y);
                entities.Add(entity);
                entity.OnSpawn(window);
            }
        }
        public void Spawn(Entity entity)
        {
            entities.Add(entity);
            entity.OnSpawn(window);
        }
        private float MinDistanceX(Entity moved, float direction)
        {
            float minDistance = entities
                .Where(other => other.Obstacle)
                .Where(other => other != moved)
                .Where(other => 
                    MathF.Sign(direction) ==
                    MathF.Sign(other.Left - moved.Left)
                )
                .Where(other => other.Top < moved.Bottom)
                .Where(other => other.Bottom > moved.Top)
                .Select(other => MathF.Abs(other.Position.X - moved.Position.X) - 0.5f * (other.Size.X + moved.Size.X))
                .DefaultIfEmpty(float.MaxValue).Min();
            return minDistance;
        }
        private float MinDistanceY(Entity moved, float direction)
        {
            float minDistance = entities
                .Where(other => other.Obstacle)
                .Where(other => other != moved)
                .Where(other =>
                    MathF.Sign(direction) ==
                    MathF.Sign(other.Top - moved.Top)
                )
                .Where(other => other.Left < moved.Right)
                .Where(other => other.Right > moved.Left)
                .Select(other => MathF.Abs(other.Position.Y - moved.Position.Y) - 0.5f * (other.Size.Y + moved.Size.Y))
                .DefaultIfEmpty(float.MaxValue).Min();
            return minDistance;
        }
        private void MoveX(Entity moved, float amount)
        {
            float minDistance = MinDistanceX(moved, amount);
            if (minDistance < MathF.Abs(amount))
            {
                amount = MathF.Sign(amount) * minDistance - float.Epsilon;
            }
            moved.Position += new Vector2f(amount, 0.0f);

        }
        private void MoveY(Entity moved, float amount)
        {
            float minDistance = MinDistanceY(moved, amount);
            if (minDistance < MathF.Abs(amount))
            {
                amount = MathF.Sign(amount) * minDistance - float.Epsilon;
            }
            moved.Position += new Vector2f(0.0f, amount);

        }
        public void Move(Entity moved, Vector2f amount)
        {
            MoveX(moved, amount.X);
            MoveY(moved, amount.Y);
        }
        public bool IsGrounded(Entity entity)
        {
            float minDistance = MinDistanceY(entity, 1.0f);
            return minDistance <= float.Epsilon;
        }
        public bool IsTouchingCeiling(Entity entity)
        {
            float minDistance = MinDistanceY(entity, -1.0f);
            return minDistance <= float.Epsilon;
        }
        public IEnumerable<Entity> FindIntersects(Entity entity)
        {
            return FindAll()
            .Where(other => other != entity)
            .Where(other => !(
            other.Left > entity.Right ||
            other.Right < entity.Left ||
            other.Top > entity.Bottom ||
            other.Bottom < entity.Top));
        }
        public IEnumerable<Entity> FindAll()
        {
            return entities;
        }
        public void UpdateAll(float deltaTime)
        {
            foreach (Entity entity in entities)
            {
                entity.Update(this, deltaTime);
            }

            entities.RemoveAll(e =>
            {
                if (e.Dead)
                {
                    e.OnDespawn(window);
                    return true;
                }
                else
                {
                    return false;
                }
            });
            if (ShouldRestart)
            {
                Restart();
            }
        }
        public void Restart()
        {
            entities.ForEach(e => e.OnDespawn(window));
            entities.Clear();
            Load(filename);
            Spawn(new Player { Position = new Vector2f(200, 150) });
            ShouldRestart = false;
        }
    }
}
