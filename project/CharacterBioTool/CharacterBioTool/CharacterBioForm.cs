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

		public struct FieldDesc
		{
			public FIELD_TYPE			type;
			public bool					addToPanel;

			public String				labelText;
			public int					labelWidth;
			public int					labelHeight;
			public ContentAlignment		labelTextAlign;

			public String				controlText;
			public int					controlWidth;
			public int					controlHeight;
			public ContentAlignment		controlTextAlign;
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


			public Field(CharacterBioForm _form, FieldDesc _desc)
			{
				// store values
				form = _form;
				origin = (_desc.addToPanel) ? (_form.nextPanelFieldPosition) : (_form.nextIndependentFieldPosition);

				// create label
				label = new Label();
				label.Location = origin;
				label.Text = _desc.labelText;
				label.Width = _desc.labelWidth;
				label.Height = _desc.labelHeight;
				label.TextAlign = _desc.labelTextAlign;
				label.Font = CharacterBioForm.labelFont;
				label.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
				((_desc.addToPanel) ? (form.fieldPanel.Controls) : (form.Controls)).Add(label);

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
			public virtual void SetMidColor(Color _color)
			{
				label.BackColor = _color;
			}
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


			public ButtonField(CharacterBioForm _form, FieldDesc _desc)
				: base(_form, _desc)
			{
				// create button control
				Button button = new Button();
				button.Location = new Point(origin.X, label.Bottom + CharacterBioForm.fieldControlSpacingY);
				button.Text = _desc.controlText;
				button.Width = _desc.controlWidth;
				button.Height = _desc.controlHeight;
				button.TextAlign = _desc.controlTextAlign;
				control = button;
				((_desc.addToPanel) ? (form.fieldPanel.Controls) : (form.Controls)).Add(control);

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


			public TextBoxField(CharacterBioForm _form, FieldDesc _desc)
				: base(_form, _desc)
			{
				// create text box control
				TextBox textBox = new TextBox();
				textBox.Location = new Point(origin.X, label.Bottom + CharacterBioForm.fieldControlSpacingY);
				textBox.Width = _desc.controlWidth;
				textBox.Height = _desc.controlHeight;
				textBox.TextAlign = (System.Windows.Forms.HorizontalAlignment)_desc.controlTextAlign;
				textBox.Font = CharacterBioForm.textBoxFont;
				control = textBox;
				((_desc.addToPanel) ? (form.fieldPanel.Controls) : (form.Controls)).Add(control);

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

		// color
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
		const int				windowWidth = 1600;
		const int				windowHeight = 900;
		static readonly Size	windowSize = new Size(windowWidth, windowHeight);


		// fonts
		static readonly Font	labelFont = new Font("Roboto", 10, FontStyle.Bold);
		static readonly Font	textBoxFont = new Font("Roboto", 10);


		// color modes
		enum COLOR_MODE
		{
			DARK,
			GREY,
			LIGHT,

			MIN = DARK,
			MAX = LIGHT,
		}
		COLOR_MODE				colorMode = COLOR_MODE.DARK;
		ColorPalette[]			palettes = new ColorPalette[]
		{
			//				 form back color					field fore color					field mid color						field back color
			new ColorPalette(GreyTone(0x2f),					Color.White,						GreyTone(0x4f),						GreyTone(0x3f)), // dark
			new ColorPalette(GreyTone(0x6f),					Color.White,						GreyTone(0x67),						GreyTone(0x7f)), // grey
			new ColorPalette(GreyTone(0xdf),					Color.Black,						Color.White,						GreyTone(0xdf)), // light
		};


		// location/spacing
		const int				originX = 20;
		const int				originY = 20;

		const int				fieldSpacingX = 50;
		readonly int			fieldSpacingY = (int)(labelFont.Size);

		const int				fieldControlSpacingX = 10;
		static readonly int		fieldControlSpacingY = 0 /*(int)labelFont.Size / 2*/;

		static readonly int		defaultLabelHeight = (int)(labelFont.Size * 2.25f);

		Point					nextIndependentFieldPosition = new Point(originX, originY);
		Point					nextPanelFieldPosition = new Point(originX, originY);


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
		FieldDesc[] fieldDescriptors = new FieldDesc[]
		{
			new FieldDesc
			{
				type = FIELD_TYPE.BUTTON,
				addToPanel = false,

				labelText = CapCaseToFirstCaps(Convert.ToString(FIELD_NAME.COLOR_MODE)),
				labelWidth = 100,
				labelHeight = defaultLabelHeight,
				labelTextAlign = ContentAlignment.MiddleCenter,

				controlText = "",
				controlWidth = 100,
				controlHeight = 25,
				controlTextAlign = ContentAlignment.MiddleCenter,
			},
			new FieldDesc
			{
				type = FIELD_TYPE.TEXT_BOX,
				addToPanel = true,

				labelText = CapCaseToFirstCaps(Convert.ToString(FIELD_NAME.NAME)),
				labelWidth = 200,
				labelHeight = defaultLabelHeight,
				labelTextAlign = ContentAlignment.MiddleLeft,

				controlText = "",
				controlWidth = 200,
				controlHeight = 25,
				controlTextAlign = ContentAlignment.TopLeft,
			},
			//FIELD_TYPE.TEXT_BOX,
			//FIELD_TYPE.TEXT_BOX,
			//FIELD_TYPE.TEXT_BOX,
			//FIELD_TYPE.TEXT_BOX,
			//FIELD_TYPE.TEXT_BOX,
			//FIELD_TYPE.TEXT_BOX,
			//FIELD_TYPE.TEXT_BOX,
			//FIELD_TYPE.TEXT_BOX,
			//FIELD_TYPE.TEXT_BOX,
			//FIELD_TYPE.TEXT_BOX,
			//FIELD_TYPE.TEXT_BOX,
			//FIELD_TYPE.TEXT_BOX,
			//FIELD_TYPE.TEXT_BOX,
			//FIELD_TYPE.TEXT_BOX,
			//FIELD_TYPE.TEXT_BOX,
			//FIELD_TYPE.TEXT_BOX,
			//FIELD_TYPE.TEXT_BOX,
			//FIELD_TYPE.TEXT_BOX,
			//FIELD_TYPE.TEXT_BOX,
			//FIELD_TYPE.TEXT_BOX,
			//FIELD_TYPE.TEXT_BOX,
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
			nextIndependentFieldPosition.Y = _field.bottom + fieldSpacingY;
		}
		void RemoveField(Field _field) { fieldList.Remove(_field); }

		// functions to manage colors
		void SetFormColors(Color _formBackColor, Color _fieldForeColor, Color _fieldMidColor, Color _fieldBackColor)
		{
			BackColor = _formBackColor;
			for (int i = 0; i < fieldList.Count; ++i)
			{
				fieldList[i].SetForeColor(_fieldForeColor);
				fieldList[i].SetMidColor(_fieldMidColor);
				fieldList[i].SetBackColor(_fieldBackColor);
			}
		}
		void SetFormColors(ColorPalette _palette)
		{
			SetFormColors(_palette.formBackColor, _palette.fieldForeColor, _palette.fieldMidColor, _palette.fieldBackColor);
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
			fieldPanel.Location = new Point(0, 100);
			fieldPanel.Size = windowSize;
			fieldPanel.AutoScroll = true;
			fieldPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			Controls.Add(fieldPanel);

			// init fields and add them to form
			List<Field> independentFields = new List<Field>();
			List<Field> panelFields = new List<Field>();
			for (int i = 0; i < fieldDescriptors.Length; ++i)
			{
				switch (fieldDescriptors[i].type)
				{
					default: break;
					case FIELD_TYPE.BASIC:
						AddField(new Field(this, fieldDescriptors[i]));
						break;
					case FIELD_TYPE.BUTTON:
						AddField(new ButtonField(this, fieldDescriptors[i]));
						break;
					case FIELD_TYPE.TEXT_BOX:
						AddField(new TextBoxField(this, fieldDescriptors[i]));
						break;
				}
			}
			for (int i = 0; i < independentFields.Count; ++i)
			{

			}
			for (int i = 0; i < panelFields.Count; ++i)
			{

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
