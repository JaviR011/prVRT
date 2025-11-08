using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class EditorKeyboardMover : MonoBehaviour
{
    public float speed = 3f;
    public float rotationSpeed = 120f;
    private CharacterController cc;

    void Awake()
    {
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {
#if UNITY_EDITOR   // Solo funciona en el Editor, no en el visor
        float h = Input.GetAxis("Horizontal"); // A/D o flechas
        float v = Input.GetAxis("Vertical");   // W/S o flechas

        // Movimiento en el plano XZ relativo a hacia dónde miras
        Vector3 dir = new Vector3(h, 0, v);
        if (dir.sqrMagnitude > 0.001f)
        {
            dir = transform.TransformDirection(dir);
            cc.SimpleMove(dir * speed);
        }

        // Girar con el mouse (opcional)
        float mouseX = Input.GetAxis("Mouse X");
        transform.Rotate(0, mouseX * rotationSpeed * Time.deltaTime, 0);
#endif
    }
}
