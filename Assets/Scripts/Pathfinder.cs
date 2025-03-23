using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Collections;

public class Pathfinder : MonoBehaviour {
    Node start;
    Node goal;

    Graph graph;
    GraphView graphView;
    Queue<Node> frontierNodes;
    List<Node> exploreNodes;
    List<Node> pathNodes;

    public Color startColor = Color.green;
    public Color goalColor = Color.red;
    public Color frontierColor = Color.magenta;
    public Color exploreColor = Color.blue;
    public Color pathColor = Color.cyan;

    public bool isComplete;
    public int iterations;

    public float timeStep = 0.1f;


    public void Init(Graph graph, GraphView graphView, Node start, Node goal) {
        if (start == null || goal == null || graphView == null || graph == null) {
            Debug.LogWarning("Pathfinder error: Missing components.");
            return;
        }
        if (start.nodeType == NodeType.blocked || goal.nodeType == NodeType.blocked) {
            Debug.LogWarning("Pathfinder error: Make sure start and goal nodes are open");
            return;
        }

        this.graph = graph;
        this.graphView = graphView;
        this.start = start;
        this.goal = goal;
        frontierNodes = new Queue<Node>();
        frontierNodes.Enqueue(start);
        pathNodes = new List<Node>();
        exploreNodes = new List<Node>();

        for (int r = 0; r < graph.getWidth(); r++) {
            for (int c = 0; c < graph.getHeight(); c++) {
                this.graph.nodes[r, c].Reset();
            }
        }

        ShowColors(graphView, start, goal);

        isComplete = false;
        iterations = 0;
    }

    public void ShowColors(GraphView graphView, Node start, Node goal) {
        if (graphView == null || start == null || goal == null) {
            return;
        }

        NodeView startNodeView = graphView.nodeViews[start.xIndex, start.yIndex];
        NodeView goalNodeView = graphView.nodeViews[goal.xIndex, goal.yIndex];

        if (frontierNodes != null) {
            graphView.ColorNodes(frontierNodes.ToList(), frontierColor);
        }
        if (exploreNodes != null) {
            graphView.ColorNodes(exploreNodes, exploreColor);
        }
        if (pathNodes != null) {
            graphView.ColorNodes(pathNodes, pathColor);
        }

        if (startNodeView != null) {
            startNodeView.ColorNode(startColor);
        } else {
            Debug.LogWarning("StartNodeView does not exist");
        }
        if (goalNodeView != null) {
            goalNodeView.ColorNode(goalColor);
        } else {
            Debug.LogWarning("GoalNodeView does not exist");
        }
    }

    public void ChooseSearch(SearchType s) {
        if (s == SearchType.BFS) {
            StartCoroutine(BFSSearchRoutine(timeStep));
        } else if (s == SearchType.DFS) {
            StartCoroutine(DFSSearchRoutine(timeStep));
        }
    }

    public IEnumerator BFSSearchRoutine(float timeStep = 0.1f) {
        yield return null;
        while (!isComplete) {
            if (frontierNodes.Count > 0) {
                Node currentNode = frontierNodes.Dequeue();
                iterations++;
                if (!exploreNodes.Contains(currentNode)) {
                    exploreNodes.Add(currentNode);
                }
                ExpandFrontier(currentNode);
                if (frontierNodes.Contains(goal)) {
                    pathNodes = GetPathNodes(goal);
                    ShowColors(graphView, start, goal);
                    isComplete = true;
                }

                yield return new WaitForSeconds(timeStep);
            } else {
                isComplete = true;
            }
            ShowColors(graphView, start, goal);
        }
    }

    public IEnumerator DFSSearchRoutine(float timeStep = 0.1f) {
        yield return null;
        while (!isComplete) {
            if (frontierNodes.Count > 0) {
                Node currentNode = frontierNodes.Dequeue();
                if (!exploreNodes.Contains(currentNode)) {
                    exploreNodes.Add(currentNode);
                }
                DFS(currentNode);
                yield return new WaitForSeconds(timeStep);
            } else {
                isComplete = true;
            }
        }
        ShowColors(graphView, start, goal);
    }

    public IEnumerator DFS(Node node) {
        for (int i = 0; i < node.neighbors.Count; i++) {
            if (!(node.neighbors[i].nodeType == NodeType.blocked) && !exploreNodes.Contains(node.neighbors[i])) {
                node.neighbors[i].prev = node;
                if (!exploreNodes.Contains(node)) {
                    exploreNodes.Add(node);
                }
                yield return new WaitForSeconds(timeStep);
                StartCoroutine(DFS(node.neighbors[i]));
            }
        }
    }
    public void ExpandFrontier(Node node) {
        for (int i = 0; i < node.neighbors.Count; i++) {
            if (!exploreNodes.Contains(node.neighbors[i]) && !frontierNodes.Contains(node.neighbors[i])) {
                node.neighbors[i].prev = node;
                frontierNodes.Enqueue(node.neighbors[i]);
            }
        }
    }

    List<Node> GetPathNodes(Node goalNode) {
        List<Node> path = new List<Node>();
        if (goalNode == null) {
            return path;
        }
        path.Add(goalNode);
        Node currentNode = goalNode.prev;
        while (currentNode != null) {
            path.Insert(0, currentNode);
            currentNode = currentNode.prev;
        }
        return path;
    }
}
