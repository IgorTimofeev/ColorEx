using System.Windows.Media;

namespace ColorEx.Wpf;

public static class ColorExtensions {
	internal static Color FromArgb(in double alpha, in double red, in double green, in double blue) {
		return Color.FromArgb(
			(byte) (alpha * 255),
			(byte) (red * 255),
			(byte) (green * 255),
			(byte) (blue * 255)
		);
	}

	public static uint ToUint(this Color color) {
		return Rgb.ToUint(color.A, color.R, color.G, color.B);
	}

	public static Color FromHsb(in double hue, in double saturation, in double brightness, byte alpha = 0xFF) {
		Hsb.ToRgb(in hue, in saturation, in brightness, out var red, out var green, out var blue);

		return FromArgb(alpha, red, green, blue);
	}

	public static void ToHsb(this Color color, out double hue, out double saturation, out double brightness) {
		Hsb.FromRgb(
			color.R / 255d,
			color.G / 255d,
			color.B / 255,
			out hue,
			out saturation,
			out brightness
		);
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

	public static Color ParametricHueToColor(in double hue, in double minHue, in double maxHue, in double saturation, in double brightness, byte alpha = 0xFF) {
		Hsb.ParametricHueToRgb(
			in minHue,
			in maxHue,
			in hue,
			in saturation,
			in brightness,
			out var red,
			out var green,
			out var blue
		);

		return FromArgb(alpha, in red, in green, in blue);
	}

	public static Color Multiply(this Color source, in double multiplier) {
		return Color.FromArgb(
			source.A,
			(byte) Math.Min(source.R * multiplier, 0xFF),
			(byte) Math.Min(source.G * multiplier, 0xFF),
			(byte) Math.Min(source.B * multiplier, 0xFF)
		);
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
