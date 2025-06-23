using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseApp
{
    internal class Constants
    {
        // Паллеты
        public const double PalletWidth = 40.0;
        public const double PalletHeight = 20.0;
        public const double PalletDepth = 40.0;
        public const double PalletBaseWeight = 30.0;
        // Коробки
        public const double BoxWidth = 10.0;
        public const double BoxHeight = 10.0;
        public const double BoxDepth = 10.0;
        public const double BoxWeight = 10.0;
        public const int ExpirationDays = 100;
        // Даты для генерации рандомных
        public static readonly List<DateTime> DatesForGenerator = GenerateDates();
        private static List<DateTime> GenerateDates()
        {
            var dates = new List<DateTime>();
            var startDt = new DateTime(2024, 1, 1);
            var endDt = DateTime.Today;
            for (var date = startDt; date <= endDt; date = date.AddDays(10))
            {
                dates.Add(date.Date);
            }

            return dates;
        }
    }
}
