namespace Core
{
    /// <summary>
    /// Interface permetant à la librairie d'être utilisé sous Unity et autre engins ou IDE.
    /// </summary>
    public interface IGridGenerator
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
        /// <returns></returns>
        Node[,] CreateGrid(int gridSizeX, int gridSizeY, int unwalkableMask, Vector2 worldBottomLeft,
           float nodeRadius, float nodeDiameter);
    }
}
