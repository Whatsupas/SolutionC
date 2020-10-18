using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SolutionC
{
    class Program
    {
        static void Main(string[] args)
        {

            SortedDictionary<int, int> bookingDictionary = new SortedDictionary<int, int>();

            bool firstLine = true;
            int potentialVisitors = 0;
            int numberOfBeds = 0;


            while (true)
            {
                string inputLine = Console.ReadLine();

                if (!String.IsNullOrEmpty(inputLine))
                {

                    if (firstLine == true)
                    {
                        numberOfBeds = int.Parse(inputLine.Split(' ').Last());
                        potentialVisitors = int.Parse(inputLine.Split(' ').First());
                    }
                    else
                    {
                        int checkIn = int.Parse(inputLine.Split(' ').First());
                        int checkOut = int.Parse(inputLine.Split(' ').Last());

                        if (!bookingDictionary.ContainsKey(checkIn))
                        {
                            bookingDictionary.Add(checkIn, checkOut);
                        }
                        else
                        {
                            bookingDictionary[checkIn] = Math.Min(bookingDictionary[checkIn], checkOut);
                        }

                    }
                    firstLine = false;
                }
                else
                {
                    break;
                }
            }

            int maxForOneBed = CountMaxGuestsForOneBed(bookingDictionary);

            int maxCapacity = maxForOneBed + --numberOfBeds;
            int max = maxCapacity > potentialVisitors ? potentialVisitors : maxCapacity;

            Console.WriteLine(max);
        }


         private static int CountMaxGuestsForOneBed(SortedDictionary<int,int> bookingDictionary)
         {
            ConcurrentBag<int> maxGuestsForBed = new ConcurrentBag<int>();
            List<int> allCheckins = bookingDictionary.Keys.ToList();
            int lastCheckIn = allCheckins.Last();


                Parallel.ForEach(bookingDictionary, booking => 
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

        private static bool IsAnyCheckinBeforeCurrentCheckOut(int currentCheckIn, int currentCheckout, List<int> allCheckIns)
        {
            var availableCheckIns = allCheckIns.SkipWhile(c => c <= currentCheckIn);
            bool result = availableCheckIns.Any(c => c < currentCheckout);
            return result;
        }

    }
}