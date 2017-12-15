﻿using UnityEngine;

public class DamageDealer : MonoBehaviour, IDamageDealer
{
    [SerializeField]
    private int _damage;

    private WorldManager _worldManager;
    private Health _myHealth;

    private PlayerMovement _playerMovement;
    private bool _canDoDamage;
    private bool _isPlayer;
    private AudioSource _hitSound;

    private void Start()
    {
        _worldManager = FindObjectOfType<WorldManager>();
        _myHealth = GetComponent<Health>();
        _playerMovement = GetComponent<PlayerMovement>();

        if (_playerMovement == null)
        {
            _canDoDamage = true;
            _isPlayer = false;
        }
        else
        {
            _isPlayer = true;
            _hitSound = GetComponent<AudioSource>();
        }
    }
    
    private void Update()
    {
        if (_isPlayer)
        {
            if (_playerMovement.GetIsDashing() || _playerMovement.GetIsSwinging())
            {
                _canDoDamage = true;
            }
            else
            {
                _canDoDamage = false;
            }
        }
    }
    
    private void DealDamage(Collider2D other)
    {
        if (!_canDoDamage)
        {
            return;
        }
        // Triggers do not hit eachother
        if (other.isTrigger)
        {
            return;
        }
        // Dead can not do damage
        if (_myHealth != null && _myHealth.GetIsDead())
        {
            return;
        }

        // Try to find others health component.
        Health health = other.gameObject.GetComponent<Health>();

        // If other has health decrease it.
        if (health != null)
        {
            if (_isPlayer)
            {
                GameObject POW = Instantiate(
                    _worldManager.GetPOW(), other.transform.position, Quaternion.identity);
                Destroy(POW, 0.25f);

                other.GetComponent<Rigidbody2D>().AddForce(
                    _playerMovement.GetTargetDirection() * 25,
                    ForceMode2D.Impulse);

                _hitSound.volume = Settings.Volume;
                _hitSound.Play();
            }
            
            MeleeAnimation meleeAnimation = GetComponent<MeleeAnimation>();
            if (meleeAnimation != null &&
                meleeAnimation.GetIsHitting() == false)
            {
                return;
            }

            health.SetKiller(gameObject);
            health.DecreaseHealth(GetDamage());
            if (health.GetIsDead() && tag == "Void")
            {
                health.gameObject.SetActive(false);
            }
        }
    }

    // Getters and setters

    public int GetDamage()
    {
        return _damage;
    }

    // Collider methods.

    private void OnTriggerEnter2D(Collider2D other)
    {
        DealDamage(other);
    }
    /*
    private void OnTriggerExit2D(Collider2D other)
    {
        DealDamage(other);
    }
    */
    private void OnCollisionEnter2D(Collision2D other)
    {
        DealDamage(other.collider);
    }
    /*
    private void OnCollisionExit2D(Collision2D other)
    {
        DealDamage(other.collider);
    }
    */
}
