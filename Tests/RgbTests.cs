using ColorEx;

namespace Tests;

[TestClass]
public class RgbTests {
	[TestMethod]
	public void IsBright() {
		Assert.IsTrue(Rgb.IsBright(0xFF, 0xAA, 0xEE));
		Assert.IsFalse(Rgb.IsBright(0x00, 0x22, 0x33));

		Assert.IsTrue(Rgb.IsBright(1, 0.8, 0.7));
		Assert.IsFalse(Rgb.IsBright(0.1, 0.2, 0.5));
	}
}