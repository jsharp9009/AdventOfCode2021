using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace BeaconScanner
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadLines("input.txt").ToArray();

            var scanners = ReadInput(input);

            var locatedScanners = LocateScanners(scanners);

            var count = locatedScanners.SelectMany(s => ScannerTransforms.GetAllBeacons(s))
                        .Distinct().Count();

            Console.WriteLine("# of Beacons: " + count);

            var maxLenth = (from s1 in locatedScanners
                            from s2 in locatedScanners
                            where s1 != s2
                            select Math.Abs(s1.X - s2.X)
                            + Math.Abs(s1.Y - s2.Y)
                            + Math.Abs(s1.Z - s2.Z))
                            .Max();

            Console.WriteLine("Max length between sensors: " + maxLenth);
        }

        static List<Scanner> LocateScanners(List<Scanner> scanners)
        {
            var locatedScanners = new List<Scanner>();
            var scannersToSearch = new Queue<Scanner>();


            var first = scanners.FirstOrDefault();
            locatedScanners.Add(first);
            scannersToSearch.Enqueue(first);

            scanners.Remove(first);

            while (scannersToSearch.Any())
            {
                var scanner1 = scannersToSearch.Dequeue();
                var toRemove = new List<Scanner>();
                for (int i = 0; i < scanners.Count(); i++)
                {
                    var scanner2 = scanners[i];
                    var locatedScanner = TryToLocate(scanner1, scanner2);
                    if (locatedScanner != null)
                    {
                        locatedScanners.Add(locatedScanner);
                        scannersToSearch.Enqueue(locatedScanner);

                        toRemove.Add(scanner2);
                    }
                }

                scanners.RemoveAll(s => toRemove.Any(t => t == s));
            }
            return locatedScanners;
        }

        static Scanner TryToLocate(Scanner scanner1, Scanner scanner2)
        {
            //Console.WriteLine("Scanner " + scanner1.Number + " -> Scanner " + scanner2.Number );
            var beacons1 = ScannerTransforms.GetAllBeacons(scanner1).ToArray();
            foreach (var match in GetPotentialMatches(scanner1, scanner2))
            {
                var rotatedScanner = scanner2;
                for (var rotation = 0; rotation < 24; rotation++, rotatedScanner = ScannerTransforms.Rotate(rotatedScanner))
                {

                    var beaconRotated = ScannerTransforms.Transform(rotatedScanner, match.Item2);

                    var located2 = ScannerTransforms.Translate(rotatedScanner,
                        new Coordinate()
                        {
                            X = match.Item1.X - beaconRotated.X,
                            Y = match.Item1.Y - beaconRotated.Y,
                            Z = match.Item1.Z - beaconRotated.Z
                        });

                    //Console.WriteLine("PM " + located2.X + ", " + located2.Y + ", " + located2.Z);

                    if (ScannerTransforms.GetAllBeacons(located2)
                    .Where(c => beacons1.Any(b => b.X == c.X && b.Y == c.Y && b.Z == c.Z))
                    .Count() >= 12)
                    {
                        return located2;
                    }
                }
            }

            return null;
        }

        static IEnumerable<Tuple<Coordinate, Coordinate>> GetPotentialMatches(Scanner scanner1, Scanner scanner2)
        {
            var beaconsIn1 = ScannerTransforms.GetAllBeacons(scanner1);
            foreach (var b1 in beaconsIn1)
            {
                var tmpScan = ScannerTransforms.Translate(scanner1, new Coordinate() { X = -b1.X, Y = -b1.Y, Z = -b1.Z });
                var absolute1 = ScannerTransforms.AbsoluteCoordinates(tmpScan);

                var beaconsIn2 = ScannerTransforms.GetAllBeacons(scanner2);
                foreach (var b2 in beaconsIn2)
                {
                    tmpScan = ScannerTransforms.Translate(scanner2, new Coordinate() { X = -b2.X, Y = -b2.Y, Z = -b2.Z });
                    var absolute2 = ScannerTransforms.AbsoluteCoordinates(tmpScan);

                    if (absolute2.Count(d => absolute1.Contains(d)) >= 3 * 12)
                    {
                        yield return new Tuple<Coordinate, Coordinate>(b1, b2);
                    }
                }

            }
        }

        static List<Scanner> ReadInput(string[] input)
        {
            List<Scanner> scanners = new List<Scanner>();
            Scanner current = null;
            foreach (var line in input)
            {
                if (string.IsNullOrEmpty(line)) continue;
                if (line.StartsWith("---"))
                {
                    current = new Scanner();
                    current.Number = scanners.Count();
                    scanners.Add(current);
                    continue;
                }

                var coordinates = line.Split(',', 3);

                var beacon = new Coordinate()
                {
                    X = int.Parse(coordinates[0]),
                    Y = int.Parse(coordinates[1]),
                    Z = int.Parse(coordinates[2])
                };

                current.Beacons.Add(beacon);
            }

            return scanners;
        }

    }

    public class Scanner : Coordinate, ICloneable
    {
        public List<Coordinate> Beacons { get; set; } = new List<Coordinate>();
        public int Rotation { get; set; }

        public int Number { get; set; }

        public object Clone()
        {
            return new Scanner()
            {
                X = X,
                Y = Y,
                Z = Z,
                Rotation = Rotation,
                Beacons = Beacons,
                Number = Number
            };
        }

        public override bool Equals(object obj)
        {
            var other = (Scanner)obj;

            if (other == null) return false;

            return this.X == other.X && this.Y == other.Y && this.Z == other.Z && this.Number == other.Number;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() ^ Rotation ^ Beacons.GetHashCode() ^ Number;
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }

    public class Coordinate
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public override bool Equals(object obj)
        {
            var other = (Coordinate)obj;

            if (other == null) return false;

            return this.X == other.X && this.Y == other.Y && this.Z == other.Z;
        }

        public override int GetHashCode()
        {
            return X ^ Y ^ Z;
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }

    public static class ScannerTransforms
    {
        public static Scanner Rotate(Scanner scanner)
        {
            var newScanner = (Scanner)scanner.Clone();
            newScanner.Rotation++;
            return newScanner;
        }

        public static Scanner Translate(Scanner scanner, Coordinate translation)
        {
            var newScanner = (Scanner)scanner.Clone();
            newScanner.X += translation.X;
            newScanner.Y += translation.Y;
            newScanner.Z += translation.Z;

            return newScanner;
        }

        public static Coordinate Transform(Scanner scanner, Coordinate transform)
        {
            int x = transform.X, y = transform.Y, z = transform.Z;
            int holder;

            switch (scanner.Rotation % 6)
            {
                case 0:
                    break;
                case 1:
                    x = -x;
                    z = -z;
                    break;
                case 2:
                    holder = x;
                    x = y;
                    y = -holder;
                    break;
                case 3:
                    holder = x;
                    x = -y;
                    y = holder;
                    break;
                case 4:
                    holder = x;
                    x = z;
                    z = -holder;
                    break;
                case 5:
                    holder = x;
                    x = -z;
                    z = holder;
                    break;
            }

            switch ((scanner.Rotation / 6) % 4)
            {
                case 0:
                    break;
                case 1:
                    holder = y;
                    y = -z;
                    z = holder;
                    break;
                case 2:
                    y = -y;
                    z = -z;
                    break;
                case 3:
                    holder = y;
                    y = z;
                    z = -holder;
                    break;
            }

            return new Coordinate()
            {
                X = scanner.X + x,
                Y = scanner.Y + y,
                Z = scanner.Z + z
            };

        }

        public static IEnumerable<Coordinate> GetAllBeacons(Scanner scanner)
        {
            return scanner.Beacons.Select(b => Transform(scanner, b));
        }

        public static List<int> AbsoluteCoordinates(Scanner scanner)
        {
            var beacons = GetAllBeacons(scanner);
            var values = new List<int>();

            foreach (var b in beacons)
            {
                values.Add(Math.Abs(b.X));
                values.Add(Math.Abs(b.Y));
                values.Add(Math.Abs(b.Z));
            }

            return values;
        }
    }

}
