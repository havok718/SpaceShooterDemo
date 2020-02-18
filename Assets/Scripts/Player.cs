using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;

    [SerializeField] private int _lives = 3;

    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _tripleShotPrefab;
    [SerializeField] private GameObject _shields;
    [SerializeField] private GameObject[] _damage;

    [SerializeField] private float _fireRate = 0.5f;

    [SerializeField] private int _score;

    private Spawn_Manager _spawnManager;
    private UIManager _uiManager;
    private GameObject _laser;
    private AudioSource _audio;

    private float _canFire = 0f;

    private bool _isTripleShotActive = false;
    private bool _isSpeedActive = false;
    private bool _isShieldActive = false;

    public int Score
    {
        get => _score;
        set
        {
            if (_score == value) return;
            _score = value;
        }
    }

    void Start()
    {
        this.transform.position = new Vector3(0, -4, 0);

        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<Spawn_Manager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audio = GameObject.Find("LaserShot").GetComponent<AudioSource>();

        if (_spawnManager == null)
            Debug.LogError("SpawnManager is NULL");
    }

    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
            FireLaser();
    }

    public void UpdateScore(int value)
    {
        _score += value;
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        float boost;

        if (_isSpeedActive)
        {
            boost = 3.5f;
        }
        else
        {
            boost = 0;
        }

        transform.Translate(Vector3.right * horizontalInput * (speed + boost) * Time.deltaTime);

        transform.Translate(Vector3.up * verticalInput * (speed + boost) * Time.deltaTime);

        // Can use Math.Clamp instead
        if (transform.position.y >= 0)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }
        else if (transform.position.y <= -4)
        {
            transform.position = new Vector3(transform.position.x, -4, 0);
        }

        if (transform.position.x > 11)
        {
            transform.position = new Vector3(-11, transform.position.y);
        }
        else if (transform.position.x < -11)
        {
            transform.position = new Vector3(11, transform.position.y, 0);
        }
    }

    void FireLaser()
    {
        _canFire = Time.time + _fireRate;
        if (_isTripleShotActive)
        {
            _laser = _tripleShotPrefab;
        }
        else
        {
            _laser = _laserPrefab;
        }
        Instantiate(_laser, transform.position + new Vector3(0, 1f, 0), Quaternion.identity);

        _audio.Play();
    }

    public void Damage()
    {
        if (_isShieldActive)
        {
            _isShieldActive = false;
            _shields.SetActive(false);
            return;
        }
        
        if (_lives > 0)
        {
            _lives--;
            _damage[Random.Range(0, 2)].gameObject.SetActive(true);
            if (_lives == 1 && _damage[0].gameObject.activeSelf)
            {
                _damage[1].gameObject.SetActive(true);
            }
            else if (_lives == 1)
            {
                _damage[0].gameObject.SetActive(true);
            }
            _uiManager.UpdateLives(_lives);
        }

        if (_lives <= 0)
        {
            _spawnManager.StopSpawning();

            _audio = GameObject.Find("ExplosionSound").GetComponent<AudioSource>();
            _audio.Play();

            Destroy(this.gameObject);
        }
    }

    public void EnableTripleShot()
    {
        _isTripleShotActive = true;
        StartCoroutine(PowerdownRoutine(5));
    }

    public void EnableSpeed()
    {
        _isSpeedActive = true;
        StartCoroutine(PowerdownRoutine(5));
    }

    public void EnableShield()
    {
        _isShieldActive = true;
        _shields.SetActive(true);
    }

    IEnumerator PowerdownRoutine(float time)
    {
        yield return new WaitForSeconds(time);
        if (_isTripleShotActive == true)
            _isTripleShotActive = false;
        if (_isSpeedActive == true)
            _isSpeedActive = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "EnemyLaser")
        {
            Damage();
            Destroy(other.gameObject);
            Destroy(other.gameObject.transform.parent.gameObject);
        }
    }
}
