using UnityEngine;
using UnityEngine.UI;

public class PartyMemberHUD : MonoBehaviour
{
    public Text nameText, lvlText, typeText;
    public HealthBar healthBar;
    public Image pokemonImage;

    private Pokemon _pokemon;

    public void SetPokemonData(Pokemon pokemon)
    {
        _pokemon = pokemon;

        nameText.text = pokemon.Base.Name.ToUpper();
        lvlText.text = $"Lv {pokemon.Level}";
        if (pokemon.Base.Type2 == PokemonType.None)
        {
            typeText.text = pokemon.Base.Type1.ToString().ToUpper();
        }
        else
        {
            typeText.text = $"{pokemon.Base.Type1.ToString().ToUpper()} - {pokemon.Base.Type2.ToString().ToUpper()}";
        }

        healthBar.SetHP(pokemon);
        pokemonImage.sprite = pokemon.Base.FrontSprite;

        if (pokemon.HP <= 0)
        {
            GetComponent<Image>().color = new Color(233f / 255, 85f / 255, 85f / 255);
        }
    }

    public void SetSelectedPokemon(bool selected)
    {
        if (selected)
        {
            nameText.color = ColorManager.SharedInstance.selectedColor;
        }
        else
        {
            nameText.color = Color.black;
        }
    }
}