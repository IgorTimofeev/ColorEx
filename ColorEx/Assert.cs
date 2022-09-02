using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColorEx;

static internal class Assert {
	internal static void IsValueInRange0_1(in double value, string argumentName) {
		if (value is < 0 or > 1)
			throw new ArgumentException($"{argumentName} value was expected to be in the range [0.0; 1.0], but {value} was given");
	}
}
