using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLasering : MonoBehaviour
{
    public int damagePerShot = 50;
    public float timeBetweenBullets = 0.15f;
    public float range = 100f;
    Transform player;
    public GameObject Player;
    Animator animator;
    float timer;
    Ray shootRay = new Ray();
    RaycastHit shootHit;
    int shootableMask;
    ParticleSystem gunParticles;
    AudioSource gunAudio;
    Light gunLight;
    float effectsDisplayTime = 0.2f;
    bool IsDead = false;
    PlayerHealth playerHealth;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        shootableMask = LayerMask.GetMask("Laser");
        gunParticles = GetComponent<ParticleSystem>();
        gunAudio = GetComponent<AudioSource>();
        gunLight = GetComponent<Light>();
        playerHealth = player.GetComponent<PlayerHealth>(); //Get player health to disable firing when dead
        animator = Player.GetComponent<Animator>();
    }


    void Update()
    {
        timer += Time.deltaTime;


        if (Input.GetButton("Fire2") && timer >= timeBetweenBullets && Time.timeScale != 0 && playerHealth.currentHealth > 0)
        {
            Shoot();
        }

        if (timer >= timeBetweenBullets * effectsDisplayTime)
        {
            DisableEffects();
        }


    }


    public void DisableEffects()
    {

        gunLight.enabled = false;
    }


    void Shoot()
    {
        timer = 0f;
        animator.SetTrigger("Attack");
        gunLight.enabled = true;
        gunAudio.Play();
        gunParticles.Stop();

        gunParticles.Play();


        shootRay.origin = transform.position;
        shootRay.direction = transform.forward;

        if (Physics.Raycast(shootRay, out shootHit, range, shootableMask))
        {
            EnemyHealth enemyHealth = shootHit.collider.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damagePerShot, shootHit.point);
            }
        }

    }
}
