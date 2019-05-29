using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public int damagePerShot = 50;
    public float timeBetweenBullets = 0.15f;
    public float range = 100f;

    
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
    Transform player;
    public GameObject Player;

    void Awake ()
    {
        shootableMask = LayerMask.GetMask ("Shootable");
        gunParticles = GetComponent<ParticleSystem> ();
        gunAudio = GetComponent<AudioSource> ();
        gunLight = GetComponent<Light> ();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealth = player.GetComponent<PlayerHealth>(); //Get player health to disable firing when dead
        animator = Player.GetComponent<Animator>();
    }


    void Update ()
    {
        timer += Time.deltaTime;
        

        if (Input.GetButton ("Fire1") && timer >= timeBetweenBullets && Time.timeScale != 0 && playerHealth.currentHealth > 0)
        {
            Shoot ();
        }

        if(timer >= timeBetweenBullets * effectsDisplayTime)
        {
            DisableEffects ();
        }

     
    }


    public void DisableEffects ()
    {

        gunLight.enabled = false;
    }


    void Shoot ()
    {
        timer = 0f;
        animator.SetTrigger("Attack");
        gunLight.enabled = true;
        gunAudio.Play();
        gunParticles.Stop ();

        gunParticles.Play ();


        shootRay.origin = transform.position;
        shootRay.direction = transform.forward;

        if(Physics.Raycast (shootRay, out shootHit, range, shootableMask))
        {
            EnemyHealth enemyHealth = shootHit.collider.GetComponent <EnemyHealth> ();
            if(enemyHealth != null)
            {
                enemyHealth.TakeDamage (damagePerShot, shootHit.point);
            }
        }

    }
}
