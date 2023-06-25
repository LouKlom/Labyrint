using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PacManMonogame.Core
{
    public static class Collision
    {
        public enum Direction
        {
            NONE = -1,
            LEFT = 0,
            RIGHT = 1,
            TOP = 2,
            BOTTOM = 3
        }

        private static Color GetColorAt(GameObject gameObject, World world)
        {
            Color color = world.collisionColor;

            if ((int)gameObject.Position.X >= 0 && (int)gameObject.Position.X < world.Texture.Width
                && (int)gameObject.Position.Y >= 0 && (int)gameObject.Position.Y < world.Texture.Height)
            {
                switch (gameObject.direction)
                {
                    case Direction.RIGHT:
                        {
                            color = world.colorTab[((int)gameObject.Position.X + gameObject.frameWidth) + ((int)gameObject.Position.Y + (gameObject.frameHeight / 2)) * world.Texture.Width];
                        }
                        break;
                    case Direction.LEFT:
                        {
                            color = world.colorTab[(int)gameObject.Position.X + ((int)gameObject.Position.Y + (gameObject.frameHeight / 2)) * world.Texture.Width];
                        }
                        break;
                    case Direction.BOTTOM:
                        {
                            color = world.colorTab[((int)gameObject.Position.X + (gameObject.frameWidth / 2)) + ((int)gameObject.Position.Y + gameObject.frameHeight) * world.Texture.Width];
                        }
                        break;
                    case Direction.TOP:
                        {
                            color = world.colorTab[((int)gameObject.Position.X + (gameObject.frameWidth / 2)) + (int)gameObject.Position.Y * world.Texture.Width];
                        }
                        break;
                }
            }

            return color;
        }

        public static bool Collided(GameObject gameObject, World world)
        {
            bool b = false;
            Color color = GetColorAt(gameObject, world);

            if (color != world.collisionColor)
                b = false;
            else
                b = true;

            return b;
        }
    }
}
