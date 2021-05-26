using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using RuntimeTerror.Model;

namespace RuntimeTerror.Persistence
{
    public class ResultDataAccess
    {
        public ResultDiary result { get; set; }

       /* public static async Task<ResultDiary> LoadAsync(string path)
        {
            try
            {
                using (StreamReader reader = new StreamReader(path)) // fájl megnyitása
                {
                    string line = await reader.ReadLineAsync();
                    string[] numbers = line.Split(" ");
                    int robotCount = Int32.Parse(numbers[0]);
                    ResultDiary loaded = new ResultDiary();
                    List<int> energy = new List<int>();
                    for (int i = 0; i < robotCount; ++i)
                    {
                        line = await reader.ReadLineAsync();
                        string[] numbersLine = line.Split(" ");
                        energy[i] = (Int32.Parse(numbersLine[i])); 
                        
                    }
                    loaded.Energy = energy;

                    line = await reader.ReadLineAsync();
                    numbers = line.Split(" ");
                    int energySum = Int32.Parse(numbers[0]);
                    loaded.EnergySum = energySum;

                    line = await reader.ReadLineAsync();
                    numbers = line.Split(" ");
                    int steps = Int32.Parse(numbers[0]);
                    loaded.Steps = steps;

                    return loaded;
                }
            }
            catch
            {
                throw new Exception("File megnyitas kivétel keletkezett.");
            }
        }*/
        /// <summary>
        /// Mentési stratégia:
        /// !robotok=mentett enrgiák szama! 
        /// robotok által egyesével elhasznált energia
        /// összes elhasznált energia
        /// lépések száma
        /// </summary>
        /// <param name="path"></param>
        /// <param name="controller"></param>
        /// <returns></returns>
        public static async Task SaveAsync(ResultDiary result, string path = @".\ResultData\result.txt" )
        {
            CheckAndMakeFolder();
            try
            {
                using (StreamWriter writer = File.AppendText(path))
                {
                    await writer.WriteAsync(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss |  \n"));
                    await writer.WriteAsync(" \trobotok száma: " + result.Energy.Count );
                    await writer.WriteAsync(" \n\tösszes elhasznált energia: " + result.EnergySum );
                    await writer.WriteAsync(" \n\tlépések száma: " + result.Steps );
                    await writer.WriteAsync(" \n\trobotonként elhasznált energia mennyisége: ");
                    for (int i = 0; i < result.Energy.Count; ++i)
                    {
                        await writer.WriteAsync("\n\t\t"+(i+1) +". robot: "+result.Energy[i] + " energia; ");   
                    }
                    await writer.WriteAsync("\n\n");
                }
            }
            catch (Exception e)
            {
                throw new Exception("File mentes kozben kivetel keletkezett. \n"+e.Message);
            }
        }

        public static void CheckAndMakeFolder(string path = @".\ResultData")
        {
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
        }


    }
}
