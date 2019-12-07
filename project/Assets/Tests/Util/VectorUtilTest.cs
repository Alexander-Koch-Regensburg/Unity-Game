using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using UnityEngine;

[TestClass]
public class VectorUtilTest {

	[Test]
	[TestCase(-5, -4)]
	[TestCase(4, -4)]
	[TestCase(10, -4)]
	public void SwapXY_ValuesSwapped(int x, int y) {
		Vector2Int vector = new Vector2Int(x, y);
		Vector2Int result = VectorUtil.SwapXY(vector);
		NUnit.Framework.Assert.AreEqual(vector.x, result.y);
		NUnit.Framework.Assert.AreEqual(vector.y, result.x);
	}

	[Test]
	[TestCase(0f, 1f, 0f)]
	[TestCase(90f, 0f, 1f)]
	[TestCase(-270f, 0f, 1f)]
	[TestCase(180f, -1f, 0f)]
	[TestCase(-180f, -1f, 0f)]
	[TestCase(270f, 0f, -1f)]
	[TestCase(-90f, 0f, -1f)]
	[TestCase(45f, 0.70710678f, 0.70710678f)]
	[TestCase(-315f, 0.70710678f, 0.70710678f)]
	public void Deg2Vector2_ResultCorrect(float deg, float resX, float resY) {
		Vector2 result = VectorUtil.Deg2Vector2(deg);

		NUnit.Framework.Assert.That(result.x, Is.EqualTo(resX).Within(0.00001));
		NUnit.Framework.Assert.That(result.y, Is.EqualTo(resY).Within(0.00001));
	}

	[Test]
	[TestCase(0f, 1f, 0f)]
	[TestCase(Mathf.PI * 0.5f, 0f, 1f)]
	[TestCase(-Mathf.PI * 1.5f, 0f, 1f)]
	[TestCase(Mathf.PI, -1f, 0f)]
	[TestCase(-Mathf.PI, -1f, 0f)]
	[TestCase(Mathf.PI * 1.5f, 0f, -1f)]
	[TestCase(-Mathf.PI * 0.5f, 0f, -1f)]
	[TestCase(Mathf.PI * 0.25f, 0.70710678f, 0.70710678f)]
	[TestCase(-Mathf.PI * 1.75f, 0.70710678f, 0.70710678f)]
	public void Rad2Vector2_ResultCorrect(float rad, float resX, float resY) {
		Vector2 result = VectorUtil.Rad2Vector2(rad);

		NUnit.Framework.Assert.That(result.x, Is.EqualTo(resX).Within(0.00001));
		NUnit.Framework.Assert.That(result.y, Is.EqualTo(resY).Within(0.00001));
	}

	[Test]
	[TestCase(0f, 0f, 42f, 0f, 0f)]
	[TestCase(1f, 1f, 0f, 1f, 1f)]
	[TestCase(1f, 2f, 90f, -2f, 1f)]
	[TestCase(1f, 2f, -90f, 2f, -1f)]
	[TestCase(-1f, 2f, 90f, -2f, -1f)]
	[TestCase(-1f, 2f, -90f, 2f, 1f)]
	[TestCase(-3f, -4f, 540f, 3f, 4f)]
	public void RotateVector2ByDeg_ResultCorrect(float x, float y, float deg, float resX, float resY) {
		Vector2 vector = new Vector2(x, y);

		Vector2 result = VectorUtil.RotateVector2ByDeg(vector, deg);

		NUnit.Framework.Assert.That(result.x, Is.EqualTo(resX).Within(0.00001));
		NUnit.Framework.Assert.That(result.y, Is.EqualTo(resY).Within(0.00001));
	}

	[Test]
	[TestCase(0f, 0f, 42f, 0f, 0f)]
	[TestCase(1f, 1f, 0f, 1f, 1f)]
	[TestCase(1f, 2f, Mathf.PI * 0.5f, -2f, 1f)]
	[TestCase(1f, 2f, -Mathf.PI * 0.5f, 2f, -1f)]
	[TestCase(-1f, 2f, Mathf.PI * 0.5f, -2f, -1f)]
	[TestCase(-1f, 2f, -Mathf.PI * 0.5f, 2f, 1f)]
	[TestCase(-3f, -4f, Mathf.PI * 3f, 3f, 4f)]
	public void RotateVector2ByRad_ResultCorrect(float x, float y, float rad, float resX, float resY) {
		Vector2 vector = new Vector2(x, y);

		Vector2 result = VectorUtil.RotateVector2ByRad(vector, rad);

		NUnit.Framework.Assert.That(result.x, Is.EqualTo(resX).Within(0.00001));
		NUnit.Framework.Assert.That(result.y, Is.EqualTo(resY).Within(0.00001));
	}

	[Test]
	[TestCase(-1f, -1f, 1f, 2f, 0f, 0f)]
	[TestCase(0f, -1f, -1f, 1f, 4f, 0.5f)]
	public void CompareDistanceToTarget2D_DistanceSmaller(float x1, float y1, float x2, float y2, float tarX, float tarY) {
		int result = VectorUtil.CompareDistanceToTarget2D(new Vector2(x1, y1), new Vector2(x2, y2), new Vector2(tarX, tarY));
		NUnit.Framework.Assert.IsTrue(result < 0);
	}

	[Test]
	[TestCase(-1f, -1f, 1f, 1f, 1f, -1f)]
	public void CompareDistanceToTarget2D_DistanceEqual(float x1, float y1, float x2, float y2, float tarX, float tarY) {
		int result = VectorUtil.CompareDistanceToTarget2D(new Vector2(x1, y1), new Vector2(x2, y2), new Vector2(tarX, tarY));
		NUnit.Framework.Assert.IsTrue(result == 0);
	}

	[Test]
	[TestCase(1f, 2f, -1f, -1f, 0f, 0f)]
	[TestCase(-1f, 1f, 0f, -1f, 4f, 0.5f)]
	public void CompareDistanceToTarget2D_DistanceBigger(float x1, float y1, float x2, float y2, float tarX, float tarY) {
		int result = VectorUtil.CompareDistanceToTarget2D(new Vector2(x1, y1), new Vector2(x2, y2), new Vector2(tarX, tarY));
		NUnit.Framework.Assert.IsTrue(result > 0);
	}
}
