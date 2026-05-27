using System;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(CharacterAnimator))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private string trainerName;
    [SerializeField] private Sprite trainerSprite;

    public string TrainerName => trainerName;

    public Sprite TrainerSprite => trainerSprite;

    private Vector2 input;

    private Character _character;

    public event Action OnPokemonEncountered;
    public event Action<Collider2D> OnEnterTrainersFoV;

    private float timeSinceLastClick;
    [SerializeField] float timeBetweenClicks = 0.5f;

    void Awake()
    {
        _character = GetComponent<Character>();
    }

    public void HandleUpdate()
    {
        timeSinceLastClick += Time.deltaTime;

        if (!_character.IsMoving)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            if (input != Vector2.zero)
            {
                StartCoroutine(_character.MoveTowards(input, OnMoveFinish));
            }
        }

        _character.HandleUpdate();

        if (Input.GetAxisRaw("Submit") != 0)
        {
            if (timeSinceLastClick >= timeBetweenClicks)
                Interact();
        }
    }

    void OnMoveFinish()
    {
        CheckForPokemon();
        CheckForInTrainersFoV();
    }

    private void Interact()
    {
        timeSinceLastClick = 0;

        var facingDirection = new Vector3(_character.Animator.MoveX, _character.Animator.MoveY);
        var interactPosition = transform.position + facingDirection;

        var collider = Physics2D.OverlapCircle(interactPosition, 0.2f, GameLayers.SharedInstance.InteractableLayer);

        if (collider != null)
        {
            collider.GetComponent<Interactable>()?.Interact(transform.position);
        }
    }

    [SerializeField] float verticalOffset = 0.2f;

    private void CheckForPokemon()
    {
        if (Physics2D.OverlapCircle(transform.position - new Vector3(0, verticalOffset), 0.2f,
                GameLayers.SharedInstance.PokemonLayer) != null)
        {
            if (Random.Range(0, 100) <= 15)
            {
                _character.Animator.IsMoving = false;
                OnPokemonEncountered();
            }
        }
    }

    private void CheckForInTrainersFoV()
    {
        var collider = Physics2D.OverlapCircle(transform.position - new Vector3(0, verticalOffset),
            0.2f, GameLayers.SharedInstance.FovLayer);

        if (collider != null)
        {
            _character.Animator.IsMoving = false;
            OnEnterTrainersFoV?.Invoke(collider);
        }
    }
}