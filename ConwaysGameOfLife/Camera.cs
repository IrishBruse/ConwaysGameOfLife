using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ConwaysGameOfLife
{
    public class Camera
    {
        public Vector2 Position { get; private set; }
        public Vector2 Origin { get; private set; }
        public float Zoom { get; private set; } = 1;
        public Matrix Matrix { get; private set; }
        public Matrix InverseMatrix { get; private set; }

        private MouseState oldMouse;

        public void Update(GameTime time)
        {
            var mouse = Mouse.GetState();

            if (mouse.MiddleButton == ButtonState.Pressed)
            {
                Point pos = mouse.Position - oldMouse.Position;
                Position += new Vector2(pos.X, pos.Y) * 30 / (Zoom * 0.5f) * (float)time.ElapsedGameTime.TotalSeconds;
            }

            int scrollDelta = mouse.ScrollWheelValue - oldMouse.ScrollWheelValue;

            if (scrollDelta < 0)
            {
                Zoom /= 2;
                if (Zoom < 0.0125f)
                {
                    Zoom = 0.0125f;
                }
            }
            else if (scrollDelta > 0)
            {
                Zoom *= 2;
                if (Zoom > 4)
                {
                    Zoom = 4;
                }
            }

            Matrix = Matrix.CreateTranslation(Position.X, Position.Y, 0) * Matrix.CreateScale(Zoom) * Matrix.CreateTranslation(Origin.X, Origin.Y, 0);
            InverseMatrix = Matrix.Invert(Matrix);
            oldMouse = mouse;
        }

        public void Resize(int width, int height)
        {
            Origin = new Vector2(width / 2f, height / 2f);
        }
    }
}