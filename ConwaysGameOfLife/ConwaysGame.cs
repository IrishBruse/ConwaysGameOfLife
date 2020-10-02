using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ConwaysGameOfLife
{
    public class ConwaysGame : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        Grid grid;

        public ConwaysGame()
        {
            graphics = new GraphicsDeviceManager(this);
        }

        protected override void Initialize()
        {
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            Window.AllowUserResizing = true;
            Window.Title = "Conway's Game Of Life";
            grid = new Grid(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            Window.ClientSizeChanged += (o, e) => grid.Resize(graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            grid.LoadContent(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
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
