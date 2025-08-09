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
        public int Id { get; private set; }
        public double Width { get; private set; }
        public double Height { get; private set; }
        public double Depth { get; private set; }
        public double Volume { get; private set; }
        public double Weight { get; private set; }
        public DateTime? ProductionDt { get; private set; }
        public DateTime ExpirationDt { get; private set; }
        public Box(int id, double width, double height, double depth, double weight, DateTime? productionDt = null, DateTime? expirationDt = null)
        {
            Id = id;
            Width = width;
            Height = height;
            Depth = depth;
            Volume = Math.Round(Width * Height * Depth, 3);
            Weight = weight;
            if (!productionDt.HasValue && !expirationDt.HasValue)
                throw new ArgumentException("Одна из дат должна быть указана");
            ProductionDt = productionDt;
            ExpirationDt = expirationDt.HasValue ? expirationDt.Value.Date : ProductionDt.Value.AddDays(100).Date;
            if (expirationDt.HasValue && productionDt.HasValue && expirationDt.Value < productionDt.Value)
                throw new ArgumentException("Дата производства больше срока годности");
        }
    }
    class Pallet : WarehouseItem
    {
        public int Id { get; private set; }
        public double Width { get; private set; }
        public double Height { get; private set; }
        public double Depth { get; private set; }
        public double Volume { get; private set; }
        public double Weight { get; private set; }
        public List<Box> Boxes { get; private set; }
        public DateTime ExpirationDt { get; private set; }
        public Pallet(int id, double width, double height, double depth, List<Box> boxes)
        {
            Id = id;
            Width = width;
            Height = height;
            Depth = depth;
            Boxes = boxes;
            if (boxes.Any(x => x.Width > Width || x.Depth > Depth))
                throw new ArgumentException("Размеры коробки больше размера паллета");
            if (boxes.Count == 0)
                throw new ArgumentException("На паллете нет коробок");
            ExpirationDt = boxes.Min(x => x.ExpirationDt);
            double weight = boxes.Sum(x => x.Weight) + Constants.PalletBaseWeight;
            Weight = Math.Round(weight, 3);
            double volume = Boxes.Sum(x => x.Volume) + (Width * Height * Depth);
            Volume = Math.Round(volume, 3);
        }
    }
}