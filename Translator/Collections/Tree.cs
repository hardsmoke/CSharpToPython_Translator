using System;
using System.Collections.Generic;

namespace Translator.Collections
{
    public class Tree
    {
        private Node _root;
        public Node Root => _root;

        public Tree()
        {
            _root = null;
        }

        public Tree(Node root)
        {
            _root = root;
        }

        public void Print(string treeName = default)
        {
            Console.ForegroundColor = Program.DEBUG_COLOR;
            Console.WriteLine($"{treeName} TREE");
            Console.ResetColor();

            Print(_root);

            Console.ForegroundColor = Program.DEBUG_COLOR;
            Console.WriteLine($"END OF {treeName} TREE");
            Console.ResetColor();
        }

        private void Print(Node node)
        {
            for (int i = 0; i < node.GetDepth() - 1; i++)
            {
                Console.Write("│");
            }

            if (node.GetDepth() > 0)
            {
                Console.Write("├ ");
            }

            Console.WriteLine(node);
            foreach (var child in node.Children)
            {
                Print(child);
            }
        }

        public Tree Copy()
        {
            Node rootCopy = new Node(Root.ToString());
            Copy(Root, rootCopy);
            return new Tree(rootCopy);
        }

        private void Copy(Node node, Node copyToNode)
        {
            foreach (var child in node.Children)
            {
                Node newChild = new Node(child.ToString());
                copyToNode.AddChild(newChild);
                Copy(child, newChild);
            }
        }

        public Tree CopyWithoutNode(string data)
        {
            Node rootCopy = new Node(Root.ToString());
            CopyWithoutNode(Root, rootCopy, data);
            return new Tree(rootCopy);
        }

        private void CopyWithoutNode(Node node, Node copyToNode, string data)
        {
            foreach (var child in node.Children)
            {
                if (child.ToString() == data)
                {
                    CopyWithoutNode(child, copyToNode, data);
                }
                else
                {
                    Node newNode = new Node(child.ToString());
                    copyToNode.AddChild(newNode);
                    CopyWithoutNode(child, newNode, data);
                }
            }
        }

        public List<Node> GetNodes(string data)
        {
            List<Node> nodes = new List<Node>();
            Root.Traverse((node) => {
                if (node.ToString() == data)
                {
                    nodes.Add(node);
                }
                });
            return nodes;
        }
    }
}
