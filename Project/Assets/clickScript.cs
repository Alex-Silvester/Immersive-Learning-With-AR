using UnityEngine;
using UnityEngine.UI;

public class clickScript : MonoBehaviour
{
    [SerializeField]
    Canvas cube_UI;

    [SerializeField]
    RawImage background;

    [SerializeField]
    Color correct_color;

    [SerializeField]
    Color incorrect_color;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cube_UI.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void show()
    {
        cube_UI.gameObject.SetActive(true);
    }

    public void hide()
    {
        cube_UI.gameObject.SetActive(false);
    }

    public void answer(bool correct)
    {
        if(correct)
        {
            background.color = correct_color;
            return;
        }
        background.color = incorrect_color;
    }
}
