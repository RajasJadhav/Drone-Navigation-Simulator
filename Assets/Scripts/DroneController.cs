using UnityEngine;

public class DroneController : MonoBehaviour
{
    public Transform frontLeftPropeller;
    public Transform frontRightPropeller;
    public Transform backLeftPropeller;
    public Transform backRightPropeller;
    public Transform droneModel;

    public float propellerSpeed = 1000f;

    public float moveSpeed = 10f;
    public float throttleSpeed = 5f;
    public float yawSpeed = 80f;


    public float maxTiltAngle = 20f;
    public float tiltSpeed = 5f;

    private float hoverHeight;

    private Rigidbody rb;

    float throttle = 0f; //up & down
    float yaw = 0f; //rotation

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
            RotatePropellersIdle(); // propellers spin slowly while disarmed
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
        // Reset every frame
        throttle = 0f;
        yaw = 0f;

        // Hold Space to go up
        if (Input.GetKey(KeyCode.Space))
        {
            throttle = 1f;
        }

        // Hold Left Ctrl to go down
        if (Input.GetKey(KeyCode.LeftControl))
        {
            throttle = -1f;
        }

        // Hold Q to rotate left
        if (Input.GetKey(KeyCode.Q))
        {
            yaw = -1f;
        }

        // Hold E to rotate right
        if (Input.GetKey(KeyCode.E))
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
        if (throttle != 0)
        {
            hoverHeight += throttle * throttleSpeed * Time.deltaTime;
        }

        float difference = hoverHeight - transform.position.y;

        return Vector3.up * difference * 5f;
    }

    void Movement()
    {
        Vector3 movement = MoveForward() + MoveSideWays() + MoveVertical();

        Vector3 velocity = rb.linearVelocity;

        // Horizontal movement
        velocity.x = movement.x;
        velocity.z = movement.z;

        // Vertical movement
        if (throttle != 0)
        {
            velocity.y = movement.y;
        }
        else
        {
            velocity.y = 0f; // Hold current altitude
        }

        Vector3 targetVelocity = movement;

        rb.linearVelocity = Vector3.Lerp(
            rb.linearVelocity,
            targetVelocity,
            5f * Time.deltaTime
        );
    }

    void Rotation()
    {
        transform.Rotate(Vector3.up * yaw * yawSpeed * Time.deltaTime);
    }

    void RotatePropellers()
    {
        float currentSpeed = 700f;

        // Faster when flying
        if (throttle != 0)
            currentSpeed = 1200f;

        // Even faster while moving
        if (Input.GetAxis("Vertical") != 0 ||
            Input.GetAxis("Horizontal") != 0 ||
            yaw != 0)
            currentSpeed = 1500f;

        frontLeftPropeller.Rotate(Vector3.up * currentSpeed * Time.deltaTime);
        backRightPropeller.Rotate(Vector3.up * currentSpeed * Time.deltaTime);

        frontRightPropeller.Rotate(Vector3.down * currentSpeed * Time.deltaTime);
        backLeftPropeller.Rotate(Vector3.down * currentSpeed * Time.deltaTime);
    }

    void TiltDrone()
    {
        // Read movement input
        float pitch = Input.GetAxis("Vertical");
        float roll = Input.GetAxis("Horizontal");

        // Calculate desired tilt
        Quaternion targetRotation = Quaternion.Euler(
            -pitch * maxTiltAngle,
            0,
            -roll * maxTiltAngle
        );

        // Smoothly tilt the drone model
        droneModel.localRotation = Quaternion.Lerp(
            droneModel.localRotation,
            targetRotation,
            tiltSpeed * Time.deltaTime
        );
    }

    public void ArmDrone()
    {
        isArmed = true;
    }

    public void DisarmDrone()
    {
        isArmed = false;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    public float GetSpeed()
    {
        return rb.linearVelocity.magnitude;
    }

    public float GetAltitude()
    {
        return transform.position.y;
    }

    void RotatePropellersIdle()
    {
        float idleSpeed = 300f;

        frontLeftPropeller.Rotate(Vector3.up * idleSpeed * Time.deltaTime);
        backRightPropeller.Rotate(Vector3.up * idleSpeed * Time.deltaTime);

        frontRightPropeller.Rotate(Vector3.down * idleSpeed * Time.deltaTime);
        backLeftPropeller.Rotate(Vector3.down * idleSpeed * Time.deltaTime);
    }

}
