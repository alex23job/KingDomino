using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public class ResourseSet
    {
        private int countMoney;
        private int countFood;
        private int countTree;
        private int countIron;
        private int countStone;

        public ResourseSet() { }
        public ResourseSet(int mon, int food, int tree, int iron, int stone)
        {
            countMoney = mon;
            countFood = food;
            countTree = tree;
            countIron = iron;
            countStone = stone;
        }
    }
}
