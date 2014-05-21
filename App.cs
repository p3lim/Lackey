﻿using Microsoft.Win32;
using MouseKeyboardActivityMonitor;
using MouseKeyboardActivityMonitor.WinApi;
using System;
using System.Reflection;
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
		public bool layerUsed;
		public bool simulated;

		private NotifyIcon trayIcon;
		private ContextMenu trayMenu;

		public App()
		{
			AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
			{
				string resourceName = new AssemblyName(args.Name).Name + ".dll";
				string resource = Array.Find(this.GetType().Assembly.GetManifestResourceNames(), element => element.EndsWith(resourceName));

				using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resource))
				{
					Byte[] assemblyData = new Byte[stream.Length];
					stream.Read(assemblyData, 0, assemblyData.Length);
					return Assembly.Load(assemblyData);
				}
			};

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
			layerUsed = true;
			simulated = true;
			e.SuppressKeyPress = true;
			simulator.Keyboard.TextEntry(p);
		}

		private void SimulateKeyDown(KeyEventArgs e, VirtualKeyCode virtualKeyCode)
		{
			layerUsed = true;
			simulated = true;
			e.SuppressKeyPress = true;
			simulator.Keyboard.KeyDown(virtualKeyCode);
		}

		private void SimulateKeyUp(KeyEventArgs e, VirtualKeyCode virtualKeyCode)
		{
			e.SuppressKeyPress = true;
			simulated = true;
			simulator.Keyboard.KeyUp(virtualKeyCode);
		}

		private void keyboardHook_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Space && !simulated)
			{
				e.SuppressKeyPress = true;
				isFnHeld = true;
			}

			simulated = false;

			// Might want to do this in the registry instead
			if (e.KeyCode == Keys.Capital)
				SimulateKeyDown(e, VirtualKeyCode.LCONTROL);
			if (e.KeyValue == 226)
				e.SuppressKeyPress = true;

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
					SimulateKeyDown(e, VirtualKeyCode.OEM_3);
				else if (e.KeyCode == Keys.D1)
					SimulateKeyDown(e, VirtualKeyCode.F1);
				else if (e.KeyCode == Keys.D2)
					SimulateKeyDown(e, VirtualKeyCode.F2);
				else if (e.KeyCode == Keys.D3)
					SimulateKeyDown(e, VirtualKeyCode.F3);
				else if (e.KeyCode == Keys.D4)
					SimulateKeyDown(e, VirtualKeyCode.F4);
				else if (e.KeyCode == Keys.D5)
					SimulateKeyDown(e, VirtualKeyCode.F5);
				else if (e.KeyCode == Keys.D6)
					SimulateKeyDown(e, VirtualKeyCode.F6);
				else if (e.KeyCode == Keys.D7)
					SimulateKeyDown(e, VirtualKeyCode.F7);
				else if (e.KeyCode == Keys.D8)
					SimulateKeyDown(e, VirtualKeyCode.F8);
				else if (e.KeyCode == Keys.D9)
					SimulateKeyDown(e, VirtualKeyCode.F9);
				else if (e.KeyCode == Keys.D0)
					SimulateKeyDown(e, VirtualKeyCode.F10);
				else if (e.KeyCode == Keys.OemMinus)
					SimulateKeyDown(e, VirtualKeyCode.F11);
				else if (e.KeyCode == Keys.Oemplus)
					SimulateKeyDown(e, VirtualKeyCode.F12);
				else if (e.KeyCode == Keys.U)
					SimulateKeyDown(e, VirtualKeyCode.HOME);
				else if (e.KeyCode == Keys.O)
					SimulateKeyDown(e, VirtualKeyCode.END);
				else if (e.KeyCode == Keys.H)
					SimulateKeyDown(e, VirtualKeyCode.PRIOR);
				else if (e.KeyCode == Keys.N)
					SimulateKeyDown(e, VirtualKeyCode.NEXT);
				else if (e.KeyCode == Keys.I)
					SimulateKeyDown(e, VirtualKeyCode.UP);
				else if (e.KeyCode == Keys.K)
					SimulateKeyDown(e, VirtualKeyCode.DOWN);
				else if (e.KeyCode == Keys.J)
					SimulateKeyDown(e, VirtualKeyCode.LEFT);
				else if (e.KeyCode == Keys.L)
					SimulateKeyDown(e, VirtualKeyCode.RIGHT);
				else if (e.KeyCode == Keys.P)
					SimulateKeyDown(e, VirtualKeyCode.SNAPSHOT);
				else if (e.KeyCode == Keys.OemPeriod)
					SimulateKeyDown(e, VirtualKeyCode.F20);
				else if (e.KeyCode == Keys.OemQuestion)
					SimulateKeyDown(e, VirtualKeyCode.F21);
				else if (e.KeyCode == Keys.Back)
					SimulateKeyDown(e, VirtualKeyCode.DELETE);
				else if (e.KeyCode == Keys.B)
					SimulateKeyDown(e, VirtualKeyCode.SPACE);
			} else if (e.KeyCode == Keys.Oemtilde && (!e.Shift || e.Control))
				SimulateKeyDown(e, VirtualKeyCode.ESCAPE);
		}

		private void keyboardHook_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Space && !simulated)
			{
				if (!layerUsed)
					simulator.Keyboard.KeyPress(VirtualKeyCode.SPACE);

				layerUsed = false;
				isFnHeld = false;
				e.SuppressKeyPress = true;
			}

			simulated = false;

			// Might want to do this in the registry instead
			if (e.KeyCode == Keys.Capital)
				SimulateKeyUp(e, VirtualKeyCode.LCONTROL);
			if (e.KeyValue == 226)
				e.SuppressKeyPress = true;

			if (layerUsed)
			{
				if (e.KeyCode == Keys.Z)
					e.SuppressKeyPress = true;
				else if (e.KeyCode == Keys.X)
					e.SuppressKeyPress = true;
				else if (e.KeyCode == Keys.C)
					e.SuppressKeyPress = true;
				else if (e.KeyCode == Keys.D3 && e.Shift)
					e.SuppressKeyPress = true;
				else if (e.KeyCode == Keys.D4 && e.Shift)
					e.SuppressKeyPress = true;
				else if (e.KeyCode == Keys.Escape && !e.Shift)
					SimulateKeyUp(e, VirtualKeyCode.OEM_3);
				else if (e.KeyCode == Keys.D1)
					SimulateKeyUp(e, VirtualKeyCode.F1);
				else if (e.KeyCode == Keys.D2)
					SimulateKeyUp(e, VirtualKeyCode.F2);
				else if (e.KeyCode == Keys.D3)
					SimulateKeyUp(e, VirtualKeyCode.F3);
				else if (e.KeyCode == Keys.D4)
					SimulateKeyUp(e, VirtualKeyCode.F4);
				else if (e.KeyCode == Keys.D5)
					SimulateKeyUp(e, VirtualKeyCode.F5);
				else if (e.KeyCode == Keys.D6)
					SimulateKeyUp(e, VirtualKeyCode.F6);
				else if (e.KeyCode == Keys.D7)
					SimulateKeyUp(e, VirtualKeyCode.F7);
				else if (e.KeyCode == Keys.D8)
					SimulateKeyUp(e, VirtualKeyCode.F8);
				else if (e.KeyCode == Keys.D9)
					SimulateKeyUp(e, VirtualKeyCode.F9);
				else if (e.KeyCode == Keys.D0)
					SimulateKeyUp(e, VirtualKeyCode.F10);
				else if (e.KeyCode == Keys.OemMinus)
					SimulateKeyUp(e, VirtualKeyCode.F11);
				else if (e.KeyCode == Keys.Oemplus)
					SimulateKeyUp(e, VirtualKeyCode.F12);
				else if (e.KeyCode == Keys.U)
					SimulateKeyUp(e, VirtualKeyCode.HOME);
				else if (e.KeyCode == Keys.O)
					SimulateKeyUp(e, VirtualKeyCode.END);
				else if (e.KeyCode == Keys.H)
					SimulateKeyUp(e, VirtualKeyCode.PRIOR);
				else if (e.KeyCode == Keys.N)
					SimulateKeyUp(e, VirtualKeyCode.NEXT);
				else if (e.KeyCode == Keys.I)
					SimulateKeyUp(e, VirtualKeyCode.UP);
				else if (e.KeyCode == Keys.K)
					SimulateKeyUp(e, VirtualKeyCode.DOWN);
				else if (e.KeyCode == Keys.J)
					SimulateKeyUp(e, VirtualKeyCode.LEFT);
				else if (e.KeyCode == Keys.L)
					SimulateKeyUp(e, VirtualKeyCode.RIGHT);
				else if (e.KeyCode == Keys.P)
					SimulateKeyUp(e, VirtualKeyCode.SNAPSHOT);
				else if (e.KeyCode == Keys.OemPeriod)
					SimulateKeyUp(e, VirtualKeyCode.F20);
				else if (e.KeyCode == Keys.OemQuestion)
					SimulateKeyUp(e, VirtualKeyCode.F21);
				else if (e.KeyCode == Keys.Back)
					SimulateKeyUp(e, VirtualKeyCode.DELETE);
				else if (e.KeyCode == Keys.B)
					SimulateKeyUp(e, VirtualKeyCode.SPACE);
			}
			else if (e.KeyCode == Keys.Oemtilde && (!e.Shift || e.Control))
				SimulateKeyUp(e, VirtualKeyCode.ESCAPE);
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
