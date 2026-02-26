using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.Properties;
using UnityEngine;

public class Button : MonoBehaviour
{
    [SerializeField] private GameObject _door;
    [SerializeField] private GameObject _ui;

    private bool _isPlayerInside = false;
    private bool _isOpened = false;

    private void Start()
    {
        if (_ui != null) _ui.SetActive(false);
    }

    private void Update()
    {
        if (_isPlayerInside && !_isOpened && Input.GetKeyDown(KeyCode.E))
        {
            OpenDoor();
        }
    }

    private void OpenDoor()
    {
        _isOpened = true;
        _ui.SetActive(false);
        Destroy(_door);
        Debug.Log($"Una porta si è aperta");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !_isOpened)
        {
            _isPlayerInside = true;
            if (_ui != null) _ui.SetActive(true);
        }
    }

    private void OnTriggerExit (Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerInside = false;
            if (_ui != null) _ui.SetActive(false);
        }
    }
}
