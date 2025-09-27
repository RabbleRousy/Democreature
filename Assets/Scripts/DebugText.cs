using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
    public class DebugText : MonoBehaviour
    {
        [SerializeField] private TMP_Text blood;
        [SerializeField] private TMP_Text brain;
        [SerializeField] private TMP_Text heart;
        [SerializeField] private TMP_Text bean;
        [SerializeField] private TMP_Text allBeans;
        [SerializeField] private TMP_Text sweetBeans;
        [SerializeField] private TMP_Text sourBeans;
        [SerializeField] private TMP_Text policeBeans;
        
        private void Update()
        {
            blood.text = "Blood: " + GameManager.Instance.Blood;
            heart.text = "Heart: " + GameManager.Instance.HeartCorruption;
            brain.text = "Brain: " + GameManager.Instance.BrainCorruption;
            bean.text = "Bean: " + GameManager.Instance.BeanCorruption;
            allBeans.text = "All Beans: " + BeanManager.Instance.AllBeans;
            sweetBeans.text = "Sweet Beans: " + BeanManager.Instance.SweetBeans;
            sourBeans.text = "Sour Beans: " + BeanManager.Instance.SourBeans;
            policeBeans.text = "Police Beans: " + BeanManager.Instance.PoliceBeans;
        }
    }
}