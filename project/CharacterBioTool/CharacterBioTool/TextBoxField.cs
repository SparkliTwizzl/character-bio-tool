using System;
using System.Drawing;
using System.Windows.Forms;



namespace CharacterBioTool
{
	public class TextBoxField : Field
	{
		private CharacterBioForm form;



		public TextBoxField(CharacterBioForm _form, FieldDesc _desc)
			: base(_form, _desc)
		{
			InitTextBox();
		}

		private void InitTextBox()
		{
			TextBox textBox = new TextBox();

			textBox.Dock = (Desc.controlStyle.dockStyle != DockStyle.None)
				? (Desc.controlStyle.dockStyle)
				: (DockStyle.Bottom);
			textBox.Text = Desc.controlText;
			if (Desc.controlStyle.width != 0)
			{
				textBox.Width = Desc.controlStyle.width;
			}
			if (Desc.controlStyle.height != 0)
			{
				textBox.Height = Desc.controlStyle.height;
			}
			if (Desc.panelStyle.height == 0 && Desc.controlStyle.height == 0)
			{
				Height += textBox.Height;
			}
			switch (Desc.controlStyle.textAlign)
			{
				default:
				case ContentAlignment.TopLeft:
				case ContentAlignment.MiddleLeft:
				case ContentAlignment.BottomLeft:
					textBox.TextAlign = HorizontalAlignment.Left;
					break;
				case ContentAlignment.TopCenter:
				case ContentAlignment.MiddleCenter:
				case ContentAlignment.BottomCenter:
					textBox.TextAlign = HorizontalAlignment.Center;
					break;
				case ContentAlignment.TopRight:
				case ContentAlignment.MiddleRight:
				case ContentAlignment.BottomRight:
					textBox.TextAlign = HorizontalAlignment.Right;
					break;
			}
			textBox.BorderStyle = (Desc.controlStyle.border)
				? (BorderStyle.Fixed3D) 
				: (BorderStyle.None);
			textBox.Font = CharacterBioForm.textBoxFont;
			textBox.WordWrap = true;

			// add to panel
			Control = textBox;
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
