using System;
using System.Drawing;
using System.Windows.Forms;



namespace CharacterBioTool
{
	public class ButtonField : Field
	{
		private CharacterBioForm form;



		public ButtonField(CharacterBioForm _form, FieldDesc _desc)
			: base(_form, _desc)
		{
			InitButton();
		}

		private void InitButton()
		{
			Button button = new Button();

			button.Dock = (Desc.controlStyle.dockStyle != DockStyle.None)
				? (Desc.controlStyle.dockStyle)
				: (DockStyle.Bottom);
			button.Text = Desc.controlText;
			if (Desc.controlStyle.width == 0 && Desc.controlStyle.height == 0)
			{
				button.AutoSize = true;
			}
			if (Desc.controlStyle.width != 0)
			{
				button.Width = Desc.controlStyle.width;
			}
			if (Desc.controlStyle.height != 0)
			{
				button.Height = Desc.controlStyle.height;
			}
			if (Desc.panelStyle.height == 0 && Desc.controlStyle.height == 0)
			{
				Height += button.Height;
			}
			button.TextAlign = Desc.controlStyle.textAlign;

			// add to panel
			Control = button;
			Controls.Add(Control);
		}

		public override void SetForeColor(Color _color)
		{
			Control.ForeColor = _color;
			base.SetForeColor(_color);
		}
		public override void SetMidColor(Color _color)
		{
			Control.BackColor = _color;
			base.SetMidColor(_color);
		}

	} // end class
} // end namespace
