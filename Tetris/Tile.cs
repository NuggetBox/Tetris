using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Tetris
{
    class Tile
    {
        public Texture2D box;
        public Color color;
        public Vector2 position;

        public Tile(Texture2D box, Color color)
        {
            this.box = box;
            this.color = color;
            position = Vector2.Zero;
        }

        public Tile(Texture2D box, Color color, Vector2 position)
        {
            this.box = box;
            this.color = color;
            this.position = position;
        }
    }
}
