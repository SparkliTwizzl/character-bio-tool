using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static CharacterBioTool.StringManipulation;



namespace CharacterBioTool
{
	public partial class CharacterBioForm : Form
	{

		#region field wrapper classes

		// wrapper classes for winform controls to bundle labels with other functionality
		// fields are limited to one functional control each
		
		public class ButtonField : Field
		{
			//public Button button;


			public ButtonField(CharacterBioForm _form, Desc _desc)
				: base(_form, _desc)
			{
				// init button
				Button button = new Button();
				button.Dock = (desc.controlStyle.dockStyle != DockStyle.None) ? (desc.controlStyle.dockStyle) : (DockStyle.Bottom);
				button.Text = desc.controlText;
				if (desc.controlStyle.width == 0 && desc.controlStyle.height == 0) button.AutoSize = true;
				if (desc.controlStyle.width != 0) button.Width = desc.controlStyle.width;
				if (desc.controlStyle.height != 0) button.Height = desc.controlStyle.height;
				if (desc.panelStyle.height == 0 && desc.controlStyle.height == 0)
					Height += button.Height;
				button.TextAlign = desc.controlStyle.textAlign;

				// add to panel
				control = button;
				Controls.Add(control);
			}

			public override void SetForeColor(Color _color)
			{
				control.ForeColor = _color;
				base.SetForeColor(_color);
			}
			public override void SetMidColor(Color _color)
			{
				control.BackColor = _color;
				base.SetMidColor(_color);
			}
		} // end class
		public class TextBoxField : Field
		{
			//public TextBox textBox;


			public TextBoxField(CharacterBioForm _form, Desc _desc)
				: base(_form, _desc)
			{
				// init text box
				TextBox textBox = new TextBox();
				textBox.Dock = (desc.controlStyle.dockStyle != DockStyle.None) ? (desc.controlStyle.dockStyle) : (DockStyle.Bottom);
				textBox.Text = desc.controlText;
				if (desc.controlStyle.width != 0) textBox.Width = desc.controlStyle.width;
				if (desc.controlStyle.height != 0) textBox.Height = desc.controlStyle.height;
				if (desc.panelStyle.height == 0 && desc.controlStyle.height == 0)
					Height += textBox.Height;
				switch (desc.controlStyle.textAlign)
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
				textBox.BorderStyle = (desc.controlStyle.border) ? (BorderStyle.Fixed3D) : (BorderStyle.None);
				textBox.Font = CharacterBioForm.textBoxFont;
				textBox.WordWrap = true;

				// add to panel
				control = textBox;
				Controls.Add(control);
			}

			public override void SetForeColor(Color _color)
			{
				control.ForeColor = _color;
				base.SetForeColor(_color);
			}
			public override void SetMidColor(Color _color)
			{
				control.BackColor = _color;
				base.SetMidColor(_color);
			}
		} // end class

		#endregion field wrapper classes



		#region enums, structs

		// color
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


		#endregion enums, structs



		#region form variables

		// window
		public const int				windowWidth = 1600;
		public const int				windowHeight = 900;
		public static readonly Size		windowSize = new Size(windowWidth, windowHeight);


		// fonts
		public static readonly Font		labelFont = new Font("Roboto", 10, FontStyle.Bold);
		public static readonly Font		textBoxFont = new Font("Roboto", 9);


		// color modes
		public enum COLOR_MODE
		{
			DARK,
			GREY,
			LIGHT,

			MIN = DARK,
			MAX = LIGHT,
		}
		public COLOR_MODE				colorMode = COLOR_MODE.LIGHT;
		public ColorPalette[]			palettes = new ColorPalette[]
		{
			//				 form back color					field fore color					field mid color						field back color
			new ColorPalette(GreyTone(0x1f),					Color.White,						GreyTone(0x67),						GreyTone(0x4f)), // dark
			new ColorPalette(GreyTone(0x6f),					Color.White,						GreyTone(0x67),						GreyTone(0x7f)), // grey
			new ColorPalette(GreyTone(0xcf),					Color.Black,						Color.White,						GreyTone(0xe7)), // light
		};


