using Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Systems;

namespace Entities
{
    public class Bullet
    {
        private const float MOVE_AMOUNT = 2f/1000; // in game units to move each millisecond
        private const int RANGE = 3;
        static Texture2D bulletSprite;
        public static void init(ContentManager content)
        {
            bulletSprite = content.Load<Texture2D>("Sprites/SquareSprite");
        }
        public static Entity createProjectile(float x, float y, Entity target, uint level)
        {
            var bullet = new Entity();

            bullet.Add(new Components.Sprite(bulletSprite, Color.Yellow, Color.Black, .75f));
            bullet.Add(new Components.Position(x, y, .1f, .1f));

            var targetPos = target.GetComponent<Components.Position>();

            var angle = CoordinateSystem.angle(new Vector2(x, y), new Vector2(targetPos.x, targetPos.y));
            bullet.Add(new Components.Orientation(90));
            var angleComp = bullet.GetComponent<Components.Orientation>();
            angleComp.radians = angle;

            bullet.Add(new Components.Damage((uint)Math.Pow(2, level*2)));
            bullet.Add(new Components.BulletComponent(target, MOVE_AMOUNT * (float)(level+1)));
            return bullet;
        }

        public static Entity createMissle(float x, float y, Entity target, uint level, TargetType type)
        {
            var bullet = new Entity();

            bullet.Add(new Components.Sprite(bulletSprite, Color.Yellow, Color.Black, .75f));
            bullet.Add(new Components.Position(x, y, .1f, .1f));

            var targetPos = target.GetComponent<Components.Position>();

            var angle = CoordinateSystem.angle(new Vector2(x, y), new Vector2(targetPos.x, targetPos.y));
            bullet.Add(new Components.Orientation(90));
            var angleComp = bullet.GetComponent<Components.Orientation>();
            angleComp.radians = angle;

            bullet.Add(new Components.Damage((uint)Math.Pow(4, level * 2)));
            bullet.Add(new Components.BulletComponent(target, MOVE_AMOUNT * (float)(level + 1), Components.bulletType.missle));
            bullet.Add(new Components.Bomb(RANGE-1, type));
            return bullet;
        }

        public static Entity createBomb(float x, float y, Entity target, uint level, TargetType type)
        {
            var bullet = new Entity();

            bullet.Add(new Components.Sprite(bulletSprite, Color.Yellow, Color.Black, .75f));
            bullet.Add(new Components.Position(x, y, .1f, .1f));

            var targetPos = target.GetComponent<Components.Position>();

            var angle = CoordinateSystem.angle(new Vector2(x, y), new Vector2(targetPos.x, targetPos.y));
            bullet.Add(new Components.Orientation(90));
            var angleComp = bullet.GetComponent<Components.Orientation>();
            angleComp.radians = angle;

            bullet.Add(new Components.Damage((uint)Math.Pow(4, level * 2)));
            bullet.Add(new Components.BulletComponent(target, MOVE_AMOUNT * 3 * (float)(level + 1), Components.bulletType.bomb, new Vector2(targetPos.x, targetPos.y)));
            bullet.Add(new Components.Bomb(RANGE, type));
            return bullet;
        }
    }
}