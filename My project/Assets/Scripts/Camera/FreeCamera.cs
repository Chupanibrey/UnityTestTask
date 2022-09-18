using UnityEngine;

public class FreeCamera : MonoBehaviour
{
	[SerializeField]
	float speed = 10;
	[SerializeField]
	float acceleration = 5f;
	[SerializeField]
	float sensitivity = 2f;

	public Camera mainCamera;
	public BoxCollider boxCollider;

	Rigidbody body;
	float rotY;
	Vector3 direction;

	void Start()
	{
		body = GetComponent<Rigidbody>();
		body.freezeRotation = true;
		body.useGravity = false;
		body.mass = 0.1f;
		body.drag = 10;

		SetBoxColliderSize();
	}

	void SetBoxColliderSize()
	{
		Vector3 point_A = mainCamera.ScreenPointToRay(Vector2.zero).origin;

		// определяем размер коллайдера по ширине экрана
		Vector3 point_B = mainCamera.ScreenPointToRay(new Vector2(Screen.width, 0)).origin;

		var dist = Vector3.Distance(point_A, point_B);
		boxCollider.size = new Vector3(dist, boxCollider.size.y, 0.1f);

		// определяем размер бокса по высоте
		point_B = mainCamera.ScreenPointToRay(new Vector2(0, Screen.height)).origin;

		dist = Vector3.Distance(point_A, point_B);
		boxCollider.size = new Vector3(boxCollider.size.x, dist, 0.1f);

		boxCollider.center = new Vector3(0, 0, mainCamera.nearClipPlane);
	}

	void Update()
	{
		var h = Input.GetAxis("Horizontal");
		var v = Input.GetAxis("Vertical");

		var rotX = mainCamera.transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivity;
		rotY += Input.GetAxis("Mouse Y") * sensitivity;
		rotY = Mathf.Clamp(rotY, -90, 90);
		mainCamera.transform.localEulerAngles = new Vector3(-rotY, rotX, 0);

		direction = new Vector3(h, 0, v);
		direction = mainCamera.transform.TransformDirection(direction);
	}

	void FixedUpdate()
	{
		body.AddForce(direction.normalized * speed * acceleration);

		if (Mathf.Abs(body.velocity.x) > speed) body.velocity = new Vector3(Mathf.Sign(body.velocity.x) * speed, body.velocity.y, body.velocity.z);
		if (Mathf.Abs(body.velocity.z) > speed) body.velocity = new Vector3(body.velocity.x, body.velocity.y, Mathf.Sign(body.velocity.z) * speed);
		if (Mathf.Abs(body.velocity.y) > speed) body.velocity = new Vector3(body.velocity.x, Mathf.Sign(body.velocity.y) * speed, body.velocity.z);
	}
}