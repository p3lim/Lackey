using MouseKeyboardActivityMonitor;
using MouseKeyboardActivityMonitor.WinApi;
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

		private KeyboardHookListener keyboardHook;

		public bool isFnHeld;

		private NotifyIcon trayIcon;
		private ContextMenu trayMenu;

		public App()
		{
			InitializeComponent();
		}

		private void InitializeComponent()
		{
			keyboardHook = new KeyboardHookListener(new GlobalHooker());
			keyboardHook.Enabled = true;
			keyboardHook.KeyDown += keyboardHook_KeyDown;
			keyboardHook.KeyUp += keyboardHook_KeyUp;

			trayMenu = new ContextMenu();
			trayMenu.MenuItems.Add("Exit", OnExit);

			trayIcon = new NotifyIcon();
			trayIcon.Text = "Lackey";
			trayIcon.Icon = Resources.Icon;
			trayIcon.ContextMenu = trayMenu;
			trayIcon.Visible = true;
		}

		private void keyboardHook_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyValue == 255)
				isFnHeld = true;
		}

		private void keyboardHook_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyValue == 255)
				isFnHeld = false;
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

		protected override void OnLoad(EventArgs e)
		{
			Visible = false;
			ShowInTaskbar = false;

			base.OnLoad(e);
		}
	}
}
