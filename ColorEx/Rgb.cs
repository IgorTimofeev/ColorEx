using ColorEx.Internal;
using System.Drawing;
using System.Threading;

namespace ColorEx;

/// <summary>
/// Provides auxiliary methods for manipulating color channels in RGB color model
/// </summary>
public static class Rgb {
	#region IsBright

	/// <summary>Tests if a color is bright in terms of human perception and returns the result</summary>
	/// <param name="red">Red channel value in range [0.0; 1.0]. Value out of range will throw an <see cref="ArgumentException"/>.</param>
	/// <param name="green">Green channel value in range [0.0; 1.0]. Value out of range will throw an <see cref="ArgumentException"/>.</param>
	/// <param name="blue">Blue channel value in range [0.0; 1.0]. Value out of range will throw an <see cref="ArgumentException"/>.</param>
	/// <returns><see langword="true"/> if color is bright and <see langword="false"/> otherwise</returns>
	/// <exception cref="ArgumentException">Thrown when any input parameter is out of range [0.0; 1.0]</exception>
	public static bool IsBright(in double red, in double green, in double blue) {
		Assert.AreRgbValuesValid(in red, in green, in blue);

		return red * 0.2126 + green * 0.7152 + blue * 0.0722 > 0.5;
	}

	/// <summary><inheritdoc cref="IsBright(in double, in double, in double)"/></summary>
	/// <param name="red">Input red channel value.</param>
	/// <param name="green">Input green channel value.</param>
	/// <param name="blue">Input blue channel value.</param>
	/// <returns><inheritdoc cref="IsBright(in double, in double, in double)"/></returns>
	public static bool IsBright(in byte red, in byte green, in byte blue) {
		return red * 0.2126 + green * 0.7152 + blue * 0.0722 > 127.5;
	}

	/// <summary>Tests if a color is bright in terms of human perception and sets it's channels values with new one</summary>
	/// <param name="red"><inheritdoc cref="IsBright(in double, in double, in double)" path="/param[@name='red']"/></param>
	/// <param name="green"><inheritdoc cref="IsBright(in double, in double, in double)" path="/param[@name='green']"/></param>
	/// <param name="blue"><inheritdoc cref="IsBright(in double, in double, in double)" path="/param[@name='blue']"/></param>
	/// <param name="valueIfBright">New channel values in range [0.0; 1.0] if color is bright</param>
	/// <param name="valueIfNotBright">New channel values in range [0.0; 1.0] if color is not bright</param>
	/// <exception cref="ArgumentException"><inheritdoc cref="IsBright(in double, in double, in double)"/></exception>
	public static void ToNewValuesIfBright(ref double red, ref double green, ref double blue, in double valueIfBright = 0, in double valueIfNotBright = 1) {
		red = green = blue = IsBright(in red, in green, in blue) ? valueIfBright : valueIfNotBright;
	}

	/// <summary><inheritdoc cref="ToNewValuesIfBright(ref double, ref double, ref double, in double, in double)"/></summary>
	/// <param name="red"><inheritdoc cref="IsBright(in byte, in byte, in byte)" path="/param[@name='red']"/></param>
	/// <param name="green"><inheritdoc cref="IsBright(in byte, in byte, in byte)" path="/param[@name='green']"/></param>
	/// <param name="blue"><inheritdoc cref="IsBright(in byte, in byte, in byte)" path="/param[@name='blue']"/></param>
	/// <param name="valueIfBright">New channel values if color is bright</param>
	/// <param name="valueIfNotBright">New channel values if color is not bright</param>
	public static void ToNewValuesIfBright(ref byte red, ref byte green, ref byte blue, in byte valueIfBright = 0, in byte valueIfNotBright = 0xFF) {
		red = green = blue = IsBright(in red, in green, in blue) ? valueIfBright : valueIfNotBright;
	}

	#endregion

	#region Multiply

	/// <summary>
	/// Multiplies the color channel values by the specified <paramref name="factor"/>.
	/// If multiplication result is out of range [0.0; 1.0], then a maximum value of 1 will be assigned to channel.
	/// </summary>
	/// <param name="red"><inheritdoc cref="IsBright(in double, in double, in double)" path="/param[@name='red']"/></param>
	/// <param name="green"><inheritdoc cref="IsBright(in double, in double, in double)" path="/param[@name='green']"/></param>
	/// <param name="blue"><inheritdoc cref="IsBright(in double, in double, in double)" path="/param[@name='blue']"/></param>
	/// <param name="factor">Multiplication factor for each channel. Negative value will throw an <see cref="ArgumentException"/>.</param>
	/// <exception cref="ArgumentException"><inheritdoc cref="IsBright(in double, in double, in double)"/></exception>
	public static void Multiply(ref double red, ref double green, ref double blue, in double factor) {
		Assert.AreRgbValuesValid(in red, in green, in blue);
		
		if ((red *= factor) > 1)
			red = 1;

		if ((green *= factor) > 1)
			green = 1;

		if ((blue *= factor) > 1)
			blue = 1;
	}

