using System.Drawing;

namespace ColorEx;

/// <summary>
/// Provides methods for conversion between RGB and HSB color models
/// </summary>
public static class Hsb {
	/// <summary>Converts HSB color model values to RGB values</summary>
	/// <param name="hue">Input <paramref name="hue"/> value in range [0.0; 1.0]</param>
	/// <param name="saturation">Input <paramref name="saturation"/> value in range [0.0; 1.0]</param>
	/// <param name="brightness">Input <paramref name="brightness"/> value in range [0.0; 1.0]</param>
	/// <param name="red">Output <paramref name="red"/> channel value in range [0.0; 1.0]</param>
	/// <param name="green">Output <paramref name="green"/> channel value in range [0.0; 1.0]</param>
	/// <param name="blue">Output <paramref name="blue"/> channel value in range [0.0; 1.0]</param>
	/// <exception cref="ArgumentException"><inheritdoc cref="Rgb.IsBright(in double, in double, in double)"/></exception>
	public static void ToRgb(in double hue, in double saturation, in double brightness, out double red, out double green, out double blue) {
		Assert.IsValueInRange0_1(hue, "Hue");
		Assert.IsValueInRange0_1(saturation, "Saturation");
		Assert.IsValueInRange0_1(brightness, "Brightness");

		double hueSector = hue * 6;
		int hueSectorIntegerPart = (int) hueSector;
		double hueSectorFractionalPart = hueSector - hueSectorIntegerPart;

		double
			p = brightness * (1 - saturation),
			q = brightness * (1 - hueSectorFractionalPart * saturation),
			t = brightness * (1 - (1 - hueSectorFractionalPart) * saturation);

		switch (hueSectorIntegerPart) {
			case 1:
				red = q;
				green = brightness;
				blue = p;
				break;

			case 2:
				red = p;
				green = brightness;
				blue = t;
				break;

			case 3:
				red = p;
				green = q;
				blue = brightness;
				break;

			case 4:
				red = t;
				green = p;
				blue = brightness;
				break;

			case 5:
				red = brightness;
				green = p;
				blue = q;
				break;

			default:
				red = brightness;
				green = t;
				blue = p;
				break;
		}
	}

	/// <summary>Converts RGB color model values to HSB values</summary>
	/// <param name="red">Input <paramref name="red"/> channel value in range [0.0; 1.0]</param>
	/// <param name="green">Input <paramref name="green"/> channel value in range [0.0; 1.0]</param>
	/// <param name="blue">Input <paramref name="blue"/> channel value in range [0.0; 1.0]</param>
	/// <param name="hue">Output <paramref name="hue"/> value in range [0.0; 1.0]</param>
	/// <param name="saturation">Output <paramref name="saturation"/> value in range [0.0; 1.0]</param>
	/// <param name="brightness">Output <paramref name="brightness"/> value in range [0.0; 1.0]</param>
	/// <exception cref="ArgumentException"><inheritdoc cref="Rgb.IsBright(in double, in double, in double)"/></exception>
	public static void FromRgb(in double red, in double green, in double blue, out double hue, out double saturation, out double brightness) {
		Assert.IsValueInRange0_1(red, "Red");
		Assert.IsValueInRange0_1(green, "Green");
		Assert.IsValueInRange0_1(blue, "Blue");

		hue = 0;

		// Max of r/g/b
		brightness =
			red > green
			? red > blue ? red : blue
			: green > blue ? green : blue;

		// Max - min of r/g/b
		var delta =
			brightness - (
				red < green
				? red < blue ? red : blue
				: green < blue ? green : blue
			);

		saturation = brightness == 0 ? 0 : delta / brightness;

		if (saturation != 0) {
			// Determining hue sector
			if (red == brightness) {
				hue = (green - blue) / delta;
			}
			else if (green == brightness) {
				hue = 2 + (blue - red) / delta;
			}
			else if (blue == brightness) {
				hue = 4 + (red - green) / delta;
			}

			// Sector to hue
			hue *= 1d / 6d;

			// For cases like R = MAX & B > G
			if (hue < 0)
				hue += 1d;
		}
	}

	/// <summary>Generates a color in HSB model with random <see langword="hue"/> and converts it to RGB values in range [0.0; 1.0]</summary>
	/// <param name="random">Input instance of <see cref="Random"/> class that should be used for color generation</param>
	/// <param name="saturation">Input <paramref name="saturation"/> value in range [0.0; 1.0]</param>
	/// <param name="brightness">Input <paramref name="brightness"/> value in range [0.0; 1.0]</param>
	/// <param name="red">Output <paramref name="red"/> channel value in range [0.0; 1.0]</param>
	/// <param name="green">Output <paramref name="green"/> channel value in range [0.0; 1.0]</param>
	/// <param name="blue">Output <paramref name="blue"/> channel value in range [0.0; 1.0]</param>
	/// <exception cref="ArgumentException"><inheritdoc cref="Rgb.IsBright(in double, in double, in double)"/></exception>
	public static void RandomHueToRgb(Random random, in double saturation, in double brightness, out double red, out double green, out double blue) {
		ToRgb(random.NextDouble(), in saturation, in brightness, out red, out green, out blue);
	}

	public static void ParametricHueToRgb(in double minHue, in double maxHue, in double hue, in double saturation, in double brightness, out double red, out double green, out double blue) {
		Assert.IsValueInRange0_1(hue, "Value");

		ToRgb(minHue + hue * (maxHue - minHue), in saturation, in brightness, out red, out green, out blue);
	}
}
