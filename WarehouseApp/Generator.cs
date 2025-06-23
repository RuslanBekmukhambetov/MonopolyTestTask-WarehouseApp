using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseApp
{
    internal class Generator
    {
        private static readonly Random _random = new Random();
        public static List<Box> GenerateBoxes (int boxesCnt)
        {
            var boxes = new List<Box>();
            for (int id = 0; id < boxesCnt; id++)
            {
                var randomDouble = _random.NextDouble();
                var boxWidth = Math.Min(Constants.BoxWidth * randomDouble, Constants.PalletWidth);
                var boxHeight = Math.Min(Constants.BoxHeight * randomDouble, Constants.PalletHeight);
                var boxDepth = Math.Min(Constants.BoxDepth * randomDouble, Constants.PalletDepth);
                var boxWeight = Constants.BoxWeight * randomDouble;
                var box = new Box(
                    id,
                    Math.Round(boxWidth, 3), //Math.Round(Constants.BoxWidth * randomDouble, 3),
                    Math.Round(boxHeight, 3), //Math.Round(Constants.BoxHeight * randomDouble, 3),
                    Math.Round(boxDepth, 3), //Math.Round(Constants.BoxDepth * randomDouble, 3),
                    Math.Round(boxWeight, 3),
                    GetRandomDate(),
                    null // Конструктор вычисляет срок годности из даты производства
                );
                boxes.Add( box );
            }
            return boxes;
        }
        public static List<Pallet> GeneratePallets(int palletCnt)
        {
            var pallets = new List<Pallet>();
            for (int id = 0; id < palletCnt; id++)
            {
                int boxCnt = _random.Next(1, 100);
                var boxes = GenerateBoxes(boxCnt);
                var pallet = new Pallet(
                    id,
                    Constants.PalletWidth,
                    Constants.PalletHeight,
                    Constants.PalletDepth,
                    boxes
                );
                pallets.Add( pallet );
            }
            return pallets;
        }

        public static DateTime GetRandomDate()
        {
            if (Constants.DatesForGenerator.Count == 0)
                throw new ArgumentException("Нет дат производства");
            var randomInt = _random.Next(0, Constants.DatesForGenerator.Count);
            return Constants.DatesForGenerator[randomInt];
        }
    }
}
