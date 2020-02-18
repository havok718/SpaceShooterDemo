using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed = 4;

    [SerializeField] private GameObject _laser;

    private AudioSource _audio;

    private Player p;

    private Animator _anim;

    private float _canFire;
    private bool _isEnemyAlive = true;

    void Start()
    {
        p = GameObject.Find("Player").GetComponent<Player>();
        if (p == null)
            Debug.Log("Player is NULL");

        _anim = GetComponent<Animator>();
        if (_anim == null)
            Debug.Log("Animator is NULL");

        _audio = GameObject.Find("ExplosionSound").GetComponent<AudioSource>();
    }

    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -6)
        {
            transform.position = new Vector3(Random.Range(-9f, 9f), 7, 0);
        }

        if (Time.time > _canFire && _isEnemyAlive)
            FireLaser();
    }

    void FireLaser()
    {
        _canFire = Time.time + Random.Range(2f, 5f);

        Instantiate(_laser, transform.position + new Vector3(0, -1.2f, 0), Quaternion.identity);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (p != null)
            {
                p.Damage();
            }

            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;

            _audio.Play();

            StartCoroutine(WaitForDeathAnimation(this.gameObject));
        }

        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);

            if (p != null)
                p.UpdateScore(10);

            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;

            _audio.Play();

            StartCoroutine(WaitForDeathAnimation(this.gameObject));
        }
    }

    IEnumerator WaitForDeathAnimation(GameObject thing)
    {
        _isEnemyAlive = false;
        this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitForSeconds(2.38f);
        
        Destroy(thing);
    }
}
