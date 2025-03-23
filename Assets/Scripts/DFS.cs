using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DFS : ScriptableObject {
    Node goal;

    Graph graph;
    Pathfinder pathfinder;
    List<Node> exploreNodes;
    Stack<Node> frontierNodes;
    List<Node> pathNodes;
    public bool isComplete;
    public int iterations;

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

        frontierNodes = new Stack<Node>();
        frontierNodes.Push(start);
        pathNodes = new List<Node>();
        exploreNodes = new List<Node>();

        for (int r = 0; r < graph.getWidth(); r++) {
            for (int c = 0; c < graph.getHeight(); c++) {
                this.graph.nodes[r, c].Reset();
            }
        }

        isComplete = false;
    }

    public IEnumerator DFSSearchRoutine() {
        yield return null;

        while (!isComplete) {
            if (frontierNodes.Count > 0) {
                Node currentNode = frontierNodes.Pop();
                iterations++;
                if (!exploreNodes.Contains(currentNode)) {
                    exploreNodes.Add(currentNode);
                }

                // add new bordering nodes to the stack
                foreach (Node n in currentNode.neighbors) {
                    if (n.nodeType != NodeType.blocked && !exploreNodes.Contains(n)) {
                        frontierNodes.Push(n);
                        n.prev = currentNode;
                    }
                }

                // check if the current node was found
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
            if (n.nodeType != NodeType.blocked && !exploreNodes.Contains(n)) {
                frontierNodes.Push(n);
                n.prev = node;
            }
        }
    }
}

