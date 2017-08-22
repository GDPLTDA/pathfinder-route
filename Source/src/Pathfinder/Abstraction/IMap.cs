using System;
using System.Collections.Generic;

namespace Pathfinder
{
    public interface IMap
    {
        Node this[int y, int x] { get; set; }
        MapGeneratorEnum MapType { get; set; }

        DiagonalMovement Diagonal { get; set; }
        Node EndNode { get; set; }
        int Height { get; set; }
        Node[,] Nodes { get; set; }
        Node StartNode { get; set; }
        int Width { get; set; }

        void AddInClosedList(Node node);
        void AddInOpenList(Node node);
        void Clear();
        int ClosedListCount();
        void DefineAllNodes();
        void DefineAllNodes(IList<Node> nodes);
        void DefineNode(Node node);
        Node GetDirectionNode(Node node, bool ByRef = true, bool valid = true);
        Node GetDirectionNode(Node node, DirectionMovement direction, bool ByRef = true, bool valid = true);
        int GetMaxExpandedNodes();
        void SetMaxExpandedNodes(int value);
        IList<Node> GetNeighbors(Node node, bool ByRef = true, bool valid = true);
        IList<Node> GetNeighbors(Node node, DiagonalMovement diag, bool ByRef = true, bool valid = true);
        IEnumerable<Node> GetNodesInClosedLit();
        IEnumerable<Node> GetNodesInOpenList();
        IEnumerable<Node> GetPath();
        bool IsClosed(Node e);
        bool IsInside(int y, int x);
        bool IsOpen(Node e);
        bool IsWalkableAt(int y, int x);
        bool IsWalkableAt(Node node);
        void OrderOpenList(Func<Node, object> predicate);
        int OpenListCount();
        Node PopOpenList();
        void PushInOpenList(Node node);
        void UpdateClosedList(IList<Node> newList);
        void UpdateMaxNodes();
        void UpdateOpenList(IList<Node> newList);
        bool ValidMap();
    }
}