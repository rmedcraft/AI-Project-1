using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AStar : ScriptableObject {
    Node goal;

    Graph graph;
    Pathfinder pathfinder;
    List<Node> exploreNodes;
    Queue<Node> frontierNodes;
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

        frontierNodes = new Queue<Node>();
        frontierNodes.Enqueue(start);
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
    }
}