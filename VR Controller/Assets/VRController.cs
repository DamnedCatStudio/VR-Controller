using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//Must be placed on VR Rig as the gameObjectToMove will rotate around the game object this script is placed on.
//Only Tested on Oculus Quest 2
public class VRController : MonoBehaviour
{
    [Tooltip("Must be the game object you want to move")]
    public GameObject gameObjectToMove;

    [Header("Right Controller Grip")]
    [Tooltip("Must use Oculus Touch Controller (Right Hand)")]
    public InputAction rightControllerGrip;

    [Header("Left Controller Grip")]
    [Tooltip("Must use Oculus Touch Controller (Left Hand)")]
    public InputAction leftControllerGrip;

    //Need to create an Action Map for Velocity and Position. It wont work if you code them in the script for some reason
    //Then create a Right/Left Velocity and Right/Left Position with Action Type: Value and Control Type: Vector 3
    //Use the Oculus Touch Controller (Right/Left Hand) with deviceVelocity/devicePosition
    [Tooltip("Must use Use Reference")]
    public InputActionProperty rightControllerPositionProperty, leftControllerPositionProperty, rightControllerVelocityProperty, leftControllerVelocityProperty;

    public float moveSpeed, rotateSpeed, moveVelocityMinimumThreshold, rotateVelocityMinimumThreshold;

    Vector3 rightStartControllerPosition, leftStartControllerPosition;

    float velocity;

    bool isRightGripped, isLeftGripped;

    public bool invertControls;

    // Start is called before the first frame update
    void Start()
    {
        //Enables the rightControllerGrip and leftgrip input actions
        rightControllerGrip.Enable();
        leftControllerGrip.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        // Sets isRightGripped to true and rightStartControllerPosition to the right controllers position when the right grip button is pushed
        rightControllerGrip.started += ctx => isRightGripped = true;
        rightControllerGrip.started += ctx => rightStartControllerPosition = rightControllerPositionProperty.action.ReadValue<Vector3>();

        // Sets isRightGripped to false when the right grip button is released
        rightControllerGrip.canceled += ctx => isRightGripped = false;

        // Sets isLeftGripped to true amd leftStartControllerPosition to the left controllers position  when the left grip button is pushed
        leftControllerGrip.started += ctx => isLeftGripped = true;
        leftControllerGrip.started += ctx => leftStartControllerPosition = leftControllerPositionProperty.action.ReadValue<Vector3>();

        // Sets isLeftGripped to false when the left grip button is released
        leftControllerGrip.canceled += ctx => isLeftGripped = false;
    }

