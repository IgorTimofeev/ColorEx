using System.Windows.Media;

namespace ColorEx.Wpf;

public static class ColorExtensions {
	public static Color FromArgb(in double alpha, in double red, in double green, in double blue) {
		return Color.FromArgb(
			(byte) (alpha * 255),
			(byte) (red * 255),
			(byte) (green * 255),
			(byte) (blue * 255)
		);
	}
	
	public static void ToRgb(this Color color, out double red, out double green, out double blue) {
		red = color.R / 255d;
		green = color.G / 255d;
		blue = color.B / 255;
	}

	public static void ToArgb(this Color color, out double alpha, out double red, out double green, out double blue) {
		alpha = color.A / 255d;

		color.ToRgb(out red, out green, out blue);
	}

	public static Color FromHsb(in double hue, in double saturation, in double brightness, byte alpha = 0xFF) {
		Hsb.ToRgb(in hue, in saturation, in brightness, out var red, out var green, out var blue);

		return FromArgb(alpha, red, green, blue);
	}

	public static void ToHsb(this Color color, out double hue, out double saturation, out double brightness) {
		color.ToRgb(out var red, out var green, out var blue);

		Hsb.FromRgb(
			red,
			green,
			blue,
			out hue,
			out saturation,
			out brightness
		);
	}

	public static uint ToUint(this Color color) {
		return Rgb.ToUint(color.A, color.R, color.G, color.B);
	}

	public static Color ToColor(this uint color) {
		Rgb.FromUint(color, out byte alpha, out byte red, out byte green, out byte blue);

		return Color.FromArgb(alpha, red, green, blue);
	}

	public static bool IsBright(this Color color) {
		return Rgb.IsBright(color.R, color.G, color.B);
	}

	public static Color ToBlackIfBright(this Color color) {
		return color.IsBright() ? Colors.Black : Colors.White;
	}

	public static Color NextHSBColor(this Random random, in double saturation, in double brightness, byte alpha = 0xFF) {
		Hsb.RandomHueToRgb(
			random,
			in saturation,
			in brightness,
			out var red,
			out var green,
			out var blue
		);

		return FromArgb(alpha, in red, in green, in blue);
	}

	public static Color InterpolateHueToColor(in double minHue, in double maxHue, in double hueFactor, in double saturation, in double brightness, byte alpha = 0xFF) {
		Hsb.InterpolateHueToRgb(
			in minHue,
			in maxHue,
			in hueFactor,
			in saturation,
			in brightness,
			out var red,
			out var green,
			out var blue
		);

		return FromArgb(alpha, in red, in green, in blue);
	}

	public static Color Multiply(this Color color, in double factor) {
		byte
			red = color.R,
			green = color.G,
			blue = color.B;

		Rgb.Multiply(ref red, ref green, ref blue, in factor);

		return Color.FromArgb(color.A, red, green, blue);
	}

	public static Color Desaturate(this Color color, in double factor) {
		byte
			red = color.R,
			green = color.G,
			blue = color.B;

		Rgb.Desaturate(ref red, ref green, ref blue, in factor);

		return Color.FromArgb(color.A, red, green, blue);
	}

	public static byte Average(this Color color) {
		byte
			red = color.R,
			green = color.G,
			blue = color.B;

		return Rgb.Average(ref red, ref green, ref blue);
	}

	public static Color ToNewColorWithDiferentAlpha(this Color color, in byte newAlpha) {
		return Color.FromArgb(newAlpha, color.R, color.G, color.B);
	}

	public static SolidColorBrush ToNewBrushWithDifferentAlpha(this SolidColorBrush source, in byte newAlpha) {
		var color = source.Color;
		color.A = newAlpha;

		return new SolidColorBrush(color);
	}

	public static SolidColorBrush ToSolidColorBrush(this uint color) {
		return new SolidColorBrush(color.ToColor());
	}
}
