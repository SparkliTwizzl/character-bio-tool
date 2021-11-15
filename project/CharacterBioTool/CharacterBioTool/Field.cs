using System;
using System.Drawing;
using System.Windows.Forms;



namespace CharacterBioTool
{

	public enum FieldType
	{
		Basic,
		Button,
		TextBox,
	} // end enum

	public struct ControlStyle
	{
		public ContentAlignment textAlign;
		public DockStyle dockStyle;
		public bool autoSize;
		public int width;
		public int height;
		public bool border;
		public int padLeft;
		public int padRight;
		public int padTop;
		public int padBottom;
		public int marginLeft;
		public int marginRight;
		public int marginTop;
		public int marginBottom;
	} // end struct

	public struct FieldDesc
	{
		public FieldType type;
		public bool addToFormPanel;
		public ControlStyle panelStyle;
		public string labelText;
		public ControlStyle labelStyle;
		public string controlText;
		public ControlStyle controlStyle;
	} // end struct



	public class Field : Panel
	{
		private CharacterBioForm form;

		private FieldDesc desc;
		public FieldDesc Desc
		{
			get => desc;
			set => desc = value;
		}

		private Label label;
		public Label Label
		{
			get => label;
			set => label = value;
		}

		private Control control;
		public Control Control
		{
			get => control;
			set => control = value;
		}



		public Field(CharacterBioForm _form, FieldDesc _desc)
		{
			form = _form;
			Desc = _desc;
			AddFieldToParentControl();
			InitPanel();
			InitLabel();
		}

		~Field()
		{
			form.Controls.Remove(Label);
		}

		private void AddFieldToParentControl()
		{
			var controls = (Desc.addToFormPanel)
				? (form.fieldPanel.Controls)
				: (form.Controls);
			controls.Add(this);
		}

		private void InitPanel()
		{
			Location = (Desc.addToFormPanel)
				? (form.nextPanelFieldPosition)
				: (form.nextIndependentFieldPosition);

			if (Desc.panelStyle.width == 0 && Desc.panelStyle.height == 0)
			{
				AutoSize = true;
			}
			var width = (Desc.panelStyle.width != 0)
						? (Desc.panelStyle.width)
						: (Math.Max(Desc.labelStyle.width, Desc.controlStyle.width));
			var height = (Desc.panelStyle.height != 0)
						? (Desc.panelStyle.height)
						: (Desc.labelStyle.height + Desc.controlStyle.height);
			Size = new Size(
				width + Desc.panelStyle.padLeft + Desc.panelStyle.padRight,
				height + Desc.panelStyle.padTop + Desc.panelStyle.padBottom);
			if (Desc.panelStyle.border)
			{
				BorderStyle = BorderStyle.FixedSingle;
			}
			Padding = new Padding(Desc.panelStyle.padLeft, Desc.panelStyle.padRight, Desc.panelStyle.padTop, Desc.panelStyle.padBottom);
		}
		private void InitLabel()
		{
			Label = new Label();

			Label.Dock = (Desc.labelStyle.dockStyle != DockStyle.None)
				? (Desc.labelStyle.dockStyle)
				: (DockStyle.Top);
			Label.Text = Desc.labelText;
			if (Desc.labelStyle.width == 0 && Desc.labelStyle.height == 0)
			{
				Label.AutoSize = true;
			}
			if (Desc.labelStyle.width != 0)
			{
				Label.Width = Desc.labelStyle.width;
			}
			if (Desc.labelStyle.height != 0)
			{
				Label.Height = Desc.labelStyle.height;
			}
			Label.TextAlign = Desc.labelStyle.textAlign;
			Label.Font = CharacterBioForm.labelFont;
			Label.BorderStyle =
				(Desc.labelStyle.border)
				? (BorderStyle.FixedSingle)
				: (BorderStyle.None);

			Controls.Add(Label);
		}

		public virtual void SetForeColor(Color _color)
		{
			Label.ForeColor = _color;
		}
		public virtual void SetMidColor(Color _color)
		{
			Label.BackColor = _color;
		}
		public virtual void SetBackColor(Color _color)
		{
			BackColor = _color;
		}

	} // end class

} // end namespace
