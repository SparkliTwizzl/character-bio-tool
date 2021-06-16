using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace CharacterBioTool
{

	public partial class CharacterBioForm : Form
	{

		// convenience functions
		public static Color GreyTone(int _tone) { return Color.FromArgb(_tone, _tone, _tone); }



		// field class wrappers for winform controls to bundle labels and other functionality

		public class Field
		{
			public struct Extents
			{
				public Point origin;
				public int left;
				public int right;
				public int top;
				public int bottom;
				public int width;
				public int height;

				public Extents(int _l, int _r, int _t, int _b)
				{
					origin = new Point(_l, _t);
					left = _l;
					right = _r;
					top = _t;
					bottom = _b;
					width = Math.Abs(_r - _l);
					height = Math.Abs(_b - _t);
				}
				public Extents(Point _origin, int _w, int _h)
				{
					origin = _origin;
					width = _w;
					height = _h;
					left = origin.X;
					right = left + width;
					top = origin.Y;
					bottom = top + height;
				}

				// gets left/right/top/bottom from origin/size
				public void CalculateBounds()
				{
					left = origin.X;
					right = left + width;
					top = origin.Y;
					bottom = top + height;
				}
				// gets width/height from left/right/top/bottom
				public void CalculateSize()
				{
					width = Math.Abs(right - left);
					height = Math.Abs(bottom - top);
				}
			}

			public CharacterBioForm form;
			public Extents extents;
			public bool useLabel;
			public Label label;

			public class FieldDesc
			{
				public bool useLabel;
				public String labelText;

				public FieldDesc() {}
				public FieldDesc(bool _useLabel, String _labelText)
				{
					useLabel = _useLabel;
					labelText = _labelText;
				}
			} // end class FieldDesc


			public Field(CharacterBioForm _form, bool _useLabel, String _labelText = "")
			{
				// store values
				form = _form;
				extents.origin = _form.nextFieldLocation;
				useLabel = _useLabel;

				// if label is used, create label
				if (useLabel)
				{
					label = new Label();
					label.Text = _labelText;
					label.Location = extents.origin;
					form.Controls.Add(label);
				}

				// store extents
				extents.left = extents.origin.X;
				extents.right = (useLabel) ? (label.Right) : (extents.left); // without label, field has no width
				extents.top = extents.origin.Y;
				extents.bottom = (useLabel) ? (label.Bottom) : (extents.top); // without label, field has no height
				extents.CalculateSize();
			}
			public Field(CharacterBioForm _form, FieldDesc _desc)
				: this(_form, _desc.useLabel, _desc.labelText) { }

			~Field()
			{
				if (useLabel)
					form.Controls.Remove(label);
			}

			public virtual void SetForeColor(Color _color)
			{
				if (useLabel)
					label.ForeColor = _color;
			}
			public virtual void SetMidColor(Color _color) {}
			public virtual void SetBackColor(Color _color) {}
		} // end class Field
		public class ButtonField : Field
		{
			public new class FieldDesc : Field.FieldDesc
			{
				public String buttonText;

				public FieldDesc() {}
				public FieldDesc(bool _useLabel, String _labelText, String _buttonText)
					: base(_useLabel, _labelText)
				{
					buttonText = _buttonText;
				}
			} // end class FieldDesc

			public Button button;


			public ButtonField(CharacterBioForm _form, bool _useLabel, String _labelText = "", String _buttonText = "")
				: base(_form, _useLabel, _labelText)
			{
				// create button control
				button = new Button();
				button.Location = new Point(extents.origin.X, (_useLabel) ? label.Bottom : extents.origin.Y);
				button.Text = _buttonText;

				// store extents
				extents.right = Math.Max(extents.right, button.Right);
				extents.bottom = button.Bottom;
				extents.CalculateSize();

				// add button to form controls
				form.Controls.Add(button);
			}
			public ButtonField(CharacterBioForm _form, FieldDesc _desc)
				: this(_form, _desc.useLabel, _desc.labelText, _desc.buttonText) { }

			~ButtonField()
			{
				form.Controls.Remove(button);
			}

			public override void SetForeColor(Color _color)
			{
				base.SetForeColor(_color);
				button.ForeColor = _color;
			}
			public override void SetMidColor(Color _color)
			{
				base.SetMidColor(_color);
				button.BackColor = _color;
			}
			public override void SetBackColor(Color _color)
			{
				base.SetBackColor(_color);
			}
		} // end class ButtonField
		public class TextBoxField : Field
		{
			public new class FieldDesc : Field.FieldDesc
			{
				public FieldDesc(bool _useLabel, String _labelText)
					: base(_useLabel, _labelText) {}
			} // end class FieldDesc

			public TextBox textBox;


			public TextBoxField(CharacterBioForm _form, bool _useLabel, String _labelText = "")
				: base(_form, _useLabel, _labelText)
			{
				// create text box control
				textBox = new TextBox();
				textBox.Location = new Point(extents.origin.X, (_useLabel) ? label.Bottom : extents.origin.Y);

				// store extents
				extents.right = Math.Max(extents.right, textBox.Right);
				extents.bottom = textBox.Bottom;
				extents.CalculateSize();

				// add text box to form controls
				form.Controls.Add(textBox);
			}
			public TextBoxField(CharacterBioForm _form, FieldDesc _desc)
				: this(_form, _desc.useLabel, _desc.labelText) { }

			~TextBoxField()
			{
				form.Controls.Remove(textBox);
			}

			public override void SetForeColor(Color _color)
			{
				base.SetForeColor(_color);
				textBox.ForeColor = _color;
			}
			public override void SetMidColor(Color _color)
			{
				base.SetMidColor(_color);
				textBox.BackColor = _color;
			}
			public override void SetBackColor(Color _color)
			{
				base.SetBackColor(_color);
			}
		} // end class TextField



		// form variables

		// color modes
		public enum COLOR_MODE
		{
			LIGHT = 0,
			DARK,
			GREY,

			MIN = LIGHT,
			MAX = GREY,
		}
		public struct ColorPalette
		{
			public Color	formBackColor;
			public Color	fieldForeColor;
			public Color	fieldMidColor;
			public Color	fieldBackColor;

			public ColorPalette(Color _formBackColor, Color _fieldForeColor, Color _fieldMidColor, Color _fieldBackColor)
			{
				formBackColor = _formBackColor;
				fieldForeColor = _fieldForeColor;
				fieldMidColor = _fieldMidColor;
				fieldBackColor = _fieldBackColor;
			}
		}

		COLOR_MODE colorMode = COLOR_MODE.LIGHT;
		ColorPalette[] palettes = new ColorPalette[]
		{
			//				 form back color					field fore color					field mid color						field back color
			new ColorPalette(GreyTone(0xdf),					Color.Black,						Color.White,						GreyTone(0xdf)),
			new ColorPalette(GreyTone(0x2f),					Color.White,						GreyTone(0x4f),						GreyTone(0x3f)),
			new ColorPalette(GreyTone(0x6f),					Color.White,						GreyTone(0x67),						GreyTone(0x7f)),
		};

		// location/spacing
		const int	originX = 20;
		const int	originY = 20;
		const int	spacingX = 50;
		const int	spacingY = 10;
		Point		nextFieldLocation = new Point(originX, originY);

		// fields
		List<Field>		fields;
		ButtonField		colorModeButton;
		TextBoxField	nameBox;
		TextBoxField	nicknameBox;
		TextBoxField	raceBox;
		TextBoxField	genderBox;
		TextBoxField	sexBox;
		// literal age
		// phyiscal age
		// apparent age



		// main form method
		public CharacterBioForm()
		{
			InitializeComponent();

			// init fields
			fields			= new List<Field>();
			Field.FieldDesc desc;
			desc = new ButtonField.FieldDesc();

			AddField(colorModeButton	= new ButtonField(this, true, "Color Mode", ""));
			AddField(nameBox			= new TextBoxField(this, true, "Name"));
			AddField(nicknameBox		= new TextBoxField(this, true, "Nickname"));
			AddField(raceBox			= new TextBoxField(this, true, "Race"));
			AddField(genderBox			= new TextBoxField(this, true, "Gender"));
			AddField(sexBox				= new TextBoxField(this, true, "Sex"));

			// set initial control focus
			ActiveControl = nameBox.textBox;

			// create color mode lambdas
			Func<COLOR_MODE, ColorPalette> GetPalette = _colorMode => palettes[Convert.ToInt32(colorMode)];
			Action<COLOR_MODE> SetColorMode = _colorMode =>
			{
				ColorPalette palette = GetPalette(colorMode);
				SetFormColors(palette);
				colorModeButton.button.Text = Convert.ToString(colorMode);
			};

			// set initial color mode
			SetColorMode(colorMode);


			// set field custom behaviors

			// set color mode button behavior with event handler lambda
			colorModeButton.button.Click += (obj, eventArgs) =>
			{
				this.ActiveControl = null; // defocus button after clicking it
				colorMode = (colorMode == COLOR_MODE.MAX) ? (COLOR_MODE.MIN) : (colorMode + 1);
				SetColorMode(colorMode);
			};
		}

		// functions to manage fields
		public void AddField(Field _field)
		{
			fields.Add(_field);
			nextFieldLocation.Y = _field.extents.bottom + spacingY;
		}
		public void RemoveField(Field _field) { fields.Remove(_field); }

		// functions to manage colors
		public void SetFormColors(Color _formBackColor, Color _fieldForeColor, Color _fieldMidColor, Color _fieldBackColor)
		{
			this.BackColor = _formBackColor;
			for (int i = 0; i < fields.Count; ++i)
			{
				fields[i].SetForeColor(_fieldForeColor);
				fields[i].SetMidColor(_fieldMidColor);
				fields[i].SetBackColor(_fieldBackColor);
			}
		}
		public void SetFormColors(ColorPalette _palette)
		{
			this.BackColor = _palette.formBackColor;
			for (int i = 0; i < fields.Count; ++i)
			{
				fields[i].SetForeColor(_palette.fieldForeColor);
				fields[i].SetMidColor(_palette.fieldMidColor);
				fields[i].SetBackColor(_palette.fieldBackColor);
			}
		}
		public void SetFieldForeColor(Color _color)
		{
			for (int i = 0; i < fields.Count; ++i)
				fields[i].SetForeColor(_color);
		}
		public void SetFieldMidColor(Color _color)
		{
			for (int i = 0; i < fields.Count; ++i)
				fields[i].SetMidColor(_color);
		}
		public void SetFieldBackColor(Color _color)
		{
			for (int i = 0; i < fields.Count; ++i)
				fields[i].SetBackColor(_color);
		}

	} // end class CharacterProfileForm

} // end namespace winforms_stuff
