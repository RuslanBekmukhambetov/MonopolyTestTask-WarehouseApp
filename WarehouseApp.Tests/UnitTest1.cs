using Microsoft.VisualStudio.TestPlatform.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using WarehouseApp;
using Xunit;
using Xunit.Abstractions;

namespace WarehouseApp.Tests
{
    public class BoxTests
    {
        [Fact]
        public void BoxConstructorWithOnlyProductionDt()
        {
            var productionDt = new DateTime(2024, 1, 1);
            var box = new Box(1, 10, 10, 10, 5, productionDt);

            Assert.Equal(productionDt.AddDays(100).Date, box.ExpirationDt);
            Assert.Equal(1000.0, box.Volume);
            Assert.Equal(5.0, box.Weight);
        }

        [Fact]
        public void BoxConstructorWithOnlyExpirationDt()
        {
            var expirationDt = new DateTime(2024, 5, 1);
            var box = new Box(1, 10, 10, 10, 5, null, expirationDt);

            Assert.Equal(expirationDt.Date, box.ExpirationDt);
            Assert.Null(box.ProductionDt);
        }

        [Fact]
        public void BoxConstructorWithBothDt()
        {
            var productionDt = new DateTime(2024, 1, 1);
            var expirationDt = new DateTime(2024, 6, 1);
            var box = new Box(1, 10, 10, 10, 5, productionDt, expirationDt);

            Assert.Equal(expirationDt.Date, box.ExpirationDt);
            Assert.Equal(productionDt.Date, box.ProductionDt.Value.Date);
        }

        [Fact]
        public void BoxConstructorNoDtException()
        {
            var exception = Assert.Throws<ArgumentException>(() => new Box(1, 10, 10, 10, 5));
            Assert.Equal("Одна из дат должна быть указана", exception.Message);
        }

        [Fact]
        public void BoxConstructorExpirationDtIsLessProductionDt()
        {
            var productionDt = new DateTime(2024, 1, 1);
            var expirationDt = new DateTime(2023, 12, 31);
            var exception = Assert.Throws<ArgumentException>(() => new Box(1, 10, 10, 10, 5, productionDt, expirationDt));
            Assert.Equal("Дата производства больше срока годности", exception.Message);
        }

        [Fact]
        public void BoxConstructorMathRound()
        {
            var box = new Box(1, 10.1234, 10.5678, 10.9999, 5, new DateTime(2024, 1, 1));
            Assert.Equal(1176.792, box.Volume, 3); // Учитывая округление
        }
    }

    public class PalletTests
    {
        [Fact]
        public void PalletConstructorCalculatesWeightAndVolume()
        {
            var box1 = new Box(1, 10, 10, 10, 5, new DateTime(2024, 1, 1));
            var box2 = new Box(2, 10, 10, 10, 10, new DateTime(2024, 2, 1));
            var boxes = new List<Box> { box1, box2 };

            var pallet = new Pallet(1, 40, 20, 40, boxes);

            Assert.Equal(45.0, pallet.Weight);
            Assert.Equal(34000.0, pallet.Volume); // (1000 + 1000) + (40*20*40 = 32000) = 34000
            Assert.Equal(box1.ExpirationDt, pallet.ExpirationDt); // Min из двух, box1 раньше
        }

        [Fact]
        public void PalletExpirationDtFromBoxes()
        {
            var box1 = new Box(1, 10, 10, 10, 5, null, new DateTime(2024, 5, 1));
            var box2 = new Box(2, 10, 10, 10, 5, null, new DateTime(2024, 4, 1));
            var boxes = new List<Box> { box1, box2 };

            var pallet = new Pallet(1, 40, 20, 40, boxes);

            Assert.Equal(new DateTime(2024, 4, 1), pallet.ExpirationDt);
        }

        [Fact]
        public void PalletConstructorNoBoxesException()
        {
            var exception = Assert.Throws<ArgumentException>(() => new Pallet(1, 40, 20, 40, new List<Box>()));
            Assert.Equal("На паллете нет коробок", exception.Message);
        }

        [Fact]
        public void PalletConstructorBoxIsBiggerThanPalletException()
        {
            var box = new Box(1, 50, 10, 10, 5, new DateTime(2024, 1, 1)); // Width 50 > 40
            var boxes = new List<Box> { box };
            var exception = Assert.Throws<ArgumentException>(() => new Pallet(1, 40, 20, 40, boxes));
            Assert.Equal("Размеры коробки больше размера паллета", exception.Message);
        }

