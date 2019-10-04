using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cami.Collections;

namespace CamiFramwork.PathFinding
{
    public class AStarPath
    {
        public AStarPath(Heuristic heuristic, ExpandPathFunc expandFunc)
        {
            this.heuristic = heuristic;
            this.expandFunc = expandFunc;

            Complete = true;
            MaxSearchSteps = int.MaxValue;
            MaxCost = int.MaxValue;

            openNodes = new BinaryHeap<int>(new System.Comparison<int>(CompareNodes));
            expandedNodes = new ArrayList<IPathNode>();
            nodes = new Dictionary<int, PathNodeWrapper>();
            wrapperPool = new Queue<PathNodeWrapper>();
            goalNodeCache = new ArrayList<int>();
        }

        public void Start(IPathNode start, IPathNode goal, object state)
        {
            Start(start, goal, state, false);
        }

        public void Start(IPathNode start, IPathNode goal, object state, bool cacheGoalNodes)
        {
            this.start = start;
            this.goal = goal;
            this.state = state;
            this.cacheGoalNodes = cacheGoalNodes;
            SearchSteps = 0;
            MaxCostReached = false;

            Clear();

            if (!cacheGoalNodes || this.goal != goal)
                goalNodeCache.Clear();

            currentNode = GetPathNodeWrapper(start);

            if (start.NodeId == goal.NodeId)
            {
                Complete = true;
                GoalFound = true;
                return;
            }

            currentNode.HeuristicValue = heuristic(ref currentNode.Node, ref goal);

            Complete = false;
            GoalFound = false;
        }

        public void Step()
        {
            if (Complete)
                return;

            if (cacheGoalNodes && goalNodeCache.Contains(currentNode.Node.NodeId))
            {
                AddPathToGoalNodes();
                GoalFound = true;
                Complete = true;
                return;
            }

            ++SearchSteps;
            expandedNodes.Clear();
            expandFunc(currentNode.Node, expandedNodes, state);

            for (int i = 0; i < expandedNodes.Count; ++i)
            {
                IPathNode expandedNode = expandedNodes[i];

                PathNodeWrapper expandedWrapper = GetPathNodeWrapper(expandedNode);

                if (!expandedWrapper.Closed)
                {
                    if (!openNodes.Contains(expandedNode.NodeId))
                    {
                        expandedWrapper.HeuristicValue = heuristic(ref expandedNode, ref goal);
                        expandedWrapper.Parent = currentNode;
                        expandedWrapper.Cost = currentNode.Cost + heuristic(ref expandedNode, ref currentNode.Node);

                        openNodes.Add(expandedNode.NodeId);
                    }
                    else
                    {
                        if (expandedWrapper.Parent.Node.NodeId != currentNode.Node.NodeId)
                        {
                            float costViaCurrent = currentNode.Cost + heuristic(ref expandedNode, ref currentNode.Node);
                            if (costViaCurrent < expandedWrapper.Cost)
                            {
                                expandedWrapper.Parent = currentNode;
                                expandedWrapper.Cost = costViaCurrent;

                                openNodes.EnsureItemPosition(expandedNode.NodeId);
                            }
                        }
                    }
                }
            }
            currentNode.Closed = true;

            if (openNodes.Count > 0)
            {
                currentNode = nodes[openNodes[0]];
                openNodes.RemoveAt(0);
            }
            else
            {
                MaxCostReached = false;
                Complete = true;
                GoalFound = false;
            }
            if (currentNode.HeuristicValue == 0)
            {
                GoalFound = true;
                Complete = true;
            }
            if (currentNode.Cost >= MaxCost)
            {
                MaxCostReached = true;
                Complete = true;
            }
            if (SearchSteps >= MaxSearchSteps)
            {
                MaxCostReached = false;
                Complete = true;
                GoalFound = false;
            }
        }

        public bool RunUntilComplete()
        {
            while (!Complete)
                Step();

            return GoalFound;   
        }
        
