using System;
using System.Windows.Forms;

namespace Lackey
{
	public class App : Form
	{
		[STAThread]
		public static void Main()
		{
			Application.Run(new App());
		}

		public App()
		{

		}
	}
}
