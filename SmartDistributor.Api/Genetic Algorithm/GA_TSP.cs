using System;
using System.Collections.Generic;

namespace SmartDistributor.Api.Genetic_Algorithm
{
    public class GA_TSP
    {
        protected int SellerCount;
        protected int populationSize;
        protected double mutationPercent;
        protected int matingPopulationSize;
        protected int favoredPopulationSize;
        protected int cutLength;
        protected int generation;
        protected bool started = false;
        protected Seller[] Sellers;
        protected Chromosome[] chromosomes;

        List<Tuple<Guid ,double, double>> Ans = new List<Tuple<Guid , double, double>>();
        public void Initialization(List<Tuple<Guid , double,double>> positions)
        {
            Random randObj = new Random();
            try
            {
                SellerCount = positions.Count;
                populationSize = 50;
                mutationPercent = 0.05;
            }
            catch (Exception e)
            {
                SellerCount = 100;
            }
            matingPopulationSize = populationSize / 2;
            favoredPopulationSize = 1;
            cutLength = SellerCount * 80 / 100;
            Sellers = new Seller[SellerCount];
            for (int i = 0; i < SellerCount; i++)
            {
                Sellers[i] = new Seller(positions[i].Item1 , positions[i].Item2, positions[i].Item3);
            }

            // create the initial chromosomes
            chromosomes = new Chromosome[populationSize];

            for (int i = 0; i < populationSize; i++)
            {
                chromosomes[i] = new Chromosome(Sellers);
                chromosomes[i].assignCut(cutLength);
                chromosomes[i].assignMutation(mutationPercent);
            }
            Chromosome.sortChromosomes(chromosomes, populationSize);
            started = true;
            generation = 0;
        }
        public List<Tuple<Guid , double, double>> TSPCompute()
        {
            double thisCost = 500.0;
            double oldCost = 0.0;
            double dcost = 500.0;
            int countSame = 0;
            int iter = 0;
            Random randObj = new Random();
            while (countSame < 20 || iter > 1000)
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
                    chromosomes[i].calculateCost(Sellers);
                }

                Chromosome.sortChromosomes(chromosomes, matingPopulationSize);
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

            for (int i = 0; i < Sellers.Length; i++)
            {
                Ans.Add(chromosomes[i].ReturnSeller(i, Sellers));
            }
            return Ans;
        }
    }

    public class Seller
    {
        private double xcoord;
        private double ycoord;
        private Guid id;

        public Seller(Guid id , double x, double y)
        {
            xcoord = x;
            ycoord = y;
            this.id = id;

        }

        public double getx()
        {
            return xcoord;
        }
        public double gety()
        {
            return ycoord;
        }
        public Guid getId()
        {
            return id;
        }

        // Returns the distance from the Seller to another Seller.
        public double proximity(Seller cother)
        {
            return proximity(cother.getx(), cother.gety());
        }

        public double proximity(double x, double y)
        {
            double xdiff = xcoord - x;
            double ydiff = ycoord - y;
            return Math.Sqrt(xdiff * xdiff + ydiff * ydiff);
        }
    }
}
