using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpawnButton : MonoBehaviour
{
    private CharacterData characterData;
    private Button button;
    private Image cooldownImage;
    private float currentCooldown;

    public void Initialize(CharacterData data)
    {
        characterData = data;
        button = GetComponent<Button>();
        cooldownImage = transform.Find("Cooldown").GetComponent<Image>();

        transform.Find("Icon").GetComponent<Image>().sprite = characterData.icon;
        transform.Find("Cost").GetComponent<TextMeshProUGUI>().text = characterData.cost.ToString();

        button.onClick.AddListener(OnClick);
    }

    private void Update()
    {
        if (currentCooldown > 0)
        {
            currentCooldown -= Time.deltaTime;
            cooldownImage.fillAmount = currentCooldown / characterData.cooldown;
            button.interactable = false;
        }
        else
        {
            button.interactable = true;
        }
    }

    private void OnClick()
    {
        if (currentCooldown <= 0)
        {
            SpawnManager.Instance.SpawnCharacter(characterData);
            currentCooldown = characterData.cooldown;
        }
    }
}