﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BizHawk.MultiClient
{
	public partial class TAStudio : Form
	{
		//TODO:
		//If null emulator do a base virtualpad so getmnemonic doesn't fail
		//Right-click - Go to current frame
		//Clicking a frame should go there
		//Multiple timeline system
		//Macro listview
		//	Double click brings up a macro editing window
		//NES Controls virtualpad (Power-on & Reset, eventually FDS options)
		//SMS virtualpad
		//PCE virtualpad
		//Dynamic virtualpad system based on platform
		//ensureVisible when recording

		int defaultWidth;     //For saving the default size of the dialog, so the user can restore if desired
		int defaultHeight;
		
		public bool Engaged; //When engaged the Client will listen to TAStudio for input
		List<VirtualPad> Pads = new List<VirtualPad>();

		//Movie header object - to have the main project header data
		//List<string> MacroFiles - list of .macro files (simply log files)
		//List<string> TimeLines - list of .tas files
		//List<string> Bookmarks - list of savestate files

		public TAStudio()
		{
			InitializeComponent();
			Closing += (o, e) => SaveConfigSettings();
			TASView.QueryItemText += new QueryItemTextHandler(TASView_QueryItemText);
			TASView.QueryItemBkColor += new QueryItemBkColorHandler(TASView_QueryItemBkColor);
			TASView.VirtualMode = true;
		}

		public void UpdateValues()
		{
			if (!this.IsHandleCreated || this.IsDisposed) return;
			if (Global.MainForm.UserMovie.Mode == MOVIEMODE.INACTIVE)
				TASView.ItemCount = 0;
			else
				DisplayList();
		}

		public string GetMnemonic()
		{
			StringBuilder str = new StringBuilder("|0|"); //TODO: Control Command virtual pad
			
			for (int x = 0; x < Pads.Count; x++)
				str.Append(Pads[x].GetMnemonic());
			return str.ToString();
		}

		private void TASView_QueryItemBkColor(int index, int column, ref Color color)
		{
			if (index == Global.Emulator.Frame)
				color = Color.LightGreen;
		}

		private void TASView_QueryItemText(int index, int column, out string text)
		{
			text = "";
			if (column == 0)
				text = String.Format("{0:#,##0}", index);
			if (column == 1)
				text = Global.MainForm.UserMovie.GetInputFrame(index);
		}

		private void DisplayList()
		{
			TASView.ItemCount = Global.MainForm.UserMovie.Length();
			TASView.ensureVisible(Global.Emulator.Frame);
		}

		private void TAStudio_Load(object sender, EventArgs e)
		{
			//TODO: don't engage until new/open project
			//
			Engaged = true;
			Global.RenderPanel.AddMessage("TAStudio engaged");

			LoadConfigSettings();
			ReadOnlyCheckBox.Checked = Global.MainForm.ReadOnly;
			DisplayList();

			//Add virtual pads
			switch (Global.Emulator.SystemId)
			{
				case "NULL":
				default:
					break;
				case "NES":
					VirtualPadNES v1 = new VirtualPadNES();
					v1.Location = new Point(8, 19);
					VirtualPadNES v2 = new VirtualPadNES();
					v2.Location = new Point(188, 19);
					Pads.Add(v1);
					Pads.Add(v2);
					ControllerBox.Controls.Add(Pads[0]);
					ControllerBox.Controls.Add(Pads[1]);
					break;
			}
		}

		private void LoadConfigSettings()
		{
			defaultWidth = Size.Width;     //Save these first so that the user can restore to its original size
			defaultHeight = Size.Height;
		}

		private void SaveConfigSettings()
		{
			Engaged = false;
			Global.Config.TASWndx = this.Location.X;
			Global.Config.TASWndy = this.Location.Y;
			Global.Config.TASWidth = this.Right - this.Left;
			Global.Config.TASHeight = this.Bottom - this.Top;
		}

		public void Restart()
		{

		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void settingsToolStripMenuItem_DropDownOpened(object sender, EventArgs e)
		{
			saveWindowPositionToolStripMenuItem.Checked = Global.Config.TAStudioSaveWindowPosition;
			autoloadToolStripMenuItem.Checked = Global.Config.AutoloadTAStudio;
		}

		private void saveWindowPositionToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Global.Config.TAStudioSaveWindowPosition ^= true;
		}

		private void restoreWindowToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.Size = new System.Drawing.Size(defaultWidth, defaultHeight);
		}

		private void StopButton_Click(object sender, EventArgs e)
		{
			Global.MainForm.StopMovie();
		}

		private void FrameAdvanceButton_Click(object sender, EventArgs e)
		{
			Global.MainForm.PressFrameAdvance = true;
		}

		private void RewindButton_Click(object sender, EventArgs e)
		{
			Global.MainForm.PressRewind = true;
		}

		private void PauseButton_Click(object sender, EventArgs e)
		{

			Global.MainForm.TogglePause();
		}

		private void autoloadToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Global.Config.AutoloadTAStudio ^= true;
		}

		private void checkBox1_CheckedChanged(object sender, EventArgs e)
		{
			Global.MainForm.SetReadOnly(ReadOnlyCheckBox.Checked);
			if (ReadOnlyCheckBox.Checked)
			{
				ReadOnlyCheckBox.BackColor = System.Drawing.SystemColors.Control;
				toolTip1.SetToolTip(this.ReadOnlyCheckBox, "Currently Read-Only Mode");
			}
			else
			{
				ReadOnlyCheckBox.BackColor = Color.LightCoral;
				toolTip1.SetToolTip(this.ReadOnlyCheckBox, "Currently Read+Write Mode");
			}
		}

		private void toolStripButton1_Click(object sender, EventArgs e)
		{
			Global.MainForm.PlayMovieFromBeginning();
		}

		private void RewindToBeginning_Click(object sender, EventArgs e)
		{
			Global.MainForm.Rewind(Global.Emulator.Frame);
			DisplayList();
		}

		private void FastForwardToEnd_Click(object sender, EventArgs e)
		{

		}

		private void editToolStripMenuItem_DropDownOpened(object sender, EventArgs e)
		{
			if (Global.MainForm.ReadOnly)
			{
				insertFrameToolStripMenuItem.Enabled = false;
			}
			else
			{
				insertFrameToolStripMenuItem.Enabled = true;
			}
		}

		private void insertFrameToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (Global.MainForm.ReadOnly)
				return;
		}
	}
}
