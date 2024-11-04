using System;
using System.Linq;

namespace statistics
{
    class Program
    {
        static void Main(string[] args)
        {
            string[,] data = {
                {"StdNum", "Name", "Math", "Science", "English"},
                {"1001", "Alice", "85", "90", "78"},
                {"1002", "Bob", "92", "88", "84"},
                {"1003", "Charlie", "79", "85", "88"},
                {"1004", "David", "94", "76", "92"},
                {"1005", "Eve", "72", "95", "89"}
            };
            // You can convert string to double by
            // double.Parse(str)

            int stdCount = data.GetLength(0) - 1;
            // ---------- TODO ----------
            double mathaver=0;
            double sciaver=0;
            double Engaver=0;
            
            
            for(int i = 1; i < 6; i++)
            {   
                sciaver+=double.Parse(data[i,3]);
            }
            sciaver/=stdCount;
            for(int i = 1; i < 6; i++)
            {   
                Engaver+=double.Parse(data[i,4]);
            }
            Engaver/=stdCount;
            for(int i = 1; i < 6; i++)
            {   
                mathaver+=double.Parse(data[i,2]);
            }
            mathaver/=stdCount;
            
            
            
            
            double maxMath = double.Parse(data[1, 2]);
            double minMath = double.Parse(data[1, 2]);
            double maxScience = double.Parse(data[1, 3]);
            double minScience = double.Parse(data[1, 3]);
            double maxEnglish = double.Parse(data[1, 4]);
            double minEnglish = double.Parse(data[1, 4]);

            for (int i = 2; i <= stdCount; i++)
            {
                double mathScore = double.Parse(data[i, 2]);
                double scienceScore = double.Parse(data[i, 3]);
                double englishScore = double.Parse(data[i, 4]);

                maxMath = Math.Max(maxMath, mathScore);
                minMath = Math.Min(minMath, mathScore);

                maxScience = Math.Max(maxScience, scienceScore);
                minScience = Math.Min(minScience, scienceScore);

                maxEnglish = Math.Max(maxEnglish, englishScore);
                minEnglish = Math.Min(minEnglish, englishScore);
            }

            
            
            
            double[] totalScores = new double[stdCount];
            string[] names = new string[stdCount];
            int[] ranks = new int[stdCount];

            for (int i = 1; i <= stdCount; i++)
            {
                double mathScore = double.Parse(data[i, 2]);
                double scienceScore = double.Parse(data[i, 3]);
                double englishScore = double.Parse(data[i, 4]);
                totalScores[i - 1] = mathScore + scienceScore + englishScore;
                names[i - 1] = data[i, 1];
                ranks[i - 1] = 1;  
            }

            for (int i = 0; i < stdCount; i++)
            {
                for (int j = 0; j < stdCount; j++)
                {
                    if (totalScores[i] < totalScores[j])
                    {
                        ranks[i]++;
                    }
                }
            }


            Console.WriteLine("Average Scores: ");
            Console.WriteLine($"Math: {mathaver}");
            Console.WriteLine($"Science: {sciaver}");
            Console.WriteLine($"English: {Engaver}");
            Console.WriteLine("Max and min Scores: ");
            Console.WriteLine($"Math: ({maxMath},{minMath})");
            Console.WriteLine($"Science ({maxScience},{minScience})");
            Console.WriteLine($"English ({maxEnglish},{minEnglish})");


            Console.WriteLine("Students rank by total scores:");
            for (int i = 0; i < stdCount; i++)
            {
                string suffix = ranks[i] == 1 ? "st" : ranks[i] == 2 ? "nd" : ranks[i] == 3 ? "rd" : "th";
                Console.WriteLine($"{names[i]}: {ranks[i]}{suffix}");
            }
            // --------------------
        }
    }
}

/* example output

Average Scores: 
Math: 84.40
Science: 86.80
English: 86.20

Max and min Scores: 
Math: (94, 72)
Science: (95, 76)
English: (92, 78)

Students rank by total scores:
Alice: 2nd
Bob: 5th
Charlie: 1st
David: 4th
Eve: 3rd

*/
