using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu (fileName = "TowerData", menuName = "Data/Tower")]     // creat�� �޴��� ���´�
public class TowerData : ScriptableObject
{
    [SerializeField] TowerInfo[] towers;
    public TowerInfo[] Towers { get { return towers; } }

    [Serializable]
    public class TowerInfo          // �� ���� ����Ǹ� �װ��� ������ ���� �����Ǵ� SerializeField�� ����Ұ����ϵ���
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
