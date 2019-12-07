using System;
using System.Collections;
using System.Collections.Generic;

public class FibonacciNode<T> where T : IComparable<T> {

	public T key;
	public bool marked;

	public FibonacciNode<T> parent;
	public LinkedList<FibonacciNode<T>> children;

	public FibonacciNode(T data) {
		this.key = data;
		marked = false;
		children = new LinkedList<FibonacciNode<T>>();
	}

	public FibonacciNode<T> AddChild(FibonacciNode<T> child) {
		if (children == null) {
			children = new LinkedList<FibonacciNode<T>>();
		}
		children.AddLast(child);
		child.parent = this;

		return child;
	}

	public FibonacciNode<T> RemoveChild(FibonacciNode<T> child) {
		children.Remove(child);
		child.parent = null;

		return child;
	}
}
