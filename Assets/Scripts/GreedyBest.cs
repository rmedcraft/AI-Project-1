using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GreedyBest : ScriptableObject {
    Node goal;

    Graph graph;
    Pathfinder pathfinder;
    List<Node> exploreNodes;
    List<Node> frontierNodes;
    List<Node> pathNodes;
    public bool isComplete;
    public int iterations;

    int maxFrontier = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Init(Pathfinder pathfinder, Graph graph, Node start, Node goal) {
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

        for (int r = 0; r < graph.getWidth(); r++) {
            for (int c = 0; c < graph.getHeight(); c++) {
                this.graph.nodes[r, c].Reset();
            }
        }

        isComplete = false;
    }

    public IEnumerator SearchRoutine() {
        yield return null;
        while (!isComplete) {
            if (frontierNodes.Count > 0) {
                // find the current node we're using based on which has the lowest manhattan distance in the frontier nodes
                Node currentNode = frontierNodes[0];
                foreach (Node n in frontierNodes) {
                    if (ManhattanDist(currentNode, goal) > ManhattanDist(n, goal)) {
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

            if (frontierNodes.Count > maxFrontier) {
                maxFrontier = frontierNodes.Count;
            }

            int totalExplored = exploreNodes.Count + frontierNodes.Count;

            Debug.Log("Iterations: " + iterations);
            Debug.Log("Explored Nodes: " + totalExplored);
            Debug.Log("Max Frontier: " + maxFrontier);
        }
    }

    public static int ManhattanDist(Node n1, Node n2) {
        return Mathf.Abs((n2.xIndex - n1.xIndex) + (n2.yIndex - n1.yIndex));
    }

    public void ExpandFrontier(Node node) {
        for (int i = 0; i < node.neighbors.Count; i++) {
            if (!exploreNodes.Contains(node.neighbors[i]) && !frontierNodes.Contains(node.neighbors[i])) {
                node.neighbors[i].prev = node;
                frontierNodes.Add(node.neighbors[i]);
            }
        }
    }
}