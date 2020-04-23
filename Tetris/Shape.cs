using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Tetris
{
    class Shape
    {
        Random random = new Random();
        
        public Vector2 position;

        public List<Tile> tiles;

        public Shape(Texture2D box, Vector2 position)
        {
            Color color = Color.White;
            this.position = position;
            tiles = new List<Tile>();
            
            int randomized = random.Next(0, 7);

            if (randomized == 0)
            {
                color = Color.CornflowerBlue;
                tiles.Add(new Tile(box, color, new Vector2(0, -2)));
                tiles.Add(new Tile(box, color, new Vector2(0, -3)));
            }
            else if (randomized == 1)
            {
                color = Color.Yellow;
                tiles.Add(new Tile(box, color, new Vector2(1, 0)));
                tiles.Add(new Tile(box, color, new Vector2(1, -1)));
            }
            else if (randomized == 2)
            {
                color = Color.Purple;
                tiles.Add(new Tile(box, color, new Vector2(1, -1)));
                tiles.Add(new Tile(box, color, new Vector2(-1, -1)));
            }
            else if (randomized == 3)
            {
                color = Color.Blue;
                tiles.Add(new Tile(box, color, new Vector2(-1, 0)));
                tiles.Add(new Tile(box, color, new Vector2(0, -2)));
            }
            else if (randomized == 4)
            {
                color = Color.Orange;
                tiles.Add(new Tile(box, color, new Vector2(1, 0)));
                tiles.Add(new Tile(box, color, new Vector2(0, -2)));
            }
            else if (randomized == 5)
            {
                color = Color.LimeGreen;
                tiles.Add(new Tile(box, color, new Vector2(-1, 0)));
                tiles.Add(new Tile(box, color, new Vector2(1, -1)));
            }
            else if (randomized == 6)
            {
                color = Color.Red;
                tiles.Add(new Tile(box, color, new Vector2(-1, -1)));
                tiles.Add(new Tile(box, color, new Vector2(1, 0)));
            }

            tiles.Add(new Tile(box, color, new Vector2(0, 0)));
            tiles.Add(new Tile(box, color, new Vector2(0, -1)));
        }

        public Shape(Shape shape)
        {
            position = shape.position;
            tiles = new List<Tile>();

            for (int i = 0; i < shape.tiles.Count; ++i)
            {
                tiles.Add(new Tile(shape.tiles[i].box, shape.tiles[i].color, shape.tiles[i].position));
            }
        }
    }
}
