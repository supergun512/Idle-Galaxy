﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EventDispatcher;

public class UpgradeObj_Special : MonoBehaviour
{
    [System.Serializable]
    public enum Type
    {
        NONE,
        UPGRADING,
        UPGRADED
    }

    public int ID;
    public Type type;
    public Text txtName;
    public Text txtDescription;
    public Text txtTime;
    public Text txtPrice;
    public Text txtCoin;
    public MyButton btnBuy;
    public MyButton btnBuyNow;
    public Button btnBuyAds;
    private MineShaft thisMineShaft;
    public GameObject panelTime;
    int coin;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (thisMineShaft != null && type == Type.UPGRADING)
        {
            txtTime.text = transformToTime(thisMineShaft.timeUpgradeSpecial[ID]);
        }
    }

    string transformToTime(float time = 0)
    {
        int minutes = Mathf.FloorToInt(time / 60F);
        int seconds = Mathf.FloorToInt(time - minutes * 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void SetInfo(int _id, MineShaft _mineShaft, string _name, string _description, long _price, UpgradeObj_Special.Type _type, int _coin)
    {
        this.gameObject.SetActive(true);
        this.ID = _id;
        txtName.text = _name;
        txtDescription.text = _description;
        txtPrice.text = UIManager.Instance.ToLongString(_price);
        type = _type;
        thisMineShaft = _mineShaft;
        btnBuy.thisPrice = _price;
        btnBuy.type = MyButton.Type.GOLD;
        coin = _coin;
        btnBuyNow.thisPrice = coin;
        btnBuyNow.type = MyButton.Type.COIN;
        txtCoin.text = UIManager.Instance.ToLongString(coin);
        
        GameManager.Instance.AddGold(0);
        switch (type)
        {
            case Type.NONE:
                panelTime.SetActive(false);
                break;
            case Type.UPGRADING:
                panelTime.SetActive(true);
                break;
            case Type.UPGRADED:
                btnBuy.thisPrice = long.MaxValue;
                SetBought();
                break;
            default:
                break;
        }
    }

    public void SetBought()
    {
        panelTime.SetActive(false);
        btnBuy.thisButton.interactable = false;
        txtDescription.text = "Bought !!!";
    }

    public void BtnBuy()
    {
        thisMineShaft.BuySpecialUpgrade(this.ID);

        panelTime.SetActive(true);
    }

    public void BtnBuyNow()
    {
        thisMineShaft.UpgradeCoin(this.ID, coin);
    }
    public void BtnBuyAds()
    {
        thisMineShaft.UpgradeAds(this.ID);
    }
}
