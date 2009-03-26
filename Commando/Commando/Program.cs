using System;
#if !XBOX
using System.Windows.Forms;
#endif

namespace Commando
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            try
            {
                using (Engine game = new Engine())
                {
                    game.Run();
                }
            }
            catch (Exception e)
            {
#if !XBOX
                MessageBox.Show(e.Message);
                MessageBox.Show(e.StackTrace);
#endif
            }
        }
    }
}

