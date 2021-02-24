using System;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace ListViewX
{
	#region WM - Window Messages
	public enum WM
	{
		WM_NULL                   = 0x0000,
		WM_CREATE                 = 0x0001,
		WM_DESTROY                = 0x0002,
		WM_MOVE                   = 0x0003,
		WM_SIZE                   = 0x0005,
		WM_ACTIVATE               = 0x0006,
		WM_SETFOCUS               = 0x0007,
		WM_KILLFOCUS              = 0x0008,
		WM_ENABLE                 = 0x000A,
		WM_SETREDRAW              = 0x000B,
		WM_SETTEXT                = 0x000C,
		WM_GETTEXT                = 0x000D,
		WM_GETTEXTLENGTH          = 0x000E,
		WM_PAINT                  = 0x000F,
		WM_CLOSE                  = 0x0010,
		WM_QUERYENDSESSION        = 0x0011,
		WM_QUIT                   = 0x0012,
		WM_QUERYOPEN              = 0x0013,
		WM_ERASEBKGND             = 0x0014,

	}
	#endregion

	#region RECT
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
	public struct RECT
	{
		public int left;
		public int top;
		public int right;
		public int bottom;
	}
	#endregion



	public class ListViewFF : System.Windows.Forms.ListView
	{
		bool updating;
        int itemnumber;

		#region Imported User32.DLL functions
		[DllImport("user32.dll", CharSet=CharSet.Auto)]
		static public extern bool ValidateRect(IntPtr handle, ref RECT rect);
		#endregion

		public ListViewFF()
		{
            this.DoubleBuffered = true;
        }

		/// <summary>
		/// When adding an item in a loop, use this to update the newly added item.
		/// </summary>
		/// <param name="iIndex">Index of the item just added</param>
		public void UpdateItem(int iIndex)
		{
			updating = true;
			itemnumber = iIndex;
			this.Update();
			updating = false;
		}

		protected override void WndProc(ref Message messg)
		{
			if (updating)
			{
				// We do not want to erase the background, turn this message into a null-message
				if ((int)WM.WM_ERASEBKGND == messg.Msg)
					messg.Msg = (int) WM.WM_NULL;
				else if ((int)WM.WM_PAINT == messg.Msg)
				{
					RECT vrect = this.GetWindowRECT();
					// validate the entire window				
					ValidateRect(this.Handle, ref vrect);

					//Invalidate only the new item
					Invalidate(this.Items[itemnumber].Bounds);
				}

			}
			base.WndProc(ref messg);			
		}


		#region private helperfunctions

		// Get the listview's rectangle and return it as a RECT structure
		private RECT GetWindowRECT()
		{
			RECT rect = new RECT();
			rect.left = this.Left;
			rect.right = this.Right;
			rect.top = this.Top;
			rect.bottom = this.Bottom;
			return rect;
		}

        #endregion
    }
}
