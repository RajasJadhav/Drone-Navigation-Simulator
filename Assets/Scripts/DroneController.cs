using UnityEngine;

public class DroneController : MonoBehaviour
{
    // Drone propellers
    public Transform frontLeftPropeller;
    public Transform frontRightPropeller;
    public Transform backLeftPropeller;
    public Transform backRightPropeller;

    // Drone body for tilt animation
    public Transform droneModel;

    // Speed variables
    public float propellerSpeed = 1000f;
    public float moveSpeed = 10f;
    public float throttleSpeed = 5f;
    public float yawSpeed = 80f;

    // Tilt settings
    public float maxTiltAngle = 20f;
    public float tiltSpeed = 5f;

    // Stores the height where the drone should hover
    private float hoverHeight;

    private Rigidbody rb;

    // Movement inputs
    float throttle = 0f; 
    float yaw = 0f;

    // Checks whether the drone is armed
    public bool isArmed = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        hoverHeight = transform.position.y;

        rb.useGravity = false;
        rb.linearDamping = 3f;
        rb.angularDamping = 3f;

    }

    void Update()
    {

        if (!isArmed)
        {
            RotatePropellersIdle(); 
            return;
        }

        ReadInput();
        Movement();
        Rotation();
        RotatePropellers();
        TiltDrone();
    }

    void ReadInput()
    {
        
        throttle = 0f;
        yaw = 0f;

        if (Input.GetKey(KeyCode.Space))
        {
            throttle = 1f;
        }

        if (Input.GetKey(KeyCode.LeftControl))
        {
            throttle = -1f;
        }

        if (Input.GetKey(KeyCode.Q))
        {
            yaw = -1f;
        }

        if (Input.GetKey(KeyCode.E))
        {
            yaw = 1f;
        }
    }
    // Move forward and backward
    Vector3 MoveForward()
    {
        float pitch = Input.GetAxis("Vertical"); 
        Vector3 forwardmovement = transform.forward * pitch * moveSpeed;
        return forwardmovement;
    }

    // Move left and right
    Vector3 MoveSideWays()
    {
        float roll = Input.GetAxis("Horizontal"); //left & right
        Vector3 sidemovement = transform.right * roll * moveSpeed;
        return sidemovement;
    }

    // Move up and down
    Vector3 MoveVertical()
    {
        if (throttle != 0)
        {
            hoverHeight += throttle * throttleSpeed * Time.deltaTime;
        }

        float difference = hoverHeight - transform.position.y;

        return Vector3.up * difference * 5f;
    }

    // Apply movement
    void Movement()
    {
        Vector3 movement = MoveForward() + MoveSideWays() + MoveVertical();

        Vector3 velocity = rb.linearVelocity;

        velocity.x = movement.x;
        velocity.z = movement.z;

        if (throttle != 0)
        {
            velocity.y = movement.y;
        }
        else
        {
            velocity.y = 0f; 
        }

        Vector3 targetVelocity = movement;

        rb.linearVelocity = Vector3.Lerp(
            rb.linearVelocity,
            targetVelocity,
            5f * Time.deltaTime
        );
    }

    // Rotate the drone
    void Rotation()
    {
        if (yaw != 0)
        {
            rb.MoveRotation(
                rb.rotation * Quaternion.Euler(0, yaw * yawSpeed * Time.deltaTime, 0)
            );
        }
    }

    // Rotate propellers
    void RotatePropellers()
    {
        float currentSpeed = 700f;

        if (throttle != 0)
            currentSpeed = 1200f;

        if (Input.GetAxis("Vertical") != 0 ||
            Input.GetAxis("Horizontal") != 0 ||
            yaw != 0)
            currentSpeed = 1500f;

        frontLeftPropeller.Rotate(Vector3.up * currentSpeed * Time.deltaTime);
        backRightPropeller.Rotate(Vector3.up * currentSpeed * Time.deltaTime);

        frontRightPropeller.Rotate(Vector3.down * currentSpeed * Time.deltaTime);
        backLeftPropeller.Rotate(Vector3.down * currentSpeed * Time.deltaTime);
    }

    // Tilt the drone while moving
    void TiltDrone()
    {
        float pitch = Input.GetAxis("Vertical");
        float roll = Input.GetAxis("Horizontal");

        Quaternion targetRotation = Quaternion.Euler(
            -pitch * maxTiltAngle,
            0,
            -roll * maxTiltAngle
        );

        droneModel.localRotation = Quaternion.Lerp(
            droneModel.localRotation,
            targetRotation,
            tiltSpeed * Time.deltaTime
        );
    }

    // Arm the drone
    public void ArmDrone()
    {
        isArmed = true;
    }

    // Disarm the drone
    public void DisarmDrone()
    {
        isArmed = false;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    // Get current speed
    public float GetSpeed()
    {
        return rb.linearVelocity.magnitude;
    }

    // Get current height
    public float GetAltitude()
    {
        return transform.position.y;
    }

    // Spin propellers slowly when disarmed
    void RotatePropellersIdle()
    {
        float idleSpeed = 300f;

        frontLeftPropeller.Rotate(Vector3.up * idleSpeed * Time.deltaTime);
        backRightPropeller.Rotate(Vector3.up * idleSpeed * Time.deltaTime);

        frontRightPropeller.Rotate(Vector3.down * idleSpeed * Time.deltaTime);
        backLeftPropeller.Rotate(Vector3.down * idleSpeed * Time.deltaTime);
    }

}
