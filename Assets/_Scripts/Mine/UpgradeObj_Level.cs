﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeObj_Level : MonoBehaviour
{
    [System.Serializable]
    public enum Type
    {
        NONE,
        UPGRADING
    }
    public Type type;

    [Header("UI")]
    public Text txtName;
    public Text txtDescription;
    public Text txtCondition;
    public Image imgCondition;
    public Text txtLv_cur;
    public Text txtLv_next;
    public Text txtCap_cur;
    public Text txtCap_next;
    public Text txtTime_cur;
    public Text txtTime_next;
    public Text txtUnitPrice_cur;
    public Text txtUnitPrice_next;
    public Text txtPrice;
    public MyButton btnUpgrade;

    private MineShaft thisMineShaft;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (thisMineShaft != null && type == Type.UPGRADING)
        {
            txtPrice.text = UIManager.Instance.transformToTime(thisMineShaft.timeUpgradeLevel);
        }
    }

    public void SetInfo(MineShaft _mine, Type _type)
    {
        thisMineShaft = _mine;
        type = _type;
        if (thisMineShaft.properties.level < 6)
        {
            if (thisMineShaft.numberMine >= GameConfig.Instance.lstPropertiesMap[thisMineShaft.ID].Upgrade_condition[thisMineShaft.properties.level - 1])
            {
                //btnUpgrade.thisPrice = thisMineShaft.properties.upgradePrice;
                //txtPrice.text = "Upgrade";// +btnUpgrade.thisPrice;
                btnUpgrade.thisPrice = 0;
                imgCondition.enabled = true;
            }
            else
            {
                btnUpgrade.thisPrice = long.MaxValue;
                btnUpgrade.thisButton.interactable = false;
                imgCondition.enabled = false;
            }
            txtPrice.text = "Upgrade";
            GameManager.Instance.AddGold(0);
            txtName.text = thisMineShaft.properties.name;
            txtDescription.text = "Level " + thisMineShaft.properties.level + " -> " + (thisMineShaft.properties.level + 1);
            txtCondition.text = "Need : " + GameConfig.Instance.lstPropertiesMap[thisMineShaft.ID].Upgrade_condition[thisMineShaft.properties.level - 1] + " mine";
            //txtLv_cur.text = thisMineShaft.properties.level.ToString();
            //txtLv_next.text = (thisMineShaft.properties.level + 1).ToString();
            if (thisMineShaft.properties.level % 2 == 1)
            {
                txtCap_cur.text = thisMineShaft.properties.capacity.ToString();
                txtCap_next.text = thisMineShaft.properties.capacity.ToString();
                txtCap_next.color = Color.white;
                txtTime_cur.text = UIManager.Instance.ToDateTimeString((int)thisMineShaft.properties.miningTime);
                txtTime_next.text = UIManager.Instance.ToDateTimeString((int)(thisMineShaft.properties.miningTime/2));
                txtTime_next.color = Color.yellow;
                
            }
            else
            {
                txtCap_cur.text = thisMineShaft.properties.capacity.ToString();
                txtCap_next.text = (thisMineShaft.properties.capacity*2).ToString();
                txtCap_next.color = Color.yellow;
                txtTime_cur.text = UIManager.Instance.ToDateTimeString((int)thisMineShaft.properties.miningTime);
                txtTime_next.text = UIManager.Instance.ToDateTimeString((int)thisMineShaft.properties.miningTime);
                txtTime_next.color = Color.white;
            }
            
            //txtUnitPrice_cur.text = GameConfig.Instance.lstPropertiesMap[thisMineShaft.ID].Unit_Price[thisMineShaft.properties.level - 1].ToString();
            //txtUnitPrice_next.text = GameConfig.Instance.lstPropertiesMap[thisMineShaft.ID].Unit_Price[thisMineShaft.properties.level].ToString();
            btnUpgrade.type = MyButton.Type.GOLD;

            if (type == Type.UPGRADING)
            {
                btnUpgrade.thisButton.onClick.RemoveAllListeners();
                btnUpgrade.thisButton.onClick.AddListener(() =>
                {
                    UIManager.Instance.ShowPanelCoinAds(10, () => UpgradeCoin());
                });
            }
        }
        else
        {
            btnUpgrade.thisPrice = long.MaxValue;
            btnUpgrade.thisButton.interactable = false;
            txtPrice.text = "Max";
            txtDescription.text = "Max level";
        }
    }

    public void Upgrading()
    {
        type = Type.UPGRADING;
    }

    public void Upgrade()
    {
        thisMineShaft.Btn_UpgradeLevel();

        btnUpgrade.thisButton.onClick.RemoveAllListeners();
        btnUpgrade.thisButton.onClick.AddListener(() =>
        {
            UIManager.Instance.ShowPanelCoinAds(10, () => UpgradeCoin());
        });
    }

    void UpgradeCoin()
    {
        thisMineShaft.UpgradeLevel_Coin();

        btnUpgrade.thisButton.onClick.RemoveAllListeners();
        btnUpgrade.thisButton.onClick.AddListener(() => Upgrade());
    }
}
