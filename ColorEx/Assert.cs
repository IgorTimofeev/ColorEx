using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColorEx;

static internal class Assert {
	internal static void IsValueInRange0_1(in double value, string argumentName) {
		if (value is < 0 or > 1)
			throw new ArgumentException($"\"{argumentName}\" value was expected to be in the range [0.0; 1.0], but {value} was given");
	}

	internal static void IsHsbValuesInRange0_1(in double hue, in double saturation, in double brightness) {
		IsValueInRange0_1(in hue, "hue");
		IsValueInRange0_1(in saturation, "saturation");
		IsValueInRange0_1(in brightness, "brightness");
	}

	internal static void IsRgbValuesInRange0_1(in double red, in double green, in double blue) {
		IsValueInRange0_1(in red, "red");
		IsValueInRange0_1(in green, "green");
		IsValueInRange0_1(in blue, "blue");
	}

	internal static void IsValuePositive(in double value, string argumentName) {
		if (value is < 0)
			throw new ArgumentException($"\"{argumentName}\" value was expected to be positive, but {value} was given");
	}
}
