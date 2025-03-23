using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Collections;

public class Pathfinder : MonoBehaviour {
    Node start;
    Node goal;

    Graph graph;
    GraphView graphView;

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

        for (int r = 0; r < graph.getWidth(); r++) {
            for (int c = 0; c < graph.getHeight(); c++) {
                this.graph.nodes[r, c].Reset();
            }
        }

        ShowColors(null, null, null);

        isComplete = false;
        iterations = 0;
    }

    public void ShowColors(List<Node> frontierNodes, List<Node> exploreNodes, List<Node> pathNodes) {
        if (graphView == null || start == null || goal == null) {
            return;
        }

        // resets the color of the open nodes so we can run another search without resetting the game
        // the walls never change, so we dont need to reset them
        graphView.ColorNodes(graph.paths, graphView.openColor);

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
        ShowColors(null, null, null);
        if (s == SearchType.BFS) {
            BFS bfs = ScriptableObject.CreateInstance<BFS>();
            bfs.Init(this, graph, start, goal);
            StartCoroutine(bfs.SearchRoutine());
        } else if (s == SearchType.DFS) {
            DFS dfs = ScriptableObject.CreateInstance<DFS>();
            dfs.Init(this, graph, start, goal);
            StartCoroutine(dfs.SearchRoutine());
        } else if (s == SearchType.Dijkstra) {
            Dijkstra d = ScriptableObject.CreateInstance<Dijkstra>();
            d.Init(this, graph, start, goal);
            StartCoroutine(d.SearchRoutine());
        } else if (s == SearchType.GreedyBest) {
            GreedyBest greedyBest = ScriptableObject.CreateInstance<GreedyBest>();
            greedyBest.Init(this, graph, start, goal);
            StartCoroutine(greedyBest.SearchRoutine());
        } else if (s == SearchType.AManhattan) {
            AStar a = ScriptableObject.CreateInstance<AStar>();
            a.Init(this, graph, start, goal, true);
            StartCoroutine(a.SearchRoutine());
        } else if (s == SearchType.AEuclidean) {
            AStar a = ScriptableObject.CreateInstance<AStar>();
            a.Init(this, graph, start, goal, false);
            StartCoroutine(a.SearchRoutine());
        }
    }



    public List<Node> GetPathNodes(Node goalNode) {
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
