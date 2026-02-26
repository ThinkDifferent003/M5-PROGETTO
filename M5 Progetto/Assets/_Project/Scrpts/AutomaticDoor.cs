using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AutomaticDoor : MonoBehaviour
{
    [SerializeField] private GameObject _door;
    [SerializeField] private float _speedOpen = 5f;
    [SerializeField] private float _heightOpen = 7f;
    private Vector3 _closePos;
    private Vector3 _openPos;
    private bool _isOpen = false;
    
    void Start()
    {
        if (_door != null)
        {
            _closePos = _door.transform.position;
            _openPos = _closePos + Vector3.up * _heightOpen;
        }
    }

    
    void Update()
    {
        Vector3 target = _isOpen ? _openPos : _closePos;
        _door.transform.position = Vector3.MoveTowards(_door.transform.position, target, _speedOpen * Time.deltaTime );
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isOpen = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _isOpen = false;
    }
}
