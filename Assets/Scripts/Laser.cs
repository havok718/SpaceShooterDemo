using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8;

    void Update()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        if (transform.position.y > 8 || transform.position.y < -9)
        {
            if (this.transform.parent != null)
                Destroy(this.gameObject.transform.parent.gameObject);
            Destroy(this.gameObject);
        }
    }
}
