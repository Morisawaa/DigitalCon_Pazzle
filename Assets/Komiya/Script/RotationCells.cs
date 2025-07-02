using UnityEngine;

public class RotationCells : MonoBehaviour
{
    Piece Piece_;

    private void Start()
    {
        Piece_ = GetComponentInParent<Piece>();

    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(MousePos, Vector2.zero);

            if(hit.collider != null && hit.collider.gameObject == this.gameObject)
            {
                Piece_.Rotate(true);
            }
        }
    }
}
