using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets;
using Core;
using Vector2 = UnityEngine.Vector2;

public class Move : MonoBehaviour
{
    [SerializeField]
    private float _speed;

    private GameObject _pathfindinGameObject;
    private List<Node> _path;

    private void Start()
    {
      _pathfindinGameObject = GameObject.Find("Pathfinding");
       
    }

    private void Update()
    {
        if (_path == null)
        {
            _path = _pathfindinGameObject.GetComponent<DrawGrid>().GetPath();
        }

        if (_path != null && _path.Count>0)
        {
            if (_path[0].SeekerStatusOnNode == SeekerStatus.Jumping)
            {
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }

            else if (_path[0].SeekerStatusOnNode == SeekerStatus.Jumping)
            {
                GetComponent<Rigidbody2D>().velocity = Vector2.down;
            }

            Vector3 targetPos = new Vector3(_path[0].WorldPosition.X, _path[0].WorldPosition.Y,0);

            GoTo(targetPos);
            if (Vector3.Distance(transform.position,targetPos) <= 0.5f )
            {
                _path.RemoveAt(0);
            }
        }

    }

    private void GoTo(Vector3 worldPositionOfTarget)
    {
        Vector3 mePosition = transform.position;

        Vector3 moveTo = worldPositionOfTarget - mePosition;

        moveTo.Normalize();

        transform.Translate(_speed * moveTo);
    }

}
