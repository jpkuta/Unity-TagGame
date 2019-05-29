using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Animator))]
public class PlayerController : MonoBehaviour {

	public Transform rightGunBone;
	public Transform leftGunBone;
	public Arsenal[] arsenal;
    public float speed = 10f;

    Vector3 movement;
    Rigidbody playerRigidbody;
    int floorMask;
    float camRayLength;


	private Animator animator;

	void Awake() {
		animator = GetComponent<Animator> ();
		if (arsenal.Length > 0)
			SetArsenal (arsenal[1].name);

        floorMask = LayerMask.GetMask("Terrain");

		}

    private void FixedUpdate()
    {
        //get raw input for movement
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

       // Move(h, v);
        //Turning();
        Animating(h, v);

    }

    public void SetArsenal(string name) {
		foreach (Arsenal hand in arsenal) {
			if (hand.name == name) {
				if (rightGunBone.childCount > 0)
					Destroy(rightGunBone.GetChild(0).gameObject);
				if (leftGunBone.childCount > 0)
					Destroy(leftGunBone.GetChild(0).gameObject);
				if (hand.rightGun != null) {
					GameObject newRightGun = (GameObject) Instantiate(hand.rightGun);
					newRightGun.transform.parent = rightGunBone;
					newRightGun.transform.localPosition = Vector3.zero;
					newRightGun.transform.localRotation = Quaternion.Euler(90, 0, 0);
					}
				if (hand.leftGun != null) {
					GameObject newLeftGun = (GameObject) Instantiate(hand.leftGun);
					newLeftGun.transform.parent = leftGunBone;
					newLeftGun.transform.localPosition = Vector3.zero;
					newLeftGun.transform.localRotation = Quaternion.Euler(90, 0, 0);
				}
				animator.runtimeAnimatorController = hand.controller;
				return;
				}
		}
	}

	[System.Serializable]
	public struct Arsenal {
		public string name;
		public GameObject rightGun;
		public GameObject leftGun;
		public RuntimeAnimatorController controller;
	}

    void Move(float h, float v)
    {
        // Set the movement vector based on the axis input.
        movement.Set(h, 0f, v);

        // Normalise the movement vector and make it proportional to the speed per second.
        movement = movement.normalized * speed * Time.deltaTime;

        // Move the player to it's current position plus the movement.
        playerRigidbody.MovePosition(transform.position + movement);
    }

    void Turning()
    {
        //Get mouse position for character turning
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit floorHit;
        if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))
        {
            Vector3 playerToMouse = floorHit.point - transform.position;
            playerToMouse.y = 0f;

            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
            playerRigidbody.MoveRotation(newRotation);
        }
    }

    void Animating(float h, float v)
    {
        bool isShiftKeyDown = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        bool walking = h != 0f || v != 0f;
        if (walking)
        {
            animator.SetFloat("Speed", 1);
            if (isShiftKeyDown)
            {
                animator.SetFloat("Speed", .3f);
            }
        }
        else
        {
            animator.SetFloat("Speed", 0);
        }
    }

}
