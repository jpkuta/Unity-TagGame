using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public PlayerHealth playerHealth;       // Reference to the player's health.
    public float restartDelay = 25f;         // Time to wait before restarting the level

   
    Animator anim;                          // Reference to the animator component.
    float restartTimer;                     // Timer to count up to restarting the level


    void Awake()
    {
        // Set up the reference.
        anim = GetComponent<Animator>();
       
    }


    void Update()
    {
        Scene loadedLevel = SceneManager.GetActiveScene(); //get current scene
        //When player has died
        if (playerHealth.currentHealth <= 0)
        {
            // Animator game over
            anim.SetTrigger("GameOver");

            // Increment a timer to count up to restarting.
            restartTimer += Time.deltaTime;

            // If it reaches the restart delay
            if (restartTimer >= restartDelay)
            {
                // Then reload the currently loaded level.
                SceneManager.LoadScene(loadedLevel.buildIndex);
            }
        }
    }
}