using TMPro;
using UnityEngine;

public class DeveloperCredits : MonoBehaviour
{
    public TextMeshProUGUI developersText;

    void Start()
    {
        developersText.text =
            "<size=150%><color=#FFD700>Game Credits</color></size>\n\n" +
            "<size=120%><color=#FF4500>Participants:</color></size>\n" +
            "<size=100%><color=#FFFFFF>" +
            "1. Omar Adel\n" +
            "2. Omar Mohammad\n" +
            "3. Tarek Zeyad\n" +
            "4. Ziad Sultan\n" +
            "5. Khaled Attia\n" +
            "6. Farida Mousa\n" +
            "7. Habiba Hilal\n" +
            "8. Lujain Tarek" +
            "</color></size>\n\n" +
            "<size=80%><color=#BBBBBB>This list represents the talented individuals who contributed to the development of the game. Their efforts and creativity brought this project to life.</color></size>";
    }
}
