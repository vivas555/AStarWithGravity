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
        private Core.Vector2 _gridWorldSize;
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
        private float _nodeDiameter;
        private Pathfinding _pathfinding;


        private Vector2 _worldBottomLeft;
        private Vector2 _targetPos;
        private Vector2 _seekerPos;



     private   void Start()
        {
            _worldBottomLeft = new Vector2();
            _targetPos = new Vector2();
            _seekerPos = new Vector2();

            _worldBottomLeft.X = transform.position.x - _gridWorldSize.X / 2;
            _worldBottomLeft.Y = transform.position.y - _gridWorldSize.Y / 2;

            _targetPos.X = _target.transform.position.x;
            _targetPos.Y = _target.transform.position.y;

            _seekerPos.X = _seeker.transform.position.x;
            _seekerPos.Y = _seeker.transform.position.y;

            UnityGridGenerator gridGenerator = new UnityGridGenerator();

            _nodeDiameter = nodeRadius * 2;

            _grid = new Grid(
                transform.position.x,
                transform.position.y,

                gridGenerator.CreateGrid((int)_gridWorldSize.X,
                (int)_gridWorldSize.Y,
                _unwalkable,
                _worldBottomLeft,
                nodeRadius,
                _nodeDiameter),
                _gridWorldSize,
                nodeRadius
                );

            _pathfinding = new Pathfinding(_grid,_maxJumpValue,_targetPos);
        }


        private void Update()
        {
            
                _pathfinding.FindPath(_seekerPos);
            
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(transform.position, new UnityEngine.Vector2(_gridWorldSize.X, _gridWorldSize.Y));


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

                    UnityEngine.Vector2 _cubePosition = new UnityEngine.Vector2(n.WorldPosition.X,n.WorldPosition.Y);

                    Gizmos.DrawCube(_cubePosition, Vector3.one * (_nodeDiameter - .1f));
                }
            }
        }

        public List<Node> GetPath()
        {
            return _grid.Path;
        }
    }
}
