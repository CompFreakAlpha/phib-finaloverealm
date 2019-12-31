using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{

    // NEXT TIME MAKE A GetItemStat() and 

    public static ItemDatabase instance;

    public List<Item> itemDatabase = new List<Item>();

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }
            

        PreInitializeItemDatabase();
    }



    public static Item GetItem(string _ID)
    {
        if (ItemDatabase.instance.itemDatabase.Find(x => x.ID == _ID) != null)
        {
            return ItemDatabase.instance.itemDatabase.Find(x => x.ID == _ID);
        }
        else
        {
            ThrowItemDatabaseError("Item {" + _ID + "} doesn't exist!");
            return null;
        }
    }

    public static void ThrowItemDatabaseError(string _error)
    {

        Debug.Log("\n==// ITEMDATABASE ERROR \\\\==\n# " + _error + "\n==\\\\ ITEMDATABASE ERROR //==\n");

    }

    public Item AddItem(Item _item)
    {

        itemDatabase.Add(_item);
        Debug.Log(itemDatabase.Count + ": Item {" + _item.ID + "} initiliazed");
        return _item;
    }


    public void PostInitializeItemDatabase()
    {

        Debug.Log("==Initialization of [" + itemDatabase.Count + "] items complete==");
        Debug.Log("==\\\\ ITEM INIT //==");

    }



    public void InitializeItemDatabase()
    {


        // ======ITEM INIT======

        AddItem(new Item("test_item")  //Item ID
            .SetName("Test Item")  //Item Name
            .SetDescription(new string[] { "You shouldn't have this!" }) //Item Description
            .SetStats(new Dictionary<Item.EnumItemStat, float>() { //Item Stats
				{ Item.EnumItemStat.Value, 999999999.0f },
                { Item.EnumItemStat.Damage, 9001.0f }
            })
            .SetItemType(Item.EnumItemType.Null //Item Type
        ));

        AddItem(new Item("sword_metal")  //Item ID
            .SetName("Metal Sword")  //Item Name
            .SetDescription(new string[] { "A basic means of fighting." }) //Item Description
            .SetStats(new Dictionary<Item.EnumItemStat, float>() { //Item Stats
				{ Item.EnumItemStat.Value, 10.0f },
                { Item.EnumItemStat.Damage, 10.0f }
            })
            .SetItemType(Item.EnumItemType.WepBlunt //Item Type
        ));

        AddItem(new Item("tome_master")  //Item ID
            .SetName("Master Tome")  //Item Name
            .SetDescription(new string[] { "One of fourteen..." }) //Item Description
            .SetStats(new Dictionary<Item.EnumItemStat, float>() { //Item Stats
				{ Item.EnumItemStat.Value, 400.0f }
            })
            .SetItemType(Item.EnumItemType.Ability)) //Item Type
            .HasCustomPickupSound("tome"); // Custom Pickup Sound

        AddItem(new Item("poop")  //Item ID
            .SetName("Poop")  //Item Name
            .SetDescription(new string[] { "Literally just poop..." }) //Item Description
            .SetStats(new Dictionary<Item.EnumItemStat, float>() { //Item Stats
				{ Item.EnumItemStat.Value, 1.0f }
            })
            .SetItemType(Item.EnumItemType.Generic)); //Item Type

        AddItem(new Item("leaf_autumn")  //Item ID
            .SetName("Autumn Leaf")  //Item Name
            .SetDescription(new string[] { "Essence of Autumn" }) //Item Description
            .SetStats(new Dictionary<Item.EnumItemStat, float>() { //Item Stats
				{ Item.EnumItemStat.Value, 200.0f }
            })
            .SetItemType(Item.EnumItemType.Material)); //Item Type

        // ======END ITEM INIT======

        PostInitializeItemDatabase();
    }

    public void PreInitializeItemDatabase()
    {

        Debug.Log("==// ITEM INIT \\\\==");



        InitializeItemDatabase();
    }

    public static void SpawnDrop(ItemStack _data, Vector2 _position)
    {
        GameObject g = Instantiate(Resources.Load<GameObject>("Prefabs/GroundItem"), _position, Quaternion.identity);
        g.GetComponent<GroundItem>().itemData = _data;
    }
}


[System.Serializable]
public class Item
{

    public enum EnumItemType
    {
        Null,
        Generic,
        Material,
        Special,
        Ability,
        WepBlunt,
        WepTech,
        WepMagic
    }

    public enum EnumItemStat
    {
        Value,
        Damage,
        Armor
    }

    public string ID;
    public string name;
    public string[] description;
    public Dictionary<EnumItemStat, float> stats;
    public EnumItemType type;
    public Sprite icon;
    public AudioClip pickupSound;

    public Item(string _ID, string _name = null, string[] _description = null, Dictionary<EnumItemStat, float> _stats = null, EnumItemType _type = EnumItemType.Null, AudioClip _pickupSound = null)
    {

        ID = _ID;
        name = _name;
        description = _description;
        stats = _stats;
        type = _type;

        pickupSound = _pickupSound;

        if(pickupSound == null)
        {
            pickupSound = AudioManager.instance.GetClip("itempickup_default");
        }
        
        icon = Sprite.Create(Resources.Load<Texture2D>("Textures/Items/" + _ID), new Rect(0, 0, 16, 16), new Vector2(0.5f, 0.0625f), 16);

    }

    public Item SetID(string _ID)
    {
        ID = _ID;
        return this;
    }

    public Item SetName(string _name)
    {
        name = _name;
        return this;
    }

    public Item SetDescription(string[] _description)
    {
        description = _description;
        return this;
    }

    public Item SetStats(Dictionary<EnumItemStat, float> _stats)
    {
        stats = _stats;
        return this;
    }

    public Item SetItemType(EnumItemType _type)
    {
        type = _type;
        return this;
    }

    public Item SetPickupSound(AudioClip _soundEffect)
    {
        pickupSound = _soundEffect;
        return this;
    }

    // If customP is filled out, will use as prefix for pickup sound, otherwise will use ID
    public Item HasCustomPickupSound(string _customP = null)
    {
        if(_customP == null)
        {
            pickupSound = AudioManager.instance.GetClip(ID + "_pickup");
        } else
        {
            pickupSound = AudioManager.instance.GetClip(_customP + "_pickup");
        }
        
        return this;
    }

}

[System.Serializable]
public class ItemStack
{
    public Item item;
    public int count = 1;

    public ItemStack(Item _item, int _count = 1)
    {
        this.item = _item;
        this.count = _count;
    }
}