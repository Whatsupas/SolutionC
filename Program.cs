using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SolutionC
{
    class Program
    {
        const string inputErrorMessage = "Input should be:" + "\n" + "\n" + 
                                         "The first line of input contains two integers n and k(1 <= k<n <= 100000)" + "\n" +
                                         "The number of potential visiting kittens and the number of beds." + "\n" +
                                         "Then follow n lines, each containing two integers x(i) and y(i)" + "\n" +
                                         "meaning that kitten i wants to arrive at time x(i) and leave at time y(i)." + "\n" +
                                         "This means that two kittens i and j where y(i) = x(i) can use the same bed" + "\n" +
                                         "as one kitten leaves at the same time as the other arrives.You may assume that" + "\n" +
                                         "0 <= x(i) < y(i) <= 1 000 000 000" + "\n" + "\n" +
                                         "Please try again";

        static void Main(string[] args)
        {

            var bookingDictionary = new SortedDictionary<int, int>();
            var isFirstLine = true;
            var potentialVisitors = 0;
            var numberOfBeds = 0;
          
            try
            {
                // Input
                while (true)
                {
                    string inputLine = Console.ReadLine();

                    if (string.IsNullOrEmpty(inputLine))
                        break;

                    if (isFirstLine)
                    {
                        numberOfBeds = int.Parse(inputLine.Split().Last());
                        potentialVisitors = int.Parse(inputLine.Split().First());
                    }
                    else
                    {
                        int checkIn = int.Parse(inputLine.Split().First());
                        int checkOut = int.Parse(inputLine.Split().Last());

                        if (!bookingDictionary.ContainsKey(checkIn))
                            bookingDictionary.Add(checkIn, checkOut);
                        else
                            bookingDictionary[checkIn] = Math.Min(bookingDictionary[checkIn], checkOut);
                    }
                    isFirstLine = false;
                }

                //Output
                int maxForOneBed = CountMaxGuestsForOneBed(bookingDictionary);

                int maxCapacity = maxForOneBed + --numberOfBeds;

                int max = maxCapacity > potentialVisitors ? potentialVisitors : maxCapacity;

                Console.WriteLine(max);
            }
            catch (Exception e)
            {
                Console.WriteLine();
                Console.WriteLine(e.Message);
                Console.WriteLine();
                Console.WriteLine(inputErrorMessage);
            }
        }

         private static int CountMaxGuestsForOneBed(SortedDictionary<int,int> bookingDictionary)
         {
            var maxGuestsForBed = new ConcurrentBag<int>();
            var allCheckins = bookingDictionary.Keys.AsEnumerable();
            var lastCheckIn = allCheckins.Last();

            var parallelOptions = new ParallelOptions()
            {
                MaxDegreeOfParallelism = Environment.ProcessorCount > 1 ?
                                         Environment.ProcessorCount - 1 :
                                         Environment.ProcessorCount
            };

            Parallel.ForEach(bookingDictionary, parallelOptions, booking => 
                {
                    int checkIn = booking.Key;
                    int checkOut = booking.Value;
                    int guests = 1;

                        while (checkOut <= lastCheckIn)
                        {
                            if (bookingDictionary.ContainsKey(checkOut) && !IsAnyCheckinBeforeCurrentCheckOut(checkOut, bookingDictionary[checkOut], allCheckins))
                            {
                                checkIn = checkOut;
                                checkOut = bookingDictionary[checkIn];
                                guests++;

                            }
                            else
                            {
                                checkOut = ++checkOut;
                            }
                        }

                    maxGuestsForBed.Add(guests);

                });
           
            return maxGuestsForBed.Max();
         }

        private static bool IsAnyCheckinBeforeCurrentCheckOut(int currentCheckIn, int currentCheckout, IEnumerable<int> allCheckIns)
        {
            var availableCheckIns = allCheckIns.SkipWhile(c => c <= currentCheckIn);
            var result = availableCheckIns.Any(c => c < currentCheckout);
            return result;
        }
    }
}