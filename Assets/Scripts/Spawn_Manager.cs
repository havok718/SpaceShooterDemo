using System.Collections;
using UnityEngine;

public class Spawn_Manager : MonoBehaviour
{
    [SerializeField] private GameObject _enemy;

    [SerializeField] private GameObject _enemyContainer;

    [SerializeField] private GameObject[] _powerup;

    [SerializeField] private GameObject _powerupContainer;

    [SerializeField] private float _enemySpawnTime = 6;

    private bool _stopSpawning = false;
    
    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    public void StopSpawning()
    {
        _stopSpawning = true;
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3);

        while (!_stopSpawning)
        {
            if (_enemySpawnTime >= 0.3f)
            {
                _enemySpawnTime /= 1.1f;
                GameObject newEnemy = Instantiate(_enemy, new Vector3(Random.Range(-9f, 9f), 7f, 0), Quaternion.identity);
                newEnemy.transform.parent = _enemyContainer.transform;
                yield return new WaitForSeconds(_enemySpawnTime);
            }
            else
            {
                GameObject newEnemy = Instantiate(_enemy, new Vector3(Random.Range(-9f, 9f), 7f, 0), Quaternion.identity);
                newEnemy.transform.parent = _enemyContainer.transform;
                yield return new WaitForSeconds(0.25f);
            }
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3);

        while (!_stopSpawning)
        {
            GameObject newPowerup = Instantiate(_powerup[Random.Range(0, 3)], new Vector3(Random.Range(-5f, 4f), 7, 0), Quaternion.identity);
            newPowerup.transform.parent = _powerupContainer.transform;
            yield return  new WaitForSeconds(Random.Range(3f, 7f));
        }
    }
}
