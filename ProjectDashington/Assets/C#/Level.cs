﻿using UnityEngine;

public class Level : MonoBehaviour {

    [SerializeField]
    private int _star;

    private int _levelNumber;

    private Transform[] _children;
    private GameObject[] _resetables;
    private Vector3[] _defaultPositions;
    private Vector3[] _defaultRotations;

    private bool ignore = true;

    private void Awake()
    {
        CreateLevelNumber();

        FindResetables();
    }

    private void OnEnable()
    {
        ResetDiables();
        EnableAllChildren();
    }

    private void OnDisable()
    {
        for (int i = 0; i < _children.Length; i++)
        {
            _children[i].gameObject.SetActive(false);
        }
    }

    private void CreateLevelNumber()
    {
        char[] chars = name.ToCharArray();

        foreach (char c in chars)
        {
            // if number (0-9)
            if (c >= 48 && c <= 57)
            {
                // tens, hundreds...
                if (_levelNumber != 0)
                {
                    _levelNumber *= 10;
                }
                _levelNumber += c - 48;
            }
        }
    }

    private void FindResetables()
    {
        _children = GetComponentsInChildren<Transform>(true);

        _resetables = new GameObject[_children.Length];
        _defaultPositions = new Vector3[_children.Length];
        _defaultRotations = new Vector3[_children.Length];

        for (int i = 0; i < _children.Length; i++)
        {
            _resetables.SetValue(_children[i].gameObject, i);
            _defaultPositions.SetValue(_children[i].transform.position, i);
            _defaultRotations.SetValue(_children[i].transform.localEulerAngles, i);
        }
    }

    private void EnableAllChildren()
    {
        for (int i = 0; i < _children.Length; i++)
        {
            _children[i].gameObject.SetActive(true);
            EnableChildsComponents(_children[i]);
        }
    }

    private void EnableChildsComponents(Transform child)
    {
        EnemyMovement enemyMovement = child.GetComponent<EnemyMovement>();
        if (enemyMovement != null)
        {
            enemyMovement.enabled = true;
        }

        JumpMovement jumpMovement = child.GetComponent<JumpMovement>();
        if (jumpMovement != null)
        {
            jumpMovement.enabled = true;
        }

        Health health = child.GetComponent<Health>();
        if (health != null)
        {
            health.enabled = true;
        }

        RangeSingle rangeSingle = child.GetComponent<RangeSingle>();
        if (rangeSingle != null)
        {
            rangeSingle.enabled = true;
        }

        RangeAOE rangeAOE = child.GetComponent<RangeAOE>();
        if (rangeAOE != null)
        {
            rangeAOE.enabled = true;
        }
    }

    private void ResetDiables()
    {
        for (int i = 0; i < _resetables.Length; i++)
        {
            _resetables[i].transform.position = _defaultPositions[i];
            _resetables[i].transform.localEulerAngles = _defaultRotations[i];
        }
    }

    // Getters

    public int GetLevelNumber()
    {
        return _levelNumber;
    }

    public int GetStarValue()
    {
        return _star;
    }
}
