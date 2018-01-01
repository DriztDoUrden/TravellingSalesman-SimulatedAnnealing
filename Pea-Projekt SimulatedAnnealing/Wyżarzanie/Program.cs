using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TSP
{
    class Program
    {
        static void Main(string[] args)
        {
            SimulatedAnnealing problem = new SimulatedAnnealing();
            //problem.FilePath = "Cities.txt";

            //double srednia = 0;
            //double srednia2 = 9999999;
            //int ilosc_powtorzen = 10;

            //// do liczenia mediany:
            //ArrayList mediana = new ArrayList();
            //ArrayList times = new ArrayList();

            //TimeSpan diff = new TimeSpan();
            //for (int i = 0; i < ilosc_powtorzen; i++)
            //{
            //    TravellingSalesmanProblem problem = new TravellingSalesmanProblem();
            //    problem.FilePath = "Cities.txt";
            //    var startTime = DateTime.Now;
            //    problem.Anneal();
            //    var stopTime = DateTime.Now;
            //    diff = stopTime - startTime;
            //    srednia += problem.ShortestDistance;
            //    mediana.Add(problem.ShortestDistance);
            //    times.Add(diff);
            //    if (srednia2 > problem.ShortestDistance) srednia2 = problem.ShortestDistance;
            //    Console.WriteLine("Bump " + i);
            //}

            //mediana.Sort();
            //times.Sort();
            //TimeSpan czas = (TimeSpan)times[ilosc_powtorzen/2];
            //Console.WriteLine("Mediana: " + mediana[ilosc_powtorzen / 2]);
            //Console.WriteLine("Średni czas pracy: " + (czas.TotalMilliseconds) / 1000 +"s");
            //srednia /= ilosc_powtorzen;
            //Console.WriteLine("Srednia: " + srednia);

            //Console.WriteLine("Najlepsza sciezka koszt: " + srednia2);
            problem.FilePath = "Cities.txt";

            double coolingRate = 0.99999;
            double minTemperature = 0.000000001;
            double temperature = 100000000;

            problem.Anneal(temperature,coolingRate,minTemperature);
            string path = "";
            for (int i = 0; i < problem.CitiesOrder.Count - 1; i++)
            {
                path += problem.CitiesOrder[i] + " -> ";
            }
            path += problem.CitiesOrder[problem.CitiesOrder.Count - 1];

            Console.WriteLine("Shortest path: " + path);

            Console.WriteLine("The shortest distance is: " + problem.ShortestDistance.ToString());

            Console.ReadLine();
        }
    }
}
