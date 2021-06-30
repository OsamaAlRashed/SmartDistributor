using System;
using System.Collections.Generic;

namespace SmartDistributor.Api.Genetic_Algorithm
{
    public class GA_TSP
    {
        protected int SellerCount; // عدد المتاجر
        protected int populationSize; // حجم المجتمع
        protected double mutationPercent; // نسبة الطفرة
        protected int matingPopulationSize; // عدد الأزوراج
        protected int favoredPopulationSize; // عدد أفراد النخبة
        protected int cutLength; // طول مجال التصلب
        protected int generation; // رقم الجيل
        protected bool started = false;
        protected Seller[] Sellers;
        protected Chromosome[] chromosomes;
        Chromosome bestAns;
        List<Tuple<Guid ,double, double>> Ans = new List<Tuple<Guid , double, double>>();
        Seller startPoint;
        // توليد الجيل الأول 
        public void Initialization(List<Tuple<Guid , double , double>> positions , Tuple<Guid, double, double> startPoint)
        {
            try
            {
                SellerCount = positions.Count;
                populationSize = 1000;
                mutationPercent = 0.05;
            }
            catch (Exception e)
            {
                SellerCount = 100;
            }
            matingPopulationSize = populationSize / 2;
            favoredPopulationSize = matingPopulationSize / 2;
            cutLength = SellerCount * 20 / 100;
            Sellers = new Seller[SellerCount];
            for (int i = 0; i < SellerCount; i++)
            {
                Sellers[i] = new Seller(positions[i].Item1 , positions[i].Item2, positions[i].Item3);
            }

            this.startPoint = new Seller(startPoint.Item1, startPoint.Item2, startPoint.Item3);
            bestAns = new Chromosome(Sellers, this.startPoint);
            bestAns.setCost(Double.MaxValue);
            // create the initial chromosomes
            chromosomes = new Chromosome[populationSize];

            for (int i = 0; i < populationSize; i++)
            {
                chromosomes[i] = new Chromosome(Sellers , this.startPoint);
                chromosomes[i].assignCut(cutLength);
                chromosomes[i].assignMutation(mutationPercent);
            }
            Chromosome.sortChromosomes(chromosomes, populationSize);
            started = true;
            generation = 0;
        }

        // حساب المسافة الأقصر
        public List<Tuple<Guid , double, double>> TSPCompute()
        {
            double thisCost = 500.0;
            double oldCost = 0.0;
            double dcost = 500.0;
            int countSame = 0;
            int iter = 0;
            Random randObj = new Random();

            // شرط التوقف
            while (countSame < 120) //|| iter < 100
            {
                generation++;
                iter++;
                int ioffset = matingPopulationSize;
                int mutated = 0;
                for (int i = 0; i < favoredPopulationSize; i++)
                {
                    Chromosome cmother = chromosomes[i];
                    int father = (int)(randObj.NextDouble() * (double)matingPopulationSize);
                    Chromosome cfather = chromosomes[father];
                    mutated += cmother.mate(cfather, chromosomes[ioffset], chromosomes[ioffset + 1]);
                    ioffset += 2;
                }

                for (int i = 0; i < matingPopulationSize; i++)
                {

                    chromosomes[i] = chromosomes[i + matingPopulationSize];
                    chromosomes[i].calculateCost(Sellers , startPoint);
                }

                Chromosome.sortChromosomes(chromosomes, matingPopulationSize);
                
                if(bestAns.getCost() > chromosomes[0].getCost())
                {
                    bestAns = chromosomes[0];
                }
                

                double cost = chromosomes[0].getCost();
                dcost = Math.Abs(cost - thisCost);
                thisCost = cost;

                if ((int)thisCost == (int)oldCost)
                {
                    countSame++;
                }
                else
                {
                    countSame = 0;
                    oldCost = thisCost;
                }
            }

            // إرجاع الحل
            for (int i = 0; i < Sellers.Length; i++)
            {
                Ans.Add(bestAns.ReturnSeller(i, Sellers));
            }
            return Ans;
        }
    }

    public class Seller
    {
        private double x;
        private double y;
        private Guid id;

        public Seller(Guid id , double x, double y)
        {
            this.id = id;
            this.x = x;
            this.y = y;
        }

        public double getx()
        {
            return x;
        }
        public double gety()
        {
            return y;
        }
        public Guid getId()
        {
            return id;
        }

        // القرب
        public double proximity(Seller cother)
        {
            return proximity(cother.getx(), cother.gety());
        }
        public double proximity(double x, double y)
        {
            double xdiff = this.x - x;
            double ydiff = this.y - y;
            return Math.Sqrt(xdiff * xdiff + ydiff * ydiff);
        }
    }
}
