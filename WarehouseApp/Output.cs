using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseApp
{
    internal class Output
    {
        public static Dictionary<DateTime, List<Pallet>> PalletSorting (List<Pallet> pallets)
        {
            var palletsGroups = pallets.OrderBy(x => x.ExpirationDt).GroupBy(x => x.ExpirationDt)
                                       .ToDictionary(x => x.Key, x => x.OrderBy(x => x.Weight).ToList());
            return palletsGroups;
        }

        public static List<Pallet> PalletsWithMaxExpirationDt (List<Pallet> pallets)
        {
            var palletsWithMaxExpirationDt = pallets.OrderByDescending(x => x.ExpirationDt)
                                                    .Take(3)
                                                    .OrderBy(x => x.Volume)
                                                    .ToList();
            return palletsWithMaxExpirationDt;
        }
    }

}
