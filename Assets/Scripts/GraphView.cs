using System.Collections.Generic;
using UnityEngine;

public class GraphView : MonoBehaviour {
    public GameObject nodeViewPrefab;
    public Color openColor = Color.white;
    public Color blockedColor = Color.black;
    public NodeView[,] nodeViews;

    public void Init(Graph graph) {
        if (graph == null) {
            Debug.LogWarning("Graph does not exist u stupid idiot");
            return;
        }

        nodeViews = new NodeView[graph.getWidth(), graph.getHeight()];
        foreach (Node n in graph.nodes) {
            GameObject instance = Instantiate(nodeViewPrefab, Vector3.zero, Quaternion.identity);
            NodeView nodeView = instance.GetComponent<NodeView>();
            Debug.Log("Position: " + n.position.x + ", " + n.position.z);
            if (nodeView != null) {
                nodeView.Init(n);
                nodeViews[n.xIndex, n.yIndex] = nodeView;
                if (n.nodeType == NodeType.blocked) {
                    nodeView.ColorNode(blockedColor);
                } else {
                    nodeView.ColorNode(openColor);
                }
            }
        }
    }

    public void ColorNodes(List<Node> nodes, Color color) {
        foreach (Node n in nodes) {
            if (n != null && n.nodeType != NodeType.blocked) {
                NodeView nodeView = nodeViews[n.xIndex, n.yIndex];
                if (nodeView != null) {
                    nodeView.ColorNode(color);
                }
            }
        }
    }
}