        private int CompareNodes(int node1Id, int node2Id)
        {
            PathNodeWrapper node1 = nodes[node1Id];
            PathNodeWrapper node2 = nodes[node2Id];

            float costToGoal1 = node1.Cost + node1.HeuristicValue;
            float costToGoal2 = node2.Cost + node2.HeuristicValue;
            if (costToGoal1 < costToGoal2)
            {
                return -1;
            }
            else if (costToGoal1 > costToGoal2)
            {
                return 1;
            }
            return 0;
        }

        private PathNodeWrapper WrapPathNode(IPathNode node)
        {
            PathNodeWrapper wrapper;
            if (wrapperPool.Count > 0)
            {
                wrapper = wrapperPool.Dequeue();
            }
            else
            {
                wrapper = new PathNodeWrapper();
            }
            wrapper.Node = node;
            wrapper.Parent = null;
            wrapper.HeuristicValue = 0;
            wrapper.Cost = 0;
            wrapper.Closed = false;
            nodes.Add(node.NodeId, wrapper);
            return wrapper;
        }

        private PathNodeWrapper GetPathNodeWrapper(IPathNode node)
        {
            if (nodes.ContainsKey(node.NodeId))
            {
                return nodes[node.NodeId];
            }
            return WrapPathNode(node);
        }

        private void AddPathToGoalNodes()
        {
            PathNodeWrapper pathNode = currentNode;
            while (pathNode != null)
            {
                if (!goalNodeCache.Contains(pathNode.Node.NodeId))
                    goalNodeCache.Add(pathNode.Node.NodeId);
                pathNode = pathNode.Parent;
            }
        }

        public void GetPath<T>(ICollection<T> path)
        {
            path.Clear();
            PathNodeWrapper pathNode = currentNode;
            while (pathNode != null)
            {
                path.Add((T)pathNode.Node);
                pathNode = pathNode.Parent;
            }
        }

        public void GetOpenCells(ICollection<IPathNode> openCells)
        {
            openCells.Clear();

            for (int i = 0; i < openNodes.Count; ++i )
            {
                openCells.Add(nodes[openNodes[i]].Node);
            }
        }

        public void GetClosedCells(ICollection<IPathNode> closedCells)
        {
            closedCells.Clear();

            foreach (KeyValuePair<int, PathNodeWrapper> kvp in nodes)
            {
                if (kvp.Value.Closed)
                {
                    closedCells.Add(kvp.Value.Node);
                }
            }
        }

        public void Clear()
        {
            openNodes.Clear();
            expandedNodes.Clear();
            foreach (KeyValuePair<int, PathNodeWrapper> kvp in nodes)
                wrapperPool.Enqueue(kvp.Value);
            nodes.Clear();
        }

        #region Properties
        public Heuristic PathHeuristic
        {
            get { return heuristic; }
            set { heuristic = value; }
        }
        public bool Complete { get; private set; }
        public bool GoalFound { get; private set; }
        public int SearchSteps { get; private set; }
        public int MaxSearchSteps { get; set; }
        public float MaxCost { get; set; }
        public bool MaxCostReached { get; private set; }
        public float GoalCost { get { return currentNode.Cost + currentNode.HeuristicValue; } }
        #endregion Properties

        #region Fields
        private ExpandPathFunc expandFunc;
        private IPathNode start;
        private IPathNode goal;
        private PathNodeWrapper currentNode;
        private ArrayList<IPathNode> expandedNodes;
        private Dictionary<int, PathNodeWrapper> nodes;
        private BinaryHeap<int> openNodes;
        private Heuristic heuristic;
        private Queue<PathNodeWrapper> wrapperPool;
        private object state;
        private bool cacheGoalNodes;
        private ArrayList<int> goalNodeCache;

        public delegate float Heuristic(ref IPathNode nodeA, ref IPathNode nodeB);
        public delegate void ExpandPathFunc(IPathNode source, ICollection<IPathNode> expanded, object state);
        #endregion Fields

        class PathNodeWrapper
        {
            public PathNodeWrapper()
            {
            }

            public IPathNode Node;
            public float Cost;
            public float HeuristicValue;
            public PathNodeWrapper Parent;
            public bool Closed;
        }
    }
}