using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using ExtensionMethods;



namespace CharacterBioTool
{
	public partial class CharacterBioForm : Form
	{

		public struct ColorPalette
		{
			public string name;
			public Color formBackColor;
			public Color fieldForeColor;
			public Color fieldMidColor;
			public Color fieldBackColor;
		}

		public enum ColorMode
		{
			Dark,
			Grey,
			Light,

			Min = Dark,
			Max = Light,
		}

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



		private const int windowWidth = 1600;
		public int WindowWidth
		{
			get => windowWidth;
		}

		private const int windowHeight = 900;
		public int WindowHeight
		{
			get => windowHeight;
		}

		private static readonly Size windowSize = new Size(windowWidth, windowHeight);
		public Size WindowSize
		{
			get => windowSize;
		}


		private static readonly Font labelFont = new Font("Roboto", 10, FontStyle.Bold);
		public Font LabelFont
		{
			get => labelFont;
		}

		private static readonly Font textBoxFont = new Font("Roboto", 9);
		public Font TextBoxFont
		{
			get => textBoxFont;
		}


		private static readonly ColorPalette[] palettes = new ColorPalette[]
		{
			new ColorPalette
			{
				name = "dark",
				formBackColor   = CreateGreyColor(0x1f),
				fieldForeColor  = Color.White,
				fieldMidColor   = CreateGreyColor(0x67),
				fieldBackColor  = CreateGreyColor(0x4f)
			},
			new ColorPalette
			{
				name = "grey",
				formBackColor   = CreateGreyColor(0x6f),
				fieldForeColor  = Color.White,
				fieldMidColor   = CreateGreyColor(0x67),
				fieldBackColor  = CreateGreyColor(0x7f)
			},
			new ColorPalette
			{
				name = "light",
				formBackColor   = CreateGreyColor(0xcf),
				fieldForeColor  = Color.Black,
				fieldMidColor   = Color.White,
				fieldBackColor  = CreateGreyColor(0xe7)
				},
		};
		public ColorPalette[] Palettes
		{
			get => palettes;
		}

		private static ColorMode activeColorMode = ColorMode.Light;
		public ColorMode ActiveColorMode
		{
			get => activeColorMode;
			set => activeColorMode = value;
		}


		private const int originX = 20;
		public int OriginX
		{
			get => originX;
		}

		private const int originY = 20;
		public int OriginY
		{
			get => originY;
		}


		private const int fieldSpacingX = 50;
		public int FieldSpacingX
		{
			get => fieldSpacingX;
		}

		private readonly int fieldSpacingY = (int)(labelFont.Size);
		public int FieldSpacingY
		{
			get => fieldSpacingY;
		}


		private const int defaultLabelWidth = 200;
		public int DefaultLabelWidth
		{
			get => defaultLabelWidth;
		}

		private static readonly int defaultLabelHeight = (int)(labelFont.Size * 2.25f);
		public int DefaultLabelHeight
		{
			get => defaultLabelHeight;
		}

		private const int defaultControlWidth = defaultLabelWidth;
		public int DefaultControlWidth
		{
			get => defaultControlWidth;
		}

		private const int defaultControlHeight = 50;
		public int DefaultControlHeight
		{
			get => defaultControlHeight;
		}


		private Point nextIndependentFieldPosition = new Point(originX, originY);
		public Point NextIndependentFieldPosition
		{
			get => nextIndependentFieldPosition;
			set => nextIndependentFieldPosition = value;
		}

		private Point nextPanelFieldPosition = new Point(originX, originY);
		public Point NextPanelFieldPosition
		{
			get => nextPanelFieldPosition;
			set => nextPanelFieldPosition = value;
		}


		private static readonly ControlStyle defaultPanelStyle = new ControlStyle
		{
			border = true,
			padLeft = 5,
			padRight = 5,
			padTop = 5,
			padBottom = 5,
		};
		public ControlStyle DefaultPanelStyle
		{
			get => defaultPanelStyle;
		}

		private static readonly ControlStyle defaultTextFieldLabelStyle = new ControlStyle
		{
			textAlign = ContentAlignment.MiddleLeft,
			width = defaultLabelWidth,
			height = defaultLabelHeight,
			border = true,
		};
		public ControlStyle DefaultTextFieldLabelStyle
		{
			get => defaultTextFieldLabelStyle;
		}

		private static readonly ControlStyle defaultTextFieldControlStyle = new ControlStyle
		{
			textAlign = (ContentAlignment)HorizontalAlignment.Left,
			width = defaultControlWidth,
			border = true,
		};
		public ControlStyle DefaultTextFieldControlStyle
		{
			get => defaultTextFieldControlStyle;
		}


