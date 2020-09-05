using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
        
        private readonly CollisionWorld _world = new CollisionWorld();

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
            //
            // _world.Add(new PlayerEntity {Bounds = new RectangleF(150, 150, 50, 50)});

            var circle = default(BroadphaseCircle);
            circle.Center = new Vector2(100, 100);
            circle.Radius = 50;
            var ball = new BallEntity();
            _world.CreateCollider(ball, circle);
            
            // for (var i = 0; i < 500; i++)
            // {
            //     var circle = default(BroadphaseCircle);
            //     circle.Center = new Vector2(Random.Next(-MapWidth, MapWidth * 2), Random.Next(0, MapHeight));
            //     circle.Radius = Random.Next(5, 15);
            //     var ball = new BallEntity();
            //     _world.CreateCollider(ball, circle);
            // }
            
            var rectangle = default(BroadphaseCircle);
            rectangle.Center = new Vector2(100, 50);
            rectangle.Radius = 100;
            var box = new CubeEntity();
            _world.CreateCollider(box, rectangle);

            // for (var i = 0; i < 500; i++)
            // {
            //     var rectangle = default(BroadphaseRectangle);
            //     rectangle.Maximum = new Vector2(Random.Next(-MapWidth, MapWidth * 2), Random.Next(0, MapHeight));
            //     rectangle.Minimum = new Vector2(Random.Next(-MapWidth, MapWidth * 2), Random.Next(0, MapHeight));
            //     var box = new CubeEntity();
            //     _world.CreateCollider(box, rectangle);
            // }
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
            
            // foreach (var entity in _world.GetEntities())
            // {
            //     entity.Draw(_spriteBatch);
            // }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}