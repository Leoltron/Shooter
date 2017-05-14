using System;
using System.Windows.Forms;

namespace Shooter
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.Run(new GameForm());
        }
    }
}