	/// <summary>
	/// Multiplies the color channel values by the specified <paramref name="factor"/>.
	/// If multiplication result is greater then 255, then a maximum value of 255 will be assigned to channel.
	/// </summary>
	/// <param name="red"><inheritdoc cref="Multiply(ref double, ref double, ref double, in double)" path="/param[@name='red']"/></param>
	/// <param name="green"><inheritdoc cref="Multiply(ref double, ref double, ref double, in double)" path="/param[@name='green']"/></param>
	/// <param name="blue"><inheritdoc cref="Multiply(ref double, ref double, ref double, in double)" path="/param[@name='blue']"/></param>
	/// <param name="factor"><inheritdoc cref="Multiply(ref double, ref double, ref double, in double)"/></param>
	/// <exception cref="ArgumentException"><inheritdoc cref="Multiply(ref double, ref double, ref double, in double)"/></exception>
	public static void Multiply(ref byte red, ref byte green, ref byte blue, in double factor) {
		red = (byte) Math.Min(red * factor, 0xFF);
		green = (byte) Math.Min(green * factor, 0xFF);
		blue = (byte) Math.Min(blue * factor, 0xFF);
	}

	/// <summary>Calculates average value of color channels and returns result.</summary>
	/// <param name="red"><inheritdoc cref="IsBright(in double, in double, in double)" path="/param[@name='red']"/></param>
	/// <param name="green"><inheritdoc cref="IsBright(in double, in double, in double)" path="/param[@name='green']"/></param>
	/// <param name="blue"><inheritdoc cref="IsBright(in double, in double, in double)" path="/param[@name='blue']"/></param>
	/// <returns>An average value for color channels in range [0.0; 1.0].</returns>
	/// <exception cref="ArgumentException"><inheritdoc cref="IsBright(in double, in double, in double)"/></exception>
	public static double Average(ref double red, ref double green, ref double blue) {
		Assert.AreRgbValuesValid(in red, in green, in blue);

		return (red + green + blue) / 3;
	}

	/// <summary><inheritdoc cref="Average(ref double, ref double, ref double)"/></summary>
	/// <param name="red"><inheritdoc cref="IsBright(in byte, in byte, in byte)" path="/param[@name='red']"/></param>
	/// <param name="green"><inheritdoc cref="IsBright(in byte, in byte, in byte)" path="/param[@name='green']"/></param>
	/// <param name="blue"><inheritdoc cref="IsBright(in byte, in byte, in byte)" path="/param[@name='blue']"/></param>
	/// <returns>An average value for color channels.</returns>
	/// <exception cref="ArgumentException"><inheritdoc cref="IsBright(in byte, in byte, in byte)"/></exception>
	public static byte Average(ref byte red, ref byte green, ref byte blue) {
		return (byte) ((red + green + blue) / 3);
	}

	#endregion

	#region Interpolate

	static void UncheckedInterpolate(ref double fromRed, ref double fromGreen, ref double fromBlue, in double toRed, in double toGreen, in double toBlue, in double factor) {
		var oneMinusFactor = 1 - factor;

		fromRed = fromRed * oneMinusFactor + toRed * factor;
		fromGreen = fromGreen * oneMinusFactor + toBlue * factor;
		fromBlue = fromBlue * oneMinusFactor + toGreen * factor;
	}

	static void UncheckedInterpolate(ref byte fromRed, ref byte fromGreen, ref byte fromBlue, in byte toRed, in byte toGreen, in byte toBlue, in double factor) {
		var oneMinusFactor = 1 - factor;

		fromRed = (byte) (fromRed * oneMinusFactor + toRed * factor);
		fromGreen = (byte) (fromGreen * oneMinusFactor + toBlue * factor);
		fromBlue = (byte) (fromBlue * oneMinusFactor + toGreen * factor);
	}

