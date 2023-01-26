using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialSign : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (this.gameObject.name == "Road Sign 1"&&collision.gameObject.CompareTag("Player"))
            _text.text = "Press SPACE button to jump";
        else if (this.gameObject.name == "Road Sign 2" && collision.gameObject.CompareTag("Player"))
            _text.text = "Press SPACE button 2 times for double jumps";
        else if (this.gameObject.name == "Road Sign 3" && collision.gameObject.CompareTag("Player"))
            _text.text = "Press C and LEFT or RIGHT button to roll";
        else if (this.gameObject.name == "Road Sign 4" && collision.gameObject.CompareTag("Player"))
            _text.text = "I hope this coin works for something";
        else if (this.gameObject.name == "Road Sign 5" && collision.gameObject.CompareTag("Player"))
            _text.text = "Press LEFT or RIGHT while touching the wall to slide";
        else if (this.gameObject.name == "Road Sign 6" && collision.gameObject.CompareTag("Player"))
            _text.text = "Even the programmer doesn't know what key does";
        else if (this.gameObject.name == "Road Sign 7" && collision.gameObject.CompareTag("Player"))
            _text.text = "Press SPACE key while sliding to wall jump.";
        else if (this.gameObject.name == "Road Sign 8" && collision.gameObject.CompareTag("Player"))
            _text.text = "You finally reached the teleport";
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((this.gameObject.name == "Road Sign 1" || this.gameObject.name == "Road Sign 2" || this.gameObject.name == "Road Sign 3" || this.gameObject.name == "Road Sign 4"||
            this.gameObject.name == "Road Sign 5" || this.gameObject.name == "Road Sign 6" || this.gameObject.name == "Road Sign 7" || this.gameObject.name == "Road Sign 8")&& collision.gameObject.CompareTag("Player"))
            _text.text = " ";
    }
}
