using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LockPin : MonoBehaviour
{
    public Rigidbody pinRigidBody;
    public Transform pin;
    public Transform lockPosition; // The position where the pin locks
    public Transform lowerLimit;
    public Transform upperLimit;
    public float snapThreshold = 0.05f; // Distance within which locking occurs
    public float force = 10;
    public AudioSource lockSound;
    public Collider[] collidersToIgnore;
    public UnityEvent onLocked; // Event triggered when the pin locks

    private bool isLocked = false;

    void Start()
    {
        Collider localCollider = GetComponent<Collider>();
        if (localCollider != null)
        {
            foreach (Collider singleCollider in collidersToIgnore)
            {
                Physics.IgnoreCollision(localCollider, singleCollider);
            }
        }
    }

    void Update()
    {
        // Skip logic if the pin is already locked
        if (isLocked) return;

        // Ensure the pin stays within its bounds
        if (pin.localPosition.y > upperLimit.localPosition.y)
        {
            pin.localPosition = upperLimit.localPosition;
        }
        else if (pin.localPosition.y < lowerLimit.localPosition.y)
        {
            pin.localPosition = lowerLimit.localPosition;
        }
        else
        {
            // Apply force to keep the pin moving
            pinRigidBody.AddForce(pin.transform.up * force * Time.deltaTime);
        }

        // Check if the pin is within snapping range of the lock position
        if (Vector3.Distance(pin.position, lockPosition.position) <= snapThreshold)
        {
            LockPinInPlace();
        }
    }

    private void LockPinInPlace()
    {
        isLocked = true; // Prevent further movement
        pin.position = lockPosition.position; // Snap pin to the lock position
        pinRigidBody.isKinematic = true; // Disable physics for the pin

        // Play lock sound, if assigned
        if (lockSound != null)
        {
            lockSound.Play();
        }

        // Invoke the locked event
        onLocked.Invoke();
    }
}