using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using ExtensionMethods;



namespace CharacterBioTool
{
	public partial class CharacterBioForm : Form
	{


		#region form variables

		// window
		public const int				windowWidth = 1600;
		public const int				windowHeight = 900;
		public static readonly Size		windowSize = new Size(windowWidth, windowHeight);


		// fonts
		public static readonly Font		labelFont = new Font("Roboto", 10, FontStyle.Bold);
		public static readonly Font		textBoxFont = new Font("Roboto", 9);


		// color modes
		public struct ColorPalette
		{
			public string	name { get; set; }
			public Color	formBackColor { get; set; }
			public Color	fieldForeColor { get; set; }
			public Color	fieldMidColor { get; set; }
			public Color	fieldBackColor { get; set; }
		}
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
			new ColorPalette
			{
				name = "dark",
				formBackColor	= CreateGreyColor(0x1f),
				fieldForeColor	= Color.White,
				fieldMidColor	= CreateGreyColor(0x67),
				fieldBackColor	= CreateGreyColor(0x4f)
			},
			new ColorPalette
			{
				name = "grey",
				formBackColor	= CreateGreyColor(0x6f),
				fieldForeColor	= Color.White,
				fieldMidColor	= CreateGreyColor(0x67),
				fieldBackColor	= CreateGreyColor(0x7f)
			},
			new ColorPalette
			{
				name = "light",
				formBackColor	= CreateGreyColor(0xcf),
				fieldForeColor	= Color.Black,
				fieldMidColor	= Color.White,
				fieldBackColor	= CreateGreyColor(0xe7)
				},
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
		public enum FieldName
		{
			ColorMode,
			Name,
			Nickname,
			Race,
			Gender,
			Sex,
			BirthDate,
			DeathDate,
			LiteralAge,
			PhysicalAge,
			ApparentAge,
			Height,
			Weight,
			Build,
			SkinColor,
			EyeColor,
			HairLength,
			HairStyle,
			HairColor,
			FacialHairLength,
			FacialHairStyle,
			FacialHairColor,
			Attire,

			Count
		}

		public static ControlStyle		defaultPanelStyle = new ControlStyle
		{
			border = true,
			padLeft = 5,
			padRight = 5,
			padTop = 5,
			padBottom = 5,
		};
		public static ControlStyle		defaultTextFieldLabelStyle = new ControlStyle
		{
			textAlign = ContentAlignment.MiddleLeft,
			width = defaultLabelWidth,
			height = defaultLabelHeight,
			border = true,
		};
		public static ControlStyle		defaultTextFieldControlStyle = new ControlStyle
		{
			textAlign = (ContentAlignment)HorizontalAlignment.Left,
			width = defaultControlWidth,
			border = true,
		};

