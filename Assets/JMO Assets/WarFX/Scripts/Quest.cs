using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Quest : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI quest1Text, quest2Text;

    [SerializeField]
    private GameObject checkMark1, checkMark2, victoryText;

    public int placedDucks = 0;
    public int blownDucks = 0;

    // Update is called once per frame
    void Update()
    {
        quest1Text.text = "Place 10 ducks on to the field. " + placedDucks + "/10";
        quest2Text.text = "Blow 10 ducks away from the field. " + blownDucks + "/10";

        if(placedDucks >= 10)
        {
            checkMark1.SetActive(true);
            quest1Text.color = new Color(quest1Text.color.r, quest1Text.color.g, quest1Text.color.b, quest1Text.color.a * 0.5f);
        }

        if (blownDucks >= 10)
        {
            checkMark2.SetActive(true);
            quest2Text.color = new Color(quest2Text.color.r, quest2Text.color.g, quest2Text.color.b, quest2Text.color.a * 0.5f);
        }

        // Peli voitettu
        if(placedDucks >= 10 && blownDucks >= 10)
        {
            victoryText.SetActive(true);
        }
    }
}
