using UnityEngine;
using TMPro;

public class CharacterHoverText : MonoBehaviour
{
    // Reference to the TextMeshPro UI Text element that will show hover text
    public TMP_Text hoverText;

    // Hover text for each character
    public string barbarianText = "<b>Bash</b>\n" +
                                       "<i>Type:</i> Basic\n" +
                                       "<i>Activation:</i> Select Enemy\n" +
                                       "<i>Damage:</i> 5\n" +
                                       "<i>Cooldown:</i> 5 seconds\n\n" +

                                       "<b>Shield</b>\n" +
                                       "<i>Type:</i> Defensive\n" +
                                       "<i>Activation:</i> Instant\n" +
                                       "<i>Damage:</i> 1\n" +
                                       "<i>Cooldown:</i> 1 second\n\n" +

                                       "<b>Iron Maelstrom</b>\n" +
                                       "<i>Type:</i> Wild Card\n" +
                                       "<i>Activation:</i> Instant\n" +
                                       "<i>Damage:</i> 10\n" +
                                       "<i>Cooldown:</i> 10 seconds\n\n" +

                                       "<b>Charge</b>\n" +
                                       "<i>Type:</i> Ultimate\n" +
                                       "<i>Activation:</i> Select Position\n" +
                                       "<i>Kills:</i> 5 (Main Level), 10 (Boss Level)";
    public string sorcererText = "<b>Fireball</b>\n" +
                                       "<i>Type:</i> Basic\n" +
                                       "<i>Activation:</i> Select Enemy\n" +
                                       "<i>Damage:</i> 5\n" +
                                       "<i>Cooldown:</i> 1 seconds\n\n" +

                                       "<b>Teleport</b>\n" +
                                       "<i>Type:</i> Defensive\n" +
                                       "<i>Activation:</i>  Select Position\r\n\n" +
                                       "<i>Damage:</i> 0\n" +
                                       "<i>Cooldown:</i> 10 second\n\n" +

                                       "<b>Clone</b>\n" +
                                       "<i>Type:</i> Wild Card\n" +
                                       "<i>Activation:</i> Select Position\n" +
                                       "<i>Damage:</i> 10\n" +
                                       "<i>Cooldown:</i> 10 seconds\n\n" +

                                       "<b>Inferno</b>\n" +
                                       "<i>Type:</i> Ultimate\n" +
                                       "<i>Activation:</i> Select Position\n" +
                                       "<i>Damage:</i> 10\n" +
                                       "<i>Kills:</i> 15";
    public string erikaText = "<b>Bash</b>\n" +
                                       "<i>Type:</i> Basic\n" +
                                       "<i>Activation:</i> Select Enemy\n" +
                                       "<i>Damage:</i> 5\n" +
                                       "<i>Cooldown:</i> 5 seconds\n\n" +

                                       "<b>Shield</b>\n" +
                                       "<i>Type:</i> Defensive\n" +
                                       "<i>Activation:</i> Instant\n" +
                                       "<i>Damage:</i> 1\n" +
                                       "<i>Cooldown:</i> 1 second\n\n" +

                                       "<b>Iron Maelstrom</b>\n" +
                                       "<i>Type:</i> Wild Card\n" +
                                       "<i>Activation:</i> Instant\n" +
                                       "<i>Damage:</i> 10\n" +
                                       "<i>Cooldown:</i> 10 seconds\n\n" +

                                       "<b>Charge</b>\n" +
                                       "<i>Type:</i> Ultimate\n" +
                                       "<i>Activation:</i> Select Position\n" +
                                       "<i>Kills:</i> 5 (Main Level), 10 (Boss Level)";

    // This method is called when the mouse enters the ability's collider
    void OnMouseEnter()
    {
        // Check the tag of the object the mouse is hovering over and update the text accordingly
        switch (gameObject.tag)
        {
            case "Barbarian":
                hoverText.text = barbarianText; // Show Barbarian's ability text
                break;
            case "Sorcerer":
                hoverText.text = sorcererText;  // Show Sorcerer's ability text
                break;
            case "Erika":
                hoverText.text = erikaText;     // Show Erika's ability text
                break;
            default:
                hoverText.text = "Unknown Ability"; // Default case if tag is unknown
                break;
        }

        // Enable the hover text
        hoverText.enabled = true;
    }

    // This method is called when the mouse exits the ability's collider
    void OnMouseExit()
    {
        // Disable the hover text when the mouse exits the collider
        hoverText.enabled = false;
    }
}
