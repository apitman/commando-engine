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
        private readonly List<PlayerIndex> ALL_PLAYERS = new List<PlayerIndex> { PlayerIndex.One, PlayerIndex.Two, PlayerIndex.Three, PlayerIndex.Four };

        private SpriteBatch spriteBatch;
        private SpriteFont font;
        private readonly Exception exception;

        private float adjX = 0f;
        private float adjY = 0f;

        private float speed = 1f;

        public CrashDebugGame(Exception exception)
        {
            this.exception = exception;
            new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void LoadContent()
        {
            font = Content.Load<SpriteFont>("SpriteFonts/Kootenay");
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            foreach (PlayerIndex pi in ALL_PLAYERS)
            {
                GamePadState gps = GamePad.GetState(pi);
                if (gps.Buttons.Back == ButtonState.Pressed)
                    Exit();
                if (gps.DPad.Left == ButtonState.Pressed)
                    adjX += 1f * speed;
                if (gps.DPad.Right == ButtonState.Pressed)
                    adjX -= 1f * speed;
                if (gps.DPad.Up == ButtonState.Pressed)
                    adjY += 1f * speed;
                if (gps.DPad.Down == ButtonState.Pressed)
                    adjY -= 1f * speed;
                if (gps.Buttons.LeftShoulder == ButtonState.Pressed)
                    speed -= .1f;
                if (gps.Buttons.RightShoulder == ButtonState.Pressed)
                    speed += .1f;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            spriteBatch.DrawString(
               font,
               "**** CRASH LOG ****",
               new Vector2(100f + adjX, 100f + adjY),
               Color.White);
            spriteBatch.DrawString(
               font,
               "Press Back to Exit",
               new Vector2(100f + adjX, 120f + adjY),
               Color.White);
            spriteBatch.DrawString(
               font,
               string.Format("Exception: {0}", exception.Message),
               new Vector2(100f + adjX, 140f + adjY),
               Color.White);
            spriteBatch.DrawString(
               font, string.Format("Stack Trace:\n{0}", exception.StackTrace),
               new Vector2(100f + adjX, 160f + adjY),
               Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
