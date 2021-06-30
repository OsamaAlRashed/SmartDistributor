using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartDistributor.Api.Genetic_Algorithm
{
    public class Chromosome
    {
        // التكلفة
        protected double cost;
        // جينات الكروموزوم
        protected int[] SellerList;
        // احتمال حدوث الطفرة
        protected double mutationPercent;
        // crossover نقطة
        protected int crossoverPoint;

        Random randObj = new Random();
        public double getCost()
        {
            return cost;
        }

        public void setCost(double cost)
        {
            this.cost = cost;
        }

        public int getSeller(int i)
        {
            return SellerList[i];
        }

        // تهيئة الكروموزوم
        public Chromosome(Seller[] Sellers , Seller startPoint)
        {
            bool[] taken = new bool[Sellers.Length];
            SellerList = new int[Sellers.Length];
            cost = 0.0;
            for (int i = 0; i < SellerList.Length; i++)
            {
                taken[i] = false;
            }
            for (int i = 0; i < SellerList.Length - 1; i++)
            {
                int icandidate;
                // اختيار جين غير مكرر
                do
                {
                    icandidate = (int)(randObj.NextDouble() * (double)SellerList.Length);
                } while (taken[icandidate]);
                SellerList[i] = icandidate;
                taken[icandidate] = true;
                // إعطاء  آخر جين الرمز المتبقي
                if (i == SellerList.Length - 2)
                {
                    icandidate = 0;
                    while (taken[icandidate]) icandidate++;
                    SellerList[i + 1] = icandidate;
                }
            }
            calculateCost(Sellers , startPoint);
            crossoverPoint = 1;
        }

        // حساب التكلفة
        public void calculateCost(Seller[] Sellers , Seller startPoint)
        {
            cost = 0;
            cost += startPoint.proximity(Sellers[SellerList[0]]);
            for (int i = 0; i < SellerList.Length - 1; i++)
            {
                double dist = Sellers[SellerList[i]].proximity(Sellers[SellerList[i + 1]]);
                cost += dist;
            }
        }

        
        // عملية التصالب
        public int mate(Chromosome father, Chromosome offspring1, Chromosome offspring2)
        {
            //نقطة البداية
            int crossoverPostion1 = (int)((randObj.NextDouble()) * (double)(SellerList.Length - crossoverPoint));
            // نقطة النهاية
            int crossoverPostion2 = crossoverPostion1 + crossoverPoint;

            int[] offset1 = new int[SellerList.Length];
            int[] offset2 = new int[SellerList.Length];
            bool[] taken1 = new bool[SellerList.Length];
            bool[] taken2 = new bool[SellerList.Length];

            for (int i = 0; i < SellerList.Length; i++)
            {
                taken1[i] = false;
                taken2[i] = false;
            }

            for (int i = 0; i < SellerList.Length; i++)
            {
                // الجينات خارج مجال التصالب
                if (i < crossoverPostion1 || i >= crossoverPostion2)
                {
                    offset1[i] = -1;
                    offset2[i] = -1;
                }
                else
                {
                    int imother = SellerList[i];
                    int ifather = father.getSeller(i);
                    offset1[i] = ifather;
                    offset2[i] = imother;
                    taken1[ifather] = true;
                    taken2[imother] = true;
                }
            }
            
            for (int i = 0; i < crossoverPostion1; i++)
            {
                if (offset1[i] == -1)
                {
                    for (int j = 0; j < SellerList.Length; j++)
                    {
                        int imother = SellerList[j];
                        if (!taken1[imother])
                        {
                            offset1[i] = imother;
                            taken1[imother] = true;
                            break;
                        }
                    }
                }

                if (offset2[i] == -1)
                {
                    for (int j = 0; j < SellerList.Length; j++)
                    {
                        int ifather = father.getSeller(j);
                        if (!taken2[ifather])
                        {
                            offset2[i] = ifather;
                            taken2[ifather] = true;
                            break;
                        }
                    }
                }
            }

            for (int i = SellerList.Length - 1; i >= crossoverPostion2; i--)
            {
                if (offset1[i] == -1)
                {
                    for (int j = SellerList.Length - 1; j >= 0; j--)
                    {
                        int imother = SellerList[j];
                        if (!taken1[imother])
                        {
                            offset1[i] = imother;
                            taken1[imother] = true;
                            break;
                        }
                    }
                }

                if (offset2[i] == -1)
                {
                    for (int j = SellerList.Length - 1; j >= 0; j--)
                    {
                        int ifather = father.getSeller(j);
                        if (!taken2[ifather])
                        {
                            offset2[i] = ifather;
                            taken2[ifather] = true;
                            break;
                        }
                    }
                }
            }

            offspring1.assignSellers(offset1);
            offspring2.assignSellers(offset2);
            int mutate = 0;
            int swapPoint1 = 0;
            int swapPoint2 = 0;


            // طفرة
            if (randObj.NextDouble() < mutationPercent)
            {
                swapPoint1 = (int)(randObj.NextDouble() * (double)(SellerList.Length));
                swapPoint2 = (int)(randObj.NextDouble() * (double)SellerList.Length);
                int i = offset1[swapPoint1];
                offset1[swapPoint1] = offset1[swapPoint2];
                offset1[swapPoint2] = i;
                mutate++;
            }
            if (randObj.NextDouble() < mutationPercent)
            {
                swapPoint1 = (int)(randObj.NextDouble() * (double)(SellerList.Length));
                swapPoint2 = (int)(randObj.NextDouble() * (double)SellerList.Length);
                int i = offset2[swapPoint1];
                offset2[swapPoint1] = offset2[swapPoint2];
                offset2[swapPoint2] = i;
                mutate++;
            }

            return mutate;
        }


        // إرجاع متجر
        public Tuple<Guid , double, double> ReturnSeller(int i, Seller[] Sellers)
        {
            return new Tuple<Guid , double, double>(Sellers[SellerList[i]].getId(), Sellers[SellerList[i]].getx(), Sellers[SellerList[i]].gety());
        }
         
        // ترتيب الكروموزومات حسب الأفضل
        public static void sortChromosomes(Chromosome[] chromosomes, int num)
        {
            bool swapped = true;
            Chromosome dummy;
            while (swapped)
            {
                swapped = false;
                for (int i = 0; i < num - 1; i++)
                {
                    if (chromosomes[i].getCost() > chromosomes[i + 1].getCost())
                    {
                        dummy = chromosomes[i];
                        chromosomes[i] = chromosomes[i + 1];
                        chromosomes[i + 1] = dummy;
                        swapped = true;
                    }
                }
            }
        }

        #region assign 
        public void assignSellers(int[] list)
        {
            for (int i = 0; i < SellerList.Length; i++)
            {
                SellerList[i] = list[i];
            }
        }

        public void assignSeller(int index, int value)
        {
            SellerList[index] = value;
        }

        public void assignCut(int cut)
        {
            crossoverPoint = cut;
        }

        public void assignMutation(double prob)
        {
            mutationPercent = prob;
        }

        #endregion

    }
}
