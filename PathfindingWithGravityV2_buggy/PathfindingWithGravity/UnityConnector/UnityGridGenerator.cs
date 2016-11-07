using Core;
using UnityEngine;
using Vector2 = Core.Vector2;

namespace UnityConnector
{
    /// <summary>
    /// Permet la génération d'une grille à partir de Unity.
    /// </summary>
    /// <seealso cref="Core.IGridGenerator" />
    public class UnityGridGenerator : IGridGenerator
    {
        /// <summary>
        /// Remplis la grille de nodes.
        /// </summary>
        /// <param name="gridToPopulate">La grille à remplir</param>
        /// <param name="unwalkableMask">Le layer à concidérer comme intraversable par le seeker</param>
        public Node[,] PopulateGrid(Grid gridToPopulate, int unwalkableMask)
        {
            Vector2 worldBottomLeft = new Vector2();

            worldBottomLeft.X = gridToPopulate.CurrentPos.X - gridToPopulate.GridWorldSize.X / 2;
            worldBottomLeft.Y = gridToPopulate.CurrentPos.Y - gridToPopulate.GridWorldSize.Y / 2;

            float nodeDiameter = gridToPopulate.NodeRadius*2;

            Node[,] grid = new Node[gridToPopulate.GridSizeX, gridToPopulate.GridSizeY];

            for (int x = 0; x < gridToPopulate.GridSizeX; x++)
            {
                for (int y = 0; y < gridToPopulate.GridSizeY; y++)
                {
                    Vector2 worldPoint = new Vector2
                    {
                        X = worldBottomLeft.X + (x * nodeDiameter + gridToPopulate.NodeRadius),
                        Y = worldBottomLeft.Y+ (y * nodeDiameter + gridToPopulate.NodeRadius)
                    };

                    UnityEngine.Vector2 worldPointUnity = new UnityEngine.Vector2(worldPoint.X, worldPoint.Y);
                    bool walkable = !(Physics2D.OverlapCircle(worldPointUnity, gridToPopulate.NodeRadius, unwalkableMask));
                    grid[x, y] = new Node(walkable, worldPoint, x, y);
                    
                }
            }
            return grid;
        }
    }
}
