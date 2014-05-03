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

		private NotifyIcon trayIcon;
		private ContextMenu trayMenu;

		public App()
		{
			InitializeComponent();
		}

		private void InitializeComponent()
		{
			trayMenu = new ContextMenu();
			trayMenu.MenuItems.Add("Exit", OnExit);

			trayIcon = new NotifyIcon();
			trayIcon.Text = "Lackey";
			trayIcon.ContextMenu = trayMenu;
			trayIcon.Visible = true;
		}

		public void OnExit(object sender, EventArgs e)
		{
			Application.Exit();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				trayIcon.Dispose();
			}

			base.Dispose(disposing);
		}
	}
}
