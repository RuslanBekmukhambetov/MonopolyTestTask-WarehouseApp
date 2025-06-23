// See https://aka.ms/new-console-template for more information
using WarehouseApp;

Console.WriteLine("Укажите количество паллет:");
string? input = Console.ReadLine();
int palletCnt = 0;
while (!int.TryParse(input, out palletCnt) || palletCnt < 1)
{
    Console.WriteLine("Укажите корректное количество паллет:");
    input = Console.ReadLine();
}
Console.WriteLine("Генерация паллет и коробок.");
var pallets = Generator.GeneratePallets(palletCnt);
Console.WriteLine($"Создана коллекция из {pallets.Count} паллет");
Console.WriteLine(@"Выберите действие:\n" +
                  "1. сгруппировать все паллеты по сроку годности, отсортировать по возрастанию срока годности, в каждой группе отсортировать паллеты по весу;\n" +
                  "2. 3 паллеты, которые содержат коробки с наибольшим сроком годности, отсортированные по возрастанию объема;\n" +
                  "0. завершение работы.");
string? choice = "default";
choice = Console.ReadLine();
while (string.IsNullOrEmpty(choice) || !int.TryParse(choice, out var _))
{
    Console.WriteLine("Выберите корректный вариант.");
    choice = Console.ReadLine();
} 
while (choice != "0")
{
    switch (choice)
    {
        case "1":
            var palletGroups = Output.PalletSorting(pallets);
            int groupCnt = 1;
            foreach (var palletGroup in palletGroups)
            {
                Console.WriteLine($"Группа {groupCnt} - Срок годности {palletGroup.Key}");
                foreach (var pallet in palletGroup.Value)
                {
                    Console.WriteLine($"ID - {pallet.Id}, вес - {pallet.Weight} кг");
                }
                groupCnt++;
            }
            break;
        case "2":
            var palletList = Output.PalletsWithMaxExpirationDt(pallets);
            foreach (var pallet in palletList)
            {
                Console.WriteLine($"ID - {pallet.Id}, объем - {pallet.Volume}");
            }
            break;
    }
    Console.WriteLine("Выберите действие:\n" +
                  "1. сгруппировать все паллеты по сроку годности, отсортировать по возрастанию срока годности, в каждой группе отсортировать паллеты по весу;\n" +
                  "2. 3 паллеты, которые содержат коробки с наибольшим сроком годности, отсортированные по возрастанию объема;\n" +
                  "0. завершение работы.");
    choice = Console.ReadLine();
    while (string.IsNullOrEmpty(choice) || !int.TryParse(choice, out var _))
    {
        Console.WriteLine("Выберите корректный вариант.");
        choice = Console.ReadLine();
    }
}

Console.WriteLine("Завершение работы");

