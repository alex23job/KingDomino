using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public class Loskut
    {
        public Loskut() 
        {
            landID = -1;
            arHT = new List<HalfData>();
        }
        public Loskut(HalfData half) 
        {
            landID = half.LandID;
            arHT = new List<HalfData>();
            AddHalf(half);
        }

        int landID = -1;
        List<HalfData> arHT;
        int countPlayerHalf = 0;
        int countBotHalf = 0;

        public int CountBotHalf { get { return countBotHalf; } }
        public int CountPlayerHalf {  get { return countPlayerHalf; } }

        public int LandID { get { return landID; } }

        /// <summary>
        /// Возвращает число звёзд у игрока или бота
        /// </summary>
        /// <param name="mode">Для кого считаем звёзды: 1 - Player, 2 - Bot</param>
        /// <returns>число звёзд</returns>
        public int GetStars(int mode)
        {
            int sumStars = 0;
            foreach(HalfData hd in arHT)
            {
                if (mode == hd.NumPlayer) sumStars += hd.Stars;
            }
            return sumStars;
        }

        public List<HalfData> GetAr()
        {
            return arHT;
        }

        public void AddHalf(HalfData halfData)
        {
            arHT.Add(halfData);
            if (halfData.NumPlayer == 1) countPlayerHalf++;
            if (halfData.NumPlayer == 2) countBotHalf++;
        }

        public bool IsContains(int n)
        {
            foreach(HalfData hd in arHT)
            {
                if (hd.NumPos == n) return true;
            }
            return false;
        }

        public void AddLoskut(Loskut los)
        {
            List<HalfData> ar = los.GetAr();
            arHT.AddRange(ar);
            countPlayerHalf += los.countPlayerHalf;
            countBotHalf += los.countBotHalf;
        }

        /// <summary>
        /// Функция возвращает набор ресурсов, полученных на участках этой области
        /// для игрока - mode = 1, для бота - mode = 2
        /// </summary>
        /// <param name="mode">1 - игрок, 2 - бот</param>
        /// <returns>суммарный набор ресурсов</returns>
        public ResourseSet GetResourses(int mode)
        {
            ResourseSet res = new ResourseSet(0);
            foreach(HalfData hd in arHT)
            {
                if (hd.NumPlayer == mode)
                {
                    res.AddResourse(hd.GetLandResourse());
                    res.AddResourse(hd.GetBuildResourses());
                }
            }
            return res;
        }

        public override string ToString()
        {
            return $"landID={landID}    cnt={arHT.Count}   pl_H={countPlayerHalf}   bot_H={countBotHalf}";
        }
    }

    public class TailPos2
    {
        public int hf1;
        public int hf2;
        public int countHalf;
        public int land;

        public TailPos2(int n1, int n2, int ch = 0, int l = 0)
        {
            hf1 = n1;
            hf2 = n2;
            countHalf = ch;
            land = l;
        }

        public override string ToString()
        {
            return $"hf1={hf1}  hf2={hf2}  countHalf={countHalf}  landID={land}";
        }
    }
}