		// location/spacing
		public const int				originX = 20;
		public const int				originY = 20;

		public const int				fieldSpacingX = 50;
		public readonly int				fieldSpacingY = (int)(labelFont.Size);

		public static readonly int		defaultLabelWidth = 200;
		public static readonly int		defaultLabelHeight = (int)(labelFont.Size * 2.25f);
		public static readonly int		defaultControlWidth = defaultLabelWidth;
		public static readonly int		defaultControlHeight = 50;

		public Point					nextIndependentFieldPosition = new Point(originX, originY);
		public Point					nextPanelFieldPosition = new Point(originX, originY);



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

		public static Field.ControlStyle		defaultPanelStyle = new Field.ControlStyle
		{
			border = true,
			padLeft = 5,
			padRight = 5,
			padTop = 5,
			padBottom = 5,
		};
		public static Field.ControlStyle		defaultTextFieldLabelStyle = new Field.ControlStyle
		{
			textAlign = ContentAlignment.MiddleLeft,
			width = defaultLabelWidth,
			height = defaultLabelHeight,
			border = true,
		};
		public static Field.ControlStyle		defaultTextFieldControlStyle = new Field.ControlStyle
		{
			textAlign = (ContentAlignment)HorizontalAlignment.Left,
			width = defaultControlWidth,
			border = true,
		};

