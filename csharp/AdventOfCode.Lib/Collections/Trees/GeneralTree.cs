using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Lib.Collections.Trees
{
    public class GeneralTree<TValue> :
        ITree<GeneralTreeNode<TValue>, TValue>,
        IDFSPreOrderEnumerable<GeneralTreeNode<TValue>, TValue>
    {
        private GeneralTreeNode<TValue> root;

        public GeneralTreeNode<TValue> Root
        {
            get => root;
            set
            {
                MakeRoot(value);
                root = value;
            }
        }

        public IEnumerable<GeneralTreeNode<TValue>> EnumerateDFSPreOrder() => TraverseDFSPreOrder(Root);

        private void MakeRoot(GeneralTreeNode<TValue> node)
        {
            if (node.Parent == null)
                return;

            var parent = node.Parent;
            parent.RemoveChild(node);
            MakeRoot(parent);
            node.AddChild(parent);
        }

        private IEnumerable<GeneralTreeNode<TValue>> TraverseDFSPreOrder(GeneralTreeNode<TValue> node)
        {
            yield return node;

            foreach (var child in node.Children.SelectMany(TraverseDFSPreOrder))
                yield return child;
        }
    }
}
