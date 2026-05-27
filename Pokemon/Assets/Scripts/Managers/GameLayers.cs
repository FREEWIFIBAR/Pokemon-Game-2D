using UnityEngine;

public class GameLayers : MonoBehaviour
{
    [SerializeField] private LayerMask solidObjectsPlayer, pokemonLayer, interactableLayer, playerLayer, fovLayer;

    public LayerMask SolidObjectsPlayer => solidObjectsPlayer;
    public LayerMask PokemonLayer => pokemonLayer;
    public LayerMask InteractableLayer => interactableLayer;
    public LayerMask PlayerLayer => playerLayer;
    public LayerMask FovLayer => fovLayer;

    public static GameLayers SharedInstance;

    private void Awake()
    {
        if (SharedInstance == null)
        {
            SharedInstance = this;
        }
    }

    public LayerMask CollisionLayers => SolidObjectsPlayer | InteractableLayer | PlayerLayer;
}