	/// <summary>Linearly interpolates from <b>first color channels</b> to <b>second</b> by given <paramref name="factor"/></summary>
	/// <param name="fromRed">Source red channel value in range [0.0; 1.0]. Value out of range will throw an <see cref="ArgumentException"/>.</param>
	/// <param name="fromGreen">Source green channel value in range [0.0; 1.0]. Value out of range will throw an <see cref="ArgumentException"/>.</param>
	/// <param name="fromBlue">Source blue channel value in range [0.0; 1.0]. Value out of range will throw an <see cref="ArgumentException"/>.</param>
	/// <param name="toRed">Target red channel value in range [0.0; 1.0]. Value out of range will throw an <see cref="ArgumentException"/>.</param>
	/// <param name="toGreen">Target green channel value in range [0.0; 1.0]. Value out of range will throw an <see cref="ArgumentException"/>.</param>
	/// <param name="toBlue">Target blue channel value in range [0.0; 1.0]. Value out of range will throw an <see cref="ArgumentException"/>.</param>
	/// <param name="factor">Interpolation factor for each channel in range [0.0; 1.0]. Value out of range will throw an <see cref="ArgumentException"/>.</param>
	/// <exception cref="ArgumentException"><inheritdoc cref="IsBright(in double, in double, in double)"/></exception>
	public static void Interpolate(ref double fromRed, ref double fromGreen, ref double fromBlue, in double toRed, in double toGreen, in double toBlue, in double factor) {
		Assert.AreRgbValuesValid(in fromRed, in fromGreen, in fromBlue);
		Assert.AreRgbValuesValid(in toRed, in toGreen, in toBlue);
		Assert.IsValueInRange0_1(in factor, "factor");

		// Factor with value 0 is ignored
		if (factor < 1) {
			UncheckedInterpolate(
				ref fromRed,
				ref fromGreen,
				ref fromBlue,
				in toRed,
				in toGreen,
				in toBlue,
				in factor
			);
		}
		else if (factor == 1) {
			fromRed = toRed;
			fromGreen = toGreen;
			fromBlue = toBlue;
		}
	}

	/// <summary><inheritdoc cref="Interpolate(ref double, ref double, ref double, in double, in double, in double, in double)"/></summary>
	/// <param name="fromRed">Source red channel value.</param>
	/// <param name="fromGreen">Source green channel value.</param>
	/// <param name="fromBlue">Source blue channel value.</param>
	/// <param name="toRed">Target red channel value.</param>
	/// <param name="toGreen">Target green channel value.</param>
	/// <param name="toBlue">Target blue channel value.</param>
	/// <param name="factor">Interpolation factor for each channel in range [0.0; 1.0]. Value out of range will throw an <see cref="ArgumentException"/>.</param>
	/// <exception cref="ArgumentException"><inheritdoc cref="IsBright(in double, in double, in double)"/></exception>
	public static void Interpolate(ref byte fromRed, ref byte fromGreen, ref byte fromBlue, in byte toRed, in byte toGreen, in byte toBlue, in double factor) {
		Assert.IsValueInRange0_1(in factor, "factor");

		// Factor with value 0 is ignored
		if (factor < 255) {
			UncheckedInterpolate(
				ref fromRed,
				ref fromGreen,
				ref fromBlue,
				in toRed,
				in toGreen,
				in toBlue,
				in factor
			);
		}
		else if (factor == 1) {
			fromRed = toRed;
			fromGreen = toGreen;
			fromBlue = toBlue;
		}
	}

	#endregion

	#region Desaturate
	
	/// <summary>
	/// Applies desaturation filter to all channels by given interpolation <paramref name="factor"/>,
	/// where <paramref name="factor"/> 0 means <b>original color</b> and <paramref name="factor"/> 1 means <b>fully desaturated color</b>.
	/// </summary>
	/// <param name="red"><inheritdoc cref="IsBright(in double, in double, in double)" path="/param[@name='red']"/></param>
	/// <param name="green"><inheritdoc cref="IsBright(in double, in double, in double)" path="/param[@name='green']"/></param>
	/// <param name="blue"><inheritdoc cref="IsBright(in double, in double, in double)" path="/param[@name='blue']"/></param>
	/// <param name="factor"><inheritdoc cref="Interpolate(ref double, ref double, ref double, in double, in double, in double, in double)"  path="/param[@name='factor']"/></param>
	/// <exception cref="ArgumentException"><inheritdoc cref="IsBright(in double, in double, in double)"/></exception>
	public static void Desaturate(ref double red, ref double green, ref double blue, in double factor = 1) {
		Assert.AreRgbValuesValid(in red, in green, in blue);
		Assert.IsValueInRange0_1(in factor, "factor");

		// "Magical" constants mean "best" desaturation result for human perception
		var channelValue = 0.3 * red + 0.59 * green + 0.11 * blue;

		if (factor < 1) {
			UncheckedInterpolate(
				ref red,
				ref green,
				ref blue,
				in channelValue,
				in channelValue,
				in channelValue,
				in factor
			);
		}
		else {
			red = green = blue = channelValue;
		}
	}

