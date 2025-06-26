/*
Проект выполнен с выполнением всех требований ТЗ
    - реализована иерархия классов Box и Pallet с расчетом веса, объема и срока годности;
    - добавлены проверки размеров коробок;
    - реализована группировка и сортировка по сроку годности и весу;
    - добавлен генератор данных.
Так как в ТЗ не указано, каким образом генерировать ID паллет и коробок, я использовал обычный int-счетчик. Также можно было использовать генерацию с помощью Guid.
Пока не реализованы юнит-тесты, но я понимаю, как это можно сделать (например с использованием xUnit/NUnit), с тестами расчетов веса, объема, срока годности и проверкой исключений.
Готов обсудить их реализацию на техническом интервью.
*/
using WarehouseApp;
Console.WriteLine(Constants.Separator);
Console.WriteLine(Constants.Intro);
var input = Console.ReadLine();
int palletCnt;
while (string.IsNullOrEmpty(input) || !int.TryParse(input, out palletCnt) || palletCnt <= 0)
{
    Console.WriteLine("Введите корректное число паллет:");
    input = Console.ReadLine();
}
var pallets = Generator.GeneratePallets(palletCnt);
Console.WriteLine($"Создана коллекция из {pallets.Count} паллет.");
Console.WriteLine(Constants.Separator);

var choice = Helper.MakeAChoice();
while (choice != "0")
{
    switch (choice)
    {
        case "1":
            Console.WriteLine("Сгруппировать все паллеты по сроку годности, отсортировать по возрастанию срока годности, в каждой группе отсортировать паллеты по весу:");
            Console.WriteLine(Constants.Separator);
            var palletGroups = Output.PalletSorting(pallets);
            int groupCnt = 1;
            foreach (var palletGroup in palletGroups)
            {
                Console.WriteLine($@"Группа {groupCnt} - Срок годности {palletGroup.Key}");
                foreach (var pallet in palletGroup.Value)
                {
                    Console.WriteLine($@"    ID паллета - {pallet.Id}, вес паллета - {pallet.Weight} кг");
                }
                groupCnt++;
            }
            Console.WriteLine(Constants.Separator);
            break;
        case "2":
            Console.WriteLine("Три паллеты, которые содержат коробки с наибольшим сроком годности, отсортированные по возрастанию объема:");
            var palletList = Output.PalletsWithMaxExpirationDt(pallets);
            foreach (var pallet in palletList)
            {
                Console.WriteLine($@"   ID паллета - {pallet.Id}, объем паллета - {pallet.Volume}");
            }
            Console.WriteLine(Constants.Separator);
            break;
    }
    choice = Helper.MakeAChoice();
}

Console.WriteLine("Завершение работы");

