using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField] private float _speed = 3;
    [SerializeField] private float _frequency = 2;
    [SerializeField] private float _magnitude = 5;

    [SerializeField] private int _powerupID; // 0 = Triple Shot, 1 = Speed, 2 = Shield

    private AudioSource _audio;

    void Start()
    {
        _audio = GameObject.Find("PowerupSound").GetComponent<AudioSource>();
    }

    void Update()
    {
        CalculateMovement();
    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.right * Mathf.Sin(Time.time * _frequency) * _speed * _magnitude * Time.deltaTime);
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -6)
            Destroy(this.gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();

            if (player != null)
            {
                switch (_powerupID)
                {
                    case 0:
                        player.EnableTripleShot();
                        break;
                    case 1:
                        player.EnableSpeed();
                        break;
                    case 2:
                        player.EnableShield();
                        break;
                    default:
                        Debug.Log("Default Value");
                        break;
                }
            }

            _audio.Play();

            Destroy(this.gameObject);
        }
    }
}
