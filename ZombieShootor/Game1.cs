using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace ZombieShootor
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D playerText, wall;
        public static Texture2D bulletText, zombieText;
        public static Vector2 playerPos;
        SpriteFont font;
       public static SoundEffect fart, oof, pew;
        public static List<Bullet> bullets = new List<Bullet>();
        List<Zombie> zombies = new List<Zombie>();
        Random ranNumThing = new Random(System.Environment.TickCount);
        int curWave = 1;
        float health = 100;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            this.IsMouseVisible = true;
            this.graphics.PreferredBackBufferHeight = 1080;
            this.graphics.PreferredBackBufferWidth = 1920;
            this.graphics.IsFullScreen = true;
            this.graphics.ApplyChanges();
            playerPos = new Vector2(1920 / 2, 1080 / 2);
            Content.RootDirectory = "Content";
        }
        protected override void Initialize()
        {
            playerText = Content.Load<Texture2D>("player");
            zombieText = Content.Load<Texture2D>("zmbie");
            bulletText = Content.Load<Texture2D>("bullet");
            fart = Content.Load<SoundEffect>("fart");
            oof = Content.Load<SoundEffect>("hurt");
            pew = Content.Load<SoundEffect>("pew");
            wall = Content.Load<Texture2D>("wall");
            font = Content.Load<SpriteFont>("font");
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }
        Vector2 direction;
        float rotation;
        MouseState old;
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            MouseState nouse = Mouse.GetState();
            Vector2 mousePos = new Vector2(nouse.X, nouse.Y);
            direction = mousePos - playerPos;
            rotation = (float)Math.Atan2(direction.Y, direction.X);
            float speed = 5.5f;
            KeyboardState key = Keyboard.GetState();
            if (key.IsKeyDown(Keys.D)) playerPos += new Vector2(speed, 0);
            if (key.IsKeyDown(Keys.A)) playerPos += new Vector2(-speed, 0);
            if (key.IsKeyDown(Keys.W)) playerPos += new Vector2(0, -speed);
            if (key.IsKeyDown(Keys.S)) playerPos += new Vector2(0, speed);
            if (old.LeftButton == ButtonState.Pressed && nouse.LeftButton == ButtonState.Released)
            { bullets.Add(new Bullet(playerPos, direction)); pew.Play(); }
            for (int i = 0; i < bullets.Count; i++) bullets[i].update();
            for (int i = zombies.Count - 1; i >= 0; i--) { zombies[i].update(bullets); if (Vector2.Distance(zombies[i].pos, playerPos) < 25) { health -= .05f; oof.Play(); } if (zombies[i].health <= 0) zombies.RemoveAt(i); }
            if (zombies.Count <= 0) spawnWave();
            if (health <= 0) { zombies.Clear(); bullets.Clear(); curWave--; health = 100; }
            old = nouse;
            base.Update(gameTime);
        }
        private void spawnWave()
        {
            for (int i = 0; i < curWave; i++)
            {
                Vector2 pos = new Vector2();
                switch (ranNumThing.Next(4))
                {
                    case 0: { pos = new Vector2(ranNumThing.Next(Window.ClientBounds.Width), -100); break; }
                    case 1: { pos = new Vector2(ranNumThing.Next(Window.ClientBounds.Width), Window.ClientBounds.Height + 100); break; }
                    case 2: { pos = new Vector2(-100, ranNumThing.Next(Window.ClientBounds.Height)); break; }
                    case 3: { pos = new Vector2(Window.ClientBounds.Width + 100, ranNumThing.Next(Window.ClientBounds.Height)); break; }
                }
                zombies.Add(new Zombie(pos));

            }
            curWave++;
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.Draw(wall, Window.ClientBounds, Color.White);
            spriteBatch.Draw(playerText, playerPos, null, Color.White, rotation, new Vector2(16, 21), 1.0f, SpriteEffects.None, 0);
            spriteBatch.DrawString(font, Math.Round(health).ToString(), playerPos + new Vector2(-font.MeasureString(Math.Round(health).ToString()).X / 2, -50), Color.White);
            for (int i = 0; i < bullets.Count; i++) bullets[i].draw(spriteBatch);
            for (int i = 0; i < zombies.Count; i++) zombies[i].draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
