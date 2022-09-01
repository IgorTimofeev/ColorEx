using ColorEx;

namespace Tests;

[TestClass]
public class RGB {
	[TestMethod]
	public void IsBright() {
		Assert.IsTrue(Rgb.IsBright(0xFF, 0xAA, 0xEE));
		Assert.IsFalse(Rgb.IsBright(0x00, 0x22, 0x33));
	}
}