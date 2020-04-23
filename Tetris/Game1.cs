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

        List<Tile> stuckTiles = new List<Tile>();
        Shape movingShape;

        KeyboardState previousKeyboard;
        KeyboardState currentKeyboard;

        int xMax = 10;
        int yMax = 30;

        float scale = 2.5f;
        int size = 10;

        int i = 0;

        float dropTime = 0.2f;
        float dropTimer = 0f;

        float downTime = 0.3f;
        float downTimer = 0f;

        float movingTime = 0.05f;
        float movingTimer = 0;

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

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                i++;
                StickShape(new Shape(boxTexture, new Vector2(0, i)));
            }

            if (currentKeyboard.IsKeyDown(Keys.Right) && !previousKeyboard.IsKeyDown(Keys.Right))
            {
                Vector2 newPosition = new Vector2(movingShape.position.X + 1, movingShape.position.Y);

                if (CheckShapeCollision(movingShape, newPosition))
                {
                    StickShape(movingShape);
                }
                else if (!CheckWallCollision(movingShape, newPosition))
                {
                    movingShape.position = newPosition;
                }
            }
            else if (currentKeyboard.IsKeyDown(Keys.Left) && !previousKeyboard.IsKeyDown(Keys.Left))
            {
                Vector2 newPosition = new Vector2(movingShape.position.X - 1, movingShape.position.Y);

                if (CheckShapeCollision(movingShape, newPosition))
                {
                    StickShape(movingShape);
                }
                else if (!CheckWallCollision(movingShape, newPosition))
                {
                    movingShape.position = newPosition;
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

            for (int i = 0; i < stuckTiles.Count; ++i)
            {
                Tile tile = stuckTiles[i];
                spriteBatch.Draw(tile.box, size * tile.position * new Vector2(scale), null, tile.color, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
            }

            if (movingShape != null)
            {
                for (int i = 0; i < movingShape.tiles.Count; ++i)
                {
                    Tile tile = movingShape.tiles[i];
                    spriteBatch.Draw(tile.box, size * (tile.position + movingShape.position) * new Vector2(scale), null, tile.color, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
                }

                Shape projected = new Shape(movingShape);

                while (!CheckCollision(projected, new Vector2(projected.position.X, projected.position.Y + 1)))
                {
                    projected.position.Y += 1;
                }

                for (int i = 0; i < projected.tiles.Count; ++i)
                {
                    Color color = new Color(projected.tiles[i].color, 0.3f);
                    spriteBatch.Draw(projected.tiles[i].box, size * (projected.tiles[i].position + projected.position) * new Vector2(scale), null, color, 0, Vector2.Zero, scale, SpriteEffects.None, 1);
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
            for (int i = 0; i < shape.tiles.Count; ++i)
            {
                if (shape.tiles[i].position.Y + newPosition.Y >= yMax || shape.tiles[i].position.X + newPosition.X >= xMax || shape.tiles[i].position.X + newPosition.X < 0)
                {
                    return true;
                }
            }

            return false;
        }

        bool CheckShapeCollision(Shape shape, Vector2 newPosition)
        {
            for (int i = 0; i < shape.tiles.Count; ++i)
            {
                for (int j = 0; j < stuckTiles.Count; ++j)
                {
                    if (shape.tiles[i].position + newPosition == stuckTiles[j].position)
                    {
                        return true;
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

            for (int i = 0; i < shape.tiles.Count; ++i)
            {
                Tile tile = shape.tiles[i];
                stuckTiles.Add(new Tile(tile.box, tile.color, tile.position + shape.position));
            }
        }
    }
}
