using System;

namespace DzCounrties_2
{
    internal class Program
    {
        static void Main()
        {
            try
            {
                while (true)
                {
                    Console.Clear();
                    int choice = Menu.GetUserChoice();
                    switch (choice)
                    {
                        case 1:
                            RequestsCountries.ShowAllCountries(); 
                            break;
                        case 2:
                            RequestsCountries.AddCountry();
                            break;
                        case 3:
                            RequestsCountries.UpdateCountry();
                            break;
                        case 4:
                            RequestsCountries.DeleteCountry();
                            break;
                        case 0:
                            Console.WriteLine("Завершить работу программы");
                            return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
                Console.ReadKey();
            }
        }
    }
}