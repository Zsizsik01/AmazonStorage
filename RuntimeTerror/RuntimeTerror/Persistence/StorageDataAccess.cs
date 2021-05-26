using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using RuntimeTerror.Model;

namespace RuntimeTerror.Persistence
{
    public class StorageDataAccess
    {
        public Controller controller { get; set; }

        public static async Task<Controller> LoadAsync(string path)
        {
            try
            {
                using (StreamReader reader = new StreamReader(path)) // fájl megnyitása
                {
                    string line = await reader.ReadLineAsync();
                    string[] numbers = line.Split(" ");
                    int sizeX = Int32.Parse(numbers[0]);
                    int sizeY = Int32.Parse(numbers[1]);
                    Controller loaded = new Controller();

                    //storage
                    Storage str = new Storage();
                    str.SizeX = sizeX;
                    str.SizeY = sizeY;
                    str.Matrix = new Field[sizeX, sizeY];
                    for(int i = 0; i < sizeX; ++i)
                    {
                        string numbLine = await reader.ReadLineAsync();
                        string[] numbersLine = numbLine.Split(" ");
                        for (int j = 0; j < sizeY; ++j)
                        {
                            str.Matrix[i, j] = (Model.Field)(Int32.Parse(numbersLine[j])); //cast
                        }
                    }
                    loaded.Storage = str;

                    //robots
                    line = await reader.ReadLineAsync();
                    numbers = line.Split(" ");
                    int robots = Int32.Parse(numbers[0]);
                    int maxEnergy = Int32.Parse(numbers[1]);
                    loaded.MaxEnergy = maxEnergy;
                    loaded.Robots = new List<Robot>();
                    for (int i = 0; i < robots; ++i){
                        line = await reader.ReadLineAsync();
                        numbers = line.Split(" ");
                        loaded.Robots.Add(new Robot(Int32.Parse(numbers[0]), Int32.Parse(numbers[1]),(Model.Direction)(Int32.Parse(numbers[2])),maxEnergy));
                        loaded.Result.Energy.Add(0);
                    }
                    
                    
                    //itemedshelfs
                    line = await reader.ReadLineAsync();
                    numbers = line.Split(" ");
                    int itemedShelf = Int32.Parse(numbers[0]);
                    loaded.ItemedShelfs = new List<Shelf>();
                    for(int i = 0; i < itemedShelf; ++i)
                    {
                        line = await reader.ReadLineAsync();
                        numbers = line.Split(" ");
                        List<int> targets = new List<int>();
                        for(int j = 3; j < 3 + Int32.Parse(numbers[2]); ++j)
                        {
                            targets.Add(Int32.Parse(numbers[j]));
                        }
                        loaded.ItemedShelfs.Add(new Shelf(Int32.Parse(numbers[0]),Int32.Parse(numbers[1]),targets));
                    }


                    //docks
                    line = await reader.ReadLineAsync();
                    numbers = line.Split(" ");
                    int docks = Int32.Parse(numbers[0]);
                    loaded.Docks = new List<Dock>();
                    for(int i = 0; i < docks; ++i)
                    {
                        line = await reader.ReadLineAsync();
                        numbers = line.Split(" ");
                        loaded.Docks.Add(new Dock(Int32.Parse(numbers[0]), Int32.Parse(numbers[1])));
                    }


                    //destinations
                    line = await reader.ReadLineAsync();
                    numbers = line.Split(" ");
                    int dest = Int32.Parse(numbers[0]);
                    loaded.Destinations = new List<Destination>();
                    for (int i = 0; i < dest; ++i)
                    {
                        line = await reader.ReadLineAsync();
                        numbers = line.Split(" ");
                        loaded.Destinations.Add(new Destination(Int32.Parse(numbers[0]), Int32.Parse(numbers[1]), Int32.Parse(numbers[2])));
                    }
                    loaded.Item = loaded.Destinations.Count;


                    //speed
                    line = await reader.ReadLineAsync();
                    numbers = line.Split(" ");
                    loaded.Speed = Int32.Parse(numbers[0]);

                    return loaded;
                }
            }
            catch
            {
                throw new Exception("File megnyitas kivétel keletkezett.");
            }
        }
        /// <summary>
        /// Mentési stratégia:
        /// !matrix X merete! !matrix Y merete!
        /// !matrix elemei szokozokkel elvalasztva X*Y!
        /// !robotok szama! !robotok maxenergiaja!
        /// !egy sor = egy robot: Xkord Ykord dir!
        /// !arus polcok szama!
        /// !egy sor = egy arus polc : Xkord Ykord ArukSzama Aru1 Aru2..!
        /// !dockolok szama!
        /// !egy sor = egy dock: Xkord Ykord!
        /// !celallomasok szama!
        /// !egy sor = egy celallomas : Xkord Ykord ItemId
        /// !gyorsasag!
        /// </summary>
        /// <param name="path"></param>
        /// <param name="controller"></param>
        /// <returns></returns>
        public static async Task SaveAsync(String path, Controller controller)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(path)) // fájl megnyitása
                {
                    writer.Write(controller.Storage.SizeX+" "+ controller.Storage.SizeY+" \n"); //elmentjük a méretét
                    for(int i=0;i< controller.Storage.SizeX;++i) 
                    {
                        for(int j = 0; j < controller.Storage.SizeY; ++j)
                        {
                            await writer.WriteAsync((int)controller.Storage.Matrix[i,j] + " ");
                        }
                        await writer.WriteAsync("\n");
                    }

                    writer.Write(controller.Robots.Count +" " + controller.MaxEnergy +" \n");
                    foreach(Robot rob in controller.Robots)
                    {
                        await writer.WriteAsync(rob.PositionX +" " + rob.PositionY + " " + (int)rob.Direction +" \n");
                    }
                    writer.Write(controller.ItemedShelfs.Count + " \n");
                    foreach (Shelf shelf in controller.ItemedShelfs)
                    {
                        string ashelf = shelf.HomePositionX + " " + shelf.HomePositionY + " " + shelf.Targets.Count+" ";
                        foreach(int item in shelf.Targets)
                        {
                            ashelf += item + " ";
                        }
                        await writer.WriteAsync(ashelf +"\n");
                    }
                    writer.Write(controller.Docks.Count + " \n");
                    foreach(Dock dock in controller.Docks)
                    {
                        await writer.WriteAsync(dock.PositionX +" " + dock.PositionY + " \n");
                    }
                    writer.Write(controller.Destinations.Count + " \n");
                    foreach (Destination dest in controller.Destinations)
                    {
                        await writer.WriteAsync(dest.PositionX + " " + dest.PositionY + " " + dest.Item+ " \n");
                    }
                    await writer.WriteAsync(controller.Speed+"");
                }
            }
            catch
            {
                throw new Exception("File mentes kozben kivetel keletkezett.");
            }
        }

        
    }
}
