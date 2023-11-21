using ColorEx.Internal;
using System;
using System.Windows.Media;

namespace ColorEx.Wpf;

public static class ColorExtensions {
	// ----------------------------------- Rrb -----------------------------------

	public static Color FromArgb(in double alpha, in double red, in double green, in double blue) =>
		Color.FromArgb(
			(byte) (alpha * 255),
			(byte) (red * 255),
			(byte) (green * 255),
			(byte) (blue * 255)
		);

	public static Color FromRgb(in double red, in double green, in double blue) =>
		Color.FromArgb(
			0xFF,
			(byte) (red * 255),
			(byte) (green * 255),
			(byte) (blue * 255)
		);

	public static void ToRgb(this Color color, out double red, out double green, out double blue) {
		red = color.R / 255d;
		green = color.G / 255d;
		blue = color.B / 255d;
	}

	public static void ToArgb(this Color color, out double alpha, out double red, out double green, out double blue) {
		alpha = color.A / 255d;

		color.ToRgb(out red, out green, out blue);
	}

	public static uint ToUint(this Color color) {
		return Rgb.ToUint(color.A, color.R, color.G, color.B);
	}

	public static Color ToColor(this uint color) {
		Rgb.FromUint(color, out byte alpha, out byte red, out byte green, out byte blue);

		return Color.FromArgb(alpha, red, green, blue);
	}

	public static bool IsBright(this Color color) =>
		Rgb.IsBright(color.R, color.G, color.B);

	public static Color ToBlackIfBright(this Color color) =>
		color.IsBright() ? Colors.Black : Colors.White;

	public static Color Multiply(this Color color, in double factor) {
		byte
			red = color.R,
			green = color.G,
			blue = color.B;

		Rgb.Multiply(ref red, ref green, ref blue, in factor);

		return Color.FromArgb(color.A, red, green, blue);
	}

	public static byte Average(this Color color) {
		byte
			red = color.R,
			green = color.G,
			blue = color.B;

		return Rgb.Average(ref red, ref green, ref blue);
	}

	public static Color ChangeAlpha(this Color color, in byte alpha) =>
		Color.FromArgb(alpha, color.R, color.G, color.B);

	// ----------------------------------- Hsb -----------------------------------

	public static Color FromHsb(in double hue, in double saturation, in double brightness, in double alpha = 1) {
		Hsb.ToRgb(
			in hue,
			in saturation,
			in brightness,

			out var red,
			out var green,
			out var blue
		);

		return FromArgb(in alpha, in red, in green, in blue);
	}

	public static void ToHsb(this Color color, out double hue, out double saturation, out double brightness) {
		color.ToRgb(out var red, out var green, out var blue);

		Hsb.FromRgb(
			in red,
			in green,
			in blue,

			out hue,
			out saturation,
			out brightness
		);
	}

	public static Color NextHsbColor(this Random random, in double saturation, in double brightness, in double alpha = 1) {
		Hsb.RandomHueToRgb(
			random,
			in saturation,
			in brightness,

			out var red,
			out var green,
			out var blue
		);

		return FromArgb(in alpha, in red, in green, in blue);
	}

	public static Color InterpolateHue(in double hueMin, in double hueMax, in double hueFactor, in double saturation, in double brightness, in double alpha = 1) {
		Hsb.InterpolateHueToRgb(
			in hueMin,
			in hueMax,
			in hueFactor,
			in saturation,
			in brightness,

			out var red,
			out var green,
			out var blue
		);

		return FromArgb(in alpha, in red, in green, in blue);
	}

	public static Color ChangeSaturation(this Color color, in double saturation, in double alpha = 1) {
		color.ToHsb(out var hue, out _, out var brightness);

		return FromHsb(in hue, in saturation, in brightness, in alpha);
	}

	public static Color ChangeBrightness(this Color color, in double brightness, in double alpha = 1) {
		color.ToHsb(out var hue, out var saturation, out _);

		return FromHsb(in hue, in saturation, in brightness, in alpha);
	}

	public static Color ChangeSaturationAndBrightness(this Color color, in double saturation, in double brightness, in double alpha = 1) {
		color.ToHsb(out var hue, out _, out _);

		return FromHsb(in hue, in saturation, in brightness, in alpha);
	}

	public static Color MultiplySaturationAndBrightness(this Color color, in double saturationFactor, in double brightnessFactor, in double alpha = 1) {
		Assert.IsValueInRange0_1(in saturationFactor, "saturationFactor");
		Assert.IsValueInRange0_1(in brightnessFactor, "brightnessFactor");

		color.ToHsb(out var hue, out var saturation, out var brightness);

		return FromHsb(
			in hue,
			Math.Min(saturation * saturationFactor, 1),
			Math.Min(brightness * brightnessFactor, 1),
			in alpha
		);
	}

	// ----------------------------------- Brushes -----------------------------------

	public static SolidColorBrush ChangeAlpha(this SolidColorBrush source, in byte alpha) {
		var color = source.Color;
		color.A = alpha;

		return new SolidColorBrush(color);
	}

	public static SolidColorBrush ToSolidColorBrush(this uint color) =>
		new(color.ToColor());
}
