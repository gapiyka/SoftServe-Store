using UnityEngine;
using UnityEngine.UI;
public struct ObjectFormat
{
    public ObjectFormat(string sName, string fName, int priceInt, int cartContInt)
    {
        shortName = sName;
        fullName = fName;
        price = priceInt;
        cartCount = cartContInt;
    }

    public string shortName;
    public string fullName;
    public int price;
    public int cartCount;
}

public class MenuController : MonoBehaviour
{
    [SerializeField] private Image aimImage;
    [SerializeField] private Text counterText;
    [SerializeField] private Text priceText;
    [SerializeField] private Text titleText;
    [SerializeField] private GameObject hintsBar;
    [SerializeField] private GameObject cartMenu;
    [SerializeField] private GameObject objectMenu;
    [SerializeField] private GameObject prefabListBlock;
    [SerializeField] private Transform cartList;
    [SerializeField] private Transform spawnPos;

    private const int defaultCounter = 1;
    private ObjectFormat[] objectsArray;
    private GameObject tempObject;
    private int tempCounter;
    private int openedItemCounter;

    public bool IsMenuOpened = false;


    void Start()
    {
        objectsArray = new ObjectFormat[]
        {
            new ObjectFormat("bed_1", "Bed 1", 350, 0),
            new ObjectFormat("bed_2", "Bed 2", 400, 0),
            new ObjectFormat("bed_3", "Bed 3", 450, 0),
            new ObjectFormat("torchere_1", "Torchere 1", 70, 0),
            new ObjectFormat("torchere_2", "Torchere 2", 100, 0),
            new ObjectFormat("torchere_3", "Torchere 3", 120, 0),
            new ObjectFormat("torchere_4", "Torchere 4", 150, 0),
            new ObjectFormat("sofa_1", "Sofa 1", 350, 0),
            new ObjectFormat("sofa_2", "Sofa 2", 450, 0),
            new ObjectFormat("sofa_3", "Sofa 3", 400, 0),
            new ObjectFormat("sofa_4", "Sofa 4", 550, 0),
            new ObjectFormat("coffee_table_1", "Coffee Table 1", 100, 0),
            new ObjectFormat("coffee_table_2", "Coffee Table 2", 110, 0),
            new ObjectFormat("coffee_table_3", "Coffee Table 3", 120, 0),
            new ObjectFormat("coffee_table_4", "Coffee Table 4", 130, 0),
            new ObjectFormat("cabinet_1", "Cabinet 1", 260, 0),
            new ObjectFormat("kitchen_chair_1", "Kitchen Chair 1", 40, 0),
            new ObjectFormat("kitchen_chair_2", "Kitchen Chair 2", 80, 0),
            new ObjectFormat("kitchen_table_1", "Kitchen Table 1", 180, 0),
            new ObjectFormat("kitchen_table_2", "Kitchen Table 2", 190, 0),
            new ObjectFormat("kitchen_shelf_2", "Kitchen Shelf 1", 90, 0),
            new ObjectFormat("kitchen_shelf_3", "Kitchen Shelf 2", 100, 0),
            new ObjectFormat("kitchen_shelf_corner_INV", "Kitchen Shelf Corner", 70, 0),
            new ObjectFormat("modular_kitchen_table_1", "Modular Kitchen Table 1", 140, 0),
            new ObjectFormat("modular_kitchen_table_2", "Modular Kitchen Table 2", 160, 0),
            new ObjectFormat("modular_kitchen_table_3", "Modular Kitchen Table 3", 150, 0),
            new ObjectFormat("modular_kitchen_table_4", "Modular Kitchen Table 4", 170, 0)
        };
    }

    public void OpenCart()
    {
        Cursor.lockState = CursorLockMode.Confined;
        cartMenu.SetActive(true);
        IsMenuOpened = true;

        ClearList();// yeah that bad for optimization, but idk correct sollution now
        //generate list
        const int startList = 350;
        const int distList = 35;
        int goodsCounter = 0;
        foreach(ObjectFormat obj in objectsArray)
        {
            if(obj.cartCount > 0)
            {
                Vector3 blockPos = new Vector3(0, startList - (distList * goodsCounter), 0);
                GameObject listBlock = Instantiate(prefabListBlock, transform.position + blockPos, Quaternion.identity, cartList);
                listBlock.transform.GetChild(0).GetComponent<Text>().text = obj.fullName; // change item Name by index 0
                listBlock.transform.GetChild(1).GetComponent<Text>().text = obj.price + "$"; // change item Price by index 1
                listBlock.transform.GetChild(2).GetComponent<Text>().text = obj.cartCount.ToString(); // change item Quantity by index 2
                listBlock.transform.GetChild(3).GetComponent<Text>().text = (obj.cartCount * obj.price) + "$"; // change item Sum by index 3
                goodsCounter++;
            }
        }
    }

    public void OpenObjectMenu(Transform objT)
    {
        Cursor.lockState = CursorLockMode.Confined;
        objectMenu.SetActive(true);
        IsMenuOpened = true;
        tempCounter = defaultCounter;
        counterText.text = tempCounter.ToString();

        openedItemCounter = 0;
        string searchStr = objT.name.Split(" ")[0];
        foreach (ObjectFormat obj in objectsArray)
        {
            if (searchStr == obj.shortName)
            {
                CreateObj(objT.gameObject, obj);
                break;
            }
            openedItemCounter++;
        }
    }

    public void SwitchHints()
    {
        bool lever = !hintsBar.active;
        hintsBar.SetActive(lever);
    }

    public void ChangeAimColor(AimType aType)
    {
        Color aimedColor = Color.white;
        if (aType == AimType.Furniture) aimedColor = Color.blue;
        if (aType == AimType.Register) aimedColor = Color.red;
        aimImage.color = aimedColor;
    }

    public void CloseMenu()
    {
        Cursor.lockState = CursorLockMode.Locked;
        objectMenu.SetActive(false);
        IsMenuOpened = false;
    }

    public void CloseCart()
    {
        Cursor.lockState = CursorLockMode.Locked;
        cartMenu.SetActive(false);
        IsMenuOpened = false;
    }

    public void AddCounter()
    {
        tempCounter++;
        counterText.text = tempCounter.ToString();
    }

    public void ReduceCounter()
    {
        if(tempCounter>1) tempCounter--;
        counterText.text = tempCounter.ToString();
    }

    public void AddToCart()
    {
        objectsArray[openedItemCounter].cartCount += tempCounter;
    }

    public void BuyCart()
    {
        ClearList();
        for(int i = 0; i < objectsArray.Length; i++)
        {
            objectsArray[i].cartCount = 0;
        }
        // you recommend: 
        // "No backend logic for the purchase process should be implemented."
    }

    void CreateObj(GameObject gameObj, ObjectFormat objF)
    {
        if(tempObject != null)
        {
            Destroy(tempObject);
        }

        tempObject = Instantiate(gameObj, spawnPos.position, gameObj.transform.rotation);
        priceText.text = objF.price + "$";
        titleText.text = objF.fullName;
    }

    void ClearList()
    {
        int items = cartList.childCount;
        for(int i = 0; i < items; i++)
        {
            Destroy(cartList.GetChild(i).gameObject);
        }
    }
}
