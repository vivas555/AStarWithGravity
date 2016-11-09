//SeekerStatus from the library Pathfinding With Gravity
//Copyright(C) Félix Rivard 2016

//Author: Felix Rivard

//Contributors: Anthony Deschênes,Phillipe Tremblay, Alicia Lamontagne and Charles-Alexandre Lafond

//Year 2016

//This program is free software; you can redistribute it and/or modify
//it under the terms of the GNU General Public License as published by
//the Free Software Foundation; either version 3 of the License, or
//(at your option) any later version.

//This program is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
//GNU General Public License for more details.

//You should have received a copy of the GNU General Public License
//along with this program; if not, write to the Free Software Foundation,
//Inc., 51 Franklin Street, Fifth Floor, Boston, MA 02110-1301  USA

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
        /// Remplit la grille de noeuds.
        /// </summary>
        /// <param name="gridToPopulate">La grille à remplir</param>
        /// <param name="unwalkableMask">Le layer à considérer comme intraversable par le seeker</param>
        public Node[,] PopulateGrid(Grid gridToPopulate, int unwalkableMask)
        {
            Vector2 worldBottomLeft = new Vector2();
            unwalkableMask = 1 << unwalkableMask;

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
