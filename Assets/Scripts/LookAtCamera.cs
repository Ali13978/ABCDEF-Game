using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        // Get the main camera in the scene
        Camera mainCamera = Camera.main;

        if (mainCamera != null)
        {
            // Calculate the direction from the object to the camera
            Vector3 targetDirection = mainCamera.transform.position - transform.position;

            // Calculate the rotation needed to face the camera
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

            // Apply the rotation to the object
            transform.rotation = targetRotation;
        }
    }
}
