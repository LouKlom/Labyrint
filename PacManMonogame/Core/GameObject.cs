using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace PacManMonogame.Core
{
    public class GameObject
    {
        public Vector2 Position;
        public Texture2D Texture;

        // Rectangle permettant de définir la zone de l'image à afficher
        public Rectangle Source;
        // Durée depuis laquelle l'image est à l'écran
        public float time;
        // Durée de visibilité d'une image
        public float frameTime = 0.1f;
        // Indice de l'image en cours
        public framesIndex frameIndex;
        // Récuperation du monde
        public World world;
        // Prise en compte des collisions
        public Collision.Direction direction;


        // Frame de déplacement
        public enum framesIndex
        {
            RIGHT_1 = 0,
            RIGHT_2 = 1,
            BOTTOM_1 = 2,
            BOTTOM_2 = 3,
            LEFT_1 = 4,
            LEFT_2 = 5,
            TOP_1 = 6,
            TOP_2 = 7
        }

        private int _totalFrames;
        public int totalFrames
        {
            get { return _totalFrames; }
        }
        private int _frameWidth;
        public int frameWidth
        {
            get { return _frameWidth; }
        }
        private int _frameHeight;
        public int frameHeight
        {
            get { return _frameHeight; }
        }




        public void UpdateFrame(GameTime gameTime)
        {
            time += (float)gameTime.ElapsedGameTime.TotalSeconds;

            while (time > frameTime)
            {
                switch (direction)
                {
                    case Collision.Direction.TOP:
                        if (frameIndex == framesIndex.TOP_1)
                            frameIndex = framesIndex.TOP_2;
                        else
                            frameIndex = framesIndex.TOP_1;
                        break;
                    case Collision.Direction.LEFT:
                        if (frameIndex == framesIndex.LEFT_1)
                            frameIndex = framesIndex.LEFT_2;
                        else
                            frameIndex = framesIndex.LEFT_1;
                        break;
                    case Collision.Direction.BOTTOM:
                        if (frameIndex == framesIndex.BOTTOM_1)
                            frameIndex = framesIndex.BOTTOM_2;
                        else
                            frameIndex = framesIndex.BOTTOM_1;
                        break;
                    case Collision.Direction.RIGHT:
                        if (frameIndex == framesIndex.RIGHT_1)
                            frameIndex = framesIndex.RIGHT_2;
                        else
                            frameIndex = framesIndex.RIGHT_1;
                        break;
                }
                this.time = 0f;
            }

            this.Source = new Rectangle(
                (int)frameIndex * frameWidth,
                0,
                frameWidth,
                frameHeight);
        }


        public GameObject()
        {
        }
        public GameObject(int totalAnimationFrames, int frameWidth, int frameHeight, World world)
        {
            _totalFrames = totalAnimationFrames;
            _frameWidth = frameWidth;
            _frameHeight = frameHeight;
            this.world = world;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(Texture, Position, Color.White);
            spriteBatch.End();
        }

        public void DrawAnimation(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(Texture, Position, Source, Color.White);
            spriteBatch.End(); 
        }


    }

}
