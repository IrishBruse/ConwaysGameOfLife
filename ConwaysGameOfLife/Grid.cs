using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;

namespace ConwaysGameOfLife
{
    public class Grid
    {
        bool[,] gridA;
        bool[,] gridB;

        bool useA = true;

        Texture2D pixel;

        const int CellSize = 16;
        int width;
        int height;

        KeyboardState oldKeyboardState;

        Random rng = new Random(0);

        public Grid(int width, int height)
        {
            this.width = width / CellSize;
            this.height = height / CellSize;
            gridA = gridB = new bool[this.width, this.height];
        }

        public void Update()
        {
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                Vector2 pos = Mouse.GetState().Position.ToVector2() / CellSize;
                SetAtCurrentGeneration((int)pos.X, (int)pos.Y, true);
            }

            if (Mouse.GetState().RightButton == ButtonState.Pressed)
            {
                Vector2 pos = Mouse.GetState().Position.ToVector2() / CellSize;
                SetAtCurrentGeneration((int)pos.X, (int)pos.Y, false);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.R) == true && oldKeyboardState.IsKeyUp(Keys.R))
            {
                for (int x = 0; x < gridA.GetLength(0); x++)
                {
                    for (int y = 0; y < gridA.GetLength(1); y++)
                    {
                        if (rng.Next(0, 10) == 0)
                        {
                            SetAtCurrentGeneration(x, y, true);
                        }
                    }
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.C) == true && oldKeyboardState.IsKeyUp(Keys.C))
            {
                for (int x = 0; x < gridA.GetLength(0); x++)
                {
                    for (int y = 0; y < gridA.GetLength(1); y++)
                    {
                        SetAtCurrentGeneration(x, y, false);
                    }
                }
            }

            if ((Keyboard.GetState().IsKeyDown(Keys.Space) == true && oldKeyboardState.IsKeyUp(Keys.Space)) || Keyboard.GetState().IsKeyDown(Keys.V) == true)
            {
                if (useA == false)
                {
                    gridA = new bool[width, height];
                }
                else
                {
                    gridB = new bool[width, height];
                }

                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        int neightbourCount = GetNeighbourCount(x, y);

                        if (GetAtCurrentGeneration(x, y) == true && (neightbourCount == 2 || neightbourCount == 3))
                        {
                            SetAtNextGeneration(x, y, true);
                            continue;
                        }
                        if (GetAtCurrentGeneration(x, y) == false && neightbourCount == 3)
                        {
                            SetAtNextGeneration(x, y, true);
                            continue;
                        }

                        SetAtNextGeneration(x, y, false);
                    }
                }

                useA = !useA;
            }

            oldKeyboardState = Keyboard.GetState();
        }

        public void Resize(int width, int height)
        {
            this.width = width / CellSize;
            this.height = height / CellSize;

            bool[,] temp = useA ? gridA : gridB;

            gridA = gridB = new bool[this.width, this.height];

            int sizex = Math.Min(temp.GetLength(0), this.width);
            int sizey = Math.Min(temp.GetLength(1), this.height);

            for (int x = 0; x < sizex; x++)
            {
                for (int y = 0; y < sizey; y++)
                {
                    gridA[x, y] = gridB[x, y] = temp[x, y];
                }
            }
        }

        void SetAtNextGeneration(int x, int y, bool state)
        {
            if (useA == false)
            {
                gridA[(x + width) % width, (y + height) % height] = state;
            }
            else
            {
                gridB[(x + width) % width, (y + height) % height] = state;
            }
        }

        void SetAtCurrentGeneration(int x, int y, bool state)
        {
            if (useA == true)
            {
                gridA[(x + width) % width, (y + height) % height] = state;
            }
            else
            {
                gridB[(x + width) % width, (y + height) % height] = state;
            }
        }

        public bool GetAtCurrentGeneration(int x, int y)
        {
            if (useA == true)
            {
                return gridA[(x + width) % width, (y + height) % height];
            }
            else
            {
                return gridB[(x + width) % width, (y + height) % height];
            }
        }

        int GetNeighbourCount(int x, int y)
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

                    if (GetAtCurrentGeneration(x + X, y + Y) == true)
                    {
                        neightbourCount++;
                    }
                }
            }

            return neightbourCount;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            {
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        if (GetAtCurrentGeneration(x, y) == true)
                        {
                            spriteBatch.Draw(pixel, new Vector2((x * CellSize) + 1, (y * CellSize) + 1), null, Color.White, 0, Vector2.Zero, CellSize - 2, SpriteEffects.None, 0);
                        }
                    }
                }
            }
            spriteBatch.End();
        }

        public void LoadContent(GraphicsDevice graphicsDevice)
        {
            pixel = new Texture2D(graphicsDevice, 1, 1);
            pixel.SetData(new uint[] { 0xFFFFFFFF });
        }
    }
}