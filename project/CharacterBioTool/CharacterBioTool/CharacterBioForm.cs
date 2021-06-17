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

		#region field wrapper classes

		// wrapper classes for winform controls to bundle labels with other functionality
		// fields are limited to one functional control each
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


			public Field(CharacterBioForm _form, bool _addToPanel, String _labelText)
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
				((_addToPanel) ? (form.fieldPanel.Controls) : (form.Controls)).Add(label);

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


			public ButtonField(CharacterBioForm _form, bool _addToPanel, String _labelText, String _buttonText)
				: base(_form, _addToPanel, _labelText)
			{
				// create button control
				control = new Button();
				control.Location = new Point(origin.X, label.Bottom + form.fieldControlSpacingY);
				control.Text = _buttonText;

				// add button to form controls
				((_addToPanel) ? (form.fieldPanel.Controls) : (form.Controls)).Add(control);

				// store extents
				right = Math.Max(right, control.Right);
				bottom = control.Bottom;
				CalculateSize();
			}

			~ButtonField()
			{
				form.Controls.Remove(control);
			}

			public override void SetForeColor(Color _color)
			{
				base.SetForeColor(_color);
				control.ForeColor = _color;
			}
			public override void SetMidColor(Color _color)
			{
				base.SetMidColor(_color);
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


			public TextBoxField(CharacterBioForm _form, bool _addToPanel, String _labelText)
				: base(_form, _addToPanel, _labelText)
			{
				// create text box control
				control = new TextBox();
				control.Font = CharacterBioForm.textBoxFont;
				control.Location = new Point(origin.X, label.Bottom + form.fieldControlSpacingY);

				// add text box to form controls
				((_addToPanel) ? (form.fieldPanel.Controls) : (form.Controls)).Add(control);

				// store extents
				right = Math.Max(right, control.Right);
				bottom = control.Bottom;
				CalculateSize();
			}

			~TextBoxField()
			{
				form.Controls.Remove(control);
			}

			public override void SetForeColor(Color _color)
			{
				base.SetForeColor(_color);
				control.ForeColor = _color;
			}
			public override void SetMidColor(Color _color)
			{
				base.SetMidColor(_color);
				control.BackColor = _color;
			}
			public override void SetBackColor(Color _color)
			{
				base.SetBackColor(_color);
			}
		} // end class

		#endregion field wrapper classes

		#region enums, structs

		// color modes
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


		#endregion enums, structs

		#region form variables

		// window
		const int		windowWidth = 1600;
		const int		windowHeight = 900;
		readonly Size	windowSize = new Size(windowWidth, windowHeight);

		// fonts
		static Font		labelFont = new Font("Roboto", 10, FontStyle.Bold);
		static Font		textBoxFont = new Font("Roboto", 10);

		// color modes
		enum COLOR_MODE
		{
			DARK,
			GREY,
			LIGHT,

			MIN = DARK,
			MAX = LIGHT,
		}
		COLOR_MODE		colorMode = COLOR_MODE.DARK;
		ColorPalette[]	palettes = new ColorPalette[]
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
			HEIGHT,
			WEIGHT,
			BUILD,
			SKIN_COLOR,
			EYE_COLOR,
			HAIR_LENGTH,
			HAIR_STYLE,
			HAIR_COLOR,
			FACIAL_HAIR_LENGTH,
			FACIAL_HAIR_STYLE,
			FACIAL_HAIR_COLOR,
			ATTIRE,

			COUNT
		}
		FIELD_TYPE[]	fields = new FIELD_TYPE[]
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
			FIELD_TYPE.TEXT_BOX,
			FIELD_TYPE.TEXT_BOX,
			FIELD_TYPE.TEXT_BOX,
			FIELD_TYPE.TEXT_BOX,
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
		Panel			fieldPanel = new Panel();
		List<Field>		fieldList = new List<Field>();

		#endregion form variables

		#region helper functions

		// generates a Color with the specified grey tone
		public static Color GreyTone(int _tone) { return Color.FromArgb(_tone, _tone, _tone); }

		// converts a CAP_CASE string to First Capitals Case
		public static String CapCaseToFirstCaps(String _str)
		{
			StringBuilder str = new StringBuilder();

			// convert string to lowercase first so each letter doesn't have to be converted separately
			_str = _str.ToLower();
			// handle first letter separately, since there is no previous letter to compare it to
			// replace underscore with space
			if (_str[0] == '_')
				str.Append(' ');
			// capitalize char
			else
				str.Append(_str[0].ToString().ToUpper());

			// handle rest of string
			for (int i = 1; i < _str.Length; ++i)
			{
				// replace underscores with spaces
				if (_str[i] == '_')
				{
					str.Append(' ');
				}
				else
				{
					// if previous char is underscore, capitalize this char
					if (_str[i - 1] == '_')
						str.Append(_str[i].ToString().ToUpper());
					// otherwise, add char unmodified
					else
						str.Append(_str[i]);
				}
			}

			return str.ToString();
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

		#endregion helper functions



		// main form method
		public CharacterBioForm()
		{
			InitializeComponent();


			// create lambdas

			// create lambda to make accessing fields in list easier
			Func<FIELD_NAME, Field> GetField = _fieldName => ((int)_fieldName < fieldList.Count) ? (fieldList[(int)_fieldName]) : (null);

			// create color mode lambdas
			Func<COLOR_MODE, ColorPalette> GetPalette = _colorMode => palettes[(int)_colorMode];
			Action<COLOR_MODE> SetColorMode = _colorMode =>
			{
				ColorPalette palette = GetPalette(colorMode);
				SetFormColors(palette);
				GetField(FIELD_NAME.COLOR_MODE).control.Text = Convert.ToString(colorMode);
			};


			// set initial window size
			Size = windowSize;


			// init control panel
			fieldPanel.Location = new Point(0, 0);
			fieldPanel.Size = windowSize;
			fieldPanel.AutoScroll = true;
			Controls.Add(fieldPanel);

			// init fields and add them to form
			for (int i = 0; i < fields.Length; ++i)
			{
				switch (fields[i])
				{
					default: break;
					case FIELD_TYPE.BASIC:
						AddField(new Field(this, true, CapCaseToFirstCaps(Convert.ToString((FIELD_NAME)i))));
						break;
					case FIELD_TYPE.BUTTON:
						AddField(new ButtonField(this, true, CapCaseToFirstCaps(Convert.ToString((FIELD_NAME)i)), ""));
						break;
					case FIELD_TYPE.TEXT_BOX:
						AddField(new TextBoxField(this, true, CapCaseToFirstCaps(Convert.ToString((FIELD_NAME)i))));
						break;
				}
			}

			// set initial color mode
			SetColorMode(colorMode);


			// set field custom behaviors

			// set color mode button behavior with event handler lambda
			GetField(FIELD_NAME.COLOR_MODE).control.Click += (obj, eventArgs) =>
			{
				this.ActiveControl = null; // defocus button after clicking it
				colorMode = (colorMode == COLOR_MODE.MAX) ? (COLOR_MODE.MIN) : (colorMode + 1);
				SetColorMode(colorMode);
			};
		}

	} // end class

} // end namespace
