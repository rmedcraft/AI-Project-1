using UnityEngine;

public class GameController : MonoBehaviour {
    public MapData mapData;
    public Graph graph;
    public Pathfinder pathfinder;
    public int startx = 0;
    public int starty = 0;
    public int goalx = 1;
    public int goaly = 1;
    public float timeStep = 0.1f;
    private GraphView graphView;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        if (mapData != null && graph != null) {
            int[,] mapInstance = mapData.MakeMap();
            graph.Init(mapInstance);
            graphView = graph.GetComponent<GraphView>();
            if (graphView != null) {
                graphView.Init(graph);
            } else {
                Debug.Log("No graph view is found");
            }
            if (graph.IsWithinBounds(startx, starty) && graph.IsWithinBounds(goalx, goaly) && pathfinder != null) {
                Node startNode = graph.nodes[startx, starty];
                Node goalNode = graph.nodes[goalx, goaly];
                pathfinder.Init(graph, graphView, startNode, goalNode);
            } else {
                Debug.LogWarning("GameController Error: start or end nodes are not in bounds");
            }
        }
    }

    public void BeginSearch() {
        StartCoroutine(pathfinder.SearchRoutine(timeStep));
    }

    // Update is called once per frame
    void Update() {

    }
}
