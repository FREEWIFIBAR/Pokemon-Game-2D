using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public GameObject healthBar;
    public Text currentHPText;
    public Text maxHPText;

    public void SetHP(Pokemon pokemon)
    {
        float normalizedValue = (float)pokemon.HP / pokemon.MaxHP;
        healthBar.transform.localScale = new Vector3(normalizedValue, 1f);
        healthBar.GetComponent<Image>().color = ColorManager.SharedInstance.BarColor(normalizedValue);
        currentHPText.text = pokemon.HP.ToString();
        maxHPText.text = $"/{pokemon.MaxHP}";
    }

    public IEnumerator SetSmoothHP(Pokemon pokemon)
    {
        float normalizedValue = (float)pokemon.HP / pokemon.MaxHP;

        maxHPText.text = $"/{pokemon.MaxHP}";

        var seq = DOTween.Sequence();
        seq.Append(healthBar.transform.DOScaleX(normalizedValue, 1f));
        seq.Join(healthBar.GetComponent<Image>().DOColor(ColorManager.SharedInstance.BarColor(normalizedValue), 1f));
        seq.Join(currentHPText.DOCounter(pokemon.previousHPValue, pokemon.HP, 1f));
        yield return seq.WaitForCompletion();
    }
}