using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZombieShootor
{
  public  class Bullet
    {
        public Vector2 pos, vel;
        float speed = 10;
       public Bullet(Vector2 Pos, Vector2 Vel)
        {
            vel = Vel;
            vel.Normalize();
            vel *= speed;
            pos = Pos + vel;
        }
        public void update()
        {
            pos += vel;
        }
        public void draw(SpriteBatch batch)
        {
            batch.Draw(Game1.bulletText, new Rectangle((int)pos.X, (int)pos.Y, 10, 10), Color.White);
        }
    }
}