		public FieldDesc[] fieldDescriptors = new FieldDesc[]
		{
			new FieldDesc
			{
				type = FieldType.Button,
				addToFormPanel = false,
				panelStyle = defaultPanelStyle,
				labelText = Convert.ToString(FieldName.ColorMode).ToFirstCapsCase(),
				labelStyle = new ControlStyle
				{
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
				type = FieldType.TextBox,
				addToFormPanel = true,
				panelStyle = defaultPanelStyle,
				labelText = Convert.ToString(FieldName.Name).ToFirstCapsCase(),
				labelStyle = defaultTextFieldLabelStyle,
				controlStyle = defaultTextFieldControlStyle,
			},
			new FieldDesc
			{
				type = FieldType.TextBox,
				addToFormPanel = true,
				panelStyle = defaultPanelStyle,
				labelText = Convert.ToString(FieldName.Nickname).ToFirstCapsCase(),
				labelStyle = defaultTextFieldLabelStyle,
				controlStyle = defaultTextFieldControlStyle,
			},
			new FieldDesc
			{
				type = FieldType.TextBox,
				addToFormPanel = true,
				panelStyle = defaultPanelStyle,
				labelText = Convert.ToString(FieldName.Race).ToFirstCapsCase(),
				labelStyle = defaultTextFieldLabelStyle,
				controlStyle = defaultTextFieldControlStyle,
			},
			new FieldDesc
			{
				type = FieldType.TextBox,
				addToFormPanel = true,
				panelStyle = defaultPanelStyle,
				labelText = Convert.ToString(FieldName.Gender).ToFirstCapsCase(),
				labelStyle = defaultTextFieldLabelStyle,
				controlStyle = defaultTextFieldControlStyle,
			},
			new FieldDesc
			{
				type = FieldType.TextBox,
				addToFormPanel = true,
				panelStyle = defaultPanelStyle,
				labelText = Convert.ToString(FieldName.Sex).ToFirstCapsCase(),
				labelStyle = defaultTextFieldLabelStyle,
				controlStyle = defaultTextFieldControlStyle,
			},
			new FieldDesc
			{
				type = FieldType.TextBox,
				addToFormPanel = true,
				panelStyle = defaultPanelStyle,
				labelText = Convert.ToString(FieldName.BirthDate).ToFirstCapsCase(),
				labelStyle = defaultTextFieldLabelStyle,
				controlStyle = defaultTextFieldControlStyle,
			},
			new FieldDesc
			{
				type = FieldType.TextBox,
				addToFormPanel = true,
				panelStyle = defaultPanelStyle,
				labelText = Convert.ToString(FieldName.DeathDate).ToFirstCapsCase(),
				labelStyle = defaultTextFieldLabelStyle,
				controlStyle = defaultTextFieldControlStyle,
			},
			new FieldDesc
			{
				type = FieldType.TextBox,
				addToFormPanel = true,
				panelStyle = defaultPanelStyle,
				labelText = Convert.ToString(FieldName.LiteralAge).ToFirstCapsCase(),
				labelStyle = defaultTextFieldLabelStyle,
				controlStyle = defaultTextFieldControlStyle,
			},
			new FieldDesc
			{
				type = FieldType.TextBox,
				addToFormPanel = true,
				panelStyle = defaultPanelStyle,
				labelText = Convert.ToString(FieldName.PhysicalAge).ToFirstCapsCase(),
				labelStyle = defaultTextFieldLabelStyle,
				controlStyle = defaultTextFieldControlStyle,
			},
			new FieldDesc
			{
				type = FieldType.TextBox,
				addToFormPanel = true,
				panelStyle = defaultPanelStyle,
				labelText = Convert.ToString(FieldName.ApparentAge).ToFirstCapsCase(),
				labelStyle = defaultTextFieldLabelStyle,
				controlStyle = defaultTextFieldControlStyle,
			},
			new FieldDesc
			{
				type = FieldType.TextBox,
				addToFormPanel = true,
				panelStyle = defaultPanelStyle,
				labelText = Convert.ToString(FieldName.Height).ToFirstCapsCase(),
				labelStyle = defaultTextFieldLabelStyle,
				controlStyle = defaultTextFieldControlStyle,
			},
			new FieldDesc
			{
				type = FieldType.TextBox,
				addToFormPanel = true,
				panelStyle = defaultPanelStyle,
				labelText = Convert.ToString(FieldName.Weight).ToFirstCapsCase(),
				labelStyle = defaultTextFieldLabelStyle,
				controlStyle = defaultTextFieldControlStyle,
			},
			new FieldDesc
			{
				type = FieldType.TextBox,
				addToFormPanel = true,
				panelStyle = defaultPanelStyle,
				labelText = Convert.ToString(FieldName.Build).ToFirstCapsCase(),
				labelStyle = defaultTextFieldLabelStyle,
				controlStyle = defaultTextFieldControlStyle,
			},
			new FieldDesc
			{
				type = FieldType.TextBox,
				addToFormPanel = true,
				panelStyle = defaultPanelStyle,
				labelText = Convert.ToString(FieldName.SkinColor).ToFirstCapsCase(),
				labelStyle = defaultTextFieldLabelStyle,
				controlStyle = defaultTextFieldControlStyle,
			},
			new FieldDesc
			{
				type = FieldType.TextBox,
				addToFormPanel = true,
				panelStyle = defaultPanelStyle,
				labelText = Convert.ToString(FieldName.EyeColor).ToFirstCapsCase(),
				labelStyle = defaultTextFieldLabelStyle,
				controlStyle = defaultTextFieldControlStyle,
			},
			new FieldDesc
			{
				type = FieldType.TextBox,
				addToFormPanel = true,
				panelStyle = defaultPanelStyle,
				labelText = Convert.ToString(FieldName.HairLength).ToFirstCapsCase(),
				labelStyle = defaultTextFieldLabelStyle,
				controlStyle = defaultTextFieldControlStyle,
			},
			new FieldDesc
			{
				type = FieldType.TextBox,
				addToFormPanel = true,
				panelStyle = defaultPanelStyle,
				labelText = Convert.ToString(FieldName.HairStyle).ToFirstCapsCase(),
				labelStyle = defaultTextFieldLabelStyle,
				controlStyle = defaultTextFieldControlStyle,
			},
			new FieldDesc
			{
				type = FieldType.TextBox,
				addToFormPanel = true,
				panelStyle = defaultPanelStyle,
				labelText = Convert.ToString(FieldName.HairColor).ToFirstCapsCase(),
				labelStyle = defaultTextFieldLabelStyle,
				controlStyle = defaultTextFieldControlStyle,
			},
			new FieldDesc
			{
				type = FieldType.TextBox,
				addToFormPanel = true,
				panelStyle = defaultPanelStyle,
				labelText = Convert.ToString(FieldName.FacialHairLength).ToFirstCapsCase(),
				labelStyle = defaultTextFieldLabelStyle,
				controlStyle = defaultTextFieldControlStyle,
			},
			new FieldDesc
			{
				type = FieldType.TextBox,
				addToFormPanel = true,
				panelStyle = defaultPanelStyle,
				labelText = Convert.ToString(FieldName.FacialHairStyle).ToFirstCapsCase(),
				labelStyle = defaultTextFieldLabelStyle,
				controlStyle = defaultTextFieldControlStyle,
			},
			new FieldDesc
			{
				type = FieldType.TextBox,
				addToFormPanel = true,
				panelStyle = defaultPanelStyle,
				labelText = Convert.ToString(FieldName.FacialHairColor).ToFirstCapsCase(),
				labelStyle = defaultTextFieldLabelStyle,
				controlStyle = defaultTextFieldControlStyle,
			},
		};
		public Panel					fieldPanel = new Panel();
		public List<Field>				fieldList = new List<Field>();

		#endregion form variables



		#region helper functions

		// generates a Color with the specified brightness level
		public static Color CreateGreyColor(int _brightness) { return Color.FromArgb(_brightness, _brightness, _brightness); }

		public static string ConvertStringCaseToFirstCaps(string _string)
		{
			char[] stringAsChars = _string.ToLower().ToCharArray();
			// TODO: find & capitalize every letter that does not have a letter before it
			return new string(stringAsChars);
		}



		// converts a string from snake_case to First Capitals Case
		//public static string ConvertStringCaseFromSnakeToFirstCaps(string _str)
		//{
		//	//StringBuilder str = new StringBuilder();

		//	//// convert string to lowercase first so each letter doesn't have to be converted separately
		//	//_str = _str.ToLower();
		//	//// handle first letter separately, since there is no previous letter to compare it to
		//	//// replace underscore with space
		//	//if (_str[0] == '_')
		//	//	str.Append(' ');
		//	//// capitalize char
		//	//else
		//	//	str.Append(_str[0].ToString().ToUpper());

		//	//// handle rest of string
		//	//for (int i = 1; i < _str.Length; ++i)
		//	//{
		//	//	// replace underscores with spaces
		//	//	if (_str[i] == '_')
		//	//	{
		//	//		str.Append(' ');
		//	//	}
		//	//	else
		//	//	{
		//	//		// if previous char is underscore, capitalize this char
		//	//		if (_str[i - 1] == '_')
		//	//			str.Append(_str[i].ToString().ToUpper());
		//	//		// otherwise, add char unmodified
		//	//		else
		//	//			str.Append(_str[i]);
		//	//	}
		//	//}

		//	//return str.ToString();


		//	// TODO: rewrite using string manip functions
		//}

		// functions to manage fields
		void AddField(Field _field)
		{
			fieldList.Add(_field as Field);
			if (_field.Desc.addToFormPanel)
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


		
		public CharacterBioForm()
		{
			InitializeComponent();
			Size = windowSize;


			// create lambdas

			// create lambda to make accessing fields in list easier
			Func<FieldName, Field> GetField = _fieldName => ((int)_fieldName < fieldList.Count) ? (fieldList[(int)_fieldName]) : (null);

			// create color mode lambdas
			Func<COLOR_MODE, ColorPalette> GetPalette = _colorMode => palettes[(int)_colorMode];
			Action<COLOR_MODE> SetColorMode = _colorMode =>
			{
				ColorPalette palette = GetPalette(colorMode);
				SetFormColors(palette);
				GetField(FieldName.ColorMode).Control.Text = Convert.ToString(colorMode);
			};


			// init Control panel
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
					case FieldType.Basic:
						AddField(new Field(this, fieldDescriptors[i]));
						break;
					case FieldType.Button:
						AddField(new ButtonField(this, fieldDescriptors[i]));
						break;
					case FieldType.TextBox:
						AddField(new TextBoxField(this, fieldDescriptors[i]));
						break;
				}
			}

			// set initial color mode
			SetColorMode(colorMode);


			// set field custom behaviors

			// set color mode button behavior with event handler lambda
			GetField(FieldName.ColorMode).Control.Click += (obj, eventArgs) =>
			{
				ActiveControl = null; // defocus button after clicking it
				colorMode = (colorMode == COLOR_MODE.MAX) ? (COLOR_MODE.MIN) : (colorMode + 1); // cycle to next color mode
				SetColorMode(colorMode);
			};
		}

	} // end class
} // end namespace
