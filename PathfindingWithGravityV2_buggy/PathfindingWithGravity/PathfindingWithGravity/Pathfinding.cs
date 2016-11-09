//SeekerStatus from the library Pathfinding With Gravity
//Copyright(C) Félix Rivard 2016

//Author: Felix Rivard and Anthony Deschênes

//Contributors: Phillipe Tremblay, Alicia Lamontagne and Charles-Alexandre Lafond

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
using System.Collections;
using System.Security.Principal;
using Core;

namespace PathfindingWithGravity
{
    /// <summary>
    /// Classe logique de la librairie. Fait la recherche de chemin selon les paramètres spécifiés.
    /// </summary>
    public class Pathfinding
    {
        private Grid _grid;
        private int _maxJumpValue = 3;
        private Vector2 _target;

        private Heap<Node> _openSet;
        private List<Node> _closeSet;


        /// <summary>
        /// Instancie un object logique pathfinding.
        /// </summary>
        /// <param name="grid">La grille sur laquelle se baser pour trouver le chemin et dans la quel peuplé le chemin</param>
        /// <param name="maxJumpValue">La valeur de saut maximale que le seeker peut faire en node</param>
        /// <param name="targetPosX">La position dans le monde en X de la cible</param>
        /// <param name="targetPosY">La position dans le monde en Y de la cible.</param>
        public Pathfinding(Grid grid, int maxJumpValue, float targetPosX,float targetPosY)
        {
            _grid = grid;
            _maxJumpValue = maxJumpValue;
            _target.X = targetPosX;
            _target.Y = targetPosY;
        }

        /// <summary>
        /// Trouve le chemin et peuple la grille avec.
        /// </summary>
        /// <param name="XstartPosition">La position en X du seeker</param>
        /// <param name="YstartPosition">La position en Y du seeker</param>
        public void FindPath(float XstartPosition, float YstartPosition)
        {

            Vector2 startPosition = new Vector2();
            startPosition.X = XstartPosition;
            startPosition.Y = YstartPosition;

            if (_openSet != null)
            {
                ResetNodes();
            }
         
            Node startNode = _grid.NodeFromWorldPoint(startPosition);
            if (startNode.SeekerStatusOnNode != SeekerStatus.Jumping && startNode.GridPositionY - 1 >= 0 &&
                _grid.grid[startNode.GridPositionX, startNode.GridPositionY - 1].IsFlyable)
            {
                startNode.SeekerStatusOnNode = SeekerStatus.Falling;
                startNode.PassedSeekerStatus.Add(SeekerStatus.Falling);
            }
            else
            {
                startNode.SeekerStatusOnNode = SeekerStatus.OnGround;
                startNode.PassedSeekerStatus.Add(SeekerStatus.OnGround);
            }

            Node targetNode = _grid.NodeFromWorldPoint(_target);

            _openSet = new Heap<Node>(_grid.MaxSize);
            _closeSet = new List<Node>();
            _openSet.Add(startNode);
            int i = 0;
            while (_openSet.Count > 0)
            {

                i++;
                if (i > 400)
                {
                    break;
                }
                Node currentNode = _openSet.RemoveFirst();

                _closeSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    RetracePath(startNode, targetNode);
                    break;
                }

                foreach (Node neighbour in _grid.GetNeighborsWithGravity(currentNode, _maxJumpValue))
                {
                    if (_closeSet.Contains(neighbour))
                    {
                        if (neighbour.GridPositionY > currentNode.GridPositionY)
                        {
                            if (neighbour.PassedSeekerStatus.Contains(SeekerStatus.Falling))
                            {
                                continue;
                            }

                            if (neighbour.PassedSeekerStatus.Contains(SeekerStatus.Jumping))
                            {
                                int foresawJumpValue = currentNode.JumpValue + 1;
                                bool canGo = true;

                                foreach (int usedJumpValues in neighbour.UsedJumpValues)
                                {
                                    if (foresawJumpValue >= usedJumpValues)
                                    {
                                        canGo = false;
                                        break;
                                    }
                                }

                                if (canGo == false)
                                {
                                    continue;
                                }
                                else
                                {
                                    _closeSet.Remove(neighbour);
                                }
                            }
                            else
                            {
                                _closeSet.Remove(neighbour);
                            }

                        }

                        else
                        {
                            continue;
                        }
                    }

                    int newMoveCostToNeighbours = currentNode.GCost + getDistance(currentNode, neighbour);
                    if (newMoveCostToNeighbours < neighbour.GCost || !_openSet.Contains(neighbour))
                    {
                        neighbour.GCost = newMoveCostToNeighbours;
                        neighbour.HCost = getDistance(neighbour, targetNode);
                        if (!neighbour.Parent.Contains(currentNode))
                        {

                            neighbour.Parent.Push(currentNode);

                            
                            if (neighbour.Parent.Count >= 3)
                            {
                                neighbour.Parent.Pop();
                            }
                        }

                        if (neighbour.GridPositionY > currentNode.GridPositionY)
                        {
                            neighbour.JumpValue = currentNode.JumpValue + 1;
                            neighbour.SeekerStatusOnNode = SeekerStatus.Jumping;
                            neighbour.PassedSeekerStatus.Add(SeekerStatus.Jumping);

                            if (neighbour.UsedJumpValues.Contains(neighbour.JumpValue) == false)
                            {
                                neighbour.UsedJumpValues.Add(neighbour.JumpValue);
                            }
                        }
                        else if (neighbour.GridPositionY - 1 > 0 &&
                                 _grid.grid[neighbour.GridPositionX, neighbour.GridPositionY - 1].IsFlyable == false)
                        {
                            if (neighbour.PassedSeekerStatus.Contains(SeekerStatus.OnGround) == false)
                            {
                                neighbour.SeekerStatusOnNode = SeekerStatus.OnGround;
                            }
                            neighbour.PassedSeekerStatus.Add(SeekerStatus.OnGround);
                        }
                        else
                        {
                            neighbour.SeekerStatusOnNode = SeekerStatus.Falling;
                            if (neighbour.PassedSeekerStatus.Contains(SeekerStatus.Falling) == false)
                            {
                                neighbour.PassedSeekerStatus.Add(SeekerStatus.Falling);
                            }
                            neighbour.JumpValue = 0;
                        }


                        if (!_openSet.Contains(neighbour))
                        {
                            _openSet.Add(neighbour);

                        }
                        else
                        {
                            _openSet.UpdateItem(neighbour);
                        }
                    }
                }
            }
        }

