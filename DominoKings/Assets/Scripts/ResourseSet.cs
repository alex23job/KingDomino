using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public class ResourseSet
    {
        private int playerID = -1;
        private int countMoney;
        private int countFood;
        private int countTree;
        private int countIron;
        private int countStone;

        public int CountMoney { get { return countMoney; } }
        public int CountFood { get { return countFood; } }
        public int CountTree { get { return countTree; } }
        public int CountIron { get { return countIron; } }
        public int CountStone { get { return countStone; } }

        public ResourseSet() { }
        public ResourseSet(int id, int mon = 0, int food = 0, int tree = 0, int iron = 0, int stone = 0)
        {
            playerID = id;
            countMoney = mon;
            countFood = food;
            countTree = tree;
            countIron = iron;
            countStone = stone;
        }

        public void AddResourse(ResourseSet set)
        {
            countMoney += set.CountMoney;
            countFood += set.CountFood;
            countTree += set.CountTree;
            countIron += set.CountIron;
            countStone += set.CountStone;
        }
        public void AddResourse(int mon = 0, int food = 0, int tree = 0, int iron = 0, int stone = 0)
        {
            countMoney += mon;
            countFood += food;
            countTree += tree;
            countIron += iron;
            countStone += stone;
        }

        public void DecrResourse(ResourseSet set)
        {
            countMoney -= set.CountMoney;
            countFood -= set.CountFood;
            countTree -= set.CountTree;
            countIron -= set.CountIron;
            countStone -= set.CountStone;
        }

        public bool CheckResourse(ResourseSet set)
        {
            if (countMoney < set.CountMoney) return false;
            if (countFood < set.CountFood) return false;
            if (countTree < set.CountTree) return false;
            if (countIron < set.CountIron) return false;
            if (countStone < set.CountStone) return false;
            return true;
        }

        public int[] GetArResourses()
        {
            int[] ar = new int[5] { countMoney, countFood, countTree, countIron, countStone};
            return ar;
        }

        public void InitArResourse(int[] ar)
        {
            if (ar.Length == 5)
            {
                countMoney = ar[0];
                countFood = ar[1];
                countTree = ar[2];
                countIron = ar[3];
                countStone = ar[4];
            }
        }

        public override string ToString()
        {
            return $"m={countMoney} f={countFood} t={countTree} i={countIron} s={countStone}";
        }
    }
}
