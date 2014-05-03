using Microsoft.Win32;
using MouseKeyboardActivityMonitor;
using MouseKeyboardActivityMonitor.WinApi;
using System;
using System.Windows.Forms;
using WindowsInput;
using WindowsInput.Native;

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
		private MouseHookListener mouseHook;
		private InputSimulator simulator;

		public bool isFnHeld;
		public bool isMouseHeld;

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

			mouseHook = new MouseHookListener(new GlobalHooker());
			mouseHook.Enabled = true;
			mouseHook.MouseWheel += mouseHook_MouseWheel;
			mouseHook.MouseDown += mouseHook_MouseDown;
			mouseHook.MouseUp += mouseHook_MouseUp;

			simulator = new InputSimulator();

			trayMenu = new ContextMenu();
			trayMenu.MenuItems.Add("Run on startup", ToggleStartup);
			trayMenu.MenuItems.Add("Exit", OnExit);

			trayIcon = new NotifyIcon();
			trayIcon.Text = "Lackey";
			trayIcon.Icon = Resources.Icon;
			trayIcon.ContextMenu = trayMenu;
			trayIcon.Visible = true;
		}

		private void SimulateText(KeyEventArgs e, char p)
		{
			e.SuppressKeyPress = true;
			simulator.Keyboard.TextEntry(p);
		}

		private void SimulateKey(KeyEventArgs e, VirtualKeyCode virtualKeyCode)
		{
			e.SuppressKeyPress = true;
			simulator.Keyboard.KeyDown(virtualKeyCode);
		}

		private void keyboardHook_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyValue == 255)
				isFnHeld = true;

			if (isFnHeld)
			{
				if (e.KeyCode == Keys.Z)
				{
					if (e.Shift)
						SimulateText(e, '\u00c6');
					else
						SimulateText(e, '\u00e6');
				}
				else if (e.KeyCode == Keys.X)
				{
					if (e.Shift)
						SimulateText(e, '\u00d8');
					else
						SimulateText(e, '\u00f8');
				}
				else if (e.KeyCode == Keys.C)
				{
					if (e.Shift)
						SimulateText(e, '\u00c5');
					else
						SimulateText(e, '\u00e5');
				}
				else if (e.KeyCode == Keys.D3 && e.Shift)
					SimulateText(e, '\u00a3');
				else if (e.KeyCode == Keys.D4 && e.Shift)
					SimulateText(e, '\u20ac');
				else if (e.KeyCode == Keys.Escape && !e.Shift)
					SimulateKey(e, VirtualKeyCode.OEM_3);
				else if (e.KeyCode == Keys.D1)
					SimulateKey(e, VirtualKeyCode.F1);
				else if (e.KeyCode == Keys.D2)
					SimulateKey(e, VirtualKeyCode.F2);
				else if (e.KeyCode == Keys.D3)
					SimulateKey(e, VirtualKeyCode.F3);
				else if (e.KeyCode == Keys.D4)
					SimulateKey(e, VirtualKeyCode.F4);
				else if (e.KeyCode == Keys.D5)
					SimulateKey(e, VirtualKeyCode.F5);
				else if (e.KeyCode == Keys.D6)
					SimulateKey(e, VirtualKeyCode.F6);
				else if (e.KeyCode == Keys.D7)
					SimulateKey(e, VirtualKeyCode.F7);
				else if (e.KeyCode == Keys.D8)
					SimulateKey(e, VirtualKeyCode.F8);
				else if (e.KeyCode == Keys.D9)
					SimulateKey(e, VirtualKeyCode.F9);
				else if (e.KeyCode == Keys.D0)
					SimulateKey(e, VirtualKeyCode.F10);
				else if (e.KeyCode == Keys.OemMinus)
					SimulateKey(e, VirtualKeyCode.F11);
				else if (e.KeyCode == Keys.Oemplus)
					SimulateKey(e, VirtualKeyCode.F12);
				else if (e.KeyCode == Keys.OemOpenBrackets)
					SimulateKey(e, VirtualKeyCode.HOME);
				else if (e.KeyCode == Keys.Oem7)
					SimulateKey(e, VirtualKeyCode.END);
				else if (e.KeyCode == Keys.Oem6)
					SimulateKey(e, VirtualKeyCode.PRIOR);
				else if (e.KeyCode == Keys.Oem5)
					SimulateKey(e, VirtualKeyCode.NEXT);
				else if (e.KeyCode == Keys.O)
					SimulateKey(e, VirtualKeyCode.UP);
				else if (e.KeyCode == Keys.L)
					SimulateKey(e, VirtualKeyCode.DOWN);
				else if (e.KeyCode == Keys.K)
					SimulateKey(e, VirtualKeyCode.LEFT);
				else if (e.KeyCode == Keys.Oem1)
					SimulateKey(e, VirtualKeyCode.RIGHT);
				else if (e.KeyCode == Keys.Space)
					SimulateKey(e, VirtualKeyCode.MEDIA_PLAY_PAUSE);
				else if (e.KeyCode == Keys.P)
					SimulateKey(e, VirtualKeyCode.SNAPSHOT);
				else if (e.KeyCode == Keys.OemPeriod)
					SimulateKey(e, VirtualKeyCode.F20);
				else if (e.KeyCode == Keys.OemQuestion)
					SimulateKey(e, VirtualKeyCode.F21);
				else if (e.KeyCode == Keys.Back)
					SimulateKey(e, VirtualKeyCode.DELETE);
			}
			else if (e.KeyCode == Keys.Escape && e.Shift && !e.Control)
				SimulateKey(e, VirtualKeyCode.OEM_3);
		}

		private void keyboardHook_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyValue == 255)
				isFnHeld = false;
		}

		private void mouseHook_MouseWheel(object sender, MouseEventArgs e)
		{
			if (isMouseHeld)
			{
				if (e.Delta > 0)
				{
					simulator.Keyboard.KeyDown(VirtualKeyCode.VOLUME_UP);
					simulator.Keyboard.KeyDown(VirtualKeyCode.VOLUME_UP);
					simulator.Keyboard.KeyDown(VirtualKeyCode.VOLUME_UP);
				}
				else if (e.Delta < 0)
				{
					simulator.Keyboard.KeyDown(VirtualKeyCode.VOLUME_DOWN);
					simulator.Keyboard.KeyDown(VirtualKeyCode.VOLUME_DOWN);
					simulator.Keyboard.KeyDown(VirtualKeyCode.VOLUME_DOWN);
				}
			}
		}

		private void mouseHook_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
				isMouseHeld = true;
		}

		private void mouseHook_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
				isMouseHeld = false;
		}

		private void ToggleStartup(object sender, EventArgs e)
		{
			RegistryKey key = Registry.CurrentUser;
			RegistryKey group = key.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);

			if (group.GetValue("Lackey") != null)
			{
				group.DeleteValue("Lackey");
				MessageBox.Show("No longer running Lackey on startup");
			}
			else
			{
				group.SetValue("Lackey", Application.ExecutablePath, RegistryValueKind.String);
				MessageBox.Show("Now running Lackey on startup, yay!");
			}
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
