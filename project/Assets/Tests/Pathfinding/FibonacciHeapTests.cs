using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

[TestClass]
public class FibonacciHeapTest {

	// Empty Heap Tests
	[Test]
	public void EmptyHeapGetMin_DefaultValue() {
		FibonacciHeap<int> heap = new FibonacciHeap<int>();

		NUnit.Framework.Assert.AreEqual(default(int), heap.GetMin());
	}

	[Test]
	public void EmptyHeapExtractMin_ReturnsNull() {
		FibonacciHeap<int> heap = new FibonacciHeap<int>();

		NUnit.Framework.Assert.Null(heap.ExtractMin());
	}

	[Test]
	public void EmptyHeapDecreaseKey_DoesNothing() {
		FibonacciHeap<int> heap = new FibonacciHeap<int>();
		FibonacciNode<int> node = new FibonacciNode<int>(3);
		heap.DecreaseKey(node, 2);

		NUnit.Framework.Assert.True(heap.IsEmpty());
		NUnit.Framework.Assert.AreEqual(default(int), heap.GetMin());
	}

	[Test]
	public void EmptyHeapIsEmpty_True() {
		FibonacciHeap<int> heap = new FibonacciHeap<int>();

		NUnit.Framework.Assert.True(heap.IsEmpty());
	}

	[Test]
	public void EmptyHeapContainsNode_False() {
		FibonacciHeap<int> heap = new FibonacciHeap<int>();
		FibonacciNode<int> node = new FibonacciNode<int>(3);

		NUnit.Framework.Assert.IsFalse(heap.Contains(node));
	}

	[Test]
	public void EmptyHeapGetAllValues_Empty() {
		FibonacciHeap<int> heap = new FibonacciHeap<int>();

		NUnit.Framework.Assert.IsTrue(heap.GetAllValues().Count == 0);
	}
	// Empty Heap Test

	// Unconsolidated heap (all nodes in root list) test
	[Test]
	public void UnconsolidatedHeapInsertWithValue_RightOder() {
		FibonacciHeap<int> heap = new FibonacciHeap<int>();
		IList<int> input = new List<int>() {
			0,
			28,
			-13,
			80
		};

		foreach (int value in input) {
			heap.Insert(value);
		}
		IList<int> result = heap.GetAllValues();
		for (int i = 0; i < input.Count; ++i) {
			NUnit.Framework.Assert.IsTrue(input[i] == result[i]);
		}
	}

	[Test]
	public void UnconsolidatedHeapGetMin_RightValue() {
		FibonacciHeap<int> heap = new FibonacciHeap<int>();
		IList<int> input = new List<int>() {
			0,
			28,
			-13,
			80
		};

		foreach (int value in input) {
			heap.Insert(value);
		}

		NUnit.Framework.Assert.IsTrue(heap.GetMin() == -13);
	}

	[Test]
	public void UnconsolidatedHeapIsEmpty_False() {
		FibonacciHeap<int> heap = new FibonacciHeap<int>();
		IList<int> input = new List<int>() {
			0,
			28,
			-13,
			80
		};

		foreach (int value in input) {
			heap.Insert(value);
		}

		NUnit.Framework.Assert.IsFalse(heap.IsEmpty());
	}

	[Test]
	public void UnconsolidatedHeapContainsNode_True() {
		FibonacciNode<int> node = new FibonacciNode<int>(-3);
		FibonacciHeap<int> heap = new FibonacciHeap<int>();
		IList<int> input = new List<int>() {
			0,
			28,
			-13,
			80
		};

		heap.Insert(node);
		foreach (int value in input) {
			heap.Insert(value);
		}

		NUnit.Framework.Assert.IsTrue(heap.Contains(node));
	}

	[Test]
	public void UnconsolidatedHeapContainsNode_False() {
		FibonacciNode<int> node = new FibonacciNode<int>(-3);
		FibonacciHeap<int> heap = new FibonacciHeap<int>();
		IList<int> input = new List<int>() {
			0,
			28,
			-13,
			80
		};

		foreach (int value in input) {
			heap.Insert(value);
		}

		NUnit.Framework.Assert.IsFalse(heap.Contains(node));
	}

	[Test]
	public void UnconsolidatedHeapDecreaseKeyNewMin_CorrectValue() {
		FibonacciNode<int> node = new FibonacciNode<int>(-3);
		FibonacciHeap<int> heap = new FibonacciHeap<int>();
		IList<int> input = new List<int>() {
			0,
			28,
			-13,
			80
		};

		heap.Insert(node);
		foreach (int value in input) {
			heap.Insert(value);
		}
		heap.DecreaseKey(node, -20);

		NUnit.Framework.Assert.IsTrue(heap.GetMin() == -20);
	}

	[Test]
	public void UnconsolidatedHeapDecreaseKeyMinSame_CorrectValue() {
		FibonacciNode<int> node = new FibonacciNode<int>(-3);
		FibonacciHeap<int> heap = new FibonacciHeap<int>();
		IList<int> input = new List<int>() {
			0,
			28,
			-13,
			80
		};

		heap.Insert(node);
		foreach (int value in input) {
			heap.Insert(value);
		}
		heap.DecreaseKey(node, -4);

		NUnit.Framework.Assert.IsTrue(heap.GetMin() == -13);
	}

