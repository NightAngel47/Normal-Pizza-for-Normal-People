using System.Collections;
using UnityEngine;

public class CustomerLineUpgrade : ItemUpgrades
{
    [SerializeField] private GameObject newLineUI = null;

    private GameObject lineUIInstance = null;

    private CustomerLinePos customerLinePos = null;
    private Camera vrCam = null;
    
    private void Awake()
    {
        customerLinePos = GetComponent<CustomerLinePos>();
    }
    
    private void Start()
    {
        vrCam = Camera.main;
    }

    public override void TurnOnUpgrade()
    {
        customerLinePos.enabled = !customerLinePos.enabled;
        if (customerLinePos.enabled)
        {
            FindObjectOfType<CustomerLine>().AddNewCustomerLine(GetComponent<CustomerLinePos>());
            lineUIInstance = Instantiate(newLineUI, transform.position, Quaternion.identity);
            lineUIInstance.transform.LookAt(vrCam.transform);
            StartCoroutine(DestroyUI());
        }
    }
    
    private IEnumerator DestroyUI()
    {
        yield return new WaitUntil(() => FindObjectOfType<GameManager>().dayStarted);
        Destroy(lineUIInstance);
    }
}
