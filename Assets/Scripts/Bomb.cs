using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class Bomb : MonoBehaviour
{
    [SerializeField]
    private float timer, radius, force;

    [SerializeField]
    private GameObject explosion;

    private float explodeTimer = 0;
    private bool exploded = false;
    private Quest quest;

    private void Awake()
    {
        explodeTimer = 0;
        quest = GameObject.FindGameObjectWithTag("Quest").GetComponent<Quest>();
    }

    void Update()
    {
        explodeTimer += Time.deltaTime;

        if(explodeTimer >= timer && !exploded)
        {
            exploded = true;
            Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

            foreach(Collider col in colliders)
            {
                Rigidbody rig = col.GetComponent<Rigidbody>();

                if(rig != null)
                {
                    rig.AddExplosionForce(force, transform.position, radius);

                    if(col.CompareTag("Duck"))
                    {
                        quest.blownDucks++;
                    }
                }
            }

            GameObject exp = Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(exp, 3);
            Destroy(gameObject);
        }
    }
}
