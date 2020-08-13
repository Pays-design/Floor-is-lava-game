using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class SmoothIndicatorFiller : MonoBehaviour
{
    [SerializeField] protected float m_amountAddingSpeed;

    protected Image m_image;

    public float FillAmount 
    { 
        get { return m_image.fillAmount; }  
        set { m_image.fillAmount = value; }
    }

    protected virtual void Start() => m_image = GetComponent<Image>();

    public virtual void AddAmount(float amountToAdd) => StartCoroutine(AddAmountSmoothly(amountToAdd));

    protected virtual IEnumerator AddAmountSmoothly(float amountToAdd) 
    {
        float t = 0;
        while (t < amountToAdd) 
        {
            t += Time.deltaTime * m_amountAddingSpeed;
            m_image.fillAmount += Time.deltaTime * m_amountAddingSpeed;
            yield return null;
        }
    }
}
