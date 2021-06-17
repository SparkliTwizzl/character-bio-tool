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
		// field class wrappers for winform controls to bundle labels and other functionality

		public struct Dimensions
		{
			public int width;
			public int height;
		}

		public enum FIELD_TYPE
		{
			BASIC,
			BUTTON,
			TEXT_BOX,
		}
		public class Field
		{
			public Point origin;
			public int left;
			public int right;
			public int top;
			public int bottom;
			public int width;
			public int height;

			public CharacterBioForm		form;
			public Label				label;
			public Control				control;


			public Field(CharacterBioForm _form, String _labelText)
			{
				// store values
				form = _form;
				origin = _form.nextFieldPosition;

				// if label is used, create label
				label = new Label();
				label.Height = form.labelHeight;
				label.TextAlign = ContentAlignment.BottomLeft;
				label.Font = CharacterBioForm.labelFont;
				label.Text = _labelText;
				label.Location = origin;

				// add to form controls
				form.Controls.Add(label);

				// store extents
				left = origin.X;
				right = label.Right;
				top = origin.Y;
				bottom = label.Bottom;
				CalculateSize();
			}

			~Field()
			{
				form.Controls.Remove(label);
			}

			public virtual void SetForeColor(Color _color)
			{
				label.ForeColor = _color;
			}
			public virtual void SetMidColor(Color _color) {}
			public virtual void SetBackColor(Color _color) {}

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
		} // end class
		public class ButtonField : Field
		{
			//public Button button;


			public ButtonField(CharacterBioForm _form, String _labelText, String _buttonText)
				: base(_form, _labelText)
			{
				// create button control
				//button = new Button();
				//button.Location = new Point(origin.X, label.Bottom + form.fieldControlSpacingY);
				//button.Text = _buttonText;
				control = new Button();
				control.Location = new Point(origin.X, label.Bottom + form.fieldControlSpacingY);
				control.Text = _buttonText;

				// add button to form controls
				//form.Controls.Add(button);
				form.Controls.Add(control);

				// store extents
				//right = Math.Max(right, button.Right);
				//bottom = button.Bottom;
				right = Math.Max(right, control.Right);
				bottom = control.Bottom;
				CalculateSize();
			}

			~ButtonField()
			{
				//form.Controls.Remove(button);
				form.Controls.Remove(control);
			}

			public override void SetForeColor(Color _color)
			{
				base.SetForeColor(_color);
				//button.ForeColor = _color;
				control.ForeColor = _color;
			}
			public override void SetMidColor(Color _color)
			{
				base.SetMidColor(_color);
				//button.BackColor = _color;
				control.BackColor = _color;
			}
			public override void SetBackColor(Color _color)
			{
				base.SetBackColor(_color);
			}
		} // end class
		public class TextBoxField : Field
		{
			//public TextBox textBox;


			public TextBoxField(CharacterBioForm _form, String _labelText)
				: base(_form, _labelText)
			{
				// create text box control
				//textBox = new TextBox();
				//textBox.Location = new Point(origin.X, label.Bottom + form.fieldControlSpacingY);
				control = new TextBox();
				control.Font = CharacterBioForm.labelFont;
				control.Location = new Point(origin.X, label.Bottom + form.fieldControlSpacingY);

				// add text box to form controls
				//form.Controls.Add(textBox);
				form.Controls.Add(control);

				// store extents
				//right = Math.Max(right, textBox.Right);
				//bottom = textBox.Bottom;
				right = Math.Max(right, control.Right);
				bottom = control.Bottom;
				CalculateSize();
			}

			~TextBoxField()
			{
				//form.Controls.Remove(textBox);
				form.Controls.Remove(control);
			}

			public override void SetForeColor(Color _color)
			{
				base.SetForeColor(_color);
				//textBox.ForeColor = _color;
				control.ForeColor = _color;
			}
			public override void SetMidColor(Color _color)
			{
				base.SetMidColor(_color);
				//textBox.BackColor = _color;
				control.BackColor = _color;
			}
			public override void SetBackColor(Color _color)
			{
				base.SetBackColor(_color);
			}
		} // end class



		// helper functions
		public static Color GreyTone(int _tone) { return Color.FromArgb(_tone, _tone, _tone); }
		public static String CapCaseToTitleCase(String _str)
		{
			_str = _str.ToLower();
			StringBuilder str = new StringBuilder();

			if (_str[0] >= 'a' && _str[0] <= 'z')
				str.Append(_str[0].ToString().ToUpper());
			else if (_str[0] == '_')
				str.Append(' ');
			else
				str.Append(_str[0]);

			for (int i = 1; i < _str.Length; ++i)
			{
				if (_str[i - 1] == '_')
				{
					str.Append(_str[i].ToString().ToUpper());
				}
				else
				{
					if (_str[i] == '_')
						str.Append(' ');
					else
						str.Append(_str[i]);
				}
			}

			return str.ToString();
		}



		// form variables

		// fonts
		static Font labelFont = new Font("Roboto", 10);

		// color modes
		enum COLOR_MODE
		{
			DARK,
			GREY,
			LIGHT,

			MIN = DARK,
			MAX = LIGHT,
		}
		struct ColorPalette
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

		COLOR_MODE colorMode = COLOR_MODE.DARK;
		ColorPalette[] palettes = new ColorPalette[]
		{
			//				 form back color					field fore color					field mid color						field back color
			new ColorPalette(GreyTone(0x2f),					Color.White,						GreyTone(0x4f),						GreyTone(0x3f)), // dark
			new ColorPalette(GreyTone(0x6f),					Color.White,						GreyTone(0x67),						GreyTone(0x7f)), // grey
			new ColorPalette(GreyTone(0xdf),					Color.Black,						Color.White,						GreyTone(0xdf)), // light
		};

		// location/spacing
		const int		originX = 20;
		const int		originY = 20;

		const int		fieldSpacingX = 50;
		readonly int	fieldSpacingY = (int)labelFont.Size / 2;

		const int		fieldControlSpacingX = 10;
		readonly int	fieldControlSpacingY = (int)labelFont.Size / 2;

		readonly int	labelHeight = (int)labelFont.Size * 2;

		Point			nextFieldPosition = new Point(originX, originY);

		// fields
		List<Field>		fieldList = new List<Field>();
		public enum FIELD_NAME
		{
			COLOR_MODE,
			NAME,
			NICKNAME,
			RACE,
			GENDER,
			SEX,
			BIRTH_DATE,
			DEATH_DATE,
			LITERAL_AGE,
			PHYSICAL_AGE,
			APPARENT_AGE,
		}

		FIELD_TYPE[] fields = new FIELD_TYPE[]
		{
			FIELD_TYPE.BUTTON,
			FIELD_TYPE.TEXT_BOX,
			FIELD_TYPE.TEXT_BOX,
			FIELD_TYPE.TEXT_BOX,
			FIELD_TYPE.TEXT_BOX,
			FIELD_TYPE.TEXT_BOX,
			FIELD_TYPE.TEXT_BOX,
			FIELD_TYPE.TEXT_BOX,
			FIELD_TYPE.TEXT_BOX,
			FIELD_TYPE.TEXT_BOX,
		};


		//ButtonField		colorModeButton;
		//TextBoxField	nameBox;
		//TextBoxField	nicknameBox;
		//TextBoxField	raceBox;
		//TextBoxField	genderBox;
		//TextBoxField	sexBox;
		//TextBoxField	birthDateBox;
		//TextBoxField	deathDateBox;
		//TextBoxField	literalAgeBox;
		//TextBoxField	physicalAgeBox;
		//TextBoxField	apparentAgeBox;
		/*
			height
			weight
			build
			skin color
			eye color
			hair
			facial hair
			attire
		*/



		// main form method
		public CharacterBioForm()
		{
			InitializeComponent();

			// init fields and add them to form
			//AddField(colorModeButton	= new ButtonField(this, "Color Mode", ""));
			//AddField(nameBox			= new TextBoxField(this, "Name"));
			//AddField(nicknameBox		= new TextBoxField(this, "Nickname"));
			//AddField(raceBox			= new TextBoxField(this, "Race"));
			//AddField(genderBox			= new TextBoxField(this, "Gender"));
			//AddField(sexBox				= new TextBoxField(this, "Sex"));
			//AddField(birthDateBox		= new TextBoxField(this, "Birth Date"));
			//AddField(deathDateBox		= new TextBoxField(this, "Death Date"));
			//AddField(literalAgeBox		= new TextBoxField(this, "Literal Age"));
			//AddField(physicalAgeBox		= new TextBoxField(this, "Physical Age"));
			//AddField(apparentAgeBox		= new TextBoxField(this, "Apparent Age"));
			for (int i = 0; i < fields.Length; ++i)
			{
				switch (fields[i])
				{
					default: break;
					case FIELD_TYPE.BASIC:
						AddField(new Field(this, CapCaseToTitleCase(Convert.ToString((FIELD_NAME)i))));
						break;
					case FIELD_TYPE.BUTTON:
						AddField(new ButtonField(this, CapCaseToTitleCase(Convert.ToString((FIELD_NAME)i)), ""));
						break;
					case FIELD_TYPE.TEXT_BOX:
						AddField(new TextBoxField(this, CapCaseToTitleCase(Convert.ToString((FIELD_NAME)i))));
						break;
				}
			}


			// create color mode lambdas
			Func<COLOR_MODE, ColorPalette> GetPalette = _colorMode => palettes[(int)colorMode];
			Action<COLOR_MODE> SetColorMode = _colorMode =>
			{
				ColorPalette palette = GetPalette(colorMode);
				SetFormColors(palette);
				//colorModeButton.button.Text = Convert.ToString(colorMode);
				fieldList[Convert.ToInt32(FIELD_NAME.COLOR_MODE)].control.Text = Convert.ToString(colorMode);
			};

			// set initial color mode
			SetColorMode(colorMode);


			// set field custom behaviors

			// set color mode button behavior with event handler lambda
			//colorModeButton.button.Click += (obj, eventArgs) =>
			fieldList[Convert.ToInt32(FIELD_NAME.COLOR_MODE)].control.Click += (obj, eventArgs) =>
			{
				this.ActiveControl = null; // defocus button after clicking it
				colorMode = (colorMode == COLOR_MODE.MAX) ? (COLOR_MODE.MIN) : (colorMode + 1);
				SetColorMode(colorMode);
			};
		}

		// functions to manage fields
		void AddField(Field _field)
		{
			fieldList.Add(_field as Field);
			nextFieldPosition.Y = _field.bottom + fieldSpacingY;
		}
		void RemoveField(Field _field) { fieldList.Remove(_field); }

		// functions to manage colors
		void SetFormColors(Color _formBackColor, Color _fieldForeColor, Color _fieldMidColor, Color _fieldBackColor)
		{
			this.BackColor = _formBackColor;
			for (int i = 0; i < fieldList.Count; ++i)
			{
				fieldList[i].SetForeColor(_fieldForeColor);
				fieldList[i].SetMidColor(_fieldMidColor);
				fieldList[i].SetBackColor(_fieldBackColor);
			}
		}
		void SetFormColors(ColorPalette _palette)
		{
			this.BackColor = _palette.formBackColor;
			for (int i = 0; i < fieldList.Count; ++i)
			{
				fieldList[i].SetForeColor(_palette.fieldForeColor);
				fieldList[i].SetMidColor(_palette.fieldMidColor);
				fieldList[i].SetBackColor(_palette.fieldBackColor);
			}
		}
		void SetFieldForeColor(Color _color)
		{
			for (int i = 0; i < fieldList.Count; ++i)
				fieldList[i].SetForeColor(_color);
		}
		void SetFieldMidColor(Color _color)
		{
			for (int i = 0; i < fieldList.Count; ++i)
				fieldList[i].SetMidColor(_color);
		}
		void SetFieldBackColor(Color _color)
		{
			for (int i = 0; i < fieldList.Count; ++i)
				fieldList[i].SetBackColor(_color);
		}

	} // end class

} // end namespace