    private void FixedUpdate()
    {
        // Checks if only the right grip button is being pushed
        if (isRightGripped && !isLeftGripped)
        {
            // Sets velocity to the right controller's velocity
            velocity = rightControllerVelocityProperty.action.ReadValue<Vector3>().magnitude;

            // Checks if the velocity is under the threshold
            if (velocity < moveVelocityMinimumThreshold)
            {
                // Sets rightStartControllerPosition to the right controllers position and returns
                rightStartControllerPosition = rightControllerPositionProperty.action.ReadValue<Vector3>();
                return;
            }
            else
            {
                // Runs the MoveGameBoard method with the normalized vector 3 of the starting right controller position minus the current right controller position
                MoveGameBoard((rightStartControllerPosition - rightControllerPositionProperty.action.ReadValue<Vector3>()).normalized);
            }
        }

        // Checks if only the left grip button is being pushed
        if (isLeftGripped && !isRightGripped)
        {
            // Sets velocity to the left controller's velocity
            velocity = leftControllerVelocityProperty.action.ReadValue<Vector3>().magnitude;

            // Checks if the velocity is under the threshold
            if (velocity < moveVelocityMinimumThreshold)
            {
                // Sets leftStartControllerPosition to the left controllers position and returns
                leftStartControllerPosition = leftControllerPositionProperty.action.ReadValue<Vector3>();
                return;
            }
            else
            {
                // Runs the MoveGameBoard method with the normalized vector 3 of the starting left controller position minus the current left controller position
                MoveGameBoard((leftStartControllerPosition - leftControllerPositionProperty.action.ReadValue<Vector3>()).normalized);
            }
        }

        if (isRightGripped && isLeftGripped)
        {
            // Checks which controller is moving based on the magnitude of the starting positions and current positions
            if ((rightStartControllerPosition - rightControllerPositionProperty.action.ReadValue<Vector3>().normalized).magnitude > (leftStartControllerPosition - leftControllerPositionProperty.action.ReadValue<Vector3>().normalized).magnitude)
            {
                // Sets velocity to the right controller's velocity
                velocity = rightControllerVelocityProperty.action.ReadValue<Vector3>().magnitude;

                // Checks if the velocity is under the threshold
                if (velocity < rotateVelocityMinimumThreshold)
                {
                    // Sets rightStartControllerPosition to the right controllers position
                    rightStartControllerPosition = rightControllerPositionProperty.action.ReadValue<Vector3>();
                    return;
                }

                // Checks if controls are inverted
                if (invertControls)
                {
                    if (rightControllerPositionProperty.action.ReadValue<Vector3>().x > rightStartControllerPosition.x)
                    {
                        // If the right controller is moving to the right, roate the object around the rig along the positive y axis
                        gameObjectToMove.transform.RotateAround(transform.position, Vector3.up, velocity * rotateSpeed * Time.deltaTime);
                    }
                    else
                    {
                        // If the right controller is moving to the left, roate the object around the rig along the negative y axis
                        gameObjectToMove.transform.RotateAround(transform.position, Vector3.down, velocity * rotateSpeed * Time.deltaTime);
                    }
                }
                else
                {
                    if (rightControllerPositionProperty.action.ReadValue<Vector3>().x > rightStartControllerPosition.x)
                    {
                        // If the right controller is moving to the right, roate the object around the rig along the positive y axis
                        gameObjectToMove.transform.RotateAround(transform.position, Vector3.down, velocity * rotateSpeed * Time.deltaTime);
                    }
                    else
                    {
                        // If the right controller is moving to the left, roate the object around the rig along the negative y axis
                        gameObjectToMove.transform.RotateAround(transform.position, Vector3.up, velocity * rotateSpeed * Time.deltaTime);
                    }
                }
            }
            else
            {
                // Sets velocity to the left controller's velocity
                velocity = leftControllerVelocityProperty.action.ReadValue<Vector3>().magnitude;

                // Checks if the velocity is under the threshold
                if (velocity < rotateVelocityMinimumThreshold)
                {
                    // Sets leftStartControllerPosition to the left controllers position and returns
                    leftStartControllerPosition = leftControllerPositionProperty.action.ReadValue<Vector3>();
                    return;
                }

                // Checks if controls are inverted
                if (invertControls)
                {
                    if (leftControllerPositionProperty.action.ReadValue<Vector3>().x > leftStartControllerPosition.x)
                    {
                        // If the left controller is moving to the right, roate the object around the rig along the positive y axis
                        gameObjectToMove.transform.RotateAround(transform.position, Vector3.up, velocity * rotateSpeed * Time.deltaTime);
                    }
                    else
                    {
                        // If the left controller is moving to the left, roate the object around the rig along the negative y axis
                        gameObjectToMove.transform.RotateAround(transform.position, Vector3.down, velocity * rotateSpeed * Time.deltaTime);
                    }
                }
                else
                {
                    if (leftControllerPositionProperty.action.ReadValue<Vector3>().x > leftStartControllerPosition.x)
                    {
                        // If the left controller is moving to the right, roate the object around the rig along the positive y axis
                        gameObjectToMove.transform.RotateAround(transform.position, Vector3.down, velocity * rotateSpeed * Time.deltaTime);
                    }
                    else
                    {
                        // If the left controller is moving to the left, roate the object around the rig along the negative y axis
                        gameObjectToMove.transform.RotateAround(transform.position, Vector3.up, velocity * rotateSpeed * Time.deltaTime);
                    }
                }
            }
        }
    }

    // Runs when only one controller grib button is being pushed and is moving fast enough
    void MoveGameBoard(Vector3 direction)
    {
        // Checks if controls are inverted
        if (invertControls)
        {
            // Translates the objects transform based on the negative of the given direction
            gameObjectToMove.transform.Translate(-direction * moveSpeed * Time.fixedDeltaTime, Space.Self);
        }
        else
        {
            // Translates the objects transform based on the the given direction
            gameObjectToMove.transform.Translate(direction * moveSpeed * Time.fixedDeltaTime, Space.Self);
        }
    }
}
