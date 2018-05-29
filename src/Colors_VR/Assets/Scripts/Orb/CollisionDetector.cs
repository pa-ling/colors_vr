using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
	public LayerMask collidingLayer = -1;
	[Range(0.0f, 1.0f)]
	public float skinWidth = 0.1f;

	private new Rigidbody rigidbody;
	private new Collider collider;
	private Vector3 lastPosition;
	private float partialExtent;
	private float sqrMinimumExtent;

	private void Start()
	{
		rigidbody = GetComponent<Rigidbody>();
		collider = GetComponent<Collider>();
		lastPosition = rigidbody.position;

		float minimumExtent = Mathf.Min(Mathf.Min(collider.bounds.extents.x, collider.bounds.extents.y), collider.bounds.extents.z);
		partialExtent = minimumExtent * (1.0f - skinWidth);
		sqrMinimumExtent = minimumExtent * minimumExtent;
	}

	private void FixedUpdate()
	{
		Vector3 movement = rigidbody.position - lastPosition;

		if (movement.sqrMagnitude > sqrMinimumExtent)
		{
			RaycastHit raycastHit;

			if (Physics.Raycast(lastPosition, movement, out raycastHit, movement.magnitude, collidingLayer))
			{
				if (!raycastHit.collider)
					return;

				if (raycastHit.collider.isTrigger)
					raycastHit.collider.SendMessage("OnTriggerEnter", collider);

				if (!raycastHit.collider.isTrigger)
					rigidbody.position = raycastHit.point - (movement / movement.magnitude) * partialExtent;
			}
		}

		lastPosition = rigidbody.position;
	}
}
