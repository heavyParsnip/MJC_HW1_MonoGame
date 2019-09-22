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
    class SpecialCollectible : GameObject
    {
        //FIELDS
        private bool active;
        private int windowHeight;
        private int windowWidth;
        Player player;

        //PROPERTIES
        public bool Active
        {
            get { return active; }
            set { active = value; }
        }

        //CONSTRUCTORS
        public SpecialCollectible(Texture2D asset, Rectangle position, int windowHeight, int windowWidth, Player player) : base(asset, position)
        {
            this.asset = asset;
            this.position = position;
            this.active = true;
            this.windowHeight = windowHeight;
            this.windowWidth = windowWidth;
            this.player = player;
        }

        //METHODS
        public override void Draw(SpriteBatch sb)
        {
            if (active == true)
            {
                base.Draw(sb);
            }
        }

        //Special collectibles run away from the player
        public override void Update(GameTime gameTime, Player player)
        {
            //Run away!
            if (position.X < player.Position.X)
            {
                position.X -= 3;
            }
            if (position.X > player.Position.X)
            {
                position.X += 3;
            }
            if (position.Y < player.Position.Y)
            {
                position.Y -= 3;
            }
            if (position.Y > player.Position.Y)
            {
                position.Y += 3;
            }

            //Keep the fleeing collectible from leaving the play area
            if (position.Y < -150)
            {
                position.Y = -149;
            }
            if (position.Y > windowHeight - 150)
            {
                position.Y = windowHeight - 149;
            }
            if (position.X < -150)
            {
                position.X = -149;
            }
            if (position.X > windowWidth - 150)
            {
                position.X = windowWidth - 149;
            }
        }

        //METHODS
        /// <summary>
        /// Method for collision checking
        /// </summary>
        public bool CheckCollision(GameObject check)
        {
            if (active == true)
            {
                if (check.Position.Intersects(this.position) == true)
                {
                    active = false;
                    return true;
                }
            }
            return false;
        }
    }
}
