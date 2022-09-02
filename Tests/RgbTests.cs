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

		Assert.ThrowsException<ArgumentException>(() => Rgb.IsBright(-5, 0.5, 0.5));
		Assert.ThrowsException<ArgumentException>(() => Rgb.IsBright(5, 0.5, 0.5));
	}

	[TestMethod]
	public void ToUint() {
		Assert.AreEqual(Rgb.ToUint(0xAA, 0xBB, 0xCC, 0xDD), (uint) 0xAA_BB_CC_DD);
		Assert.AreEqual(Rgb.ToUint(0x00, 0x01, 0x00, 0x01), (uint) 0x00_01_00_01);
	}

	[TestMethod]
	public void Multiply() {
		double
			r = 1,
			g = 0.5,
			b = 0.8;

		Rgb.Multiply(ref r, ref g, ref b, 0.5);
		
		Assert.AreEqual(r, 0.5);
		Assert.AreEqual(g, 0.25);
		Assert.AreEqual(b, 0.4);

		r = 0.8;
		g = 0.2;
		b = 0.1;

		Rgb.Multiply(ref r, ref g, ref b, 2);
		
		Assert.AreEqual(r, 1);
		Assert.AreEqual(g, 0.4);
		Assert.AreEqual(b, 0.2);

		r = -5;
		Assert.ThrowsException<ArgumentException>(() => Rgb.Multiply(ref r, ref g, ref b, 2));

		r = 5;
		Assert.ThrowsException<ArgumentException>(() => Rgb.Multiply(ref r, ref g, ref b, 2));
	}

	[TestMethod]
	public void FromUint() {
		byte a, r, g, b;

		Rgb.FromUint(0xAA_BB_CC_DD, out a, out r, out g, out b);
		Assert.AreEqual(a, 0xAA);
		Assert.AreEqual(r, 0xBB);
		Assert.AreEqual(g, 0xCC);
		Assert.AreEqual(b, 0xDD);

		Rgb.FromUint(0x00_01_00_01, out a, out r, out g, out b);
		Assert.AreEqual(a, 0x00);
		Assert.AreEqual(r, 0x01);
		Assert.AreEqual(g, 0x00);
		Assert.AreEqual(b, 0x01);
	}
}