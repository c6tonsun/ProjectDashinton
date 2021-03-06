﻿using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class EnemyMovement : MonoBehaviour {

    [SerializeField, Range(0f, 50f)]
    private float _movementSpeed;

    [SerializeField]
    private List<Transform> _walkPath;
    [SerializeField, Tooltip("Unchecked for back and forth. Checked for loop.")]
    private bool _loopWalkPath;

    private Rigidbody2D _rb;
	private SpriteRenderer _spriteRenderer;

    private Vector3 _targetPosition;
    private Vector3 _targetDirection;
    private bool _targetReached;

    private int _walkPathIndex;
    private bool _growIndex = true;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
		_spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        _targetReached = true;
        _walkPathIndex = 0;
    }

	private void Update()
	{

		// Flips
		if (_rb.velocity.x == 0) 
		{
			// do not flip
		} 
		else if (_rb.velocity.x > 0)
		{
			_spriteRenderer.flipX = true;
		}
		else
		{
			_spriteRenderer.flipX = false;
		}
	}

    private void FixedUpdate()
    {
        if (_targetReached)
        {
            SetNewTarget();
            Dash();
            _targetReached = false;
        }

        StopMovementCheck();
    }

    // Sets new target from list.
    private void SetNewTarget()
    {
        _targetPosition = _walkPath[_walkPathIndex].position;
        _targetDirection = _targetPosition - transform.position;
        _targetDirection.Normalize();

        // If path loops
        if (_loopWalkPath)
        {
            _walkPathIndex++;

            if (_walkPathIndex == _walkPath.Count)
            {
                _walkPathIndex = 0;
            }
        }
        // If path goes back and forth.
        else
        {
            // Goes forth
            if (_growIndex)
            {
                _walkPathIndex++;

                if (_walkPathIndex == _walkPath.Count -1)
                {
                    _growIndex = false;
                }
            }
            // Goes back.
            else
            {
                _walkPathIndex--;
                
                if (_walkPathIndex == 0)
                {
                    _growIndex = true;
                }
            }

        }
    }

    // Starts dash towards target position.
    private void Dash()
    {
        _rb.AddForce(_targetDirection * _movementSpeed, ForceMode2D.Impulse);
    }

    // Checks if we reached target.
    private void StopMovementCheck()
    {
        // Where we are on this fixed frame
        Vector3 currentPosition = transform.position;
        float currentToTarget = Vector3.Distance(currentPosition, _targetPosition);

        // Where we could be on the next fixed update
        Vector3 nextPosition = currentPosition +
            _targetDirection * _movementSpeed * Time.fixedDeltaTime;
        float currentToNext = Vector3.Distance(currentPosition, nextPosition);
        
        // Set transform and stop rigidbody.
        if (currentToTarget < currentToNext)
        {
            transform.position = _targetPosition;
            _rb.velocity = Vector3.zero;
            _targetReached = true;
        }
    }

    // Getters and setters

    public void Stop()
    {
        _rb.velocity = Vector2.zero;
    }
}
