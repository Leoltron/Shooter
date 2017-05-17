using System;
using System.Windows.Forms;
using Shooter.Gui;

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
