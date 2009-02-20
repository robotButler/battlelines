#region File Description
//-----------------------------------------------------------------------------
// BattleLinesGame.cs
//
// Harbinger Games LLC
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Net;
#endregion

namespace BattleLines
{
    /// <summary>
    /// The main type for this game.
    /// </summary>
    public class BattleLinesGame : Microsoft.Xna.Framework.Game
    {
        #region Graphics Data


        /// <summary>
        /// The graphics device manager used to render the game.
        /// </summary>
        GraphicsDeviceManager graphics;


        #endregion


        #region Game State Management Data


        /// <summary>
        /// The manager for all of the user-interface data.
        /// </summary>
        ScreenManager screenManager;


        #endregion


        #region Initialization Methods


        /// <summary>
        /// Constructs a new NetRumbleGame object.
        /// </summary>
        public BattleLinesGame()
        {
            // initialize the graphics device manager
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            graphics.MinimumVertexShaderProfile = ShaderProfile.VS_1_1;
            graphics.MinimumPixelShaderProfile = ShaderProfile.PS_2_0;

            // initialize the content manager
            Content.RootDirectory = "Content";

            // initialize the gamer-services component
            //   this component enables Live sign-in functionality
            //   and updates the Gamer.SignedInGamers collection.
            Components.Add(new GamerServicesComponent(this));

            // initialize the screen manager
            screenManager = new ScreenManager(this);
            Components.Add(screenManager);

            // initialize the audio system
            AudioManager.Initialize(this, "Content/Audio/NetRumble.xgs",
                "Content/Audio/NetRumble.xwb", "Content/Audio/NetRumble.xsb");
            
            IsMouseVisible = true;
        }


        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting.
        /// This is where it can query for any required services and load any
        /// non-graphic related content.  Calling base.Initialize will enumerate through
        /// any components and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            InputManager.Initialize();
            base.Initialize();

            // load the initial screens
            //screenManager.AddScreen(new BackgroundScreen());
            screenManager.AddScreen(new MainMenuScreen());
        }


        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        #endregion

        #region Update

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            InputManager.Update();

            base.Update(gameTime);
        }

        #endregion


        #region Drawing Methods


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.White);

            base.Draw(gameTime);
        }


        #endregion


        #region Entry Point


        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            using (BattleLinesGame game = new BattleLinesGame())
            {
                game.Run();
            }
        }


        #endregion

    }
}
