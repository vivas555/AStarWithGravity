using System.Collections;
using System.Collections.Generic;


namespace Core
{
    /// <summary>
    /// Classe des nodes peuplant la grille elle contient toutes les informations sur l'état, la valeur et la position des nodes
    /// </summary>
    /// <seealso cref="Node" />
    public class Node : IHeapItem<Node>
    {
        /// <summary>
        /// Si la node est traversable par le seeker
        /// </summary>
        public bool IsFlyable;
        /// <summary>
        /// La position dans l'espace de la node
        /// </summary>
        public Vector2 WorldPosition;
        /// <summary>
        /// À quelle distance est la node de la node de départ.
        /// </summary>
        public int GCost;
        /// <summary>
        /// À quelle distance est la node de la node d'arrivé.
        /// </summary>
        public int HCost;
        /// <summary>
        /// Sa position en X dans le tableau de node
        /// </summary>
        public int GridPositionX;
        /// <summary>
        /// Sa position en Y dans le tableau de node
        /// </summary>
        public int GridPositionY;
        /// <summary>
        /// Pile représentant les parents(Node par lesquels ont accéde au node courant) du node courant.
        /// </summary>
        public Stack<Node> Parents;
        /// <summary>
        /// Obtient ou pose la valeur de HeapIndex
        /// </summary>
        /// <value>
        /// La valeur de HeapIndex
        /// </value>
        public int HeapIndex { get; set; }
        /// <summary>
        /// Le nombe de node en sautant qu'on doit traverser pour se rendre au node courant
        /// </summary>
        public int JumpValue;
        /// <summary>
        /// Dans quel étant est le seeker quand il setrouve sur le node courant.
        /// </summary>
        public SeekerStatus SeekerStatusOnNode;
        /// <summary>
        /// Les valeurs de jumpValue qui ont déjà été utilisé sur le node courant.
        /// </summary>
        public ArrayList UsedJumpValues;
        /// <summary>
        /// Les autres états dans lesquelles s'est trouvé le seeker dans le node courant.
        /// </summary>
        public ArrayList PassedSeekerStatus;

        /// <summary>
        /// Instancie un node
        /// </summary>
        /// <param name="walkable">Obtient si le node est traversable par le seeker</param>
        /// <param name="worldposition">La position dans le monde où se trouve le node</param>
        /// <param name="gridposx">La position en X dans la grille où se trouve le node</param>
        /// <param name="gridposy">La position en Y dans la grille où se trouve le node</param>
        public Node(bool walkable, Vector2 worldposition, int gridposx, int gridposy)
        {
            IsFlyable = walkable;
            WorldPosition = worldposition;
            GridPositionX = gridposx;
            GridPositionY = gridposy;
            JumpValue = 0;
            Parents = new Stack<Node>();

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
        /// Compare deux nodes entre eux.
        /// </summary>
        /// <param name="nodeToCompare">L'autre node avec lequel comparé le node courant</param>
        /// <returns>Retourne 1 si le node courant est plus proche de la node cible. Sinon retourne -1</returns>
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
