namespace ColorEx;

/// <summary>
/// Provides auxiliary methods for manipulating color channels in RGB color model
/// </summary>
public static class Rgb {
	#region IsBright

	/// <summary>Tests if a color is bright in terms of human perception and returns the result</summary>
	/// <param name="red">Input <paramref name="red"/> channel value in range [0.0; 1.0]</param>
	/// <param name="green">Input <paramref name="green"/> channel value in range [0.0; 1.0]</param>
	/// <param name="blue">Input <paramref name="blue"/> channel value in range [0.0; 1.0]</param>
	/// <returns><see langword="true"/> if color is bright and <see langword="false"/> otherwise</returns>
	/// <exception cref="ArgumentException">Thrown when any input parameter is out of range [0.0; 1.0]</exception>
	public static bool IsBright(in double red, in double green, in double blue) {
		Assert.IsValueInRange0_1(red, "Red");
		Assert.IsValueInRange0_1(green, "Green");
		Assert.IsValueInRange0_1(blue, "Blue");

		return red * 0.2126 + green * 0.7152 + blue * 0.0722 > 0.5;
	}

	/// <summary><inheritdoc cref="IsBright(in double, in double, in double)"/></summary>
	/// <param name="red">Input <paramref name="red"/> channel value in range [0; 255]</param>
	/// <param name="green">Input <paramref name="green"/> channel value in range [0; 255]</param>
	/// <param name="blue">Input <paramref name="blue"/> channel value in range [0; 255]</param>
	/// <returns><inheritdoc cref="IsBright(in double, in double, in double)"/></returns>
	public static bool IsBright(in byte red, in byte green, in byte blue) {
		return red * 0.2126 + green * 0.7152 + blue * 0.0722 > 127.5;
	}

	/// <summary>Tests if a source color is bright in terms of human perception and fills output channels with black or white values</summary>
	/// <param name="sourceRed">Input red channel value in range [0.0; 1.0]</param>
	/// <param name="sourceGreen">Input green channel value in range [0.0; 1.0]</param>
	/// <param name="sourceBlue">Input blue channel value in range [0.0; 1.0]</param>
	/// <param name="outputRed">Output red channel value in range [0.0; 1.0]</param>
	/// <param name="outputGreen">Output green channel value in range [0.0; 1.0]</param>
	/// <param name="outputBlue">Output blue channel value in range [0.0; 1.0]</param>
	/// <exception cref="ArgumentException"><inheritdoc cref="IsBright(in double, in double, in double)"/></exception>
	public static void ToBlackIfBright(in double sourceRed, in double sourceGreen, in double sourceBlue, out double outputRed, out double outputGreen, out double outputBlue) {
		if (IsBright(in sourceRed, in sourceGreen, in sourceBlue)) {
			outputRed = outputGreen = outputBlue = 0x00;
		}
		else {
			outputRed = outputGreen = outputBlue = 0xFF;
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
		Assert.IsValueInRange0_1(alpha, "Alpha");
		Assert.IsValueInRange0_1(alpha, "Red");
		Assert.IsValueInRange0_1(alpha, "Green");
		Assert.IsValueInRange0_1(alpha, "Blue");

		return ToUint(
			(byte) (alpha * 0xFF),
			(byte) (red * 0xFF),
			(byte) (green * 0xFF),
			(byte) (blue * 0xFF)
		);
	}

	#endregion
}
