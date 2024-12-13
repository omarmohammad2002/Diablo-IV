using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterHoverText : MonoBehaviour
{
    //public TMP_Text hoverText;

    public Light barbarianSpotlight;
    public Light sorcererSpotlight;
    public Light erikaSpotlight;

    private string barbarianText = "<b>Bash</b>\n" +
                              "<i>Type:</i> Basic\n" +
                              "<i>Activation:</i> Select Enemy\n" +
                              "<i>Damage:</i> 5\n" +
                              "<i>Cooldown:</i> 1 second\n\n" +

                              "<b>Shield</b>\n" +
                              "<i>Type:</i> Defensive\n" +
                              "<i>Activation:</i> Instant\n" +
                              "<i>Cooldown:</i> 10 seconds\n\n" +

                              "<b>Iron Maelstrom</b>\n" +
                              "<i>Type:</i> Wild Card\n" +
                              "<i>Activation:</i> Instant\n" +
                              "<i>Damage:</i> 10\n" +
                              "<i>Cooldown:</i> 10 seconds\n\n" +

                              "<b>Charge</b>\n" +
                              "<i>Type:</i> Ultimate\n" +
                              "<i>Activation:</i> Select Position\n" +
                              "<i>Damage:</i> Kills (Main Level), 10 (Boss Level)\n" +
                              "<i>Cooldown:</i> 10 seconds";

    private string sorcererText = "<b>Fireball</b>\n" +
                             "<i>Type:</i> Basic\n" +
                             "<i>Activation:</i> Select Enemy\n" +
                             "<i>Damage:</i> 5\n" +
                             "<i>Cooldown:</i> 1 second\n\n" +

                             "<b>Teleport</b>\n" +
                             "<i>Type:</i> Defensive\n" +
                             "<i>Activation:</i> Select Position\n" +
                             "<i>Cooldown:</i> 10 seconds\n\n" +

                             "<b>Clone</b>\n" +
                             "<i>Type:</i> Wild Card\n" +
                             "<i>Activation:</i> Select Position\n" +
                             "<i>Damage:</i> 10\n" +
                             "<i>Cooldown:</i> 10 seconds\n\n" +

                             "<b>Inferno</b>\n" +
                             "<i>Type:</i> Ultimate\n" +
                             "<i>Activation:</i> Select Position\n" +
                             "<i>Damage:</i> 10, then 2 per second\n" +
                             "<i>Cooldown:</i> 15 seconds";

    private string erikaText = "<b>Arrow</b>\n" +
                          "<i>Type:</i> Basic\n" +
                          "<i>Activation:</i> Select Enemy\n" +
                          "<i>Damage:</i> 5\n" +
                          "<i>Cooldown:</i> 1 second\n\n" +

                          "<b>Smoke Bomb</b>\n" +
                          "<i>Type:</i> Defensive\n" +
                          "<i>Activation:</i> Instant\n" +
                          "<i>Damage:</i> Stun for 5 seconds\n" +
                          "<i>Cooldown:</i> 10 seconds\n\n" +

                          "<b>Dash</b>\n" +
                          "<i>Type:</i> Wild Card\n" +
                          "<i>Activation:</i> Select Position\n" +
                          "<i>Cooldown:</i> 5 seconds\n\n" +

                          "<b>Shower Of Arrows</b>\n" +
                          "<i>Type:</i> Ultimate\n" +
                          "<i>Activation:</i> Select Position\n" +
                          "<i>Damage:</i> 10\n" +
                          "<i>Cooldown:</i> 10 seconds";

    void OnMouseEnter()
    {
        switch (gameObject.tag)
        {
            case "Barbarian":
                //hoverText.text = barbarianText;
                barbarianSpotlight.gameObject.SetActive(true);
                break;
            case "Sorcerer":
                //hoverText.text = sorcererText;
                sorcererSpotlight.gameObject.SetActive(true);
                break;
            case "Erika":
                //hoverText.text = erikaText;
                erikaSpotlight.gameObject.SetActive(true);
                break;
            default:
                //hoverText.text = "";
                break;
        }

        //hoverText.enabled = true;
    }

    void OnMouseExit()
    {
        //hoverText.enabled = false;
        barbarianSpotlight.gameObject.SetActive(false);
        sorcererSpotlight.gameObject.SetActive(false);
        erikaSpotlight.gameObject.SetActive(false);
    }

    //void OnMouseDown()
    //{
    //switch (gameObject.tag)
    //{
    //case "Barbarian":
    //SceneManager.LoadScene("BarbarianScene");
    // break;
    //case "Sorcerer":
    //SceneManager.LoadScene("SorcererScene");
    // break;
    // case "Erika":
    //SceneManager.LoadScene("ErikaScene");
    //break;
    //default:
    //Debug.LogError("No scene assigned for this character.");
    //break;
    //}
    //}
}
