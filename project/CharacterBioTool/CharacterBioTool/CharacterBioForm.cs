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

		public struct ControlStyle
		{
			public String				text;
			public ContentAlignment		textAlign;
			public DockStyle			dockStyle;
			public bool					autoSize;
			public int					width;
			public int					height;
			public bool					border;
			public int					padLeft;
			public int					padRight;
			public int					padTop;
			public int					padBottom;
			public int					marginLeft;
			public int					marginRight;
			public int					marginTop;
			public int					marginBottom;
		}
		public struct FieldDesc
		{
			public FIELD_TYPE			type;
			public bool					addToFormPanel;
			public ControlStyle			panelStyle;
			public ControlStyle			labelStyle;
			public ControlStyle			controlStyle;
		}


		public class Field : Panel
		{
			public CharacterBioForm		form;
			public FieldDesc			desc;
			public Label				label;
			public Control				control;


			public Field(CharacterBioForm _form, FieldDesc _desc)
			{
				// store values
				form = _form;
				desc = _desc;

				// add panel to parent control
				((desc.addToFormPanel) ? (form.fieldPanel.Controls) : (form.Controls)).Add(this);

				// init panel
				Location = (desc.addToFormPanel) ? (_form.nextPanelFieldPosition) : (_form.nextIndependentFieldPosition);
				if (desc.panelStyle.width == 0 && desc.panelStyle.height == 0) AutoSize = true;
				Size = new Size(
					((desc.panelStyle.width != 0) ? (desc.panelStyle.width) : (Math.Max(desc.labelStyle.width, desc.controlStyle.width))) + desc.panelStyle.padLeft + desc.panelStyle.padRight,
					((desc.panelStyle.height != 0) ? (desc.panelStyle.height) : (desc.labelStyle.height + desc.controlStyle.height)) + desc.panelStyle.padTop + desc.panelStyle.padBottom
					);
				if (desc.panelStyle.border) BorderStyle = BorderStyle.FixedSingle;
				Padding = new Padding(desc.panelStyle.padLeft, desc.panelStyle.padRight, desc.panelStyle.padTop, desc.panelStyle.padBottom);

				// init label
				label = new Label();
				label.Dock = (desc.labelStyle.dockStyle != DockStyle.None) ? desc.labelStyle.dockStyle : DockStyle.Top;
				label.Text = desc.labelStyle.text;
				if (desc.labelStyle.width == 0 && desc.labelStyle.height == 0) label.AutoSize = true;
				if (desc.labelStyle.width != 0) label.Width = desc.labelStyle.width;
				if (desc.labelStyle.height != 0) label.Height = desc.labelStyle.height;
				label.TextAlign = desc.labelStyle.textAlign;
				label.Font = CharacterBioForm.labelFont;
				label.BorderStyle = (desc.labelStyle.border) ? (BorderStyle.FixedSingle) : (BorderStyle.None);

				// add to panel
				Controls.Add(label);
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
			public virtual void SetBackColor(Color _color)
			{
				BackColor = _color;
			}
		} // end class
		public class ButtonField : Field
		{
			//public Button button;


			public ButtonField(CharacterBioForm _form, FieldDesc _desc)
				: base(_form, _desc)
			{
				// init button
				Button button = new Button();
				button.Location = new Point(button.Location.X, label.Bottom);
				button.Text = desc.controlStyle.text;
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


			public TextBoxField(CharacterBioForm _form, FieldDesc _desc)
				: base(_form, _desc)
			{
				// init text box
				TextBox textBox = new TextBox();
				textBox.Location = new Point(0, label.Bottom);
				textBox.Text = desc.controlStyle.text;
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
		public static readonly Font		labelFont = new Font("Roboto", 11, FontStyle.Bold);
		public static readonly Font		textBoxFont = new Font("Roboto", 10);


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
		static readonly int				defaultControlHeight = 50;

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
		public FieldDesc[] fieldDescriptors = new FieldDesc[]
		{
			new FieldDesc
			{
				type = FIELD_TYPE.BUTTON,
				addToFormPanel = false,
				panelStyle = new ControlStyle
				{
					border = true,
					padLeft = 5,
					padRight = 5,
					padTop = 5,
					padBottom = 5,
				},
				labelStyle = new ControlStyle
				{
					text = CapCaseToFirstCaps(Convert.ToString(FIELD_NAME.COLOR_MODE)),
					textAlign = ContentAlignment.MiddleCenter,
					width = 100,
					height = defaultLabelHeight,
					border = true,
				},
				controlStyle = new ControlStyle
				{
					textAlign = ContentAlignment.MiddleCenter,
					width = 100,
					border = true,
				},
			},
			new FieldDesc
			{
				type = FIELD_TYPE.TEXT_BOX,
				addToFormPanel = true,
				panelStyle = new ControlStyle
				{
					border = true,
					padLeft = 5,
					padRight = 5,
					padTop = 5,
					padBottom = 5,
				},
				labelStyle = new ControlStyle
				{
					text = CapCaseToFirstCaps(Convert.ToString(FIELD_NAME.NAME)),
					textAlign = ContentAlignment.MiddleLeft,
					width = defaultLabelWidth,
					height = defaultLabelHeight,
					border = true,
				},
				controlStyle = new ControlStyle
				{
					text = "test",
					textAlign = (ContentAlignment)HorizontalAlignment.Left,
					width = defaultControlWidth,
				},
			},
			new FieldDesc
			{
				type = FIELD_TYPE.TEXT_BOX,
				addToFormPanel = true,
				panelStyle = new ControlStyle
				{
					border = true,
					padLeft = 5,
					padRight = 5,
					padTop = 5,
					padBottom = 5,
				},
				labelStyle = new ControlStyle
				{
					text = CapCaseToFirstCaps(Convert.ToString(FIELD_NAME.NICKNAME)),
					textAlign = ContentAlignment.MiddleLeft,
					width = defaultLabelWidth,
					height = defaultLabelHeight,
					border = true,
				},
				controlStyle = new ControlStyle
				{
					text = "asdf",
					textAlign = (ContentAlignment)HorizontalAlignment.Left,
					width = defaultControlWidth,
				},
			},
		};
		public Panel					fieldPanel = new Panel();
		public List<Field>				fieldList = new List<Field>();

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
			nextIndependentFieldPosition.Y = _field.Bottom + fieldSpacingY;
			nextPanelFieldPosition.Y = _field.Bottom + fieldSpacingY;
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
