using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.IO;

namespace TSP
{
    public class SimulatedAnnealing
    {
        private string filePath; // ścieżka do pliku
        private List<int> currentOrder = new List<int>(); // aktualne rozwiązanie
        private List<int> nextOrder = new List<int>(); // nowe rozwiązanie
        private double[,] distances; // tabela miast
        private Random random = new Random(); 
        private double shortestDistance = 0; // najlepsza ścieżka

        public double ShortestDistance
        {
            get
            {
                return shortestDistance;
            }
            set
            {
                shortestDistance = value;
            }
        }

        public string FilePath
        {
            get
            {
                return filePath;
            }
            set
            {
                filePath = value;
            }
        }

        public List<int> CitiesOrder
        {
            get
            {
                return currentOrder;
            }
            set
            {
                currentOrder = value;
            }
        }

        public bool Included(int[] tab, int k)
        {
            bool flag = false;
            for (int i = 0; i < tab.Length; i++)
            {
                if (tab[i] == k)
                {
                    flag = false;
                    break;
                }
                else
                {
                    flag = true;
                }
            }
            return flag;
        }

        private void LoadCities()
        {
            StreamReader reader = new StreamReader(filePath);

            string cities = reader.ReadToEnd();

            reader.Close();

            string[] rows = cities.Split('\n');

            distances = new double[rows.Length, rows.Length];
            int x = 0;
            int temp1 = 0;
            int temp2 = 0;

            for (int i = 0; i < rows.Length; i++)
            {
                string[] distance = rows[i].Split(' ');
                temp1 = 0;
                temp2 = 0;
                if (distance[x] == "")
                {
                    x++;
                    temp1++;
                    temp2++;
                    if (i != rows.Length - 1) distance[rows.Length] = distance[rows.Length].Substring(0, distance[rows.Length].Length-1);
                }
                else
                {
                    if (i!= rows.Length-1) distance[rows.Length - 1] = distance[rows.Length - 1].Substring(0, distance[rows.Length - 1].Length - 1);
                }
                // powyzsze sluzy do usuniecie ze stringa znaku nowej linii
                int j;
                for (j = 0; j < distance.Length-temp1; j++,x++)
                {
                    if (double.Parse(distance[x]) < 1)
                    {
                        distances[i, j] = 99999;
                    }
                    else
                    {
                        distances[i, j] = double.Parse(distance[x]);
                    }
                    if (j == distance.Length - 1 - temp2) x = -1;
                    //currentOrder.Add(i);
                }          
            }
            int[] prohibitionTab = new int[rows.Length];
            int index = 0;
            currentOrder.Add(0);
            for (int i = 0; i < rows.Length; i++)
            {
                wroc:
                int k = random.Next(0, rows.Length);
                if (index< rows.Length-1)
                {
                    if (Included(prohibitionTab, k))
                    {
                        prohibitionTab[index] = k;
                        currentOrder.Add(k);
                        index++;
                    }
                    else
                    {
                        goto wroc;
                    }
                }
         
            }
            if (currentOrder.Count < 1)
                throw new Exception("No cities to order.");
        }

        private double GetTotalDistance(List<int> order)
        {
            double distance = 0;

            for (int i = 0; i < order.Count - 1; i++)
            {
                distance += distances[order[i], order[i + 1]];
            }

            if (order.Count > 0)
            {
                distance += distances[order[order.Count - 1], 0];
            }

            return distance;
        }

        private List<int> GetNextArrangement(List<int> order)
        {
            List<int> newOrder = new List<int>();

            for (int i = 0; i < order.Count; i++)
                newOrder.Add(order[i]);

            int firstRandomCityIndex = random.Next(1, newOrder.Count);
            int secondRandomCityIndex = random.Next(1, newOrder.Count);

            int dummy = newOrder[firstRandomCityIndex];
            newOrder[firstRandomCityIndex] = newOrder[secondRandomCityIndex];
            newOrder[secondRandomCityIndex] = dummy;

            return newOrder;
        }

        public void Anneal(double temperature, double coolingRate, double minTemperature)
        {

            int iteration = 0;
            double deltaDistance = 0;

            LoadCities();

            double distance = GetTotalDistance(currentOrder);
            
            while (temperature > minTemperature)
            {
                nextOrder = GetNextArrangement(currentOrder);

                deltaDistance = GetTotalDistance(nextOrder) - distance;

                if ((deltaDistance < 0) || (distance >= 0 && Math.Exp(-deltaDistance/temperature) > random.NextDouble()))
                {
                    for (int i = 0; i < nextOrder.Count; i++)
                        currentOrder[i] = nextOrder[i];

                    distance = deltaDistance + distance;
                }
                temperature *= coolingRate;
                iteration++;
            }
            shortestDistance = distance;
        }
    }
}