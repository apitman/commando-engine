// ATTENTION!!!
// This code taken from http://nickgravelyn.com/2008/10/catching-exceptions-on-xbox-360/
// Special thanks to Nick Gravelyn for providing this code!

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Commando
{
    public class CrashDebugGame : Game
    {
        private SpriteBatch spriteBatch;
        private SpriteFont font;
        private readonly Exception exception;

        public CrashDebugGame(Exception exception)
        {
            this.exception = exception;
            new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void LoadContent()
        {
            font = FontMap.getInstance().getFont(FontEnum.Kootenay14).getFont();
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            spriteBatch.DrawString(
               font,
               "**** CRASH LOG ****",
               new Vector2(100f, 100f),
               Color.White);
            spriteBatch.DrawString(
               font,
               "Press Back to Exit",
               new Vector2(100f, 120f),
               Color.White);
            spriteBatch.DrawString(
               font,
               string.Format("Exception: {0}", exception.Message),
               new Vector2(100f, 140f),
               Color.White);
            spriteBatch.DrawString(
               font, string.Format("Stack Trace:\n{0}", exception.StackTrace),
               new Vector2(100f, 160f),
               Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
