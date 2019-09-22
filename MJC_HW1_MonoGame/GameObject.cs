using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace MJC_HW1_MonoGame
{
    abstract class GameObject
    {
        //FIELDS
        protected Texture2D asset;
        protected Rectangle position;

        //PROPERTIES
        public Rectangle Position
        {
            get { return position; }
        }

        //CONSTRUCTORS
        public GameObject(Texture2D asset, Rectangle position)
        {
            this.asset = asset;
            this.position = position;
        }
        
        //METHODS
        /// <summary>
        /// Uses the SpriteBatch object to draw the GameObject
        /// </summary>
        /// <param name="sb"></param>
        public virtual void Draw(SpriteBatch sb)
        {
            sb.Draw(asset, position, Color.White);
        }

        /// <summary>
        /// Allows the GameObject to update itself
        /// </summary>
        public abstract void Update(GameTime gameTime, Player player);
    }
}
