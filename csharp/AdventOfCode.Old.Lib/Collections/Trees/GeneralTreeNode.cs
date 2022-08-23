using System;
using System.Collections;
using System.Collections.Generic;

namespace AdventOfCode.Lib.Collections.Trees
{
    public class GeneralTreeNode<TValue> :
        IGeneralTreeNode<GeneralTreeNode<TValue>, TValue>,
        ITreeNode<GeneralTreeNode<TValue>, TValue>,
        IEquatable<GeneralTreeNode<TValue>>
    {
        private readonly HashSet<GeneralTreeNode<TValue>> children = new HashSet<GeneralTreeNode<TValue>>();

        public GeneralTreeNode(TValue value)
        {
            Value = value;
        }

        public TValue Value { get; }

        public GeneralTreeNode<TValue> Parent { get; private set; }

        public int Depth => (Parent?.Depth + 1 ?? 0);

        public IReadOnlySet<GeneralTreeNode<TValue>> Children => children;

        public bool AddChild(TValue value) => AddChild(new GeneralTreeNode<TValue>(value));

        public bool AddChild(GeneralTreeNode<TValue> node)
        {
            if (node.Parent == this)
                return false;

            if (node.Parent != null)
                node.Parent.RemoveChild(node);

            node.Parent = this;
            return children.Add(node);
        }

        public bool RemoveChild(TValue value) => RemoveChild(new GeneralTreeNode<TValue>(value));

        public bool RemoveChild(GeneralTreeNode<TValue> node)
        {
            if (node.Parent != this)
                return false;

            node.Parent = null;
            return children.Remove(node);
        }

        public bool Equals(GeneralTreeNode<TValue> other) => other != null && EqualityComparer<TValue>.Default.Equals(Value, other.Value);

        public override bool Equals(object obj) => (obj is GeneralTreeNode<TValue>) && Equals((GeneralTreeNode<TValue>)obj);

        public override int GetHashCode() => Value.GetHashCode();
    }
}
