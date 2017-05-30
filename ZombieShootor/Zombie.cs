using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZombieShootor
{
    class Zombie
    {
       public Vector2 pos;
        float speed = 5;
        public int health = 100;
        public Zombie(Vector2 Pos)
        {
            pos = Pos;
        }
        float rotation;
        public void update(List<Bullet> bullets)
        {
            Vector2 direction = Game1.playerPos - pos;
            direction.Normalize();
            direction *= speed;
            pos += direction;
            rotation = (float)Math.Atan2(direction.Y, direction.X);
            for (int i = bullets.Count - 1; i >= 0; i--)
                if (Vector2.Distance(pos, bullets[i].pos) < 40)
                {
                    Game1.fart.Play();
                    health -= 34;
                    Game1.bullets.RemoveAt(i);
                }
        }
        public void draw(SpriteBatch batch)
        {
            batch.Draw(Game1.zombieText, pos, null, Color.White, rotation, new Vector2(16, 21), 1.0f, SpriteEffects.None, 0);

        }
    }
}
