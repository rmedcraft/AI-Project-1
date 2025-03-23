using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AStar : ScriptableObject {
    Node goal;

    Graph graph;
    Pathfinder pathfinder;
    List<Node> exploreNodes;
    List<Node> frontierNodes;
    List<Node> pathNodes;
    public bool isComplete;
    public int iterations;
    Dictionary<Node, int> distFromStart;
    bool manhattan;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Init(Pathfinder pathfinder, Graph graph, Node start, Node goal, bool manhattan) {
        if (start == null || goal == null || pathfinder == null || graph == null) {
            Debug.LogWarning("BFS error: Missing components.");
            return;
        }
        if (start.nodeType == NodeType.blocked || goal.nodeType == NodeType.blocked) {
            Debug.LogWarning("BFS error: Make sure start and goal nodes are open");
            return;
        }

        this.graph = graph;
        this.goal = goal;
        this.pathfinder = pathfinder;

        frontierNodes = new List<Node>();
        frontierNodes.Add(start);
        pathNodes = new List<Node>();
        exploreNodes = new List<Node>();

        distFromStart = new Dictionary<Node, int>();
        distFromStart.Add(start, 0);

        for (int r = 0; r < graph.getWidth(); r++) {
            for (int c = 0; c < graph.getHeight(); c++) {
                this.graph.nodes[r, c].Reset();
            }
        }

        isComplete = false;
        this.manhattan = manhattan;
    }

    public IEnumerator SearchRoutine() {
        yield return null;
        while (!isComplete) {
            if (frontierNodes.Count > 0) {
                Node currentNode = frontierNodes[0];
                foreach (Node n in frontierNodes) {
                    if (AStarDist(n, goal) < AStarDist(currentNode, goal)) {
                        currentNode = n;
                    }
                }
                frontierNodes.Remove(currentNode);

                iterations++;
                if (!exploreNodes.Contains(currentNode)) {
                    exploreNodes.Add(currentNode);
                }
                ExpandFrontier(currentNode);
                if (frontierNodes.Contains(goal)) {
                    pathNodes = pathfinder.GetPathNodes(goal);
                    pathfinder.ShowColors(frontierNodes.ToList(), exploreNodes, pathNodes);
                    isComplete = true;
                }

                yield return new WaitForSeconds(pathfinder.timeStep);
            } else {
                isComplete = true;
            }
            pathfinder.ShowColors(frontierNodes.ToList(), exploreNodes, pathNodes);
        }
    }

    public void ExpandFrontier(Node node) {
        foreach (Node n in node.neighbors) {
            if (!exploreNodes.Contains(n) && !frontierNodes.Contains(n)) {
                n.prev = node;
                distFromStart.Add(n, distFromStart[node] + 1);
                frontierNodes.Add(n);
            }
        }
    }

    float AStarDist(Node start, Node goal) {
        return distFromStart[start] + (manhattan ? GreedyBest.ManhattanDist(start, goal) : EuclideanDist(start, goal));
    }

    float EuclideanDist(Node n1, Node n2) {
        return Mathf.Sqrt(Mathf.Pow(n1.xIndex - n2.xIndex, 2) + Mathf.Pow(n1.yIndex - n2.yIndex, 2));
    }
}