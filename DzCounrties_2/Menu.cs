using System;

namespace DzCounrties_2
{
    public static class Menu
    {
        public static int GetUserChoice()
        {
            while (true)
            {
                Console.WriteLine("Выберите действие:");
                Console.WriteLine("1) Вывести всю информацию о странах");
                Console.WriteLine("2) Добавить новую страну");
                Console.WriteLine("3) Обновить данные о стране");
                Console.WriteLine("4) Удалить страну (по критерию)");
                Console.WriteLine("0) Выход");
                Console.Write("Ваш выбор: ");

                string? input = Console.ReadLine();
                if (int.TryParse(input, out int choice))
                {
                    if (choice >= 0 && choice <= 4)
                        return choice;
                }
                Console.WriteLine("Повторно введите!\n");
            }
        }
    }
}