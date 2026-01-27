using System;
using UnityEngine;

namespace FG_GP2_T3
{
	public class HexGridCamera : MonoBehaviour
	{
		[SerializeField] private Vector2 _stickZoomRange;
		[SerializeField] private Vector2 _swivelZoomRange;
		[SerializeField] private float _zoomDampingFactor;
		[SerializeField] private Vector2 _moveSpeedRange;
		[SerializeField] private float _moveDampingFactor;
		[SerializeField] private float _rotationSpeed;
		[SerializeField] private float _rotationDampingFactor;

		private Transform _swivel, _stick;
		private float _rotationAngle;
		private float _pendingZoom, _pendingRotation;
		private Vector2 _pendingMove;
		private float _zoom = 1f;

		private void Awake() 
		{
			_swivel = transform.GetChild(0);
			_stick = _swivel.GetChild(0);

			AdjustZoom(0f);
			AdjustPosition(0f, 0f);
			AdjustRotation(0f);
		}

		private void Update() 
		{
			float zoomDelta = InputManager.Instance.Controls.MapEditor.Zoom.ReadValue<float>();
			if (zoomDelta != 0f || _pendingZoom != 0f)
				AdjustZoom(zoomDelta);

			float rotationDelta = InputManager.Instance.Controls.MapEditor.Rotate.ReadValue<float>();
			if (rotationDelta != 0f || _pendingRotation != 0f)
				AdjustRotation(rotationDelta);

			Vector2 moveDelta = InputManager.Instance.Controls.MapEditor.Move.ReadValue<Vector2>();
			if (moveDelta != Vector2.zero || _pendingMove != Vector2.zero) 
				AdjustPosition(moveDelta.x, moveDelta.y);
		}
		
		private void AdjustZoom(float delta)
		{
			_pendingZoom = Mathf.Clamp(_pendingZoom + delta * Time.deltaTime, -1f, 1f);
			_zoom = Mathf.Clamp01(_zoom + _pendingZoom);

			float distance = Mathf.Lerp(_stickZoomRange.x, _stickZoomRange.y, Mathf.Sqrt(_zoom));
			_stick.localPosition = new Vector3(0f, 0f, distance);

			float angle = Mathf.Lerp(_swivelZoomRange.x, _swivelZoomRange.y, Mathf.Sqrt(_zoom));
			_swivel.localRotation = Quaternion.Euler(angle, 0f, 0f);

			_pendingZoom = Mathf.Lerp(_pendingZoom, 0f, Time.deltaTime * _zoomDampingFactor);
		}

		private void AdjustPosition(float xDelta, float zDelta)
		{
			_pendingMove += new Vector2(xDelta, zDelta) * Time.deltaTime;
			_pendingMove = Vector2.ClampMagnitude(_pendingMove, 1f);

			Vector3 movementVector = transform.localRotation * new Vector3(_pendingMove.x, 0f, _pendingMove.y) * Mathf.Lerp(_moveSpeedRange.x, _moveSpeedRange.y, _zoom) * Time.deltaTime;
			transform.localPosition = ClampPosition(transform.localPosition + movementVector);

			_pendingMove = Vector2.Lerp(_pendingMove, Vector2.zero, Time.deltaTime * _moveDampingFactor);
		}

		private Vector3 ClampPosition(Vector3 position) 
		{
			float xMax = (GameConstants.HexGrid.GRID_RADIUS - 1f) * (2f * GameConstants.HexGrid.INNER_RADIUS);
			position.x = Mathf.Clamp(position.x, -xMax, xMax);

			float zMax = GameConstants.HexGrid.GRID_RADIUS * (1.5f * GameConstants.HexGrid.OUTER_RADIUS);
			position.z = Mathf.Clamp(position.z, -zMax, zMax);

			return position;
		}

		private void AdjustRotation(float delta) 
		{
			_pendingRotation = Mathf.Clamp(_pendingRotation + delta * Time.deltaTime, -1f, 1f);
			_rotationAngle += _pendingRotation * _rotationSpeed * Mathf.PI * Time.deltaTime;

			if (_rotationAngle < 0f)
				_rotationAngle += 360f;
			else if (_rotationAngle >= 360f)
				_rotationAngle -= 360f;

			transform.localRotation = Quaternion.Euler(0f, _rotationAngle, 0f);

			_pendingRotation = Mathf.Lerp(_pendingRotation, 0f, Time.deltaTime * _rotationDampingFactor);
		}
	}
}