		private static readonly FieldDesc[] fieldDescriptors = new FieldDesc[]
		{
			new FieldDesc
			{
				type = FieldType.Button,
				addToFormPanel = false,
				panelStyle = defaultPanelStyle,
				labelText = Convert.ToString(FieldName.ColorMode).SplitAtCaps(),
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
				labelText = Convert.ToString(FieldName.Name).SplitAtCaps(),
				labelStyle = defaultTextFieldLabelStyle,
				controlStyle = defaultTextFieldControlStyle,
			},
			new FieldDesc
			{
				type = FieldType.TextBox,
				addToFormPanel = true,
				panelStyle = defaultPanelStyle,
				labelText = Convert.ToString(FieldName.Nickname).SplitAtCaps(),
				labelStyle = defaultTextFieldLabelStyle,
				controlStyle = defaultTextFieldControlStyle,
			},
			new FieldDesc
			{
				type = FieldType.TextBox,
				addToFormPanel = true,
				panelStyle = defaultPanelStyle,
				labelText = Convert.ToString(FieldName.Race).SplitAtCaps(),
				labelStyle = defaultTextFieldLabelStyle,
				controlStyle = defaultTextFieldControlStyle,
			},
			new FieldDesc
			{
				type = FieldType.TextBox,
				addToFormPanel = true,
				panelStyle = defaultPanelStyle,
				labelText = Convert.ToString(FieldName.Gender).SplitAtCaps(),
				labelStyle = defaultTextFieldLabelStyle,
				controlStyle = defaultTextFieldControlStyle,
			},
			new FieldDesc
			{
				type = FieldType.TextBox,
				addToFormPanel = true,
				panelStyle = defaultPanelStyle,
				labelText = Convert.ToString(FieldName.Sex).SplitAtCaps(),
				labelStyle = defaultTextFieldLabelStyle,
				controlStyle = defaultTextFieldControlStyle,
			},
			new FieldDesc
			{
				type = FieldType.TextBox,
				addToFormPanel = true,
				panelStyle = defaultPanelStyle,
				labelText = Convert.ToString(FieldName.BirthDate).SplitAtCaps(),
				labelStyle = defaultTextFieldLabelStyle,
				controlStyle = defaultTextFieldControlStyle,
			},
			new FieldDesc
			{
				type = FieldType.TextBox,
				addToFormPanel = true,
				panelStyle = defaultPanelStyle,
				labelText = Convert.ToString(FieldName.DeathDate).SplitAtCaps(),
				labelStyle = defaultTextFieldLabelStyle,
				controlStyle = defaultTextFieldControlStyle,
			},
			new FieldDesc
			{
				type = FieldType.TextBox,
				addToFormPanel = true,
				panelStyle = defaultPanelStyle,
				labelText = Convert.ToString(FieldName.LiteralAge).SplitAtCaps(),
				labelStyle = defaultTextFieldLabelStyle,
				controlStyle = defaultTextFieldControlStyle,
			},
			new FieldDesc
			{
				type = FieldType.TextBox,
				addToFormPanel = true,
				panelStyle = defaultPanelStyle,
				labelText = Convert.ToString(FieldName.PhysicalAge).SplitAtCaps(),
				labelStyle = defaultTextFieldLabelStyle,
				controlStyle = defaultTextFieldControlStyle,
			},
			new FieldDesc
			{
				type = FieldType.TextBox,
				addToFormPanel = true,
				panelStyle = defaultPanelStyle,
				labelText = Convert.ToString(FieldName.ApparentAge).SplitAtCaps(),
				labelStyle = defaultTextFieldLabelStyle,
				controlStyle = defaultTextFieldControlStyle,
			},
			new FieldDesc
			{
				type = FieldType.TextBox,
				addToFormPanel = true,
				panelStyle = defaultPanelStyle,
				labelText = Convert.ToString(FieldName.Height).SplitAtCaps(),
				labelStyle = defaultTextFieldLabelStyle,
				controlStyle = defaultTextFieldControlStyle,
			},
			new FieldDesc
			{
				type = FieldType.TextBox,
				addToFormPanel = true,
				panelStyle = defaultPanelStyle,
				labelText = Convert.ToString(FieldName.Weight).SplitAtCaps(),
				labelStyle = defaultTextFieldLabelStyle,
				controlStyle = defaultTextFieldControlStyle,
			},
			new FieldDesc
			{
				type = FieldType.TextBox,
				addToFormPanel = true,
				panelStyle = defaultPanelStyle,
				labelText = Convert.ToString(FieldName.Build).SplitAtCaps(),
				labelStyle = defaultTextFieldLabelStyle,
				controlStyle = defaultTextFieldControlStyle,
			},
			new FieldDesc
			{
				type = FieldType.TextBox,
				addToFormPanel = true,
				panelStyle = defaultPanelStyle,
				labelText = Convert.ToString(FieldName.SkinColor).SplitAtCaps(),
				labelStyle = defaultTextFieldLabelStyle,
				controlStyle = defaultTextFieldControlStyle,
			},
			new FieldDesc
			{
				type = FieldType.TextBox,
				addToFormPanel = true,
				panelStyle = defaultPanelStyle,
				labelText = Convert.ToString(FieldName.EyeColor).SplitAtCaps(),
				labelStyle = defaultTextFieldLabelStyle,
				controlStyle = defaultTextFieldControlStyle,
			},
			new FieldDesc
			{
				type = FieldType.TextBox,
				addToFormPanel = true,
				panelStyle = defaultPanelStyle,
				labelText = Convert.ToString(FieldName.HairLength).SplitAtCaps(),
				labelStyle = defaultTextFieldLabelStyle,
				controlStyle = defaultTextFieldControlStyle,
			},
			new FieldDesc
			{
				type = FieldType.TextBox,
				addToFormPanel = true,
				panelStyle = defaultPanelStyle,
				labelText = Convert.ToString(FieldName.HairStyle).SplitAtCaps(),
				labelStyle = defaultTextFieldLabelStyle,
				controlStyle = defaultTextFieldControlStyle,
			},
			new FieldDesc
			{
				type = FieldType.TextBox,
				addToFormPanel = true,
				panelStyle = defaultPanelStyle,
				labelText = Convert.ToString(FieldName.HairColor).SplitAtCaps(),
				labelStyle = defaultTextFieldLabelStyle,
				controlStyle = defaultTextFieldControlStyle,
			},
			new FieldDesc
			{
				type = FieldType.TextBox,
				addToFormPanel = true,
				panelStyle = defaultPanelStyle,
				labelText = Convert.ToString(FieldName.FacialHairLength).SplitAtCaps(),
				labelStyle = defaultTextFieldLabelStyle,
				controlStyle = defaultTextFieldControlStyle,
			},
			new FieldDesc
			{
				type = FieldType.TextBox,
				addToFormPanel = true,
				panelStyle = defaultPanelStyle,
				labelText = Convert.ToString(FieldName.FacialHairStyle).SplitAtCaps(),
				labelStyle = defaultTextFieldLabelStyle,
				controlStyle = defaultTextFieldControlStyle,
			},
			new FieldDesc
			{
				type = FieldType.TextBox,
				addToFormPanel = true,
				panelStyle = defaultPanelStyle,
				labelText = Convert.ToString(FieldName.FacialHairColor).SplitAtCaps(),
				labelStyle = defaultTextFieldLabelStyle,
				controlStyle = defaultTextFieldControlStyle,
			},
		};
		public FieldDesc[] FieldDescriptors
		{
			get => fieldDescriptors;
		}


