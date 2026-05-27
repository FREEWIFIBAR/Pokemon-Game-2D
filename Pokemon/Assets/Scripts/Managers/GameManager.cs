using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public enum GameState
{
    StartMenu,
    Travel,
    Battle,
    Dialog,
    Cutscene,
    GameOver
}

[RequireComponent(typeof(ColorManager))]
public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private BattleManager battleManager;
    [SerializeField] private Camera worldMainCamera;
    [SerializeField] private Image transitionPanel;

    [SerializeField] private Image startMenu;
    [SerializeField] private Image gameOver;

    private GameState _gameState;

    public AudioClip startClip, worldClip, battleClip, endClip;

    public static GameManager SharedInstance;

    private TrainerController trainer;

    private void Awake()
    {
        _gameState = GameState.StartMenu;

        if (SharedInstance != null)
        {
            Destroy(this);
        }

        SharedInstance = this;
    }

    void Start()
    {
        startMenu.gameObject.SetActive(true);
        gameOver.gameObject.SetActive(false);

        StatusConditionFactory.InitFactory();
        SoundManager.SharedInstance.PlayMusic(startClip);
        playerController.OnPokemonEncountered += StartPokemonBattle;
        playerController.OnEnterTrainersFoV += (Collider2D trainerCollider) =>
        {
            var trainer = trainerCollider.GetComponentInParent<TrainerController>();
            if (trainer != null)
            {
                _gameState = GameState.Cutscene;
                StartCoroutine(trainer.TriggerTrainerBattle(playerController));
            }
        };
        battleManager.OnBattleFinish += FinishPokemonBattle;

        DialogManager.SharedInstance.OnDialogStart += () => { _gameState = GameState.Dialog; };

        DialogManager.SharedInstance.OnDialogFinish += () =>
        {
            if (_gameState == GameState.Dialog)
                _gameState = GameState.Travel;
        };
    }

    void StartPokemonBattle()
    {
        StartCoroutine(FadeInBattle());
    }

    public void StartTrainerBattle(TrainerController trainer)
    {
        this.trainer = trainer;
        StartCoroutine(FadeInTrainerBattle(trainer));
    }

    IEnumerator FadeInBattle()
    {
        SoundManager.SharedInstance.PlayMusic(battleClip);
        _gameState = GameState.Battle;

        yield return transitionPanel.DOFade(1.0f, 1.0f).WaitForCompletion();
        yield return new WaitForSeconds(0.2f);

        battleManager.gameObject.SetActive(true);
        worldMainCamera.gameObject.SetActive(false);

        var playerParty = playerController.GetComponent<PokemonParty>();
        var wildPokemon = FindObjectOfType<PokemonMapArea>().GetComponent<PokemonMapArea>().GetRandomWildPokemon();

        var wildPokemonCopy = new Pokemon(wildPokemon.Base, wildPokemon.Level);

        battleManager.HandleStartBattle(playerParty, wildPokemonCopy);
        yield return transitionPanel.DOFade(0.0f, 1.0f).WaitForCompletion();
    }

    IEnumerator FadeInTrainerBattle(TrainerController trainerController)
    {
        SoundManager.SharedInstance.PlayMusic(battleClip);
        _gameState = GameState.Battle;

        yield return transitionPanel.DOFade(1.0f, 1.0f).WaitForCompletion();
        yield return new WaitForSeconds(0.2f);

        battleManager.gameObject.SetActive(true);
        worldMainCamera.gameObject.SetActive(false);

        var playerParty = playerController.GetComponent<PokemonParty>();
        var trainerParty = trainerController.GetComponent<PokemonParty>();

        battleManager.HandleStartTrainerBattle(playerParty, trainerParty);
        yield return transitionPanel.DOFade(0.0f, 1.0f).WaitForCompletion();
    }

    void FinishPokemonBattle(bool playerHasWon)
    {
        if (trainer != null && playerHasWon)
        {
            trainer.AfterTrainerLostBattle();
            trainer = null;
        }

        StartCoroutine(FadeOutBattle(playerHasWon));
    }

    IEnumerator FadeOutBattle(bool playerHasWon)
    {
        yield return transitionPanel.DOFade(1.0f, 0.05f).WaitForCompletion();
        yield return new WaitForSeconds(0.2f);

        battleManager.gameObject.SetActive(false);
        worldMainCamera.gameObject.SetActive(true);

        yield return transitionPanel.DOFade(0.0f, 1.0f).WaitForCompletion();

        if (playerHasWon)
        {
            _gameState = GameState.Travel;
            SoundManager.SharedInstance.PlayMusic(worldClip);
        }
        else
        {
            _gameState = GameState.GameOver;
            SoundManager.SharedInstance.PlayMusic(endClip);
            StartCoroutine(ShowGameOver());
        }
    }

    private void Update()
    {
        if (_gameState == GameState.StartMenu && Input.GetAxisRaw("Submit") != 0)
        {
            StartCoroutine(HideStartMenu());
            SoundManager.SharedInstance.PlayMusic(worldClip);
            _gameState = GameState.Travel;
        }

        if (_gameState == GameState.Travel)
        {
            playerController.HandleUpdate();
        }
        else if (_gameState == GameState.Battle)
        {
            battleManager.HandleUpdate();
        }
        else if (_gameState == GameState.Dialog)
        {
            DialogManager.SharedInstance.HandleUpdate();
        }

        if (_gameState == GameState.GameOver && Input.GetAxisRaw("Submit") != 0)
        {
            RestartGame();
        }
    }

    IEnumerator HideStartMenu()
    {
        yield return startMenu.DOFade(0f, 1f).WaitForCompletion();
        startMenu.gameObject.SetActive(false);
    }

    IEnumerator ShowGameOver()
    {
        yield return gameOver.DOFade(1f, 1f).WaitForCompletion();
        gameOver.gameObject.SetActive(true);
    }

    void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }
}