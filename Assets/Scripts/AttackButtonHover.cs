using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AttackButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Text myText;

    void Start()
    {
        myText = GetComponentInChildren<Text>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        myText.text = "+2 Spd | -20% Dmg";
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        myText.text = "Quick Attack";
    }
}