using System;
using System.Collections;
using System.Collections.Generic;

public class FibonacciHeap<T> where T : IComparable<T> {
	private FibonacciNode<T> min;

	private LinkedList<FibonacciNode<T>> rootList;
	private ISet<FibonacciNode<T>> containedNodes;

	public FibonacciHeap(){
		rootList = new LinkedList<FibonacciNode<T>>();
		containedNodes = new HashSet<FibonacciNode<T>>();
	}

	public T GetMin() {
		if (min == null) {
			return default;
		}
		return min.key;
	}

	public FibonacciNode<T> Insert(T value) {
		FibonacciNode<T> node = new FibonacciNode<T>(value);
		return Insert(node);
	}

	public FibonacciNode<T> Insert(FibonacciNode<T> node) {
		if (node == null) {
			return null;
		}
		rootList.AddLast(node);
		if (min == null) {
			min = node;
		} else {
			if (node.key.CompareTo(min.key) < 0) {
				min = node;
			}
		}
		containedNodes.Add(node);

		return node;
	}

	public FibonacciNode<T> ExtractMin() {
		if (min == null) {
			return null;
		}
		foreach (FibonacciNode<T> child in min.children) {
			child.parent = null;
			Insert(child);
		}
		rootList.Remove(min);
		containedNodes.Remove(min);
		FibonacciNode<T> oldMin = min;
		if (rootList.Count == 0) {
			min = null;
			return oldMin;
		}
		Consolidate();
		return oldMin;
	}

	private void Consolidate() {
		Dictionary<int, FibonacciNode<T>> rootNodeDeg = new Dictionary<int, FibonacciNode<T>>();
		// Copy needed for Iteration, as the original rootList will be manipulated
		LinkedList<FibonacciNode<T>> oldRootList = new LinkedList<FibonacciNode<T>>(rootList);
		foreach (FibonacciNode<T> node in oldRootList) {
			FibonacciNode<T> currNode = node;
			// Link nodes with same degree
			FibonacciNode<T> nodeWithChildDeg;
			while (rootNodeDeg.TryGetValue(currNode.children.Count, out nodeWithChildDeg) && nodeWithChildDeg != null) {
				int oldChildCount = currNode.children.Count;
				// Swap the node to link if necessary
				if (currNode.key.CompareTo(nodeWithChildDeg.key) > 0) {
					FibonacciNode<T> oldCurrNode = currNode;
					currNode = nodeWithChildDeg;
					nodeWithChildDeg = oldCurrNode;
				}
				Link(nodeWithChildDeg, currNode);
				rootNodeDeg[oldChildCount] = null;
			}
			rootNodeDeg[currNode.children.Count] = currNode;
		}
		// Restore min value
		min = null;
		foreach (FibonacciNode<T> node in rootList) {
			if (min == null) {
				min = node;
			} else if (node.key.CompareTo(min.key) < 0) {
				min = node;
			}
		}
	}

	/// <summary>
	/// Links node x from the rootList to node y from the rootList
	/// </summary>
	private void Link(FibonacciNode<T> x, FibonacciNode<T> y) {
		rootList.Remove(x);
		y.AddChild(x);
		y.marked = false;
	}

	/// <summary>
	/// Cuts x from ys children and adds it to the rootList
	/// </summary>
	private void Cut(FibonacciNode<T> node, FibonacciNode<T> parent) {
		parent.RemoveChild(node);
		node.marked = false;
		Insert(node);
	}

	public void DecreaseKey(FibonacciNode<T> node, T value) {
		if (node == null) {
			return;
		}
		if (containedNodes.Contains(node) == false) {
			return;
		}

		FibonacciNode<T> parent = node.parent;
		node.key = value;
		if (parent != null) {
			if (node.key.CompareTo(parent.key) < 0) {
				Cut(node, parent);
				CascadingCut(parent);
			}
		}
		if (min == null) {
			min = node;
		} else if (node.key.CompareTo(min.key) < 0) {
			min = node;
		}
	}

	private void CascadingCut(FibonacciNode<T> node) {
		if (node.marked) {
			FibonacciNode<T> parent = node.parent;
			if (parent != null) {
				Cut(node, parent);
				CascadingCut(parent);
			}
		} else {
			node.marked = true;
		}
	}

	public bool IsEmpty() {
		return rootList.Count == 0;
	}

	public bool Contains(FibonacciNode<T> node) {
		return containedNodes.Contains(node);
	}

	/// <summary>
	/// Gets all values in the heap with the nodes children add first
	/// </summary>
	public IList<T> GetAllValues() {
		IList<T> allValues = new List<T>();
		foreach (FibonacciNode<T> node in rootList) {
			GetNodeValueWithChildValues(node, allValues);
		}
		return allValues;
	}

	/// <summary>
	/// Recursively gets the nodes children values followed by the nodes value itself
	/// </summary>
	private IList<T> GetNodeValueWithChildValues(FibonacciNode<T> node, IList<T> allValues) {
		foreach (FibonacciNode<T> childNode in node.children) {
			GetNodeValueWithChildValues(childNode, allValues);
		}
		allValues.Add(node.key);
		return allValues;
	}
}
