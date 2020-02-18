using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private float _rotateSpeed = 3;

    [SerializeField] private GameObject _explosion;

    private Spawn_Manager _spawnManager;

    private AudioSource _audio;

    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<Spawn_Manager>();

        _audio = GameObject.Find("ExplosionSound").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, 0, _rotateSpeed * Time.deltaTime));
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            Instantiate(_explosion, this.transform.position, Quaternion.identity);

            _spawnManager.StartSpawning();

            Destroy(this.GetComponent<Collider2D>());

            Destroy(other.gameObject);
            Destroy(this.gameObject, 0.2f);
        }
        _audio.Play();
    }
}
