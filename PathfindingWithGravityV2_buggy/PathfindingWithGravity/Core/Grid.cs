//Grid from the library Pathfinding With Gravity
//Copyright(C) Félix Rivard 2016

//Author: Felix Rivard 

//Contributors: Anthony Deschênes, Phillipe Tremblay, Alicia Lamontagne and Charles-Alexandre Lafond

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

using System;
using System.Collections.Generic;

namespace Core
{
    /// <summary>
    /// Classe qui gère la grille sur laquelle se base le module de pathfinding. 
    /// Elle contient le layer quiest définit comme unwalkable ainsi que le chemin trouvé
    /// </summary>
    public class Grid
    {
        /// <summary>
        /// Contient la taille de la grille en X et en Y
        /// </summary>
        public Vector2 GridWorldSize;
        /// <summary>
        /// Le rayon des nodes. Majoritairement le rayon du seeker
        /// </summary>
        public float NodeRadius;
        /// <summary>
        /// Le tableau de nodes qui forment la grille.
        /// </summary>
        public Node[,] grid;
        /// <summary>
        /// Liste de nodes contenant le chemin trouvé.
        /// </summary>
        public List<Node> Path;
        /// <summary>
        /// La position actuel du centre de la grille
        /// </summary>
        public Vector2 CurrentPos;
        private float _nodeDiameter;
        private  int _gridSizeX;
        private  int _gridSizeY;
        private  float _xOffset;
        private  float _yOffset;

        /// <summary>
        /// Retourne la taille de la grille en Y
        /// </summary>
        public int GridSizeY
        {
            get { return _gridSizeY; }
        }
        /// <summary>
        /// Retourne la taille de la grille en Y
        /// </summary>
        public int GridSizeX
        {
            get { return _gridSizeX; }
        }

        /// <summary>
        /// Obtient la taille maximale de la grille.
        /// </summary>
        public int MaxSize
        {
            get { return _gridSizeX * _gridSizeY; }
        }

        /// <summary>
        /// Initialise une instance de la grille avec les paramètres.
        /// </summary>
        /// <param name="currentPosX">La position de la grille en X dans le monde</param>
        /// <param name="currentPosY">La position de la grille en Y dans le monde</param>
        /// <param name="gridWorldSizeX">la taille de la grille en X</param>
        /// <param name="gridWorldSizeY">la taille de la grille en Y</param>
        /// <param name="nodeRadius">Le rayon des nodes dans la grille</param>
        public Grid(float currentPosX, float currentPosY, float gridWorldSizeX, float gridWorldSizeY, float nodeRadius)
        {
            Path = null;
            GridWorldSize.X = gridWorldSizeX;
            GridWorldSize.Y = gridWorldSizeY;
            NodeRadius = nodeRadius;
            CurrentPos = new Vector2
            {
                X = currentPosX,
                Y = currentPosY
            };
            _xOffset = currentPosX - GridWorldSize.X * 0.5f;
            _yOffset = currentPosY - GridWorldSize.Y * 0.5f;
            _nodeDiameter = NodeRadius * 2;
            _gridSizeX = (int)Math.Round(GridWorldSize.X / _nodeDiameter);
            _gridSizeY = (int)Math.Round(GridWorldSize.Y / _nodeDiameter);

        }

        /// <summary>
        /// Obtient le node qui contient la position dans le monde spécifié.
        /// </summary>
        /// <param name="worldPosition">La position dans le monde pour laquelle on veut connaître la node</param>
        public Node NodeFromWorldPoint(Vector2 worldPosition)
        {
            float xGridPos = worldPosition.X - _xOffset;
            float yGridPos = worldPosition.Y - _yOffset;

            float percentX = (xGridPos) / (GridWorldSize.X);
            float percentY = (yGridPos) / (GridWorldSize.Y);

            int x = (int)Math.Round((_gridSizeX - 1) * percentX);
            int y = (int)Math.Round((_gridSizeY - 1) * percentY);
            return grid[x, y];
        }

        /// <summary>
        /// Obtiens tous les nodes voisins dans lesquelles ils est possible de se rendre à partir du node demandé
        /// </summary>
        /// <param name="node">Le node du quel on veut connaître les nodes voisins</param>
        /// <param name="maxJumpValue">La valeur de saut maximale en node du seeker</param>
        /// <returns>Une liste de node contenant les voisins</returns>
        public List<Node> GetNeighborsWithGravity(Node node, int maxJumpValue)
        {
            List<Node> neighbours = new List<Node>();

            if (node.SeekerStatusOnNode == SeekerStatus.Jumping)
            {
                if (node.JumpValue < maxJumpValue)
                {
                    if (node.GridPositionY + 1 < _gridSizeY && grid[node.GridPositionX, node.GridPositionY + 1].IsFlyable)
                        GetBottomOrTopThree(ref neighbours, node, true);
                    if (node.GridPositionY - 1 >= 0 && grid[node.GridPositionX, node.GridPositionY - 1].IsFlyable)
                        GetBottomOrTopThree(ref neighbours, node, false);
                }
                else
                {
                    if (node.GridPositionY - 1 >= 0 && grid[node.GridPositionX, node.GridPositionY - 1].IsFlyable)
                        GetBottomOrTopThree(ref neighbours, node, false);
                }
            }
            else if (node.SeekerStatusOnNode == SeekerStatus.Falling)
            {
                if (node.GridPositionY - 1 >= 0 && grid[node.GridPositionX, node.GridPositionY - 1].IsFlyable)
                    GetBottomOrTopThree(ref neighbours, node, false);
            }
            else if (node.SeekerStatusOnNode == SeekerStatus.OnGround)
            {
                if (node.GridPositionY - 1 >= 0 && grid[node.GridPositionX, node.GridPositionY - 1].IsFlyable)
                    GetBottomOrTopThree(ref neighbours, node, false);
                GetBottomOrTopThree(ref neighbours, node, true);
                for (int i = -1; i <= 1; i += 2)
                {
                    int xValue = node.GridPositionX + i;
                    if (xValue < _gridSizeX && xValue >= 0)
                    {
                        Node nodeToAdd = grid[xValue, node.GridPositionY];
                        if (nodeToAdd.IsFlyable)
                        {
                            neighbours.Add(nodeToAdd);
                        }
                    }
                }
            }

            return neighbours;
        }

        private void GetBottomOrTopThree(ref List<Node> neighbours, Node currentNode, bool isTop)
        {
            int newYValue = currentNode.GridPositionY;


            if (isTop)
            {
                newYValue += 1;
            }
            else
            {
                newYValue -= 1;
            }


            for (int i = -1; i <= 1; i++)
            {
                int newXValue = currentNode.GridPositionX + i;
                if (newXValue < _gridSizeX && newXValue >= 0 && newYValue < _gridSizeY && newYValue >= 0)
                {
                    Node nodeToAdd = grid[newXValue, newYValue];
                    if (nodeToAdd.IsFlyable && grid[newXValue, currentNode.GridPositionY].IsFlyable)
                    {
                        neighbours.Add(nodeToAdd);
                    }
                }
            }
        }
    }
}