       private void RetracePath(Node startNode, Node endNode)
        {
            List<Node> path = new List<Node>();
            Node currentNode = endNode;
            Node lastParent = null;
            while (currentNode != startNode)
            {
                path.Add(currentNode);
                if (currentNode.Parent.Count <= 0)
                {
                    currentNode.Parent.Push(lastParent);
                }
                lastParent = currentNode;
                currentNode = currentNode.Parent.Pop();
            }

            path.Reverse();

            _grid.Path = path;
        }

        private void ResetNodes()
        {
            if (_openSet != null)
            {
                while (_openSet.Count > 0)
                {
                    Node nodeToReset = _openSet.RemoveFirst();
                    ResetNode(nodeToReset);
                }

                while (_closeSet.Count > 0)
                {
                    Node nodeToReset = _closeSet[0];
                    ResetNode(nodeToReset);
                    _closeSet.Remove(nodeToReset);
                }
            }
        }

        private void ResetNode(Node nodeToReset)
        {
            nodeToReset.SeekerStatusOnNode = SeekerStatus.Default;
            nodeToReset.JumpValue = 0;
            nodeToReset.Parent = new Stack<Node>();

            nodeToReset.UsedJumpValues = new ArrayList();
            nodeToReset.PassedSeekerStatus = new ArrayList();
        }

       private int getDistance(Node nodeA, Node nodeB)
        {
            int distanceX = Math.Abs(nodeA.GridPositionX - nodeB.GridPositionX);
            int distanceY = Math.Abs(nodeA.GridPositionY - nodeB.GridPositionY);

            if (distanceX > distanceY)
            {
                return 14 * distanceY + 10 * (distanceX - distanceY);
            }

            else
            {
                return 14 * distanceX + 10 * (distanceY - distanceX);
            }
        }
    }
}