		public Field.Desc[] fieldDescriptors = new Field.Desc[]
		{
			new Field.Desc
			{
				type = Field.TYPE.BUTTON,
				addToFormPanel = false,
				panelStyle = defaultPanelStyle,
				labelText = CapCaseToFirstCaps(Convert.ToString(FIELD_NAME.COLOR_MODE)),
				labelStyle = new Field.ControlStyle
				{
					textAlign = ContentAlignment.MiddleCenter,
					width = 100,
					height = defaultLabelHeight,
					border = true,
				},
				controlStyle = new Field.ControlStyle
				{
					textAlign = ContentAlignment.MiddleCenter,
					width = 100,
					border = true,
				},
			},
			new Field.Desc
			{
				type = Field.TYPE.TEXT_BOX,
				addToFormPanel = true,
				panelStyle = defaultPanelStyle,
				labelText = CapCaseToFirstCaps(Convert.ToString(FIELD_NAME.NAME)),
				labelStyle = defaultTextFieldLabelStyle,
				controlStyle = defaultTextFieldControlStyle,
			},
			new Field.Desc
			{
				type = Field.TYPE.TEXT_BOX,
				addToFormPanel = true,
				panelStyle = defaultPanelStyle,
				labelText = CapCaseToFirstCaps(Convert.ToString(FIELD_NAME.NICKNAME)),
				labelStyle = defaultTextFieldLabelStyle,
				controlStyle = defaultTextFieldControlStyle,
			},
			new Field.Desc
			{
				type = Field.TYPE.TEXT_BOX,
				addToFormPanel = true,
				panelStyle = defaultPanelStyle,
				labelText = CapCaseToFirstCaps(Convert.ToString(FIELD_NAME.RACE)),
				labelStyle = defaultTextFieldLabelStyle,
				controlStyle = defaultTextFieldControlStyle,
			},
			new Field.Desc
			{
				type = Field.TYPE.TEXT_BOX,
				addToFormPanel = true,
				panelStyle = defaultPanelStyle,
				labelText = CapCaseToFirstCaps(Convert.ToString(FIELD_NAME.GENDER)),
				labelStyle = defaultTextFieldLabelStyle,
				controlStyle = defaultTextFieldControlStyle,
			},
			new Field.Desc
			{
				type = Field.TYPE.TEXT_BOX,
				addToFormPanel = true,
				panelStyle = defaultPanelStyle,
				labelText = CapCaseToFirstCaps(Convert.ToString(FIELD_NAME.SEX)),
				labelStyle = defaultTextFieldLabelStyle,
				controlStyle = defaultTextFieldControlStyle,
			},
			new Field.Desc
			{
				type = Field.TYPE.TEXT_BOX,
				addToFormPanel = true,
				panelStyle = defaultPanelStyle,
				labelText = ConvertCaseToFirstCaps(Convert.ToString(FIELD_NAME.BIRTH_DATE)),
				labelStyle = defaultTextFieldLabelStyle,
				controlStyle = defaultTextFieldControlStyle,
			},
			new Field.Desc
			{
				type = Field.TYPE.TEXT_BOX,
				addToFormPanel = true,
				panelStyle = defaultPanelStyle,
				labelText = CapCaseToFirstCaps(Convert.ToString(FIELD_NAME.DEATH_DATE)),
				labelStyle = defaultTextFieldLabelStyle,
				controlStyle = defaultTextFieldControlStyle,
			},
			new Field.Desc
			{
				type = Field.TYPE.TEXT_BOX,
				addToFormPanel = true,
				panelStyle = defaultPanelStyle,
				labelText = CapCaseToFirstCaps(Convert.ToString(FIELD_NAME.LITERAL_AGE)),
				labelStyle = defaultTextFieldLabelStyle,
				controlStyle = defaultTextFieldControlStyle,
			},
			new Field.Desc
			{
				type = Field.TYPE.TEXT_BOX,
				addToFormPanel = true,
				panelStyle = defaultPanelStyle,
				labelText = CapCaseToFirstCaps(Convert.ToString(FIELD_NAME.PHYSICAL_AGE)),
				labelStyle = defaultTextFieldLabelStyle,
				controlStyle = defaultTextFieldControlStyle,
			},
			new Field.Desc
			{
				type = Field.TYPE.TEXT_BOX,
				addToFormPanel = true,
				panelStyle = defaultPanelStyle,
				labelText = CapCaseToFirstCaps(Convert.ToString(FIELD_NAME.APPARENT_AGE)),
				labelStyle = defaultTextFieldLabelStyle,
				controlStyle = defaultTextFieldControlStyle,
			},
			new Field.Desc
			{
				type = Field.TYPE.TEXT_BOX,
				addToFormPanel = true,
				panelStyle = defaultPanelStyle,
				labelText = CapCaseToFirstCaps(Convert.ToString(FIELD_NAME.HEIGHT)),
				labelStyle = defaultTextFieldLabelStyle,
				controlStyle = defaultTextFieldControlStyle,
			},
			new Field.Desc
			{
				type = Field.TYPE.TEXT_BOX,
				addToFormPanel = true,
				panelStyle = defaultPanelStyle,
				labelText = CapCaseToFirstCaps(Convert.ToString(FIELD_NAME.WEIGHT)),
				labelStyle = defaultTextFieldLabelStyle,
				controlStyle = defaultTextFieldControlStyle,
			},
			new Field.Desc
			{
				type = Field.TYPE.TEXT_BOX,
				addToFormPanel = true,
				panelStyle = defaultPanelStyle,
				labelText = CapCaseToFirstCaps(Convert.ToString(FIELD_NAME.BUILD)),
				labelStyle = defaultTextFieldLabelStyle,
				controlStyle = defaultTextFieldControlStyle,
			},
			new Field.Desc
			{
				type = Field.TYPE.TEXT_BOX,
				addToFormPanel = true,
				panelStyle = defaultPanelStyle,
				labelText = CapCaseToFirstCaps(Convert.ToString(FIELD_NAME.SKIN_COLOR)),
				labelStyle = defaultTextFieldLabelStyle,
				controlStyle = defaultTextFieldControlStyle,
			},
			new Field.Desc
			{
				type = Field.TYPE.TEXT_BOX,
				addToFormPanel = true,
				panelStyle = defaultPanelStyle,
				labelText = CapCaseToFirstCaps(Convert.ToString(FIELD_NAME.EYE_COLOR)),
				labelStyle = defaultTextFieldLabelStyle,
				controlStyle = defaultTextFieldControlStyle,
			},
			new Field.Desc
			{
				type = Field.TYPE.TEXT_BOX,
				addToFormPanel = true,
				panelStyle = defaultPanelStyle,
				labelText = CapCaseToFirstCaps(Convert.ToString(FIELD_NAME.HAIR_LENGTH)),
				labelStyle = defaultTextFieldLabelStyle,
				controlStyle = defaultTextFieldControlStyle,
			},
			new Field.Desc
			{
				type = Field.TYPE.TEXT_BOX,
				addToFormPanel = true,
				panelStyle = defaultPanelStyle,
				labelText = CapCaseToFirstCaps(Convert.ToString(FIELD_NAME.HAIR_STYLE)),
				labelStyle = defaultTextFieldLabelStyle,
				controlStyle = defaultTextFieldControlStyle,
			},
			new Field.Desc
			{
				type = Field.TYPE.TEXT_BOX,
				addToFormPanel = true,
				panelStyle = defaultPanelStyle,
				labelText = CapCaseToFirstCaps(Convert.ToString(FIELD_NAME.HAIR_COLOR)),
				labelStyle = defaultTextFieldLabelStyle,
				controlStyle = defaultTextFieldControlStyle,
			},
			new Field.Desc
			{
				type = Field.TYPE.TEXT_BOX,
				addToFormPanel = true,
				panelStyle = defaultPanelStyle,
				labelText = CapCaseToFirstCaps(Convert.ToString(FIELD_NAME.FACIAL_HAIR_LENGTH)),
				labelStyle = defaultTextFieldLabelStyle,
				controlStyle = defaultTextFieldControlStyle,
			},
			new Field.Desc
			{
				type = Field.TYPE.TEXT_BOX,
				addToFormPanel = true,
				panelStyle = defaultPanelStyle,
				labelText = CapCaseToFirstCaps(Convert.ToString(FIELD_NAME.FACIAL_HAIR_STYLE)),
				labelStyle = defaultTextFieldLabelStyle,
				controlStyle = defaultTextFieldControlStyle,
			},
			new Field.Desc
			{
				type = Field.TYPE.TEXT_BOX,
				addToFormPanel = true,
				panelStyle = defaultPanelStyle,
				labelText = CapCaseToFirstCaps(Convert.ToString(FIELD_NAME.FACIAL_HAIR_COLOR)),
				labelStyle = defaultTextFieldLabelStyle,
				controlStyle = defaultTextFieldControlStyle,
			},
		};
		public Panel					fieldPanel = new Panel();
		public List<Field>				fieldList = new List<Field>();

