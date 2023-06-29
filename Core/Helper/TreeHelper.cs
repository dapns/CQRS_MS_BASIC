using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Helper
{

    public static class TreeExtensions
    {
        public static List<TreeNode<TValue>> BuildTree<TKey, TValue>(this IEnumerable<TValue> objects, Func<TValue, TKey> keySelector, Func<TValue, TKey> parentKeySelector, TKey defaultKey = default(TKey))
        {
            var roots = new List<TreeNode<TValue>>();
            var allNodes = objects.Select(overrideValue => new TreeNode<TValue>(overrideValue)).ToArray();
            var nodesByRowId = allNodes.ToDictionary(node => keySelector(node.Value));

            foreach (var currentNode in allNodes)
            {
                TKey parentKey = parentKeySelector(currentNode.Value);
                if (Equals(parentKey, defaultKey))
                {
                    roots.Add(currentNode);
                }
                else
                {
                    nodesByRowId[parentKey].Children.Add(currentNode);
                }
            }

            return roots;
        }
    }

    public class TreeNode<T>
    {
        public TreeNode(T value)
        {
            Value = value;
            Children = new List<TreeNode<T>>();
        }

        public T Value { get; private set; }
        public List<TreeNode<T>> Children { get; private set; }
    }
}
