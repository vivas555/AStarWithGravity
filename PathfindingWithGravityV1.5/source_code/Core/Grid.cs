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
        /// Le layer que le seeker va concidérer comme unwalkable
        /// </summary>
        public int UnwalkableMask;
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
        private int _gridSizeX;
        private int _gridSizeY;
        private int _maxSize;
        private Vector2 _worldBottomLeft;
        private float _xOffset;
        private float _yOffset;

        //TODO Mettre public
        private bool hasGravity = false;

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
        /// <param name="gridWorldSize">la taille de la grille en X et en Y</param>
        /// <param name="nodeRadius">Le rayon des nodes dans la grille</param>
        /// <param name="grid">Letableau de Node contenant les informations de la grille</param>
        public Grid(float currentPosX, float currentPosY, Node[,] grid, Vector2 gridWorldSize, float nodeRadius )
        {
            Path = null;

            GridWorldSize = gridWorldSize;
            NodeRadius = nodeRadius;

            this.grid = grid;
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
                //On a atteint le maximum de la hauteur de saut, on ne donne accès qu'aux 3 nodes d'en dessous
                else
                {
                    if (node.GridPositionY - 1 >= 0 && grid[node.GridPositionX, node.GridPositionY - 1].IsFlyable)
                        GetBottomOrTopThree(ref neighbours, node, false);
                }
            }
            //On tombe, on ne peut accéder que les trois noeud d'en dessous.
            else if (node.SeekerStatusOnNode == SeekerStatus.Falling)
            {
                //UnityEngine.Debug.Log("Is falling");
                if (node.GridPositionY - 1 >= 0 && grid[node.GridPositionX, node.GridPositionY - 1].IsFlyable)
                    GetBottomOrTopThree(ref neighbours, node, false);
            }
            //Sinon on est déjà sur un sol, on peut aller dans toutes les directions
            else if (node.SeekerStatusOnNode == SeekerStatus.OnGround)
            {
                if (node.GridPositionY - 1 >= 0 && grid[node.GridPositionX, node.GridPositionY - 1].IsFlyable)
                    GetBottomOrTopThree(ref neighbours, node, false);
                GetBottomOrTopThree(ref neighbours, node, true);
                //Obtention des noeuds de gauche et de droite
                for (int i = -1; i <= 1; i += 2)
                {
                    int xValue = node.GridPositionX + i;
                    if (xValue < _gridSizeX && xValue >= 0)
                    {
                        Node nodeToAdd = grid[xValue, node.GridPositionY];
                        //S'il n'est pas isFlyable, cela veut dire que c'est un mur ou un floor, donc on l'ajoute
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
                    //S'il n'est pas isFlyable, cela veut dire que c'est un mur ou un floor, donc on l'ajoute
                    if (nodeToAdd.IsFlyable && grid[newXValue, currentNode.GridPositionY].IsFlyable)
                    {
                        neighbours.Add(nodeToAdd);
                    }
                }
            }
        }
    }
}
