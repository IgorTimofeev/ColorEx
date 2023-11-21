using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColorEx.Internal;

public static class Assert {
	public static void IsValueInRange0_1(in double value, string argumentName) {
		if (value is < 0 or > 1)
			throw new ArgumentException($"\"{argumentName}\" value was expected to be in the range [0.0; 1.0], but {value} was given");
	}

	public static void IsValuePositive(in double value, string argumentName) {
		if (value is < 0)
			throw new ArgumentException($"\"{argumentName}\" value was expected to be positive, but {value} was given");
	}

	public static void AreHsbValuesValid(in double hue, in double saturation, in double brightness) {
		IsValueInRange0_1(in hue, "hue");
		IsValueInRange0_1(in saturation, "saturation");
		IsValueInRange0_1(in brightness, "brightness");

		IsValuePositive(in hue, "hue");
		IsValuePositive(in saturation, "saturation");
		IsValuePositive(in brightness, "brightness");
	}

	public static void AreRgbValuesValid(in double red, in double green, in double blue) {
		IsValueInRange0_1(in red, "red");
		IsValueInRange0_1(in green, "green");
		IsValueInRange0_1(in blue, "blue");

		IsValuePositive(in red, "red");
		IsValuePositive(in green, "green");
		IsValuePositive(in blue, "blue");
	}
}
