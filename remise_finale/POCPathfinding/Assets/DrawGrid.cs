using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Core;
using PathfindingWithGravity;
using UnityConnector;
using Vector2 = Core.Vector2;

namespace Assets
{
    public class DrawGrid : MonoBehaviour
    {
        [SerializeField]
        private float _gridWorldSizeX;
        [SerializeField]
        private float _gridWorldSizeY;
        [SerializeField]
        private GameObject _target;
        [SerializeField]
        private GameObject _seeker;
        [SerializeField]
        private float nodeRadius;
        [SerializeField]
        private LayerMask _unwalkable;
        [SerializeField]
        private int _maxJumpValue;

        private Grid _grid;
        private Pathfinding _pathfinding;

     private  void Start()
        {
            UnityGridGenerator gridGenerator = new UnityGridGenerator();

            _grid = new Grid(transform.position.x,transform.position.y,_gridWorldSizeX,_gridWorldSizeY,nodeRadius);
           _grid.grid = gridGenerator.PopulateGrid(_grid,LayerMask.NameToLayer("Water"));
            _pathfinding = new Pathfinding(_grid, _maxJumpValue, _target.transform.position.x,
            _target.transform.position.y);
        }


        private void FixedUpdate()
        {

            _pathfinding.FindPath(_seeker.transform.position.x, _seeker.transform.position.y);

        }


        public List<Node> GetPath()
        {
            if (_grid != null)
            {
                return _grid.Path;
            }
            return null;
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(transform.position, new UnityEngine.Vector2(_gridWorldSizeX, _gridWorldSizeY));
            float _nodeDiameter = nodeRadius*2;

            if (_grid != null)
            {
                foreach (Node n in _grid.grid)
                {
                    Color color = (n.IsFlyable) ? Color.white : Color.red;

                    if (_grid.Path != null)
                    {
                        if (_grid.Path.Contains(n))
                        {
                            color = Color.black;
                        }
                    }
                    color.a = 0.5f;

                    Gizmos.color = color;

                    UnityEngine.Vector2 _cubePosition = new UnityEngine.Vector2(n.WorldPosition.X, n.WorldPosition.Y);

                    Gizmos.DrawCube(_cubePosition, Vector3.one * (_nodeDiameter - .1f));
                }
            }
        }
    }
}
