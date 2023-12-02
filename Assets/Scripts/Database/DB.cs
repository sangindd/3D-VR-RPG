using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Storage;
using Firebase.Extensions;
using UnityEngine.Events;
using System;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Threading;
using System.IO;

public class DB : MonoBehaviour
{
    DatabaseReference m_Reference;
    StorageReference storageReference;

    [SerializeField]
    RawImage rawImage;

    void Start()
    {
        m_Reference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public static void ReadWeaponData(string ItemId, Weapon weapon)
    {
        FirebaseDatabase.DefaultInstance.GetReference("Weapon").Child(ItemId)
            .GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    // Handle the error...
                }
                else
                {
                    DataSnapshot snapshot = task.Result;

                    IDictionary w = (IDictionary)snapshot.Value;

                    weapon.ItemName = w["ItemName"].ToString();
                    weapon.Type = w["Type"].ToString();
                    weapon.Rank = w["Rank"].ToString();
                    weapon.Strength = w["Strength"].ToString();
                    weapon.Dexterity = w["Dexterity"].ToString();
                    weapon.Intelligence = w["Intelligence"].ToString();
                    weapon.Damage_Basic = w["Damage_Basic"].ToString();
                    //weapon.Name_Skill = new List<object>((IEnumerable<object>)w["Name_Skill"]);
                    weapon.Info = w["Info"].ToString();
                    //weapon.Info_Skill = new List<object>((IEnumerable<object>)w["Info_Skill"]);
                }
            });
    }
    public static void ReadArmorData(string ItemId, Armor armor)
    {
        FirebaseDatabase.DefaultInstance.GetReference("Armor").Child(ItemId)
            .GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    // Handle the error...
                }
                else
                {
                    DataSnapshot snapshot = task.Result;

                    IDictionary w = (IDictionary)snapshot.Value;

                    armor.ItemName = w["ItemName"].ToString();
                    armor.Type = w["Type"].ToString();
                    armor.Rank = w["Rank"].ToString();
                    armor.Hp = w["Hp"].ToString();
                    armor.Defense = w["Defense"].ToString();
                    armor.Strength = w["Strength"].ToString();
                    armor.Dexterity = w["Dexterity"].ToString();
                    armor.Intelligence = w["Intelligence"].ToString();
                    armor.Info = w["Info"].ToString();
                }
            });
    }

    void WriteWeaponData(string ItemName, string Type, string Rank, string Strength, string Dexterity, string Intelligence, string Damage_Basic, List<object> Name_Skill, string Info, List<object> Info_Skill)
    {
        //Weapon weapon = new Weapon(ItemName, Strength, Dexterity, Intelligence, Damage1, Damage2, Info);
        //string json = JsonUtility.ToJson(weapon);

        //m_Reference.Child("Weapon").Child(ItemId).SetRawJsonValueAsync(json);
        m_Reference.Child("Weapon").Child(ItemName).Child("ItemName").SetValueAsync(ItemName);
        m_Reference.Child("Weapon").Child(ItemName).Child("Type").SetValueAsync(Type);
        m_Reference.Child("Weapon").Child(ItemName).Child("Rank").SetValueAsync(Rank);
        m_Reference.Child("Weapon").Child(ItemName).Child("Strength").SetValueAsync(Strength);
        m_Reference.Child("Weapon").Child(ItemName).Child("Dexterity").SetValueAsync(Dexterity);
        m_Reference.Child("Weapon").Child(ItemName).Child("Intelligence").SetValueAsync(Intelligence);
        m_Reference.Child("Weapon").Child(ItemName).Child("Damage_Basic").SetValueAsync(Damage_Basic);
        m_Reference.Child("Weapon").Child(ItemName).Child("Name_Skill").SetValueAsync(Name_Skill);
        m_Reference.Child("Weapon").Child(ItemName).Child("Info").SetValueAsync(Info);
        m_Reference.Child("Weapon").Child(ItemName).Child("Info_Skill").SetValueAsync(Info_Skill);
    }

    void WriteArmorData(string ItemName, string Type, string Rank, string Hp, string Defense, string Strength, string Dexterity, string Intelligence, string Info)
    {
        m_Reference.Child("Armor").Child(ItemName).Child("ItemName").SetValueAsync(ItemName);
        m_Reference.Child("Armor").Child(ItemName).Child("Type").SetValueAsync(Type);
        m_Reference.Child("Armor").Child(ItemName).Child("Rank").SetValueAsync(Rank);
        m_Reference.Child("Armor").Child(ItemName).Child("Hp").SetValueAsync(Hp);
        m_Reference.Child("Armor").Child(ItemName).Child("Defense").SetValueAsync(Defense);
        m_Reference.Child("Armor").Child(ItemName).Child("Strength").SetValueAsync(Strength);
        m_Reference.Child("Armor").Child(ItemName).Child("Dexterity").SetValueAsync(Dexterity);
        m_Reference.Child("Armor").Child(ItemName).Child("Intelligence").SetValueAsync(Intelligence);
        m_Reference.Child("Armor").Child(ItemName).Child("Info").SetValueAsync(Info);
    }


    public void Write()
    {
        string[] skill = { "Skill_1", "Skill_2" };
        //WriteWeaponData("Staff_Fire", "1", "1", "1", "1", new List<object>(name), "Test", new List<object>(skill));
        //WriteArmorData("Boots_Test","Boots","Normal", "1", "1", "1", "1", "1", "Test");
        //WriteArmorData("Chest_Test", "Chest", "Normal", "1", "1", "1", "1", "1", "Test");
        //WriteArmorData("Head_Test", "Head", "Normal", "1", "1", "1", "1", "1", "Test");
        WriteWeaponData("Rapier","Weapon","Unique", "1", "1", "1", "1", new List<object>(skill), "Test", new List<object>(skill));
        //WriteWeaponData("Katana", "Weapon", "Unique", "1", "1", "1", "1", new List<object>(skill), "Test", new List<object>(skill));
        WriteWeaponData("Armingsword", "Weapon", "Unique", "1", "1", "1", "1", new List<object>(skill), "Test", new List<object>(skill));
    }

    public void Read()
    {
        Weapon w = new Weapon();
        ReadWeaponData("Staff_Fire", w);
        print("Read");
    }


    public void Img_Load()
    {
        //StartCoroutine(imageLoad("https://firebasestorage.googleapis.com/v0/b/fir-test-ccdb7.appspot.com/o/lemongrab.jpg?alt=media&token=36b0e5fa-dba7-4119-8dd4-dc7dce89baa9"));

        //initialize storage reference
        storageReference = FirebaseStorage.DefaultInstance.GetReferenceFromUrl("gs://mytest-415a1.appspot.com");

        //get reference of image
        StorageReference image = storageReference.Child("blade.png");

        //get the download link of file
        image.GetDownloadUrlAsync().ContinueWithOnMainThread(task =>
        {
            if (!task.IsFaulted && !task.IsCanceled)
            {
                StartCoroutine(imageLoad(Convert.ToString(task.Result)));
            }
        });
    }

    IEnumerator imageLoad(string MediaUrl)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl);
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error);
        }
        else
        {
            rawImage.texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
        }
    }
}
