using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Components
{
    public class AnimatedSprite : Component
    {
        public Texture2D image;
        public Color fill;
        public Color stroke;
        public float priority;
        public bool rotatable;

        public int frame = 1;
        public const float standardTimeTillNextFrame = 100;

        public float curTimeTillNextFrame;
        public int curFrame = 2;

        public int frames;
        public int frameWidth;
        public AnimatedSprite(Texture2D image, int frames, int frameWidth,  Color fill, Color stroke, float priority = .5f, bool rotatable = false)
        {
            this.image = image;
            this.fill = fill;
            this.stroke = stroke;
            this.priority = priority;
            this.rotatable = rotatable;

            this.frames = frames;
            this.frameWidth = frameWidth;

            this.curTimeTillNextFrame = standardTimeTillNextFrame;
        }

        public void update(GameTime gt) {
            curTimeTillNextFrame -= (float)gt.ElapsedGameTime.TotalMilliseconds;
            if (curTimeTillNextFrame < 0)
            {
                curTimeTillNextFrame = standardTimeTillNextFrame;
                curFrame++;
                curFrame %= frames;
            }
        }
    }
}
