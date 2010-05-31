#region File Description
//-----------------------------------------------------------------------------
// MainMenuScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RolePlayingGameData;
using Microsoft.Xna.Framework.Media;
#endregion

namespace RolePlaying
{
    /// <summary>
    /// The main menu screen is the first thing displayed when the game starts up.
    /// </summary>
    class MainMenuScreen : MenuScreen
    {
        #region Graphics Data
        
        private Texture2D selectTexture;
        private Vector2 selectPosition;
        Video mainLoop;
        VideoPlayer mainMenuLoopPlayer;

        #endregion

        #region Menu Entries

        MenuEntry newGameMenuEntry, exitGameMenuEntry;
        MenuEntry optionsMenuEntry, extrasMenuEntry;

        #endregion


        #region Initialization


        /// <summary>
        /// Constructor fills in the menu contents.
        /// </summary>
        public MainMenuScreen()
            : base()
        {
            mainMenuLoopPlayer = new VideoPlayer();
        }


        /// <summary>
        /// Load the graphics content for this screen.
        /// </summary>
        public override void LoadContent()
        {
            
            // load the textures
            ContentManager content = ScreenManager.Game.Content;
            SpriteFont copperPlate = ScreenManager.Game.Content.Load<SpriteFont>(@"Fonts\copperPlate");
            float x = 110*1.5f;
            float y = 200*1.5f;
            

            //Load the background loop
            mainLoop = content.Load<Video>(@"Video\loopmenu3");

            // add the New Game entry
            newGameMenuEntry = new MenuEntry("Start");
            newGameMenuEntry.Description = "Start the game";
            newGameMenuEntry.Font = copperPlate;
            //newGameMenuEntry.Texture = content.Load<Texture2D>(@"Textures\MainMenu\MainMenuPlank");
            newGameMenuEntry.Position = new Vector2(x, y);
            
            newGameMenuEntry.Selected += NewGameMenuEntrySelected;
            MenuEntries.Add(newGameMenuEntry);

            //Add the options entry
            optionsMenuEntry = new MenuEntry("Options");
            optionsMenuEntry.Description = "Configure the game";
            optionsMenuEntry.Font = copperPlate;
            optionsMenuEntry.Position = new Vector2(x, y += 36 * 1.5f);
            optionsMenuEntry.Selected += OptionsMenuEntrySelected;
            MenuEntries.Add(optionsMenuEntry);

            //Add extras entry
            extrasMenuEntry = new MenuEntry("Extras");
            extrasMenuEntry.Description = "Gaming goodies";
            extrasMenuEntry.Font = copperPlate;
            extrasMenuEntry.Position = new Vector2(x, y += 36 * 1.5f);
            extrasMenuEntry.Selected += ExtrasMenuEntrySelected;
            MenuEntries.Add(extrasMenuEntry);

            // create the Exit menu entry
            exitGameMenuEntry = new MenuEntry("Exit");
            exitGameMenuEntry.Description = "Quit the Game";
            exitGameMenuEntry.Font = copperPlate;
            exitGameMenuEntry.Position = new Vector2(x, y+36*1.5f);
            exitGameMenuEntry.Selected += OnCancel;
            MenuEntries.Add(exitGameMenuEntry);

            selectTexture = content.Load<Texture2D>(@"Textures\Buttons\AButton");
            
            
            // calculate the texture positions
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Vector2 backgroundPosition = new Vector2(viewport.Width / 2,
                                                    viewport.Height/ 2);
            selectPosition = backgroundPosition + new Vector2(1120, 610);

          

            // now that they have textures, set the proper positions on the menu entries
            for (int i = 0; i < MenuEntries.Count; i++)
            {
                MenuEntries[i].Position = new Vector2(MenuEntries[i].Position.X, MenuEntries[i].Position.Y);
            }
            
            base.LoadContent();
        }

        #endregion


        #region Updating


        /// <summary>
        /// Handles user input.
        /// </summary>
        public override void HandleInput()
        {
            if (InputManager.IsActionTriggered(InputManager.Action.Back) &&
                Session.IsActive)
            {
                AudioManager.PopMusic();
                ExitScreen();
                return;
            }

            base.HandleInput();
        }


