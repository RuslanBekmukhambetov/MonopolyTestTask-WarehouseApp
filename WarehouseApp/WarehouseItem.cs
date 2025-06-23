using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseApp
{
    abstract class WarehouseItem
    {
    }
    class Box : WarehouseItem
    {
        public int Id { get; init; }
        public double Width { get; init; }
        public double Height { get; init; }
        public double Depth { get; init; }
        public double Volume { get; init; }
        public double Weight { get; init; }
        public DateTime? ProductionDt { get; init; }
        public DateTime ExpirationDt { get; init; }
        public Box(int id, double width, double height, double depth, double weight, DateTime? productionDt = null, DateTime? expirationDt = null)
        {
            Id = id; // В ТЗ нет указания каким образом присваивать ID - можно пределать на GUID
            Width = width;
            Height = height;
            Depth = depth;
            Volume = Math.Round(Width * Height * Depth, 3);
            Weight = weight;
            // Додумать логику с датой производства и срока годности
            if (!productionDt.HasValue && !expirationDt.HasValue)
                throw new ArgumentException("Одна из дат должна быть указана");
            ProductionDt = productionDt;
            ExpirationDt = expirationDt ?? productionDt.Value.AddDays(Constants.ExpirationDays);
            if (expirationDt.HasValue && productionDt.HasValue && expirationDt.Value < productionDt.Value)
                throw new ArgumentException("Дата производства больше срока годности");
        }
    }
    class Pallet : WarehouseItem
    {
        public int Id { get; init; }
        public double Width { get; init; }
        public double Height { get; init; }
        public double Depth { get; init; }
        public double Volume { get; init; }
        public double Weight { get; init; }
        public List<Box> Boxes { get; init; }
        public DateTime ExpirationDt { get; init; }
        public Pallet(int id, double width, double height, double depth, List<Box> boxes)
        {
            Id = id; // В ТЗ нет указания каким образом присваивать ID - можно пределать на GUID
            Width = width;
            Height = height;
            Depth = depth;
            Boxes = boxes;
            if (boxes.Any(x => x.Width > Width || x.Depth > Depth))
                throw new ArgumentException("Размеры коробки больше размера паллета");
            // В ТЗ не указано как поступать при отсутсвии коробок на паллете - выбрасывает исключение
            if (boxes.Count == 0)
                throw new ArgumentException("На паллете нет коробок");
            double weight = Boxes.Sum(x => x.Weight) + Constants.PalletBaseWeight;
            Weight = Math.Round(weight, 3);
            double volume = Boxes.Sum(x => x.Volume) + (Width * Height * Depth);
            Volume = Math.Round(volume, 3);
        }
    }
}