using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MJC_HW1_MonoGame
{
    class Collectible : GameObject
    {
        //FIELDS
        private bool active;

        //PROPERTIES
        public bool Active
        {
            get { return active; }
        }

        //CONSTRUCTORS
        public Collectible(Texture2D asset, Rectangle position) : base(asset, position)
        {
            this.asset = asset;
            this.position = position;
            this.active = true;
        }

        //METHODS
        /// <summary>
        /// Method for collision checking
        /// </summary>
        public bool CheckCollision(GameObject check)
        {
            if(active == true)
            {
                if(check.Position.Intersects(this.position) == true)
                {
                    active = false;
                    return true;
                }
            }
            return false;
        }

        public override void Draw(SpriteBatch sb)
        {
            if(active == true)
            {
                base.Draw(sb);
            }
        }

        public override void Update(GameTime gameTime, Player player)
        {

        }
    }
}
