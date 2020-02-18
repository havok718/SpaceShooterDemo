using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text _scoreText;

    [SerializeField] private Text _gameOverText;

    [SerializeField] private Text _restartText;

    [SerializeField] private GameObject _player;

    [SerializeField] private Sprite[] _livesSprite;

    [SerializeField] private Image _livesDisplay;

    private GameManager _gm;

    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score: ";
        _gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        _scoreText.text = $"Score: {_player.GetComponent<Player>().Score}";
    }

    public void UpdateLives(int currentLives)
    {
        _livesDisplay.sprite = _livesSprite[currentLives];
        if (currentLives == 0)
        {
            GameOverSequence();
        }
    }

    void GameOverSequence()
    {
        _gameOverText.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true);

        _gm.GameOver();

        StartCoroutine(GameOverFlickerRoutine());
    }

    IEnumerator GameOverFlickerRoutine()
    {
        while (true)
        {
            _gameOverText.gameObject.SetActive(true);
            yield return new WaitForSeconds(1);
            _gameOverText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
