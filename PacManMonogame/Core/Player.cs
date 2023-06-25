using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Microsoft.Xna.Framework.Input; // Ajout reconnaissance clavier

namespace PacManMonogame.Core
{
    public class Player:GameObject
    {
        private Collision.Direction _collidedDirection;
        public Collision.Direction collidedDirection
        {
            get { return _collidedDirection; }
            set { _collidedDirection = value; }
        }


        public Player(int totalAnimationFrames, int frameWidth, int frameHeight, World world)
            : base(totalAnimationFrames, frameWidth, frameHeight, world)
        {
            direction = Collision.Direction.RIGHT;
            frameIndex = framesIndex.RIGHT_1;
            _collidedDirection = Collision.Direction.NONE;
        }

        public void Move(KeyboardState state)
        {
            if (state.IsKeyDown(Keys.Z))
            {
                direction = Collision.Direction.TOP;

                if (!Collision.Collided(this, world))
                {
                    if (collidedDirection != Collision.Direction.TOP)
                    {
                        collidedDirection = Collision.Direction.NONE;
                        Position.Y -= 1;
                    }
                }
            }
            if (state.IsKeyDown(Keys.Q))
            {
                direction = Collision.Direction.LEFT;

                if (!Collision.Collided(this, world))
                {
                    if (collidedDirection != Collision.Direction.LEFT)
                    {
                        collidedDirection = Collision.Direction.NONE;
                        Position.X -= 1;
                    }
                }
            }
            if (state.IsKeyDown(Keys.S))
            {
                direction = Collision.Direction.BOTTOM;

                if (!Collision.Collided(this, world))
                {
                    if (collidedDirection != Collision.Direction.BOTTOM)
                    {
                        collidedDirection = Collision.Direction.NONE;
                        Position.Y += 1;
                    }
                }
            }
            if (state.IsKeyDown(Keys.D))
            {
                direction = Collision.Direction.RIGHT;

                if (!Collision.Collided(this, world))
                {
                    if (collidedDirection != Collision.Direction.RIGHT)
                    {
                        collidedDirection = Collision.Direction.NONE;
                        Position.X += 1;
                    }
                }
            }
        }
    }
}
