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
    public Animator animCondition;
    public Sprite sprConditionOff;
    public Sprite sprConditionOn;
    public Image imgCondition;
    public Animator animCap;
    public Text txtCap_cur;
    public Text txtCap_next;
    public Animator animTime;
    public Text txtTime_cur;
    public Text txtTime_next;
    //public Animator animPrice;
    public Text txtUnitPrice_cur;
    public Text txtUnitPrice_next;
    public Text txtPrice;
    public Button btnUpgrade;
    private MineShaft thisMineShaft;
    public Image CapLine, TimeLine;
    public Sprite CapDefaultSprite, TimeDefaultSprite;
    public Sprite CapLineDefaultSprite, TimeLineDefaultSprite;

    // Use this for initialization
    void Awake()
    {
        this.CapDefaultSprite = this.animCap.GetComponent<Image>().sprite;
        this.TimeDefaultSprite = this.animTime.GetComponent<Image>().sprite;
        this.CapLineDefaultSprite = this.CapLine.sprite;
        this.TimeLineDefaultSprite = this.TimeLine.sprite;
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
        this.btnUpgrade.interactable = true;
        thisMineShaft = _mine;
        type = _type;
        if (thisMineShaft.properties.level < 6)
        {
            if (thisMineShaft.numberMine >= GameConfig.Instance.lstPropertiesMap[thisMineShaft.ID].Upgrade_condition[thisMineShaft.properties.level - 1])
            {
                //btnUpgrade.thisPrice = thisMineShaft.properties.upgradePrice;
                //txtPrice.text = "Upgrade";// +btnUpgrade.thisPrice;
                //btnUpgrade.thisPrice = 0;
                imgCondition.enabled = true;
                animCondition.enabled = true;
                animCondition.gameObject.GetComponent<Image>().sprite = sprConditionOn;
            }
            else
            {
                /*btnUpgrade.thisPrice = long.MaxValue;
                btnUpgrade.thisButton.interactable = false;*/
                this.btnUpgrade.interactable = false;
                imgCondition.enabled = false;
                animCondition.enabled = false;
                animCondition.gameObject.GetComponent<Image>().sprite = sprConditionOff;
            }
            txtPrice.text = "Upgrade";
            GameManager.Instance.AddGold(0);
            txtName.text = thisMineShaft.properties.name;
            txtDescription.text = "Level " + thisMineShaft.properties.level + " -> " + (thisMineShaft.properties.level + 1);
            txtCondition.text = "Need : " + GameConfig.Instance.lstPropertiesMap[thisMineShaft.ID].Upgrade_condition[thisMineShaft.properties.level - 1] + " mine";
            if (thisMineShaft.properties.level % 2 == 1)
            {
                animCap.enabled = false;
                if (this.CapDefaultSprite != null) this.animCap.GetComponent<Image>().sprite = this.CapDefaultSprite;
                if (this.CapLineDefaultSprite != null) this.CapLine.sprite = this.CapLineDefaultSprite;
                txtCap_cur.text = thisMineShaft.properties.capacity.ToString();
                txtCap_next.text = thisMineShaft.properties.capacity.ToString();
                txtCap_next.color = Color.white;

                animTime.enabled = true;
                txtTime_cur.text = UIManager.Instance.ToDateTimeString((int)thisMineShaft.properties.miningTime);
                txtTime_next.text = UIManager.Instance.ToDateTimeString((int)(thisMineShaft.properties.miningTime/2));
                txtTime_next.color = Color.yellow;               
            }
            else
            {
                animCap.enabled = true;
                txtCap_cur.text = thisMineShaft.properties.capacity.ToString();
                txtCap_next.text = (thisMineShaft.properties.capacity*2).ToString();
                txtCap_next.color = Color.yellow;

                animTime.enabled = false;
                if (this.TimeDefaultSprite != null) this.animTime.GetComponent<Image>().sprite = this.TimeDefaultSprite;
                if (this.TimeLineDefaultSprite != null) this.TimeLine.sprite = this.TimeLineDefaultSprite;
                txtTime_cur.text = UIManager.Instance.ToDateTimeString((int)thisMineShaft.properties.miningTime);
                txtTime_next.text = UIManager.Instance.ToDateTimeString((int)thisMineShaft.properties.miningTime);
                txtTime_next.color = Color.white;
            }

            txtUnitPrice_cur.text = UIManager.Instance.ToLongString(thisMineShaft.properties.unitPrice);
            txtUnitPrice_next.text = UIManager.Instance.ToLongString(thisMineShaft.properties.unitPrice);
            //btnUpgrade.type = MyButton.Type.GOLD;

            if (type == Type.UPGRADING)
            {
                UIManager.Instance.ShowPanelCoinAds(10, thisMineShaft.maxTimeUpgradeLevel, thisMineShaft.timeUpgradeLevel, () => UpgradeCoin(), () => this.UpgradeByAds());
            }
        }
        else
        {
            //btnUpgrade.thisPrice = long.MaxValue;
            btnUpgrade.interactable = false;
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
        btnUpgrade.onClick.RemoveAllListeners();
        UIManager.Instance.ShowPanelCoinAds(10, thisMineShaft.maxTimeUpgradeLevel, thisMineShaft.timeUpgradeLevel, () => UpgradeCoin(), () => this.UpgradeByAds());
    }

    void UpgradeCoin()
    {
        thisMineShaft.UpgradeLevel_Coin();
        btnUpgrade.onClick.RemoveAllListeners();
        btnUpgrade.onClick.AddListener(() => Upgrade());
    }

    void UpgradeByAds()
    {
        thisMineShaft.timeUpgradeLevel -= GameConfig.Instance.TimeReductionByWatchAds;
        if (thisMineShaft.timeUpgradeLevel < 0f) thisMineShaft.timeUpgradeLevel = 0f;
    }
}
