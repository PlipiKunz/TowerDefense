using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Components
{
    public class Sprite : Component
    {
        public Texture2D image;
        public Color fill;
        public Color stroke;
        public float priority;
        public bool rotatable;
        public Sprite(Texture2D image, Color fill, Color stroke, float priority = 0, bool rotatable = false)
        {
            this.image = image;
            this.fill = fill;
            this.stroke = stroke;
            this.priority = priority;
            this.rotatable = rotatable;
        }
    }
}
