using System;

namespace ConwaysGameOfLife
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using ConwaysGame game = new ConwaysGame();
            game.Run();
        }
    }
}
