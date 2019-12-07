using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorUtil {

	/// <summary>
	/// Returns the Vector with swapped x and y values
	/// </summary>
	/// <param name="vector"></param>
	/// <returns></returns>
	public static Vector2Int SwapXY(Vector2Int vector) {
		int x = vector.x;
		vector.x = vector.y;
		vector.y = x;
		return vector;
	}

	/// <summary>
	/// Creates a Vector2 with length 1 with the specified angle
	/// </summary>
	/// <param name="deg"></param>
	/// <returns></returns>
	public static Vector2 Deg2Vector2(float deg) {
		return Rad2Vector2(Mathf.Deg2Rad * deg);
	}

	/// <summary>
	/// Creates a Vector2 with length 1 with the specified angle
	/// </summary>
	/// <param name="rad"></param>
	/// <returns></returns>
	public static Vector2 Rad2Vector2(float rad) {
		return new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
	}

	/// <summary>
	/// Returns the given Vector2 rotated by the specified amount
	/// </summary>
	/// <param name="v"></param>
	/// <param name="deg"></param>
	/// <returns></returns>
	public static Vector2 RotateVector2ByDeg(Vector2 v, float deg) {
		float rad = deg * Mathf.Deg2Rad;
		return RotateVector2ByRad(v, rad);
	}

	/// <summary>
	/// Returns the given Vector2 rotated by the specified amount
	/// </summary>
	/// <param name="v"></param>
	/// <param name="rad"></param>
	/// <returns></returns>
	public static Vector2 RotateVector2ByRad(Vector2 v, float rad) {
		float sin = Mathf.Sin(rad);
		float cos = Mathf.Cos(rad);

		return new Vector2(cos * v.x - sin * v.y, sin * v.x + cos * v.y);
	}

	/// <summary>
	/// Compares the distance of two input vectors to a target vector
	/// </summary>
	/// <param name="v1"></param>
	/// <param name="v2"></param>
	/// <param name="target"></param>
	/// <returns></returns>
	public static int CompareDistanceToTarget2D(Vector2 v1, Vector2 v2, Vector2 target) {
		float d1 = (v1 - target).magnitude;
		float d2 = (v2 - target).magnitude;
		return d1.CompareTo(d2);
	}
}