        /// <summary>
        /// Event handler for when the New Game menu entry is selected.
        /// </summary>
        void NewGameMenuEntrySelected(object sender, EventArgs e)
        {
            if (Session.IsActive)
            {
                ExitScreen();
            }

            ContentManager content = ScreenManager.Game.Content;
            LoadingScreen.Load(ScreenManager, true, new GameplayScreen(
                content.Load<GameStartDescription>("MainGameDescription")));
        }


        void OptionsMenuEntrySelected(object sender, EventArgs e)
        {
            if (Session.IsActive)
            {
                ExitScreen();
            }

            ContentManager content = ScreenManager.Game.Content;
            //ScreenManager.AddScreen
            //LoadingScreen.Load(ScreenManager, true, new GameplayScreen(
            //    content.Load<GameStartDescription>("MainGameDescription")));
        }

        void ExtrasMenuEntrySelected(object sender, EventArgs e)
        {
            if (Session.IsActive)
            {
                ExitScreen();
            }

            ContentManager content = ScreenManager.Game.Content;
            //LoadingScreen.Load(ScreenManager, true, new GameplayScreen(
            //    content.Load<GameStartDescription>("MainGameDescription")));
        }

        /// <summary>
        /// Event handler for when the Save Game menu entry is selected.
        /// </summary>
        void SaveGameMenuEntrySelected(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(
                new SaveLoadScreen(SaveLoadScreen.SaveLoadScreenMode.Save));
        }


        /// <summary>
        /// Event handler for when the Load Game menu entry is selected.
        /// </summary>
        void LoadGameMenuEntrySelected(object sender, EventArgs e)
        {
            SaveLoadScreen loadGameScreen =
                new SaveLoadScreen(SaveLoadScreen.SaveLoadScreenMode.Load);
            loadGameScreen.LoadingSaveGame += new SaveLoadScreen.LoadingSaveGameHandler(
                loadGameScreen_LoadingSaveGame);
            ScreenManager.AddScreen(loadGameScreen);
        }


        /// <summary>
        /// Handle save-game-to-load-selected events from the SaveLoadScreen.
        /// </summary>
        void loadGameScreen_LoadingSaveGame(SaveGameDescription saveGameDescription)
        {
            if (Session.IsActive)
            {
                ExitScreen();
            }
            LoadingScreen.Load(ScreenManager, true,
                new GameplayScreen(saveGameDescription));
        }


        /// <summary>
        /// Event handler for when the Controls menu entry is selected.
        /// </summary>
        void ControlsMenuEntrySelected(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(new ControlsScreen());
        }


        /// <summary>
        /// Event handler for when the Help menu entry is selected.
        /// </summary>
        void HelpMenuEntrySelected(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(new HelpScreen());
        }


        /// <summary>
        /// When the user cancels the main menu,
        /// or when the Exit Game menu entry is selected.
        /// </summary>
        protected override void OnCancel()
        {
            // add a confirmation message box
            string message = String.Empty;
            if (Session.IsActive)
            {
                message =
                    "Are you sure you want to exit?  All unsaved progress will be lost.";
            }
            else
            {
                message = "Are you sure you want to exit?";
            }
            MessageBoxScreen confirmExitMessageBox = new MessageBoxScreen(message);
            confirmExitMessageBox.Accepted += ConfirmExitMessageBoxAccepted;
            ScreenManager.AddScreen(confirmExitMessageBox);
        }


        /// <summary>
        /// Event handler for when the user selects Yes 
        /// on the "Are you sure?" message box.
        /// </summary>
        void ConfirmExitMessageBoxAccepted(object sender, EventArgs e)
        {
            ScreenManager.Game.Exit();
        }


        #endregion


        #region Drawing

        /// <summary>
        /// Draw this screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();

            // Render the video in it's orginal resolution to the screen using SpriteBatch
            if (mainMenuLoopPlayer.State == MediaState.Playing)
            {
                spriteBatch.Draw(mainMenuLoopPlayer.GetTexture(), new Rectangle(0, 0, ScreenManager.GraphicsDevice.Viewport.Width, ScreenManager.GraphicsDevice.Viewport.Height), Color.White);
            }
            else
            {
                mainMenuLoopPlayer.Play(mainLoop);
            }

            // draw the background images
            
            // Draw each menu entry in turn.
            for (int i = 0; i < MenuEntries.Count; i++)
            {
                MenuEntry menuEntry = MenuEntries[i];
                bool isSelected = IsActive && (i == selectedEntry);
                menuEntry.Draw(this, isSelected, gameTime);
            }



            
            spriteBatch.End();
        }

        #endregion
    }
}
