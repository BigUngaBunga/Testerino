using Mirror;
using UnityEngine;

namespace BelowUs
{
    public class SubmarineMovement : MonoBehaviour
    {
        private Rigidbody2D rb2D;
        private SpriteRenderer spriteRenderer;
        private ShipResource enginePower;
        private GameObject polygonCollider;
        private FlipSubmarineComponent[] submarineComponents;
        private float subSpeed;
        private float submarineRotationSpeed;
        float angularRetardation, lateralRetardation;
        public bool IsFlipped { get; private set; }
        private bool MoveSubmarine => subController.StationPlayerController != null && NetworkClient.localPlayer == subController.StationPlayerController;
        private bool EngineIsRunning => enginePower.CurrentValue > 0;
        [SerializeField] private StationController subController;
        
        private void Start()
        {
            enginePower = GameObject.Find("EnginePower").GetComponent<ShipResource>();
            rb2D = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            polygonCollider = GameObject.Find("Submarine/Collider");
            submarineComponents = GetComponentsInChildren<FlipSubmarineComponent>();
            subSpeed = 40;
            submarineRotationSpeed = 0.75f;
            angularRetardation = 0.033f;
            lateralRetardation = 0.040f;
        }

        private void FixedUpdate()
        {
            if (MoveSubmarine)
            {
                HandleRotation();
                HandleLateralMovement();
                StopCollisionAngularMomentum();
            }
        }

        private void Update()
        {
            if (EngineIsRunning && MoveSubmarine)
                FlipSubmarine();
        }

        private void HandleRotation()
        {
            if (EngineIsRunning)
            {
                if ((transform.rotation.eulerAngles.z <= 90 || transform.rotation.eulerAngles.z >= 100) && (Input.GetButton("ReverseRight") || Input.GetButton("RotateRight")))
                    transform.Rotate(0, 0, submarineRotationSpeed);

                if ((transform.rotation.eulerAngles.z <= 100 || transform.rotation.eulerAngles.z >= 270) && (Input.GetButton("ReverseLeft") || Input.GetButton("RotateLeft")))
                    transform.Rotate(0, 0, -submarineRotationSpeed);
            }
            
        }

        private void HandleLateralMovement()
        {
            if (EngineIsRunning && Input.GetButton("MoveForward"))
                rb2D.AddForce(transform.right * subSpeed, ForceMode2D.Force);
            if (EngineIsRunning && Input.GetButton("MoveBackwards"))
                rb2D.AddForce(-transform.right * subSpeed, ForceMode2D.Force);

            rb2D.velocity = !EngineIsRunning || (!Input.GetButton("MoveForward") && !Input.GetButton("MoveBackwards"))
                ? new Vector2(Mathf.Lerp(rb2D.velocity.x, 0, lateralRetardation), Mathf.Lerp(rb2D.velocity.y, 0, lateralRetardation))
                : new Vector2(Mathf.Lerp(rb2D.velocity.x, 0, lateralRetardation / 10), Mathf.Lerp(rb2D.velocity.y, 0, lateralRetardation / 10));

        }

        private void FlipSubmarine()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                IsFlipped = !IsFlipped;
                spriteRenderer.flipX = IsFlipped;
                subSpeed *= -1;              
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, -transform.rotation.eulerAngles.z));
                foreach (FlipSubmarineComponent component in submarineComponents)
                    component.FlipObject(IsFlipped);
            }
        }

        private void StopCollisionAngularMomentum() => rb2D.angularVelocity = Mathf.Lerp(rb2D.angularVelocity, 0, angularRetardation);
    }
}