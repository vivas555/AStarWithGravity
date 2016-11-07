namespace Core
{
    /// <summary>
    /// Interface permetant à la librairie d'être utilisé sous Unity et autre engins ou IDE.
    /// </summary>
    public interface IGridGenerator
    {
        Node[,] PopulateGrid(Grid gridToPopulate , int unwalkableMask
           );
    }
}
