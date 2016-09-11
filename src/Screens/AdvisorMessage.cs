// CivOne
//
// To the extent possible under law, the person who associated CC0 with
// CivOne has waived all copyright and related or neighboring rights
// to CivOne.
//
// You should have received a copy of the CC0 legalcode along with this
// work. If not, see <http://creativecommons.org/publicdomain/zero/1.0/>.

using System;
using System.Drawing;
using System.Linq;
using CivOne.Enums;
using CivOne.Events;
using CivOne.GFX;
using CivOne.Templates;

namespace CivOne.Screens
{
	internal class AdvisorMessage : BaseScreen
	{
		private bool _update = true;

		public event EventHandler Closed;
		
		public override bool HasUpdate(uint gameTick)
		{
			if (_update)
			{
				_update = false;
				return true;
			}
			return false;
		}
		
		public override bool KeyDown(KeyboardEventArgs args)
		{
			if (Closed != null)
				Closed(this, null);
			Destroy();
			return true;
		}
		
		public override bool MouseDown(ScreenEventArgs args)
		{
			if (Closed != null)
				Closed(this, null);
			Destroy();
			return true;
		}

		public AdvisorMessage(Advisor advisor, params string[] message)
		{
			Cursor = MouseCursor.None;

			Bitmap background = Resources.Instance.GetPart("SP299", 288, 120, 32, 16);
			
			bool modernGovernment = Game.Instance.HumanPlayer.Advances.Any(a => a.Id == (int)Advance.Invention);
			Bitmap governmentPortrait = Icons.GovernmentPortrait(Game.Instance.HumanPlayer.Government, advisor, modernGovernment);
			Color[] palette = Resources.Instance.LoadPIC("SP257").Image.Palette.Entries;
			for (int i = 144; i < 256; i++)
			{
				palette[i] = governmentPortrait.Palette.Entries[i];
			}
			
			string[] advisorNames = new string[] { "Defense Minister", "Domestic Advisor", "Foreign Minister", "Science Advisor" };
			
			_canvas = new Picture(320, 200, palette);

			Bitmap[] textLines = new Bitmap[message.Length + 1];
			textLines[0] = Resources.Instance.GetText(advisorNames[(int)advisor], 0, 15);
			for (int i = 0; i < message.Length; i++)
				textLines[i + 1] = Resources.Instance.GetText(message[i], 0, 15);
			int width = textLines.Max(x => x.Width) + 51;
			int height = 62;
			int actualWidth = width;
			if (width % 4 > 0)
				width += (4 - (width % 4)); 

			Picture messageBox = new Picture(width, height);
			messageBox.FillLayerTile(background, 1, 1);
			messageBox.AddBorder(15, 8, 0, 0, actualWidth, height);
			if (width > actualWidth)
				messageBox.FillRectangle(0, actualWidth, 0, (width - actualWidth), height);
			messageBox.AddLayer(governmentPortrait, 1, 1);
			messageBox.AddLayer(textLines[0], 46, 3);
			messageBox.FillRectangle(11, 46, 10, textLines[0].Width + 2, 1);
			for (int i = 1; i < textLines.Length; i++)
				messageBox.AddLayer(textLines[i], 46, (textLines[i].Height * (i - 1)) + 12);

			_canvas.FillRectangle(5, 38, 72, actualWidth + 2, height + 2);
			AddLayer(messageBox, 39, 73);
			
			/*
			Bitmap[] textLines = new Bitmap[message.Length];
			for (int i = 0; i < message.Length; i++)
				textLines[i] = Resources.Instance.GetText(message[i], 0, 15);
			int width = textLines.Max(x => x.Width) + 10;
			int height = textLines.Sum(x => x.Height) + 9;

			int fillWidth = 4 - (width % 4); 
			width += fillWidth;
			int actualWidth = width - fillWidth;

			_messageBox = new Picture(width, height);
			_messageBox.FillLayerTile(background, 1, 1);
			if (fillWidth > 0)
				_messageBox.FillRectangle(0, actualWidth, 0, fillWidth, height);
			_messageBox.AddBorder(15, 8, 1, 1, actualWidth - 2, height - 2);
			_messageBox.AddBorder(5, 5, 0, 0, actualWidth, height);
			for (int i = 0; i < textLines.Length; i++)
				_messageBox.AddLayer(textLines[i], 5, (textLines[i].Height * i) + 5);*/
		}
	}
}