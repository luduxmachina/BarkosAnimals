using UnityEngine;

public class CameraCinematic : MonoBehaviour
{
    public float positionSmoothSpeed = 5f;
    public float rotationSmoothSpeed = 5f;

    private Vector3 targetPosition;
    private Quaternion targetRotation;

    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private bool isMoving = false;

    void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }
    void Update()
    {
        if (isMoving)
        {
            // Traslaci�n
            transform.position = Vector3.Lerp( transform.position, targetPosition, Time.deltaTime * positionSmoothSpeed);

            // Rotaci�n
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSmoothSpeed);

            if (Vector3.Distance(transform.position, targetPosition) < 0.01f && Quaternion.Angle(transform.rotation, targetRotation) < 0.5f)
            {
                isMoving = false;
            }
        }
    }

    public void MoveCameraTo(Camera cam)
    {
        targetPosition = cam.transform.position;
        targetRotation = cam.transform.rotation;
        isMoving = true;
    }
    public void MoveCameraTo(Vector3 pos, Quaternion rotation)
    {
        targetPosition = pos;
        targetRotation = rotation;
        isMoving = true;
    }
    public void Back()
    {
        targetPosition = initialPosition;
        targetRotation = initialRotation;
        isMoving = true;
    }

}
