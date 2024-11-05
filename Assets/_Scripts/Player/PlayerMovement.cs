using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if(!IsOwner)
            return;
        
        LookAtMouse();
    }
    
    private void LookAtMouse()
    {
        Vector2 mouseScreenPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mouseScreenPosition - (Vector2) transform.position).normalized;
        transform.up = direction;
    }
}
