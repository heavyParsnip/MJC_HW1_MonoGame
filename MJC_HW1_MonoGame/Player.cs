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
    class Player : GameObject
    {
        //FIELDS
        private int levelScore;
        private int totalScore;
        private int windowHeight;
        private int windowWidth;

        //PROPERTIES
        public int LevelScore
        {
            get { return levelScore; }
            set { levelScore = value; }
        }

        public int TotalScore
        {
            get { return totalScore; }
            set { totalScore = value; }
        }

        //CONSTRUCTORS
        public Player(Texture2D asset, Rectangle position, int windowHeight, int windowWidth) : base(asset, position)
        {
            levelScore = 0;
            totalScore = 0;
            this.asset = asset;
            this.position = position;
            this.windowHeight = windowHeight;
            this.windowWidth = windowWidth;
            
        }

        //METHODS
        public override void Update(GameTime gameTime, Player player)
        {
            KeyboardState kbState = Keyboard.GetState();

            //Use a series of if statements to account for multiple buttons being pressed at once
            if(kbState.IsKeyDown(Keys.Left) == true)
            {
                position.X -= 8;
            }
            if(kbState.IsKeyDown(Keys.Right) == true)
            {
                position.X += 8;
            }
            if(kbState.IsKeyDown(Keys.Up) == true)
            {
                position.Y -= 8;
            }
            if(kbState.IsKeyDown(Keys.Down) == true)
            {
                position.Y += 8;
            }

            //Screen wrapping
            if(position.Y < -165)
            {
                position.Y += windowHeight;
            }
            if(position.Y > windowHeight - 165)
            {
                position.Y -= windowHeight;
            }
            if(position.X < -118)
            {
                position.X += windowWidth;
            }
            if(position.X > windowWidth - 118)
            {
                position.X -= windowWidth;
            }
        }

        /// <summary>
        /// Sets the player's position to the center of the window
        /// </summary>
        public void Center()
        {
            position.X = (windowWidth / 2) - 118;
            position.Y = (windowHeight / 2) - 165;
        }
    }
}