		#endregion form variables



		#region helper functions

		// generates a Color with the specified grey tone
		public static Color GreyTone(int _tone) { return Color.FromArgb(_tone, _tone, _tone); }

		// converts a CAP_CASE string to First Capitals Case
		public static string CapCaseToFirstCaps(string _str)
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
			if (_field.desc.addToFormPanel)
				nextPanelFieldPosition.Y = _field.Bottom + fieldSpacingY;
			else
				nextIndependentFieldPosition.Y = _field.Bottom + fieldSpacingY;
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
			fieldPanel.BorderStyle = BorderStyle.Fixed3D;
			fieldPanel.Padding = new Padding(10);
			Controls.Add(fieldPanel);

			// init fields and add them to form
			for (int i = 0; i < fieldDescriptors.Length; ++i)
			{
				switch (fieldDescriptors[i].type)
				{
					default: break;
					case Field.TYPE.BASE:
						AddField(new Field(this, fieldDescriptors[i]));
						break;
					case Field.TYPE.BUTTON:
						AddField(new ButtonField(this, fieldDescriptors[i]));
						break;
					case Field.TYPE.TEXT_BOX:
						AddField(new TextBoxField(this, fieldDescriptors[i]));
						break;
				}
			}

			// set initial color mode
			SetColorMode(colorMode);


			// set field custom behaviors

			// set color mode button behavior with event handler lambda
			GetField(FIELD_NAME.COLOR_MODE).control.Click += (obj, eventArgs) =>
			{
				ActiveControl = null; // defocus button after clicking it
				colorMode = (colorMode == COLOR_MODE.MAX) ? (COLOR_MODE.MIN) : (colorMode + 1); // cycle to next color mode
				SetColorMode(colorMode);
			};
		}

	} // end class
} // end namespace
