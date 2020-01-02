using UnityEngine;

public class CollisionArea : MonoBehaviour {
  private Color originalColor;
  private MeshRenderer meshRenderer;

  [SerializeField]
  private Color activeColor = Color.green;

  [SerializeField]
  private LoadingState loadingState = LoadingState.NONE;

  private void Start() {
    meshRenderer = GetComponent<MeshRenderer>();
  }

  private void OnTriggerEnter(Collider other) {
    originalColor = meshRenderer.material.color;
    meshRenderer.material.color = activeColor;
    SendMessageUpwards("CollisionEntered", loadingState);
  }

  private void OnTriggerExit(Collider other) {
    meshRenderer.material.color = originalColor;
    SendMessageUpwards("CollisionExited", loadingState);
  }

}
