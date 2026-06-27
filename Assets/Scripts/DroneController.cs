using UnityEngine;

public class DroneController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float throttleSpeed = 5f;
    public float yawSpeed = 80f;

    
    private Rigidbody rb;

    float throttle = 0f; //up & down
    float yaw = 0f; //rotation

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.useGravity = false;

    }

    void Update()
    {
        ReadInput();
        Movement();
    }

    void ReadInput()
    {

        if(Input.GetKeyDown(KeyCode.Space))
        {
            throttle = 1f;
        }

        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            throttle = -1;
        }

        if(Input.GetKeyDown(KeyCode.Q))
        {
            yaw = -1f;
        }

        if(Input.GetKeyDown(KeyCode.E))
        {
            yaw = 1f;
        }
    }
    Vector3 MoveForward()
    {
        float pitch = Input.GetAxis("Vertical"); //foraward & backward
        Vector3 forwardmovement = transform.forward * pitch * moveSpeed;
        return forwardmovement;
    }

    Vector3 MoveSideWays()
    {
        float roll = Input.GetAxis("Horizontal"); //left & right
        Vector3 sidemovement = transform.right * roll * moveSpeed;
        return sidemovement;
    }

    Vector3 MoveVertical()
    {
        Vector3 verticalmovement = Vector3.up * throttle * throttleSpeed;
        return verticalmovement;
    }

    void Movement()
    {
        Vector3 movement = MoveForward() + MoveSideWays() + MoveVertical();
        rb.linearVelocity = movement;
    }

    void Rotation()
    {
        transform.Rotate(Vector3.up * yaw * yawSpeed * Time.deltaTime);
    }

}
