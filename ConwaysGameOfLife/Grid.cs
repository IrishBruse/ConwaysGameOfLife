using System;
using System.Collections.Generic;
using System.IO;
using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ConwaysGameOfLife
{
    public class Grid
    {
        private Dictionary<Point, bool> gridFrontBuffer = new Dictionary<Point, bool>();
        private Dictionary<Point, bool> gridBackbuffer = new Dictionary<Point, bool>();
        private Dictionary<Point, byte> gridNeighbourCount = new Dictionary<Point, byte>();


        private Texture2D pixel;

        private const int CellSize = 16;
        private KeyboardState oldKeyboardState;

        private readonly Camera camera;
        private Vector2 gridMousePosition;
        private FontSystem fontSystem;
        private DynamicSpriteFont coordinatesFont;
        private bool playing;

        public Grid(Camera camera)
        {
            this.camera = camera;
        }

        public void Update()
        {
            MouseState mouse = Mouse.GetState();
            KeyboardState keyboard = Keyboard.GetState();

            gridMousePosition = Vector2.Transform(mouse.Position.ToVector2(), camera.InverseMatrix);
            gridMousePosition = new Vector2(MathF.Floor(gridMousePosition.X / CellSize), MathF.Floor(gridMousePosition.Y / CellSize));

            if (mouse.LeftButton == ButtonState.Pressed)
            {
                playing = false;
                SetAtCurrentGeneration(gridMousePosition.ToPoint(), true);
            }

            if (mouse.RightButton == ButtonState.Pressed)
            {
                playing = false;
                SetAtCurrentGeneration(gridMousePosition.ToPoint(), false);
            }

            // Clear everything
            if (keyboard.IsKeyDown(Keys.R) && oldKeyboardState.IsKeyUp(Keys.R))
            {
                gridFrontBuffer = new Dictionary<Point, bool>();
            }

            if (keyboard.IsKeyDown(Keys.P) && oldKeyboardState.IsKeyUp(Keys.P))
            {
                playing = !playing;
            }

            // Run or step simulation
            if ((keyboard.IsKeyDown(Keys.Space) && oldKeyboardState.IsKeyUp(Keys.Space)) || playing)
            {
                Tick();
            }

            oldKeyboardState = keyboard;
        }

        private void Tick()
        {
            gridBackbuffer = new Dictionary<Point, bool>();
            gridNeighbourCount = new Dictionary<Point, byte>();

            foreach (var cell in gridFrontBuffer)
            {
                int neightbourCount = 0;

                for (int X = -1; X <= 1; X++)
                {
                    for (int Y = -1; Y <= 1; Y++)
                    {
                        if (X == 0 && Y == 0)
                        {
                            continue;
                        }

                        var neighbourCell = new Point(cell.Key.X + X, cell.Key.Y + Y);
                        if (GetAtCurrentGeneration(cell.Key) == true)
                        {
                            if (gridNeighbourCount.TryAdd(neighbourCell, 1) == false)
                            {
                                gridNeighbourCount[neighbourCell] += 1;
                            }
                        }

                        if (GetAtCurrentGeneration(neighbourCell) == true)
                        {
                            neightbourCount++;
                        }
                    }
                }

                bool alive = GetAtCurrentGeneration(cell.Key);

                bool rule1 = alive == true && (neightbourCount == 2 || neightbourCount == 3);
                bool rule2 = alive == false && neightbourCount == 3;

                if (rule1 || rule2)
                {
                    SetAtNextGeneration(cell.Key, true);
                    continue;
                }

                SetAtNextGeneration(cell.Key, false);
            }

            foreach (var cell in gridNeighbourCount)
            {
                if (cell.Value == 3)
                {
                    SetAtNextGeneration(cell.Key, true);
                }
            }

            gridFrontBuffer = gridBackbuffer;
        }

        private void SetAtNextGeneration(Point pos, bool state)
        {
            if (gridBackbuffer.ContainsKey(pos) == true)
            {
                gridBackbuffer[pos] = state;
            }
            else
            {
                gridBackbuffer.Add(pos, state);
            }
        }

        private void SetAtCurrentGeneration(Point pos, bool state)
        {
            if (gridFrontBuffer.ContainsKey(pos) == true)
            {
                gridFrontBuffer[pos] = state;
            }
            else
            {
                gridFrontBuffer.Add(pos, state);
            }
        }

        public bool GetAtCurrentGeneration(Point pos)
        {
            if (gridFrontBuffer.ContainsKey(pos) == true)
            {
                return gridFrontBuffer[pos];
            }
            else
            {
                return false;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: camera.Matrix);
            {
                foreach (var cell in gridFrontBuffer)
                {
                    if (GetAtCurrentGeneration(cell.Key) == true)
                    {
                        Color color = new(255, 255, 255, 255);
                        spriteBatch.Draw(pixel, new Vector2((cell.Key.X * CellSize) + 1, (cell.Key.Y * CellSize) + 1), null, color, 0, Vector2.Zero, CellSize - 2, SpriteEffects.None, 0);
                    }
                    else
                    {
                        Color color = new(128, 128, 128, 255);
                        spriteBatch.Draw(pixel, new Vector2((cell.Key.X * CellSize) + 1, (cell.Key.Y * CellSize) + 1), null, color, 0, Vector2.Zero, CellSize - 2, SpriteEffects.None, 0);
                    }
                }
            }
            spriteBatch.End();

            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            {
                spriteBatch.DrawString(coordinatesFont, $"{gridMousePosition.X},{gridMousePosition.Y}", Vector2.One * 15, Color.White);
            }
            spriteBatch.End();
        }

        public void LoadContent(GraphicsDevice graphicsDevice)
        {
            fontSystem = FontSystemFactory.Create(graphicsDevice, 1024, 1024);
            fontSystem.AddFont(File.ReadAllBytes(@"C:\\Windows\\Fonts\arial.ttf"));

            coordinatesFont = fontSystem.GetFont(24);

            pixel = new Texture2D(graphicsDevice, 1, 1);
            pixel.SetData(new uint[] { 0xFFFFFFFF });
        }
    }
}