namespace Translator.Collections
{
    public class Node
    {
        public readonly string Data;

        private List<Node> _children;
        public Node Parent;
        public IReadOnlyList<Node> Children => _children;
        public readonly int Depth;

        public Node(string data, Node parent)
        {
            Data = data;
            Parent = parent;

            _children = new List<Node>();
        }

        public Node(string data)
        {
            Data = data;
            Parent = null;

            _children = new List<Node>();
        }

        public override string ToString()
        {
            return Data;
        }

        public void AddChild(Node child)
        {
            _children.Add(child);
            child.Parent = this;
        }

        public void RemoveChild(Node child)
        {
            _children.Remove(child);
            child.Parent = null;
        }

        public int GetDepth()
        {
            Node curNode = Parent;
            int depth = 0;
            while (curNode != null)
            {
                depth++;
                curNode = curNode.Parent;
            }

            return depth;
        }

        public void Traverse(Action<Node> enterAction = null, Action<Node> exitAction = null)
        {
            enterAction?.Invoke(this);
            foreach (var child in Children)
            {
                child.Traverse(enterAction, exitAction);
            }
            exitAction?.Invoke(this);
        }

        public List<Node> GetChildren(string data)
        {
            List<Node> children = new List<Node>();

            Traverse((node) =>
            {
                if (node.Data == data)
                {
                    children.Add(node);
                }
            });

            return children;
        }

        public void ReplaceChildrenWith(List<Node> nodes)
        {
            List<Node> children = new List<Node>(_children);

            foreach (Node child in children)
            {
                RemoveChild(child);
            }

            foreach (Node node in nodes)
            {
                AddChild(node);
            }
        }
    }
}
