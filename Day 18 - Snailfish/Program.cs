using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Numerics;


namespace Snailfish
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt");
            var start = ParseLine(lines[0].ToArray());
            //start = Simplify(start);

            for(int i = 1; i < lines.Length; i++){
                var toAdd = ParseLine(lines[i].ToArray());
                start = AddNodes(start, toAdd);
                start = Simplify(start);
            }
            
            PrintResult(start);
            Console.WriteLine();
            Console.WriteLine("Magnitude: " + start.GetMagnitude());


            long max = -1;
            var allToAdd = lines.Select(l => ParseLine(l.ToArray())).ToArray();
            for(int i = 0; i < allToAdd.Length; i++){
                for(int n = 0; n < allToAdd.Length; n++){
                    if(i == n) continue;

                    var result = AddNodes(allToAdd[i].DeepCopy(), allToAdd[n].DeepCopy());
                    result = Simplify(result);
                    var magnitude = result.GetMagnitude();

                    if(magnitude > max){
                        max = magnitude;
                    }
                }
            }

            Console.WriteLine("Max magnitude: " + max);
        }

        static Node ParseLine(char[] input)
        {
            var currentNode = new Node();
            for (int i = 1; i < input.Length - 1; i++)
            {
                if (char.IsDigit(input[i]))
                {
                    if (currentNode.LeftNode == null)
                    {
                        currentNode.LeftNode = new Node() { Data = int.Parse(input[i].ToString()), Parent = currentNode };
                    }
                    else
                    {
                        currentNode.RightNode = new Node() { Data = int.Parse(input[i].ToString()), Parent = currentNode };
                    }
                }
                else if (input[i] == '[')
                {
                    if (currentNode.LeftNode == null)
                    {
                        currentNode.LeftNode = new Node() { Parent = currentNode };
                        currentNode = currentNode.LeftNode;
                    }
                    else
                    {
                        currentNode.RightNode = new Node() { Parent = currentNode };
                        currentNode = currentNode.RightNode;
                    }
                }
                else if (input[i] == ']')
                {
                    currentNode = currentNode.Parent;
                }
            }

            return currentNode;
        }

        static Node AddNodes(Node node1, Node node2)
        {
            var newParent = new Node { LeftNode = node1, RightNode = node2 };
            node1.Parent = newParent;
            node2.Parent = newParent;
            return newParent;
        }

        static Node Simplify(Node node)
        {
            bool didWork = false;
            do
            {
                didWork = false;
                var explode = NodesForExplode(node);
                while (explode.Count > 0)
                {
                    foreach (var n in explode)
                    {
                        Explode(n);
                        didWork = true;
                    }
                    explode = NodesForExplode(node);
                }

                var split = NodesForSplit(node);
                    if (split.Count > 0)
                    {
                        Split(split.FirstOrDefault());
                        didWork = true;
                    }
                
            } while (didWork);

            return node;
        }

        static void Explode(Node node)
        {
            //Console.WriteLine("Explode");
            var leftValue = node.LeftNode.Data.GetValueOrDefault();
            var parent = node.Parent;
            var previous = node;
            Node downNode = null;
            while (downNode == null)
            {
                if (parent.LeftNode != previous && parent.LeftNode != null)
                {
                    downNode = parent.LeftNode;
                }
                else
                {
                    previous = parent;
                }
                if (parent.Parent == null) break;

                parent = parent.Parent;
            }

            if (downNode != null)
            {
                Node valueNode = null;
                while (valueNode == null)
                {
                    if (downNode.Data == null)
                    {
                        downNode = downNode.RightNode;
                    }
                    {
                        downNode.Data += leftValue;
                    }

                    if (downNode.RightNode == null) break;
                }
            }

            //////////////////////////////////////////////////

            var rightValue = node.RightNode.Data.GetValueOrDefault();
            parent = node.Parent;
            previous = node;
            downNode = null;
            while (downNode == null)
            {
                if (parent.RightNode != previous && parent.RightNode != null)
                {
                    downNode = parent.RightNode;
                }
                else{
                    previous = parent;
                }
                if (parent.Parent == null) break;

                parent = parent.Parent;
            }

            if (downNode != null)
            {
                Node valueNode = null;
                while (valueNode == null)
                {
                    if (downNode.Data == null)
                    {
                        downNode = downNode.LeftNode;
                    }
                    {
                        downNode.Data += rightValue;
                    }

                    if (downNode.LeftNode == null) break;
                }
            }

            if (node.Parent.LeftNode == node)
            {
                node.Parent.LeftNode = new Node{Data = 0, Parent = node.Parent};
            }
            if (node.Parent.RightNode == node)
            {
                node.Parent.RightNode = new Node{Data = 0, Parent = node.Parent};
            }
            node.Parent.Data = node.Data;
        }

        static void Split(Node node)
        {
            //Console.WriteLine("Split");
            var data = node.Data;
            node.Data = null;
            var half = data.GetValueOrDefault() / 2.0;

            node.LeftNode = new Node() { Data = (int)Math.Floor(half), Parent = node };
            node.RightNode = new Node() { Data = (int)Math.Ceiling(half), Parent = node };
        }

        static List<Node> NodesForExplode(Node root)
        {
            var ret = new List<Node>();
            if (root.GetDepth() > 4
                && root.LeftNode != null && root.LeftNode.Data != null
                && root.RightNode != null && root.RightNode.Data != null)
            {
                ret.Add(root);
            }
            if (root.LeftNode != null && root.LeftNode.Data == null)
            {
                ret.AddRange(NodesForExplode(root.LeftNode));
            }
            if (root.RightNode != null && root.RightNode.Data == null)
            {
                ret.AddRange(NodesForExplode(root.RightNode));
            }
            return ret;
        }

        static List<Node> NodesForSplit(Node root)
        {
            var ret = new List<Node>();
            if (root.Data != null && root.Data > 9)
            {
                ret.Add(root);
            }
            if (root.LeftNode != null)
            {
                ret.AddRange(NodesForSplit(root.LeftNode));
            }
            if (root.RightNode != null)
            {
                ret.AddRange(NodesForSplit(root.RightNode));
            }
            return ret;
        }

        static void PrintResult(Node node){
            if(node.Data == null){
                Console.Write("[");
                if(node.LeftNode != null){
                    PrintResult(node.LeftNode);
                }
                Console.Write(",");
                if(node.RightNode != null){
                    PrintResult(node.RightNode);
                }
                Console.Write("]");
            }
            else{
                Console.Write(node.Data);
            }
        }
    }

    class Node
    {
        public Node Parent { get; set; }
        public Node LeftNode { get; set; }
        public Node RightNode { get; set; }
        public int? Data { get; set; }
        public int GetDepth()
        {
            if (Parent == null)
                return 1;
            return Parent.GetDepth() + 1;
        }

        public long GetMagnitude(){
            if(Data != null)
                return Data.GetValueOrDefault();
            
            return (LeftNode.GetMagnitude() * 3) + (RightNode.GetMagnitude() * 2);
        }

        public Node DeepCopy(){
            var node = new Node(){
                LeftNode = LeftNode?.DeepCopy(),
                RightNode = RightNode?.DeepCopy(),
                Data = Data
            };

            if(node.LeftNode != null)
                node.LeftNode.Parent = node;
            if(node.RightNode != null)
                node.RightNode.Parent = node;

            return node;
        }
    }
}
