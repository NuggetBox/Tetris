using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Tetris
{
    enum Rotation { Up, Right, Down, Left }

    class Shape
    {
        //Rotation rotation = Rotation.Up;

        Random random = new Random();
        
        public Vector2 position;

        public Color color;

        //public List<Tile> tiles;

        public Tile[,] tiles;

        public Shape(Texture2D box, Vector2 position)
        {
            this.position = position;

            tiles = new Tile[4, 4];

            int randomized = random.Next(0, 7);

            if (randomized == 0)
            {
                color = Color.CornflowerBlue;
                tiles[0, 1] = new Tile(box, color);
                tiles[1, 1] = new Tile(box, color);
                tiles[2, 1] = new Tile(box, color);
                tiles[3, 1] = new Tile(box, color);

                this.position += new Vector2(-1, -1);
            }
            else if (randomized == 1)
            {
                color = Color.Yellow;
                tiles[0, 0] = new Tile(box, color);
                tiles[1, 0] = new Tile(box, color);
                tiles[0, 1] = new Tile(box, color);
                tiles[1, 1] = new Tile(box, color);
            }
            else if (randomized == 2)
            {
                color = Color.Purple;
                tiles[1, 1] = new Tile(box, color);
                tiles[1, 0] = new Tile(box, color);
                tiles[0, 1] = new Tile(box, color);
                tiles[2, 1] = new Tile(box, color);
            }
            else if (randomized == 3)
            {
                color = Color.Blue;
                tiles[0, 0] = new Tile(box, color);
                tiles[0, 1] = new Tile(box, color);
                tiles[1, 1] = new Tile(box, color);
                tiles[2, 1] = new Tile(box, color);
            }
            else if (randomized == 4)
            {
                color = Color.Orange;
                tiles[0, 1] = new Tile(box, color);
                tiles[1, 1] = new Tile(box, color);
                tiles[2, 1] = new Tile(box, color);
                tiles[2, 0] = new Tile(box, color);
            }
            else if (randomized == 5)
            {
                color = Color.LimeGreen;
                tiles[0, 1] = new Tile(box, color);
                tiles[1, 0] = new Tile(box, color);
                tiles[1, 1] = new Tile(box, color);
                tiles[2, 0] = new Tile(box, color);
            }
            else if (randomized == 6)
            {
                color = Color.Red;
                tiles[0, 0] = new Tile(box, color);
                tiles[1, 0] = new Tile(box, color);
                tiles[1, 1] = new Tile(box, color);
                tiles[2, 1] = new Tile(box, color);
            }


            //if (randomized == 0)
            //{
            //    color = Color.CornflowerBlue;
            //    tiles.Add(new Tile(box, color, new Vector2(-1, 0)));
            //    tiles.Add(new Tile(box, color, new Vector2(2, 0)));
            //}
            //else if (randomized == 1)
            //{
            //    color = Color.Yellow;
            //    tiles.Add(new Tile(box, color, new Vector2(0, 1)));
            //    tiles.Add(new Tile(box, color, new Vector2(1, 1)));
            //}
            //else if (randomized == 2)
            //{
            //    color = Color.Purple;
            //    tiles.Add(new Tile(box, color, new Vector2(-1, 0)));
            //    tiles.Add(new Tile(box, color, new Vector2(0, -1)));
            //}
            //else if (randomized == 3)
            //{
            //    color = Color.Blue;
            //    tiles.Add(new Tile(box, color, new Vector2(-1, -1)));
            //    tiles.Add(new Tile(box, color, new Vector2(-1, 0)));
            //}
            //else if (randomized == 4)
            //{
            //    color = Color.Orange;
            //    tiles.Add(new Tile(box, color, new Vector2(-1, 0)));
            //    tiles.Add(new Tile(box, color, new Vector2(1, -1)));
            //}
            //else if (randomized == 5)
            //{
            //    color = Color.LimeGreen;
            //    tiles.Add(new Tile(box, color, new Vector2(1, -1)));
            //    tiles.Add(new Tile(box, color, new Vector2(2, -1)));
            //}
            //else if (randomized == 6)
            //{
            //    color = Color.Red;
            //    tiles.Add(new Tile(box, color, new Vector2(0, -1)));
            //    tiles.Add(new Tile(box, color, new Vector2(-1, -1)));
            //}

            //tiles.Add(new Tile(box, color, new Vector2(0, 0)));
            //tiles.Add(new Tile(box, color, new Vector2(1, 0)));
        }

        public Shape(Shape shape)
        {
            position = shape.position;
            tiles = new Tile[4, 4];

            for (int i = 0; i < shape.tiles.GetLength(0); ++i)
            {
                for (int j = 0; j < shape.tiles.GetLength(1); ++j)
                {
                    if (shape.tiles[i, j] != null)
                    {
                        tiles[i, j] = new Tile(shape.tiles[i, j].box, shape.tiles[i, j].color);
                    }
                }
            }
        }

        public Shape(Tile[,] tiles, Vector2 position)
        {
            this.tiles = tiles;
            this.position = position;
        }

        public void Rotate(Tile[,] newPlacement)
        {
            tiles = newPlacement;
        }

        public Tile[,] TryRotation()
        {
            if (color != Color.Yellow)
            {
                Tile[,] newPlacement = new Tile[3, 3];

                if (color == Color.CornflowerBlue)
                {
                    newPlacement = new Tile[4, 4];
                }

                for (int i = 0; i < newPlacement.GetLength(0); ++i)
                {
                    for (int j = 0; j < newPlacement.GetLength(1); ++j)
                    {
                        newPlacement[newPlacement.GetLength(0) - 1 - i, j] = tiles[j, i];
                    }
                }

                return newPlacement;
            }
            else
            {
                return null;
            }
        }
    }
}
