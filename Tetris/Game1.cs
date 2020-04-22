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

        int xMax = 10;
        int yMax = 30;

        float scale = 2.5f;
        int size = 10;

        int i = 0;

        float dropTime = 0.2f;
        float dropTimer = 0;

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

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                i++;
                StickShape(new Shape(boxTexture, new Vector2(0, i)));
            }

            dropTimer += delta;

            if (dropTimer >= dropTime)
            {
                DropDown(movingShape);
                dropTimer = 0;
            }


            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

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
            }
            
            spriteBatch.End();

            base.Draw(gameTime);
        }

        void DropDown(Shape shape)
        {
            Vector2 newPosition = new Vector2(shape.position.X, shape.position.Y + 1);

            for (int i = 0; i < shape.tiles.Count; ++i)
            {
                for (int j = 0; j < stuckTiles.Count; ++j)
                {
                    if (shape.tiles[i].position + newPosition == stuckTiles[j].position)
                    {
                        StickShape(shape);
                        return;
                    }
                }

                if (shape.tiles[i].position.Y + newPosition.Y >= yMax)
                {
                    StickShape(shape);
                    return;
                }
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
