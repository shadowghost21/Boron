using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RolePlayingGameData;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;

namespace RolePlaying.MenuScreens
{
    class IntroScreen : GameScreen
    {
        private Video intro;
        private VideoPlayer player;
        public bool isPlaying = false;

        public IntroScreen() : base(){
            player = new VideoPlayer();
        }

       public override void LoadContent()
       {
           ContentManager content = ScreenManager.Game.Content;
        
           intro = content.Load<Video>(@"Video\intro");
       }

       public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
       {
           if(player.State == MediaState.Stopped) player.Play(intro);
           if (player.PlayPosition >= player.Video.Duration || Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Space))
           {
               player.Stop();
               ScreenManager.AddScreen(new MainMenuScreen());
               ExitScreen();
           }
           base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
       }

       public override void Draw(GameTime gameTime)
       {
           SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
           // Render the video in it's orginal resolution to the screen using SpriteBatch
           spriteBatch.Begin();
           if (player.State == MediaState.Playing)
           {
               spriteBatch.Draw(player.GetTexture(), new Rectangle(0, 0, ScreenManager.GraphicsDevice.Viewport.Width, ScreenManager.GraphicsDevice.Viewport.Height), Color.White);
           }
           spriteBatch.End();
           base.Draw(gameTime);
       }

        
    }
}
