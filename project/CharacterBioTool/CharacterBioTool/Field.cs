using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace CharacterBioTool
{

	public class Field : Panel
	{
		public enum TYPE
		{
			BASE,
			BUTTON,
			TEXT_BOX,
		}
		public struct ControlStyle
		{
			public ContentAlignment		textAlign { get; set; }
			public DockStyle			dockStyle { get; set; }
			public bool					autoSize { get; set; }
			public int					width { get; set; }
			public int					height { get; set; }
			public bool					border { get; set; }
			public int					padLeft { get; set; }
			public int					padRight { get; set; }
			public int					padTop { get; set; }
			public int					padBottom { get; set; }
			public int					marginLeft { get; set; }
			public int					marginRight { get; set; }
			public int					marginTop { get; set; }
			public int					marginBottom { get; set; }
		}
		public struct Desc
		{
			public TYPE					type { get; set; }
			public bool					addToFormPanel { get; set; }
			public ControlStyle			panelStyle { get; set; }
			public string				labelText { get; set; }
			public ControlStyle			labelStyle { get; set; }
			public string				controlText { get; set; }
			public ControlStyle			controlStyle { get; set; }
		}


		private CharacterBioForm	form;
		public Desc					desc { get; set; }
		public Label				label { get; set; }
		public Control				control { get; set; }


		public Field(CharacterBioForm _form, Desc _desc)
		{
			form = _form;
			desc = _desc;
			AddFieldToParentControl();
			InitPanel();
			InitLabel();
		}

		~Field()
		{
			form.Controls.Remove(label);
		}

		private void AddFieldToParentControl()
		{
			((desc.addToFormPanel) ? (form.fieldPanel.Controls) : (form.Controls)).Add(this);
		}
		private void InitPanel()
		{
			Location = (desc.addToFormPanel)
				? (form.nextPanelFieldPosition)
				: (form.nextIndependentFieldPosition);
			if (desc.panelStyle.width == 0 && desc.panelStyle.height == 0) AutoSize = true;
			Size = new Size(
				((desc.panelStyle.width != 0)
					? (desc.panelStyle.width)
					: (Math.Max(desc.labelStyle.width, desc.controlStyle.width)))
						+ desc.panelStyle.padLeft + desc.panelStyle.padRight,
				((desc.panelStyle.height != 0)
					? (desc.panelStyle.height)
					: (desc.labelStyle.height + desc.controlStyle.height))
						+ desc.panelStyle.padTop + desc.panelStyle.padBottom
				);
			if (desc.panelStyle.border) BorderStyle = BorderStyle.FixedSingle;
			Padding = new Padding(desc.panelStyle.padLeft, desc.panelStyle.padRight,
				desc.panelStyle.padTop, desc.panelStyle.padBottom);
		}
		private void InitLabel()
		{
			label = new Label();
			label.Dock = (desc.labelStyle.dockStyle != DockStyle.None) ? (desc.labelStyle.dockStyle) : (DockStyle.Top);
			label.Text = desc.labelText;
			if (desc.labelStyle.width == 0 && desc.labelStyle.height == 0) label.AutoSize = true;
			if (desc.labelStyle.width != 0) label.Width = desc.labelStyle.width;
			if (desc.labelStyle.height != 0) label.Height = desc.labelStyle.height;
			label.TextAlign = desc.labelStyle.textAlign;
			label.Font = CharacterBioForm.labelFont;
			label.BorderStyle = (desc.labelStyle.border) ? (BorderStyle.FixedSingle) : (BorderStyle.None);
			Controls.Add(label);
		}

		public virtual void SetForeColor(Color _color)
		{
			label.ForeColor = _color;
		}
		public virtual void SetMidColor(Color _color)
		{
			label.BackColor = _color;
		}
		public virtual void SetBackColor(Color _color)
		{
			BackColor = _color;
		}

	} // end class
} // end namespace
