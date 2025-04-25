using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : MonoBehaviour
{
    int playerLayer;

    public GameObject ExplosionVFXPrefab;

    private void Start()
    {
        playerLayer = LayerMask.NameToLayer("Player");

        GameManager.RegisterOrb(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == playerLayer)
        {
            Instantiate(ExplosionVFXPrefab, transform.position, transform.rotation);
            gameObject.SetActive(false);
            AudioManager.PlayOrbAudio();

            GameManager.PlayerGrabbedOrb(this);
        }
    }
}

