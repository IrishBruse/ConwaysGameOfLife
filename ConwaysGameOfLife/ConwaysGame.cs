using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ConwaysGameOfLife
{
    public class ConwaysGame : Game
    {
        private readonly GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Camera camera;
        Grid grid;

        public ConwaysGame()
        {
            graphics = new GraphicsDeviceManager(this);
        }

        protected override void Initialize()
        {
            Window.Title = "Conway's Game Of Life";
            Window.AllowUserResizing = true;
            IsMouseVisible = true;


            camera = new Camera();
            camera.Resize(graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height);
            Window.ClientSizeChanged += (o, e) => camera.Resize(graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height);

            grid = new Grid(camera);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            grid.LoadContent(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            camera.Update(gameTime);
            grid.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(96, 96, 96, 255));

            grid.Draw(spriteBatch);

            base.Draw(gameTime);
        }
    }
}