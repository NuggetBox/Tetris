using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Tetris
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Vector2 start = new Vector2(4, 0);
        Texture2D boxTexture;

        Tile[,] stuckTiles;
        Shape movingShape;

        KeyboardState previousKeyboard;
        KeyboardState currentKeyboard;

        int xMax = 10;
        int yMax = 30;

        float scale = 2.5f;
        int size = 10;

        float dropTime = 1f;
        float dropTimer = 0f;

        float downTime = 0.3f;
        float downTimer = 0f;

        float movingTime = 0.05f;
        float movingTimer = 0;

        int level = 0;
        int clearedRows = 0;
        int score = 0;
        int[] startScores = new int[]
        {
            40, 100, 300, 1200
        };

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = (int)(size * scale * xMax);
            graphics.PreferredBackBufferHeight = (int)(size * scale * yMax);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            stuckTiles = new Tile[xMax, yMax];

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            boxTexture = Content.Load<Texture2D>("box");
            movingShape = new Shape(boxTexture, start);

            // TODO: use this.Content to load your game content here
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            currentKeyboard = Keyboard.GetState();

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            if (currentKeyboard.IsKeyDown(Keys.Right) && !previousKeyboard.IsKeyDown(Keys.Right))
            {
                Vector2 newPosition = new Vector2(movingShape.position.X + 1, movingShape.position.Y);

                if (!CheckWallCollision(movingShape, newPosition) && !CheckShapeCollision(movingShape, newPosition))
                {
                    movingShape.position = newPosition;
                }
            }
            else if (currentKeyboard.IsKeyDown(Keys.Left) && !previousKeyboard.IsKeyDown(Keys.Left))
            {
                Vector2 newPosition = new Vector2(movingShape.position.X - 1, movingShape.position.Y);

                if (!CheckWallCollision(movingShape, newPosition) && !CheckShapeCollision(movingShape, newPosition))
                {
                    movingShape.position = newPosition;
                }
            }
            else if (currentKeyboard.IsKeyDown(Keys.Up) && !previousKeyboard.IsKeyDown(Keys.Up))
            {
                Tile[,] newPlacement = movingShape.TryRotation();
                Shape tempRotated = new Shape(newPlacement, movingShape.position);

                if (!CheckCollision(tempRotated, tempRotated.position))
                {
                    movingShape.tiles = newPlacement;
                }
            }

            Vector2 downPosition = new Vector2(movingShape.position.X, movingShape.position.Y + 1);

            if (currentKeyboard.IsKeyDown(Keys.Down) && !previousKeyboard.IsKeyDown(Keys.Down))
            {
                if (CheckCollision(movingShape, downPosition))
                {
                    StickShape(movingShape);
                }
                else
                {
                    movingShape.position = downPosition;
                }
            }
            else if (currentKeyboard.IsKeyDown(Keys.Down))
            {
                downTimer += delta;

                if (downTimer >= downTime)
                {
                    movingTimer += delta;

                    if (movingTimer >= movingTime)
                    {
                        if (CheckCollision(movingShape, downPosition))
                        {
                            StickShape(movingShape);
                        }
                        else
                        {
                            movingShape.position = downPosition;
                        }

                        movingTimer = 0;
                    }
                }
                else
                {
                    movingTimer = 0;
                }
            }
            else
            {
                downTimer = 0;
            }

            dropTimer += delta;

            if (dropTimer >= dropTime)
            {
                DropDown(movingShape);
                dropTimer = 0;
            }

            previousKeyboard = currentKeyboard;

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp);

            for (int i = 0; i < stuckTiles.GetLength(0); ++i)
            {
                for (int j = 0; j < stuckTiles.GetLength(1); ++j)
                {
                    Tile tile = stuckTiles[i, j];

                    if (tile != null)
                    {
                        spriteBatch.Draw(tile.box, size * tile.position * new Vector2(scale), null, tile.color, 0, Vector2.Zero, scale, SpriteEffects.None, 0);

                    }
                }
            }

            if (movingShape != null)
            {
                for (int i = 0; i < movingShape.tiles.GetLength(0); ++i)
                {
                    for (int j = 0; j < movingShape.tiles.GetLength(1); ++j)
                    {
                        Tile tile = movingShape.tiles[i, j];

                        if (tile != null)
                        {
                            spriteBatch.Draw(tile.box, size * (new Vector2(i, j) + movingShape.position) * new Vector2(scale), null, tile.color, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
                        }
                    }
                }

                Shape projected = new Shape(movingShape);

                while (!CheckCollision(projected, new Vector2(projected.position.X, projected.position.Y + 1)))
                {
                    projected.position.Y += 1;
                }

                for (int i = 0; i < projected.tiles.GetLength(0); ++i)
                {
                    for (int j = 0; j < projected.tiles.GetLength(1); ++j) 
                    {
                        if (projected.tiles[i, j] != null)
                        {
                            Color color = new Color(projected.tiles[i, j].color, 0.3f);
                            spriteBatch.Draw(projected.tiles[i, j].box, size * (new Vector2(i, j) + projected.position) * new Vector2(scale), null, color, 0, Vector2.Zero, scale, SpriteEffects.None, 1);
                        }
                        
                    }
                }
            }
            
            spriteBatch.End();

            base.Draw(gameTime);
        }

        bool CheckCollision(Shape shape, Vector2 newPosition)
        {
            return CheckShapeCollision(shape, newPosition) || CheckWallCollision(shape, newPosition);
        }

        bool CheckWallCollision(Shape shape, Vector2 newPosition)
        {
            for (int i = 0; i < shape.tiles.GetLength(0); ++i)
            {
                for (int j = 0; j < shape.tiles.GetLength(1); ++j)
                {
                    if (shape.tiles[i, j] != null)
                    {
                        if (j + newPosition.Y >= yMax || i + newPosition.X >= xMax || i + newPosition.X < 0)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        bool CheckShapeCollision(Shape shape, Vector2 newPosition)
        {
            for (int i = 0; i < shape.tiles.GetLength(0); ++i)
            {
                for (int j = 0; j < shape.tiles.GetLength(1); ++j)
                {
                    for (int k = 0; k < stuckTiles.GetLength(0); ++k)
                    {
                        for (int l = 0; l < stuckTiles.GetLength(1); ++l)
                        {
                            if (stuckTiles[k, l] != null)
                            {
                                if (shape.tiles[i, j] != null)
                                {
                                    if (new Vector2(i, j) + newPosition == stuckTiles[k, l].position)
                                    {
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return false;
        }

        void DropDown(Shape shape)
        {
            Vector2 newPosition = new Vector2(shape.position.X, shape.position.Y + 1);

            if (CheckCollision(shape, newPosition))
            {
                StickShape(shape);
            }

            shape.position = newPosition;
        }

        void StickShape(Shape shape)
        {
            movingShape = new Shape(boxTexture, start);

            for (int i = 0; i < shape.tiles.GetLength(0); ++i)
            {
                for (int j = 0; j < shape.tiles.GetLength(1); ++j)
                {
                    Tile tile = shape.tiles[i, j];

                    if (tile != null)
                    {
                        stuckTiles[(int)(i + shape.position.X), (int)(j + shape.position.Y)] = new Tile(tile.box, tile.color, new Vector2(i, j) + shape.position);
                    }
                }
            }

            // Lose
            if (CheckShapeCollision(movingShape, start))
            {
                Exit();
            }

            ClearRows();
        }

        void ClearRows()
        {
            int cleared = 0;

            for (int i = yMax - 1; i >= 0; --i)
            {
                for (int j = 0; j < xMax; ++j)
                {
                    if (stuckTiles[j, i] == null)
                    {
                        break;
                    }
                    else
                    {
                        if (j == xMax - 1)
                        {
                            ClearRow(i, true);
                            cleared++;
                        }
                    }
                }
            }

            if (cleared > 0)
            {
                score += (level + 1) * startScores[cleared - 1];
                clearedRows += cleared;

                if (clearedRows >= 10)
                {
                    level++;
                    clearedRows = 0;
                }
            }
        }

        void ClearRow(int index, bool leftToRight)
        {
            for (int i = 0; i < xMax; ++i)
            {
                stuckTiles[i, index] = null;
            }

            DropRows(index);
        }

        void DropRows(int clearedRow)
        {
            for (int i = clearedRow - 1; i >= 0; --i)
            {
                for (int j = 0; j < xMax; ++j)
                {
                    Tile tile = stuckTiles[j, i];

                    if (tile != null)
                    {
                        stuckTiles[j, i + 1] = new Tile(tile.box, tile.color, new Vector2(tile.position.X, tile.position.Y + 1));
                        stuckTiles[j, i] = null;
                    }
                }
            }
        }
    }
}
