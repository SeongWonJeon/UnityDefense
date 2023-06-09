using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu (fileName = "TowerData", menuName = "Data/Tower")]     // creat의 메뉴로 나온다
public class TowerData : ScriptableObject
{
    [SerializeField] TowerInfo[] towers;
    public TowerInfo[] Towers { get { return towers; } }

    [Serializable]
    public class TowerInfo          // 이 값은 변경되면 그값이 게임을 꺼도 유지되니 SerializeField로 변경불가능하도록
    {
        public Tower Tower;
        
        public int damage;
        public float delay;
        public float renge;

        public float buildTime;
        public int buildCost;
        public int sellCost;
    }
}
