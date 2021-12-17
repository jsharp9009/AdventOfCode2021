using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace PacketDecoder
{
    class Program
    {
        static Dictionary<char, string> hexToBinaryConversionRules = new Dictionary<char, string>(){
            {'0',"0000"},
            {'1',"0001"},
            {'2',"0010"},
            {'3',"0011"},
            {'4',"0100"},
            {'5',"0101"},
            {'6',"0110"},
            {'7',"0111"},
            {'8',"1000"},
            {'9',"1001"},
            {'A',"1010"},
            {'B',"1011"},
            {'C',"1100"},
            {'D',"1101"},
            {'E',"1110"},
            {'F',"1111"}
        };

        static void Main(string[] args)
        {
            var input = File.ReadAllText("input.txt");
            var binaryInput = HexToBinary(input);

            var packet = ParsePacket(binaryInput);

            Console.WriteLine("Total Versions: " + packet.GetTotalVersions());
            Console.WriteLine("Value is : " + packet.GetValue());
        }

        static char[] HexToBinary(string hex){
            var result = new StringBuilder();
            foreach(char c in hex){
                result.Append(hexToBinaryConversionRules[c]);
            }
            return result.ToString().ToCharArray();
        }

        static Packet ParsePacket(char[] input){
            var versionString = input.Take(3).ToArray();
            var typeString = input.Skip(3).Take(3).ToArray();

            var packet = new Packet();
            packet.Version = Convert.ToInt32(new string(versionString), 2);
            packet.Type = Convert.ToInt32(new string(typeString), 2);

            var valuegroups = input.Skip(6).ToArray();

            if(packet.Type == 4){
                var groupCount = 0;
                var valueBuilder = new StringBuilder();
                while(true){
                    var group = valuegroups.Skip(groupCount * 5).Take(5).ToArray();
                    valueBuilder.Append(new string(group.Skip(1).ToArray()));
                    if(group[0] == '0') break;
                    groupCount++;
                }

                packet.Value = Convert.ToInt64(valueBuilder.ToString(), 2);
                packet.remainder = valuegroups.Skip((groupCount + 1) * 5).ToArray();
            }else{
                packet.LengthType = Convert.ToInt32(valuegroups[0].ToString(), 2);
                if(packet.LengthType == 0){
                    var lengthOfSubPackets = Convert.ToInt32(new string(valuegroups.Skip(1).Take(15).ToArray()), 2);
                    var subPackets = valuegroups.Skip(16).Take(lengthOfSubPackets).ToArray();
                    while(subPackets.Count() > 0){
                        var subPacket = ParsePacket(subPackets);
                        subPackets = subPacket.remainder ?? new char[0];
                        packet.Children.Add(subPacket);
                    }
                    packet.remainder = valuegroups.Skip(16 + lengthOfSubPackets).ToArray();
                }
                else {
                    var numberOfSubPackets = Convert.ToInt32(new string(valuegroups.Skip(1).Take(11).ToArray()), 2);
                    var subPackets = valuegroups.Skip(12).ToArray();
                    for(int i = 0; i < numberOfSubPackets; i++){
                        var subPacket = ParsePacket(subPackets);
                        subPackets = subPacket.remainder ?? new char[0];
                        packet.Children.Add(subPacket);
                    }

                    packet.remainder = packet.Children.Last().remainder;
                }
            }

            return packet;
        }
    }

    public class Packet{
        public int Version {get;set;}
        public int Type {get;set;}
        public long? Value{get;set;}
        public List<Packet> Children{get;set;} = new List<Packet>();

        public int LengthType {get;set;}

        public char[] remainder {get;set;}

        public int GetTotalVersions(){
            return Version + Children.Sum(p => p.GetTotalVersions());
        }

        public long GetValue(){
            if(Type == 0){
                return Children.Sum(c => c.GetValue());
            }
            if(Type == 1){
                return Children.Aggregate((long)1, (a, b) => a * b.GetValue());
            }
            if(Type == 2){
                return Children.Min(c => c.GetValue());
            }
            if(Type == 3){
                return Children.Max(c => c.GetValue());
            }
            if(Type == 4){
                return Value.GetValueOrDefault();
            }
            if(Type == 5){
                return Children[0].GetValue() > Children[1].GetValue() ? (long)1 : (long)0;
            }
            if(Type == 6){
                return Children[0].GetValue() < Children[1].GetValue() ? (long)1 : (long)0;
            }
            if(Type == 7){
                return Children[0].GetValue() == Children[1].GetValue() ? (long)1 : (long)0;
            }

            return 0;
        }
    }
}