		private Panel fieldPanel = new Panel();
		public Panel FieldPanel
		{
			get => fieldPanel;
			set => fieldPanel = value;
		}

		private List<Field> fieldList = new List<Field>();
		public List<Field> FieldList
		{
			get => fieldList;
			set => fieldList = value;
		}



		#region helper functions

		// generates a Color with the specified brightness level
		public static Color CreateGreyColor(int _brightness)
		{
			return Color.FromArgb(_brightness, _brightness, _brightness);
		}


		// functions to manage fields
		void AddField(Field _field)
		{
			fieldList.Add(_field as Field);
			if (_field.Desc.addToFormPanel)
			{
				NextPanelFieldPosition = new Point(
					NextPanelFieldPosition.X,
					_field.Bottom + fieldSpacingY);
			}
			else
			{
				NextIndependentFieldPosition = new Point(
					NextIndependentFieldPosition.X,
					_field.Bottom + fieldSpacingY);
			}
		}
		void RemoveField(Field _field)
		{
			fieldList.Remove(_field);
		}


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
		void SetForeColorForAllFields(Color _color)
		{
			for (int i = 0; i < fieldList.Count; ++i)
				fieldList[i].SetForeColor(_color);
		}
		void SetMidColorForAllFields(Color _color)
		{
			for (int i = 0; i < fieldList.Count; ++i)
				fieldList[i].SetMidColor(_color);
		}
		void SetBackColorForAllFields(Color _color)
		{
			for (int i = 0; i < fieldList.Count; ++i)
				fieldList[i].SetBackColor(_color);
		}

		#endregion helper functions


		
		public CharacterBioForm()
		{
			InitializeComponent();
			Size = WindowSize;


			// create lambdas

			// create lambda to make accessing fields in list easier
			Func<FieldName, Field> GetField = _fieldName => ((int)_fieldName < fieldList.Count) ? (fieldList[(int)_fieldName]) : (null);

			// create color mode lambdas
			Func<ColorMode, ColorPalette> GetPalette = _colorMode => Palettes[(int)_colorMode];
			Action<ColorMode> SetColorMode = _colorMode =>
			{
				ColorPalette palette = GetPalette(ActiveColorMode);
				SetFormColors(palette);
				GetField(FieldName.ColorMode).Control.Text = Convert.ToString(ActiveColorMode);
			};


			// init Control panel
			fieldPanel.Location = new Point(0, 100);
			fieldPanel.Size = WindowSize;
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
			SetColorMode(ActiveColorMode);


			// set field custom behaviors

			// set color mode button behavior with event handler lambda
			GetField(FieldName.ColorMode).Control.Click += (obj, eventArgs) =>
			{
				ActiveControl = null; // defocus button after clicking it
				// cycle to next color mode
				ActiveColorMode = (ActiveColorMode == ColorMode.Max)
					? (ColorMode.Min)
					: (ActiveColorMode + 1);
				SetColorMode(ActiveColorMode);
			};
		}

	} // end class
} // end namespace
