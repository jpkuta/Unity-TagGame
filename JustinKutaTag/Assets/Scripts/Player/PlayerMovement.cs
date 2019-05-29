using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 6f;
    PlayerController controller;
    Vector3 movement;
    Animator anim;
    Rigidbody playerRigidbody;
    int floorMask;
    float camRayLength = 100f;
    
    void Awake()
    {
        floorMask = LayerMask.GetMask("Terrain");
        anim = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();

    }

    void FixedUpdate()
    {
        //Get movement input
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        //Execute moving and aiming with animations
        Move(h, v);
        Turning();
        Animating(h, v);

    }

    private void Move(float h, float v)
    {
        movement.Set(h, 0f, v);
        movement = movement.normalized * speed * Time.deltaTime;

        playerRigidbody.MovePosition(transform.position + movement);
    }

   void Turning()
    {
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit floorHit;

        if(Physics.Raycast (camRay, out floorHit, camRayLength, floorMask))
        {
            Vector3 playerToMouse = floorHit.point - transform.position;
            playerToMouse.y = 0f;

            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
            playerRigidbody.MoveRotation(newRotation);
        }
    }

    void Animating(float h, float v)
    {
        Actions actions = GetComponent<Actions>();
        bool running = h!= 0f || v != 0f;
        anim.SetBool("IsRunning", running);


        //anim.SetBool("IsWalking", false);
       
    }

}
