using System;
using UnityEngine;

namespace Game
{
  public class CameraHelper : MonoBehaviour
  {
    [SerializeField]
    private float speed;

    private Transform _target;

    private void Start()
    {
      _target = transform.parent;
      if (!_target)
      {
        throw new ArgumentException($"Cant get target for CameraHelper {gameObject.name}");
      }
      transform.parent = null;
    }

    private void Update()
    {
      if (_target)
      {
        transform.position = Vector3.MoveTowards(transform.position, _target.position, speed * Time.deltaTime);
      }
    }
  }
}