	[Test]
	public void UnconsolidatedHeapExtractMin_CorrectValue() {
		FibonacciNode<int> node = new FibonacciNode<int>(-20);
		FibonacciHeap<int> heap = new FibonacciHeap<int>();
		IList<int> input = new List<int>() {
			0,
			28,
			-13,
			80
		};

		heap.Insert(node);
		foreach (int value in input) {
			heap.Insert(value);
		}

		NUnit.Framework.Assert.IsTrue(heap.ExtractMin() == node);
		NUnit.Framework.Assert.IsTrue(heap.GetMin() == -13);
		NUnit.Framework.Assert.IsTrue(heap.GetAllValues().Count == input.Count);
	}

	[Test]
	public void UnconsolidatedHeapExtractMin_CorrectConsolidation() {
		FibonacciHeap<int> heap = new FibonacciHeap<int>();
		IList<int> input = new List<int>() {
			0,
			28,
			-13,
			80,
			3,
			7,
			-7,
			42,
			-11,
			12
		};

		IList<int> expectedElementOrder = new List<int>() {
			42,
			7,
			-7,
			28,
			80,
			3,
			0,
			-11,
			12
		};

		foreach (int value in input) {
			heap.Insert(value);
		}
		heap.ExtractMin();

		IList<int> result = heap.GetAllValues();
		for (int i = 0; i < expectedElementOrder.Count; ++i) {
			NUnit.Framework.Assert.IsTrue(expectedElementOrder[i] == result[i]);
		}
		NUnit.Framework.Assert.IsTrue(heap.GetMin() == -11);
	}

	[Test]
	public void UnconsolidatedHeapInsertNull_Null() {
		FibonacciHeap<int> heap = new FibonacciHeap<int>();
		IList<int> input = new List<int>() {
			0,
			28,
		};

		NUnit.Framework.Assert.IsNull(heap.Insert(null));
	}

	[Test]
	public void UnconsolidatedHeapDecreaseKey_NoException() {
		FibonacciHeap<int> heap = new FibonacciHeap<int>();
		IList<int> input = new List<int>() {
			0,
			28,
		};

		heap.DecreaseKey(null, 3);
	}

	[Test]
	public void UnconsolidatedHeapContainsNull_false() {
		FibonacciHeap<int> heap = new FibonacciHeap<int>();
		IList<int> input = new List<int>() {
			0,
			28,
		};

		NUnit.Framework.Assert.IsFalse(heap.Contains(null));
	}
	// Unconsolidated heap (all nodes in root list) test

	// Consolidated heap
	[Test]
	public void ConsolidatedHeapExtractMin_CorrectConsolidation() {
		// Create consolidated heap
		FibonacciHeap<int> heap = new FibonacciHeap<int>();
		IList<int> input = new List<int>() {
			0,
			28,
			-13,
			80,
			3,
			7,
			-7,
			42,
			-11,
			12
		};

		IList<int> expectedElementOrder = new List<int>() {
			7,
			42,
			12,
			28,
			80,
			3,
			0,
			-7
		};

		foreach (int value in input) {
			heap.Insert(value);
		}
		heap.ExtractMin();

		// Trigger second consolidation
		heap.ExtractMin();

		IList<int> result = heap.GetAllValues();
		for (int i = 0; i < expectedElementOrder.Count; ++i) {
			NUnit.Framework.Assert.IsTrue(expectedElementOrder[i] == result[i]);
		}
		NUnit.Framework.Assert.IsTrue(heap.GetMin() == -7);
	}

	[Test]
	public void ConsolidatedHeapDecreaseKey_CorrectCuts() {
		FibonacciHeap<int> heap = new FibonacciHeap<int>();
		IList<FibonacciNode<int>> input = new List<FibonacciNode<int>>() {
			new FibonacciNode<int>(0),
			new FibonacciNode<int>(28),
			new FibonacciNode<int>(-13),
			new FibonacciNode<int>(80),
			new FibonacciNode<int>(3),
			new FibonacciNode<int>(7),
			new FibonacciNode<int>(-7),
			new FibonacciNode<int>(42),
			new FibonacciNode<int>(-11),
			new FibonacciNode<int>(12)
		};

		IList<int> expectedElementOrder = new List<int>() {
			7,
			-8,
			-11,
			12,
			-42,
			80,
			-1,
			-3,
			0
		};

		foreach (FibonacciNode<int> value in input) {
			heap.Insert(value);
		}
		heap.ExtractMin();

		// A decrease key with no structural changes
		heap.DecreaseKey(input[6], -8);

		// Normal cuts with parent marked
		heap.DecreaseKey(input[7], -42);
		heap.DecreaseKey(input[4], -1);

		// Double cascading cut
		heap.DecreaseKey(input[1], -3);

		IList<int> result = heap.GetAllValues();
		for (int i = 0; i < expectedElementOrder.Count; ++i) {
			NUnit.Framework.Assert.IsTrue(expectedElementOrder[i] == result[i]);
		}
		NUnit.Framework.Assert.IsTrue(heap.GetMin() == -42);
	}
	// Consolidated heap
}