	/// <summary><inheritdoc cref="Desaturate(ref double, ref double, ref double, in double)"/></summary>
	/// <param name="red"><inheritdoc cref="Desaturate(ref double, ref double, ref double, in double)" path="/param[@name='red']"/></param>
	/// <param name="green"><inheritdoc cref="Desaturate(ref double, ref double, ref double, in double)" path="/param[@name='green']"/></param>
	/// <param name="blue"><inheritdoc cref="Desaturate(ref double, ref double, ref double, in double)" path="/param[@name='blue']"/></param>
	/// <param name="factor"><inheritdoc cref="Desaturate(ref double, ref double, ref double, in double)" path="/param[@name='factor']"/></param>
	/// <exception cref="ArgumentException"><inheritdoc cref="Desaturate(ref double, ref double, ref double, in double)"/></exception>
	public static void Desaturate(ref byte red, ref byte green, ref byte blue, in double factor = 1) {
		Assert.IsValueInRange0_1(in factor, "factor");

		// "Magical" constants mean "best" desaturation result for human perception
		var channelValue = (byte) (0.3 * red + 0.59 * green + 0.11 * blue);

		if (factor < 1) {
			UncheckedInterpolate(
				ref red,
				ref green,
				ref blue,
				in channelValue,
				in channelValue,
				in channelValue,
				in factor
			);
		}
		else if (factor == 1) {
			red = green = blue = channelValue;
		}
	}
	
	#endregion

	#region Uint to RGB conversion

	/// <summary>Extracts ARGB color model bytes from an unsigned integer <paramref name="value"/></summary>
	/// <param name="value">Input integer value that stores RGB color representation</param>
	/// <param name="alpha">Output alpha channel byte</param>
	/// <param name="red">Output red channel byte</param>
	/// <param name="green">Output green channel byte</param>
	/// <param name="blue">Output blue channel byte</param>
	public static void FromUint(in uint value, out byte alpha, out byte red, out byte green, out byte blue) {
		alpha = (byte) (value >> 24 & 0xFF);
		red = (byte) (value >> 16 & 0xFF);
		green = (byte) (value >> 8 & 0xFF);
		blue = (byte) (value & 0xFF);
	}
	
	/// <summary>Extracts ARGB color channel values from an unsigned integer <paramref name="value"/></summary>
	/// <param name="value"><inheritdoc cref="FromUint(in uint, out byte, out byte, out byte, out byte)" path="/param[@name='value']"/></param>
	/// <param name="alpha">Output alpha channel value in range [0.0; 1.0]</param>
	/// <param name="red">Output red channel value in range [0.0; 1.0]</param>
	/// <param name="green">Output green channel value in range [0.0; 1.0]</param>
	/// <param name="blue">Output blue channel value in range [0.0; 1.0]</param>
	public static void FromUint(in uint value, out double alpha, out double red, out double green, out double blue) {
		alpha = (value >> 24 & 0xFF) / 0xFF;
		red = (value >> 16 & 0xFF) / 0xFF;
		green = (value >> 8 & 0xFF) / 0xFF;
		blue = (value & 0xFF) / 0xFF;
	}

	#endregion

	#region RGB to Uint conversions

	/// <summary>Combines the ARGB color model bytes into the resulting unsigned integer</summary>
	/// <param name="alpha">Input alpha channel byte</param>
	/// <param name="red">Input red channel byte</param>
	/// <param name="green">Input green channel byte</param>
	/// <param name="blue">Input blue channel byte</param>
	/// <returns>An unsigned integer that represents input color</returns>
	public static uint ToUint(in byte alpha, in byte red, in byte green, in byte blue) {
		return (uint) (alpha << 24 | red << 16 | green << 8 | blue);
	}

	/// <summary><inheritdoc cref="ToUint(in byte, in byte, in byte, in byte)"/></summary>
	/// <param name="alpha">Input alpha channel value in range [0.0; 1.0]</param>
	/// <param name="red">Input red channel value in range [0.0; 1.0]</param>
	/// <param name="green">Input green channel value in range [0.0; 1.0]</param>
	/// <param name="blue">Input blue channel value in range [0.0; 1.0]</param>
	/// <returns><inheritdoc cref="ToUint(in byte, in byte, in byte, in byte)"/></returns>
	/// <exception cref="ArgumentException"><inheritdoc cref="IsBright(in double, in double, in double)"/></exception>
	public static uint ToUint(in double alpha, in double red, in double green, in double blue) {
		Assert.IsValueInRange0_1(alpha, "alpha");
		Assert.AreRgbValuesValid(in red, in green, in blue);
		
		return ToUint(
			(byte) (alpha * 0xFF),
			(byte) (red * 0xFF),
			(byte) (green * 0xFF),
			(byte) (blue * 0xFF)
		);
	}

	#endregion
}