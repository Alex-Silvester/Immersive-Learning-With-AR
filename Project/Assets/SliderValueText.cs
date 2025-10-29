using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderValueText : MonoBehaviour
{
    [SerializeField]
    Slider slider;

    [SerializeField]
    int decimals = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.GetComponent<TMP_Text>().SetText(toDecimal(slider.value, decimals).ToString());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setValue(Slider slider)
    {
        gameObject.GetComponent<TMP_Text>().SetText(toDecimal(slider.value, decimals).ToString());
    }

    private float toDecimal(float val, int decs = 0)
    {
        return decimals == 0 ? Mathf.Floor(val) : Mathf.Floor(val * (10 * decs))/(10*decs);
    }
}
