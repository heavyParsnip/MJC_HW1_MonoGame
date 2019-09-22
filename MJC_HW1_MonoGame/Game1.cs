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
    /// <summary>
    /// Enum for game state
    /// </summary>
    enum GameState
    {
        Menu,
        Game,
        GameOver
    }


    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        //FIELDS
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        int width;
        int height;
        int level;
        double timer;

        GameState CurrentState = GameState.Menu;
        Player player;
        List<Collectible> collectibles = new List<Collectible>();
        List<SpecialCollectible> specialCollectibles = new List<SpecialCollectible>();
        KeyboardState kbState;
        KeyboardState previousKbState;
        Random random = new Random();

        Texture2D playerTexture;
        Texture2D collectibleTexture;
        SpriteFont gameFont;
        SpriteFont titleFont;

        //CONSTRUCTORS
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1600;
            graphics.PreferredBackBufferHeight = 1200;
            graphics.ApplyChanges();
            Content.RootDirectory = "Content";
        }

        //METHODS
        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            width = graphics.GraphicsDevice.Viewport.Width;
            height = graphics.GraphicsDevice.Viewport.Height;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            playerTexture = Content.Load<Texture2D>("bee");
            collectibleTexture = Content.Load<Texture2D>("flower");
            player = new Player(
                playerTexture,
                new Rectangle(
                    (width / 2) - 118,
                    (height / 2) - 165,
                    236,
                    330
                    ),
                height,
                width);
            gameFont = Content.Load<SpriteFont>("SampleText18");
            titleFont = Content.Load<SpriteFont>("SampleText32");

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //Update logic!
            previousKbState = kbState;
            kbState = Keyboard.GetState();

            switch (CurrentState)
            {
                case GameState.Menu:
                    if(SingleKeyPress(Keys.Enter) == true)
                    {
                        ResetGame();
                        CurrentState = GameState.Game;
                    }
                    break;
                case GameState.Game:
                    timer -= gameTime.ElapsedGameTime.TotalSeconds;
                    player.Update(gameTime, player);
                    
                    //Check for collectible collision
                    foreach(Collectible c in collectibles)
                    {
                        if(c.CheckCollision(player) == true)
                        {
                            player.LevelScore += 10;
                            player.TotalScore += 10;
                        }
                    }
                    foreach(SpecialCollectible s in specialCollectibles)
                    {
                        if(s.CheckCollision(player) == true)
                        {
                            player.LevelScore += 20;
                            player.TotalScore += 20;
                        }
                    }

                    int collectiblesLeft = 1 + level + specialCollectibles.Count;
                    foreach(Collectible c in collectibles)
                    {
                        if(c.Active == false)
                        {
                            collectiblesLeft--;
                        }
                    }
                    foreach(SpecialCollectible s in specialCollectibles)
                    {
                        if(s.Active == false)
                        {
                            collectiblesLeft--;
                        }
                    }

                    //Determine game state
                    if (timer <= 0.00)
                    {
                        CurrentState = GameState.GameOver;
                    }
                    else if(collectiblesLeft == 0)
                    {
                        NextLevel();
                    }
                    break;
                case GameState.GameOver:
                    if (SingleKeyPress(Keys.Enter) == true)
                    {
                        CurrentState = GameState.Menu;
                    }
                    break;
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.FloralWhite);

            //Drawing code!
            spriteBatch.Begin();
            switch (CurrentState)
            {
                case GameState.Menu:
                    Vector2 titleVec = new Vector2((width / 2) - (width / 24), (height / 2) - (height / 8));
                    Vector2 InstructVecA = new Vector2((width / 2) - 400, (height / 2) + 80);

                    spriteBatch.DrawString(titleFont, "Bees?", titleVec, Color.Black);
                    spriteBatch.DrawString(gameFont, "Use the arrow keys to move around & collect flowers before time runs out!", InstructVecA, Color.Black);
                    break;
                case GameState.Game:
                    player.Draw(spriteBatch);
                    foreach (Collectible c in collectibles)
                    {
                        c.Draw(spriteBatch);
                        c.Update(gameTime, player);
                    }
                    foreach (SpecialCollectible s in specialCollectibles)
                    {
                        s.Draw(spriteBatch);
                        s.Update(gameTime, player);
                    }


                    Vector2 levelVec = new Vector2(20, 20);
                    Vector2 scoreVec = new Vector2(20, 60);
                    Vector2 timerVec = new Vector2(20, 100);
                    Vector2 vectorVec = new Vector2(20, 140);

                    spriteBatch.DrawString(gameFont, $"Level: {level}", levelVec, Color.Black);
                    spriteBatch.DrawString(gameFont, $"Score: {player.LevelScore}", scoreVec, Color.Black);
                    spriteBatch.DrawString(gameFont, $"Time left: {String.Format("{0:0.00}", timer)}", timerVec, Color.Black);
                    spriteBatch.DrawString(gameFont, $"Position: ({player.Position.X},{player.Position.Y})", vectorVec, Color.Black);

                    break;
                case GameState.GameOver:
                    Vector2 gameOverVec = new Vector2((width / 2) - (width / 20), (height / 2) - (height / 8));
                    Vector2 finalLevelVec = new Vector2((width / 2) - 100, (height / 2) + 80);
                    Vector2 finalScoreVec = new Vector2((width / 2) - 100, (height / 2) + 120);
                    Vector2 returnVec = new Vector2((width / 2) - 100, (height / 2) + 160);

                    spriteBatch.DrawString(titleFont, "Game Over", gameOverVec, Color.Black);
                    spriteBatch.DrawString(gameFont, $"You made it to level {level}!", finalLevelVec, Color.Black);
                    spriteBatch.DrawString(gameFont, $"Final score: {player.TotalScore}", finalScoreVec, Color.Black);
                    spriteBatch.DrawString(gameFont, "Press Enter to return to the menu...", returnVec, Color.Black);

                    break;
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Advance to the next level
        /// </summary>
        private void NextLevel()
        {
            level += 1;
            timer = (10.0);
            player.LevelScore = 0;
            player.Center();
            collectibles.Clear();
            specialCollectibles.Clear();
            Point collectibleSize = new Point(300, 300);

            int numOfCollectibles = (1 + level);

            for(int i = 0; i < numOfCollectibles; i++)
            {
                Point randomPoint = new Point(random.Next(0,width-150), random.Next(0,height-150));
                Collectible newCollectible = new Collectible(collectibleTexture, new Rectangle(randomPoint, collectibleSize));
                collectibles.Add(newCollectible);
            }
            if(level > 2)
            {
                Point randomPoint = new Point(random.Next(0,width-150), random.Next(0,height-150));
                SpecialCollectible newSpecialCollectible = new SpecialCollectible(
                    collectibleTexture, 
                    new Rectangle(randomPoint, collectibleSize), 
                    height,
                    width,
                    player);
                specialCollectibles.Add(newSpecialCollectible);
                numOfCollectibles++;
            }
        }

        /// <summary>
        /// Sets up the game after leaving the menu state
        /// </summary>
        private void ResetGame()
        {
            level = 0;
            player.TotalScore = 0;
            NextLevel();
        }

        /// <summary>
        /// Method for a singular key press
        /// </summary>
        private bool SingleKeyPress(Keys key)
        {
            try
            {
                if (kbState.IsKeyDown(key) == true && previousKbState.IsKeyUp(key) == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }

        }
    }
}
