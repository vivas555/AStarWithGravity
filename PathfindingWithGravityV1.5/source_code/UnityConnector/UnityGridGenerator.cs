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
        /// Crée un tableau de node contenue dans une grille
        /// </summary>
        /// <param name="gridSizeX">La taille de la grille en X</param>
        /// <param name="gridSizeY">La taille de la grille en Y</param>
        /// <param name="unwalkableMask">Le layer a concidérer comme unwalkable</param>
        /// <param name="worldBottomLeft">Le coin en bas à gauche de la grille</param>
        /// <param name="nodeRadius">La rayon des nodes</param>
        /// <param name="nodeDiameter">Le diamètres des nodes.</param>
        /// <returns>Un tableau 2D de nodes à insérer dans un object de type Grid dans l'attribue grid</returns>
        public Node[,] CreateGrid(int gridSizeX, int gridSizeY, int unwalkableMask, Vector2 worldBottomLeft, float nodeRadius, float nodeDiameter)
        {
            Node[,] grid = new Node[gridSizeX, gridSizeY];

            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    Vector2 worldPoint = new Vector2
                    {
                        X = worldBottomLeft.X + (x * nodeDiameter + nodeRadius),
                        Y = worldBottomLeft.Y+ (y * nodeDiameter + nodeRadius)
                    };

                    UnityEngine.Vector2 worldPointUnity = new UnityEngine.Vector2(worldPoint.X, worldPoint.Y);
                    bool walkable = !(Physics2D.OverlapCircle(worldPointUnity, nodeRadius, unwalkableMask));
                    grid[x, y] = new Node(walkable, worldPoint, x, y);
                }
            }
            return grid;
        }
    }
}
