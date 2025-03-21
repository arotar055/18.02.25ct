using System;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using CountryContextLib;
using CountryLib;

namespace DzCounrties_2
{
    public static class RequestsCountries
    {
        public static void ShowAllCountries()
        {
            Console.Clear();
            Console.WriteLine("Список стран:");
            using var db = new CountryContext();
            var countries = db.Countries.Include(c => c.Continent).ToList();
            foreach (var c in countries)
            {
                Console.WriteLine($"Id: {c.Id}, Название: {c.Name}, Площадь: {c.Area}, Континент: {c.Continent?.Name}, Столица: {c.Capital}, Население: {c.Population}");
            }
            Console.WriteLine("Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }

        public static void AddCountry()
        {
            Console.Clear();
            Console.WriteLine("Добавление страны");
            string name = GetValidatedStringInput("Название: ");
            string capital = GetValidatedStringInput("Столица: ");
            long population = GetValidatedPositiveLong("Население: ");
            double area = GetValidatedPositiveDouble("Площадь: ");
            string continentName = GetValidatedStringInput("Континент: ");

            using var db = new CountryContext();
            var existingContinent = db.Continents.FirstOrDefault(c => c.Name != null && c.Name.ToLower() == continentName.ToLower());
            if (existingContinent == null)
            {
                existingContinent = new Continent { Name = continentName };
                db.Continents.Add(existingContinent);
                db.SaveChanges();
            }

            var country = new Country { Name = name, Capital = capital, Population = population, Area = area, Continent = existingContinent };
            db.Countries.Add(country);
            db.SaveChanges();

            Console.WriteLine("Страна добавлена.");
            Console.WriteLine("Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }

        public static void UpdateCountry()
        {
            Console.Clear();
            Console.WriteLine("Обновление данных страны");
            string oldName = GetValidatedStringInput("Название страны для обновления: ");

            using var db = new CountryContext();
            var country = db.Countries.Include(c => c.Continent).FirstOrDefault(c => c.Name != null && c.Name.ToLower() == oldName.ToLower());

            if (country == null)
            {
                Console.WriteLine("Страна не найдена.");
            }
            else
            {
                Console.WriteLine("Оставьте поле пустым для пропуска.");

                Console.Write($"Новое название ({country.Name}): ");
                string newName = Console.ReadLine()!;
                if (!string.IsNullOrWhiteSpace(newName) && IsOnlyLettersAndSpaces(newName))
                    country.Name = newName;

                Console.Write($"Новая столица ({country.Capital}): ");
                string newCapital = Console.ReadLine()!;
                if (!string.IsNullOrWhiteSpace(newCapital) && IsOnlyLettersAndSpaces(newCapital))
                    country.Capital = newCapital;

                Console.Write($"Новое население ({country.Population}): ");
                if (long.TryParse(Console.ReadLine(), out long population) && population > 0)
                    country.Population = population;

                Console.Write($"Новая площадь ({country.Area}): ");
                if (double.TryParse(Console.ReadLine(), out double area) && area > 0)
                    country.Area = area;

                Console.Write($"Новый континент ({country.Continent?.Name}): ");
                string newContinentName = Console.ReadLine()!;
                if (!string.IsNullOrWhiteSpace(newContinentName) && IsOnlyLettersAndSpaces(newContinentName))
                {
                    var newContinent = db.Continents.FirstOrDefault(c => c.Name != null && c.Name.ToLower() == newContinentName.ToLower());
                    if (newContinent == null)
                    {
                        newContinent = new Continent { Name = newContinentName };
                        db.Continents.Add(newContinent);
                        db.SaveChanges();
                    }
                    country.Continent = newContinent;
                }

                db.SaveChanges();
                Console.WriteLine("Данные обновлены.");
            }

            Console.WriteLine("Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }

        public static void DeleteCountry()
        {
            Console.Clear();
            Console.WriteLine("Удаление страны");

            Console.WriteLine("1. По названию");
            Console.WriteLine("2. По столице");
            Console.WriteLine("3. По континенту");
            Console.WriteLine("4. По ID");
            Console.Write("Выберите вариант: ");

            int.TryParse(Console.ReadLine(), out int choice);

            using var db = new CountryContext();
            switch (choice)
            {
                case 1:
                    Console.Write("Название: ");
                    string nameInput = Console.ReadLine()!.ToLower();
                    var toRemoveByName = db.Countries.Where(c => c.Name != null && c.Name.ToLower() == nameInput).ToList();
                    if (toRemoveByName.Any())
                    {
                        db.Countries.RemoveRange(toRemoveByName);
                        db.SaveChanges();
                        Console.WriteLine($"Удалено: {toRemoveByName.Count}");
                    }
                    else Console.WriteLine("Страна не найдена.");
                    break;

                case 2:
                    Console.Write("Столица: ");
                    string capitalInput = Console.ReadLine()!.ToLower();
                    var toRemoveByCapital = db.Countries.Where(c => c.Capital != null && c.Capital.ToLower() == capitalInput).ToList();
                    if (toRemoveByCapital.Any())
                    {
                        db.Countries.RemoveRange(toRemoveByCapital);
                        db.SaveChanges();
                        Console.WriteLine($"Удалено: {toRemoveByCapital.Count}");
                    }
                    else Console.WriteLine("Страна не найдена.");
                    break;

                case 3:
                    Console.Write("Континент: ");
                    string continentInput = Console.ReadLine()!.ToLower();
                    var toRemoveByContinent = db.Countries.Include(c => c.Continent).Where(c => c.Continent != null && c.Continent.Name != null && c.Continent.Name.ToLower() == continentInput).ToList();
                    if (toRemoveByContinent.Any())
                    {
                        db.Countries.RemoveRange(toRemoveByContinent);
                        db.SaveChanges();
                        Console.WriteLine($"Удалено: {toRemoveByContinent.Count}");
                    }
                    else Console.WriteLine("Страна не найдена.");
                    break;

                case 4:
                    Console.Write("ID: ");
                    if (int.TryParse(Console.ReadLine(), out int id))
                    {
                        var countryById = db.Countries.Find(id);
                        if (countryById != null)
                        {
                            db.Countries.Remove(countryById);
                            db.SaveChanges();
                            Console.WriteLine("Страна удалена.");
                        }
                        else Console.WriteLine("Страна не найдена.");
                    }
                    else Console.WriteLine("Некорректный ID.");
                    break;

                default:
                    Console.WriteLine("Некорректный выбор.");
                    break;
            }

            Console.WriteLine("Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }

        private static string GetValidatedStringInput(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input) && IsOnlyLettersAndSpaces(input))
                    return input;

                Console.WriteLine("Ошибка: используйте только буквы и пробелы.");
            }
        }

        private static bool IsOnlyLettersAndSpaces(string s)
        {
            return Regex.IsMatch(s, @"^[A-Za-zА-Яа-я\s\-]+$");
        }

        private static long GetValidatedPositiveLong(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                if (long.TryParse(Console.ReadLine(), out long value) && value > 0)
                    return value;

                Console.WriteLine("Ошибка: введите число больше 0.");
            }
        }

        private static double GetValidatedPositiveDouble(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                if (double.TryParse(Console.ReadLine(), out double value) && value > 0)
                    return value;

                Console.WriteLine("Ошибка: введите положительное число.");
            }
        }
    }
}
