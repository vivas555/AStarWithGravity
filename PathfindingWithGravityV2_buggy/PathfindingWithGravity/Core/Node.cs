//Node from the library Pathfinding With Gravity
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

using System.Collections;
using System.Collections.Generic;


namespace Core
{
    /// <summary>
    /// Classe node peuplant la grille, elle contient toutes les informations sur l'état, la valeur et la position des noeuds
    /// </summary>
    /// <seealso cref="Node" />
    public class Node : IHeapItem<Node>
    {
        /// <summary>
        /// Si le noeud est traversable par le seeker
        /// </summary>
        public readonly bool IsFlyable;
        /// <summary>
        /// La position dans l'espace de le noeud
        /// </summary>
        public Vector2 WorldPosition;
        /// <summary>
        /// À combien de distance est le noeud de le noeud de départ.
        /// </summary>
        public int GCost;
        /// <summary>
        /// À quelle distance est le noeud du noeud d'arrivée.
        /// </summary>
        public int HCost;
        /// <summary>
        /// Sa position en X dans le tableau de noeud
        /// </summary>
        public readonly int GridPositionX;
        /// <summary>
        /// Sa position en Y dans le tableau de noeud
        /// </summary>
        public readonly int GridPositionY;
        /// <summary>
        /// Pile représentant les parents(Noeuds par lesquels on accède au noeud courant) du noeud courant.
        /// </summary>
        public Stack<Node> Parent;
        /// <summary>
        /// Obtient ou pose la valeur de HeapIndex
        /// </summary>
        /// <value>
        /// La valeur de HeapIndex
        /// </value>
        public int HeapIndex { get; set; }

        /// <summary>
        /// Le nombre de noeuds en sautant qu'on doit traverser pour se rendre au noeud courant
        /// </summary>
        public int JumpValue;
        /// <summary>
        /// Dans quel état est le seeker quand il setrouve sur le noeud courant.
        /// </summary>
        public SeekerStatus SeekerStatusOnNode;

        /// <summary>
        /// Les valeurs de jumpValue qui ont déjà été utilisées sur le noeud courant.
        /// </summary>
        public ArrayList UsedJumpValues;
        /// <summary>
        /// Les autres états dans lesquels s'est trouvé le seeker dans le noeud courant.
        /// </summary>
        public ArrayList PassedSeekerStatus;

        /// <summary>
        /// Instancie un node
        /// </summary>
        /// <param name="walkable">Obtient si le noeud est traversable par le seeker</param>
        /// <param name="worldposition">La position dans le monde où se trouve le noeud</param>
        /// <param name="gridposx">La position en X dans la grille où se trouve le noeud</param>
        /// <param name="gridposy">La position en Y dans la grille où se trouve le noeud</param>
        public Node(bool walkable, Vector2 worldposition, int gridposx, int gridposy)
        {
            IsFlyable = walkable;
            WorldPosition = worldposition;
            GridPositionX = gridposx;
            GridPositionY = gridposy;
            JumpValue = 0;
            Parent = new Stack<Node>();

            UsedJumpValues = new ArrayList();
            PassedSeekerStatus = new ArrayList();
            SeekerStatusOnNode = SeekerStatus.Default;
        }

        /// <summary>
        /// Obtient le fcost qui est l'addition du H et du G cost
        /// </summary>
        /// <value>
        /// La valeur du fCost.
        /// </value>
        public int fCost
        {
            get
            {
                return GCost + HCost;
            }
        }

        /// <summary>
        /// Compare deux noeuds entre eux.
        /// </summary>
        /// <param name="nodeToCompare">L'autre noeud avec lequel est comparé le noeud courant</param>
        /// <returns>Retourne 1 si le noeud courant est plus proche de la node cible. Sinon retourne -1</returns>
        public int CompareTo(Node nodeToCompare)
        {
            int compare = fCost.CompareTo(nodeToCompare.fCost);

            if (compare == 0)
            {
                compare = HCost.CompareTo(nodeToCompare.HCost);
            }

            return -compare;
        }


    }
}
