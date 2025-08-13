using Pathfinding;
using UnityEngine;

public class DynamicGraphUpdater : MonoBehaviour
{
    public Transform player;
    public float repositionThreshold = 10f; // distancia mínima para mover el grafo
    private Vector3 lastPlayerPosition;

    void Start()
    {
        lastPlayerPosition = player.position;
    }

    void Update()
    {
        if (Vector3.Distance(player.position, lastPlayerPosition) > repositionThreshold)
        {
            var graph = AstarPath.active.data.gridGraph;
            graph.center = player.position;

            // Solo reescanea la parte visible
            AstarPath.active.Scan();

            lastPlayerPosition = player.position;
        }
    }
}
