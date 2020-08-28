using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace MonoCollision
{
    public class CollisionGame : Game
    {
        private const int MapWidth = 3000;
        private const int MapHeight = 2000;
        public static readonly Random Random = new Random(Guid.NewGuid().GetHashCode());
        private readonly GraphicsDeviceManager _graphics;

        private SpriteBatch _spriteBatch;
        private int fps;

        private DateTime prev = DateTime.Now;
        
        private readonly CollisionWorld _world = new CollisionWorld(new RectangleF(0, 0, MapWidth, MapHeight));

        public CollisionGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();

            _graphics.PreferredBackBufferHeight = MapHeight;
            _graphics.PreferredBackBufferWidth = MapWidth;
            _graphics.ApplyChanges();
            
            _world.Add(new PlayerEntity {Bounds = new RectangleF(150, 150, 50, 50)});

            for (var i = 0; i < 5000; i++)
            {
                _world.Add(new BallEntity
                {
                    Bounds = new CircleF(new Point2(Random.Next(-MapWidth, MapWidth * 2), Random.Next(0, MapHeight)),
                        Random.Next(5, 15))
                });
            }

            for (var i = 0; i < 5000; i++)
            {
                var size = Random.Next(5, 15);
                _world.Add(new CubeEntity
                {
                    Bounds = new RectangleF(new Point2(Random.Next(-MapWidth, MapWidth * 2), Random.Next(0, MapHeight)),
                        new Size2(size, size))
                });
            }
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            fps++;
            if (DateTime.Now - prev > TimeSpan.FromSeconds(1))
            {
                Window.Title = fps.ToString();
                fps = 0;
                prev = DateTime.Now;
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            _world.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            
            foreach (var entity in _world.GetEntities())
            {
                entity.Draw(_spriteBatch);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}