        [Fact]
        public void PalletWeightAndVolumeMathRound()
        {
            var box = new Box(1, 10.1, 10.2, 10.3, 5.123, new DateTime(2024, 1, 1));
            var boxes = new List<Box> { box };
            var pallet = new Pallet(1, 40, 20, 40, boxes);

            Assert.Equal(35.123, pallet.Weight, 3);
            Assert.Equal(32000 + Math.Round(10.1 * 10.2 * 10.3, 3), pallet.Volume, 3);
        }
    }

    public class OutputTests
    {
        private readonly ITestOutputHelper _output;

        public OutputTests(ITestOutputHelper output)
        {
            _output = output;
        }

        private List<Pallet> CreateTestPallets()
        {
            var box1 = new Box(1, 10, 10, 10, 5, new DateTime(2024, 1, 1));
            var box2 = new Box(2, 10, 10, 10, 10, new DateTime(2024, 1, 1));
            var box3 = new Box(3, 10, 10, 10, 15, new DateTime(2024, 2, 1)); 
            var box4 = new Box(4, 10, 10, 10, 20, new DateTime(2024, 3, 1));

            var pallet1 = new Pallet(1, 40, 20, 40, new List<Box> { box1 });
            var pallet2 = new Pallet(2, 40, 20, 40, new List<Box> { box2 });
            var pallet3 = new Pallet(3, 40, 20, 40, new List<Box> { box3 });
            var pallet4 = new Pallet(4, 40, 20, 40, new List<Box> { box4 });

            return new List<Pallet> { pallet1, pallet2, pallet3, pallet4 };
        }

        [Fact]
        public void PalletSorting()
        {
            var pallets = CreateTestPallets();
            var groups = Output.PalletSorting(pallets);

            Assert.Equal(3, groups.Count);
            var keys = groups.Keys.ToList();
            Assert.True(keys[0] < keys[1] && keys[1] < keys[2]);

            var group1 = groups[keys[0]]; // 04-10: pallet1 (35), pallet2 (40)
            Assert.Equal(35, group1[0].Weight);
            Assert.Equal(40, group1[1].Weight);
        }

        [Fact]
        public void PalletsWithMaxExpirationDt()
        {
            var pallets = CreateTestPallets();
            var result = Output.PalletsWithMaxExpirationDt(pallets);

            Assert.Equal(3, result.Count);
            Assert.Equal(4, result[0].Id);
            _output.WriteLine($"{result[0].ExpirationDt} <= {result[1].ExpirationDt} <= {result[2].ExpirationDt}");
            Assert.True(result[0].ExpirationDt >= result[1].ExpirationDt); 
            var differentVolPallets = new List<Pallet>
            {
                new Pallet(1, 40, 20, 40, new List<Box>{new Box(1, 10,10,10,5, null, new DateTime(2024,6,1)) }), // vol 33000
                new Pallet(2, 40, 20, 40, new List<Box>{new Box(2, 20,10,20,5, null, new DateTime(2024,6,1)) }), // vol 20*10*20=4000 +32000=36000
                new Pallet(3, 40, 20, 40, new List<Box>{new Box(3, 15,10,15,5, null, new DateTime(2024,6,1)) }), // vol 15*10*15=2250 +32000=34250
            };
            var resultDiff = Output.PalletsWithMaxExpirationDt(differentVolPallets);

            _output.WriteLine($"1. {differentVolPallets[0].Volume}");
            _output.WriteLine($"2. {differentVolPallets[1].Volume}");
            _output.WriteLine($"3. {differentVolPallets[2].Volume}");
            Assert.Equal(33000, resultDiff[0].Volume);
            Assert.Equal(34250, resultDiff[1].Volume);
            Assert.Equal(36000, resultDiff[2].Volume);
        }
    }

    public class GeneratorTests
    {
        [Fact]
        public void GenerateBoxes()
        {
            var boxes = Generator.GenerateBoxes(5);

            Assert.Equal(5, boxes.Count);
            foreach (var box in boxes)
            {
                Assert.True(box.Width <= Constants.PalletWidth);
                Assert.True(box.Depth <= Constants.PalletDepth);
                Assert.True(box.Weight > 0);
                Assert.Equal(box.ProductionDt.Value.AddDays(100).Date, box.ExpirationDt);
            }
        }

        [Fact]
        public void GeneratePallets()
        {
            var pallets = Generator.GeneratePallets(3);

            Assert.Equal(3, pallets.Count);
            foreach (var pallet in pallets)
            {
                Assert.True(pallet.Boxes.Count >= Constants.MinBoxesCnt);
                Assert.True(pallet.Boxes.Count <= Constants.MaxBoxesCnt);
                Assert.Equal(pallet.Boxes.Min(b => b.ExpirationDt), pallet.ExpirationDt);
            }
        }
    }
}