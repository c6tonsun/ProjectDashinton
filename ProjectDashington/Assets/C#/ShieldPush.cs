﻿using UnityEngine;

public class ShieldPush : MonoBehaviour {

    [SerializeField]
    private float _pushDistance;
    [SerializeField, Range(0f, 1f), Tooltip(
        "0 = pushes player backwards." +
        "1 = pushes player toward target transform.")]
    private float _pushAngle;
    [SerializeField]
    private float _pushForce;

    [SerializeField]
    private Transform _targetTransform;
    [SerializeField, Tooltip(
        "True = uses shield's pivotpoint." +
        "False = uses enemy's pivotpoint.")]
    private bool _useShieldPivot;

    private Vector3 _pivotPos;
    private Vector3 _playerPos;
    private GameObject _player;

    private void Start()
    {
        if (_useShieldPivot)
        {
            _pivotPos = gameObject.transform.position;
        }
        else
        {
            _pivotPos = gameObject.transform.parent.position;
        }
    }

    private Vector3 CalculateDirectionVector3()
    {
        Vector3 playerDirection = _playerPos - _pivotPos;
        playerDirection.Normalize();

        Vector3 targetDirection = _targetTransform.position - _pivotPos;
        targetDirection.Normalize();

        return Vector3.Lerp(playerDirection, targetDirection, _pushAngle);
    }

    // Collider methods.

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name == "Player")
        {
            _player = other.gameObject;
            _playerPos = _player.transform.position;

            Vector3 directionVector3 = CalculateDirectionVector3();
            directionVector3.Normalize();

            PlayerMovement playerMovement = _player.GetComponent<PlayerMovement>();
            playerMovement.Pushed(directionVector3, _pushDistance, _pushForce);
        }
    }
}
