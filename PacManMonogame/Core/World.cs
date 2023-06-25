using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PacManMonogame.Core
{
    public class World:GameObject
    {
        public Vector2 Position;
        public Texture2D Texture;
        public Color[] colorTab; // Pour différencier les couleurs (murs/non murs) du monde

        private Color _collisionColor;
        public Color collisionColor
        {
            get { return _collisionColor; }
        }

        // Constructeur
        public World(Color collisionColor)
        {
            _collisionColor = collisionColor;
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Color.White);
        }


    }
}
