using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public class Loskut
    {
        public Loskut() { }
        public Loskut(int li) { landID = li; }

        int landID = -1;
        List<HalfData> arHT = new List<HalfData>();
        int countPlayerHalf = 0;
        int countBotHalf = 0;

        public int CountBotHalf { get { return countBotHalf; } }
        public int CountPlayerHalf {  get { return countPlayerHalf; } }

        public int LandID { get { return landID; } }
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
        }
    }

    public struct TailPos2
    {
        public int hf1;
        public int hf2;
        public int countHalf;

        public TailPos2(int n1, int n2, int ch = 0)
        {
            hf1 = n1;
            hf2 = n2;
            countHalf = ch;
        }
    }
}
