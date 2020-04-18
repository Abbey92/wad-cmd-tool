using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Text;
using System.Globalization;
using System.Text.RegularExpressions;
using Fantome.Libraries.League.IO.WAD;
using Fantome.Libraries.League.Helpers;
using Fantome.Libraries.League.Helpers.Cryptography;

namespace wad_cmd_tool
{
    class Program
    {
        //library https://github.com/LoL-Fantome/Fantome.Libraries.League
        //help https://github.com/Crauzer/Obsidian
        //hashes https://github.com/CommunityDragon/CDTB
        //cmd tool https://github.com/Abbey92/wad-cmd-tool

        public static string app_directory = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
        public static string expression = @"^data/.*_(skins_skin|tiers_tier|(skins|tiers)_root).*\.bin$";
        private static Dictionary<ulong, string> _hash = new Dictionary<ulong, string>();
        private static Dictionary<string, byte[]> _list = new Dictionary<string, byte[]>();

        static void Main(string[] args)
        {            
            if (args.Length == 0)
            {
                //if (File.Exists(app_directory + "hashes\\hashes.game.txt"))//hashes.game.txt
                //{
                //    LoadHash(app_directory + "hashes\\hashes.game.txt");
                //}
                //if (File.Exists(app_directory + "hashes\\hashes.lcu.txt"))//hashes.lcu.txt
                //{
                //    LoadHash(app_directory + "hashes\\hashes.lcu.txt");
                //}

                //if (File.Exists("ashe.wad.client"))
                //{
                //    Export_all("ashe.wad.client");
                //    Export_Filter("Ashe.wad.client", "dds");
                //    Save_all("c:\\1"); // NOT "c:\\1\\assets            
                //    Modify_all("Ashe.wad.client", "c:\\1"); // NOT "c:\\1\\assets
                //    Console.ReadKey();
                //}

            }

            else
            {
                //---------------------------------------------------------------------------------------------------------------load hash
                if (File.Exists(app_directory + "hashes\\hashes.game.txt"))//hashes.game.txt
                {
                    LoadHash(app_directory + "hashes\\hashes.game.txt");
                }
                if (File.Exists(app_directory + "hashes\\hashes.lcu.txt"))//hashes.lcu.txt
                {
                    LoadHash(app_directory + "hashes\\hashes.lcu.txt");                 
                }

                //---------------------------------------------------------------------------------------------------------------Entrance
                if (args[0] == "Export_all" && args[1] != "") //1
                {
                    Export_all(args[1]);
                    return;
                }
                if (args[0] == "Save_all" && args[1] != "") //2
                {
                    Save_all(args[1]);
                    return;
                }
                if (args[0] == "Modify_all" && args[1] != "" && args[2] != "") //3
                {
                    Modify_all(args[1], args[2]);
                    return;
                }
                if (args[0] == "Export_Filter" && args[1] != "" && args[2] != "") //4
                {
                    Export_Filter(args[1], args[2]);
                    return;
                }
            }
        }

        //---------------------------------------------------------------------------------------------------------------Features
        public static void Save_all(string directory)
        {
            string[] array = Directory.GetFileSystemEntries(directory, "*", SearchOption.AllDirectories);
            WADFile wad = new WADFile(3, 0);
            foreach (string line in array)
            {
                bool bl = File.Exists(line);
                if (bl)
                {
                    string name = line.Replace(directory + "\\", "").Replace("\\", "/").ToLower();
                    byte[] data = File.ReadAllBytes(line);
                    wad.AddEntry(name, data, true);
                }
            }
            wad.Write(app_directory + "new.wad.client");
            Console.WriteLine("Output:" + app_directory + "new.wad.client");
        }

        public static void Modify_all(string wadfilepath, string directory)
        {
            WADFile wad = new WADFile(wadfilepath);
            XXHash64 xxHash = XXHash64.Create();
            string[] array = Directory.GetFileSystemEntries(directory, "*", SearchOption.AllDirectories);
            foreach (string line in array)
            {
                string Path = line.Replace(directory + "\\", "").Replace("\\", "/").ToLower();
                ulong hash = BitConverter.ToUInt64(xxHash.ComputeHash(Encoding.ASCII.GetBytes(Path.ToLower())), 0);
                wad.RemoveEntry(hash);
            }
            foreach (string line in array)
            {
                bool bl = File.Exists(line);
                if (bl)
                {
                    string name = line.Replace(directory + "\\", "").Replace("\\", "/").ToLower();
                    byte[] data = File.ReadAllBytes(line);
                    wad.AddEntry(name, data, true);
                }
            }
            wad.Write(app_directory + "new.wad.client");
            Console.WriteLine("Output:" + app_directory + "new.wad.client");
        }
        public static void Export_Filter(string wadfilepath, string filter)
        {
            WADFile wad = new WADFile(wadfilepath);
            string long_bin = "";
            foreach (WADEntry entries in wad.Entries)
            {
                byte[] EntryData = entries.GetContent(true);
                string name;
                if (_hash.ContainsKey(entries.XXHash))
                {
                    name = _hash[entries.XXHash].ToLower();
                    if (Regex.IsMatch(name, expression))
                    {
                        if (name.Length > 100) //255
                        {
                            long_bin += string.Format("{0} = {1}\r\n", entries.XXHash.ToString("X16") + ".bin", name);
                            //System.Diagnostics.Debug.WriteLine(long_bin);
                            name = entries.XXHash.ToString("X16") + ".bin";
                        }
                    }
                }
                else
                {
                    name = entries.XXHash.ToString("X16") + "." + Utilities.GetEntryExtension(Utilities.GetLeagueFileExtensionType(EntryData));
                }
                //System.Diagnostics.Debug.WriteLine(name);
                if (name.Contains(filter))
                {
                    //System.Diagnostics.Debug.WriteLine(name);
                    _list.Add(name, EntryData);
                }
                else
                {
                    string ufilter = filter.ToUpper();
                    if (name.Contains(ufilter))
                    {
                        //System.Diagnostics.Debug.WriteLine(name);
                        _list.Add(name, EntryData);
                    }
                }
            }
            if (long_bin.Length > 0 && filter.Contains("bin"))
            {
                Directory.CreateDirectory(app_directory + "Export Filter 搜索");
                File.WriteAllText(app_directory + "Export Filter 搜索\\longbin.txt", long_bin);
            }
            foreach (KeyValuePair<string, byte[]> kvp in _list)
            {
                Directory.CreateDirectory(app_directory + "Export Filter 搜索\\" + Path.GetDirectoryName(kvp.Key.ToString()));
                File.WriteAllBytes(app_directory + "Export Filter 搜索\\" + kvp.Key.ToString(), kvp.Value);
            }
            Console.WriteLine("Output:" + app_directory + "Export Filter 搜索");
        }

        public static void Export_all(string wadfilepath)
        {
            WADFile wad = new WADFile(wadfilepath);
            string long_bin = "";
            foreach (WADEntry entries in wad.Entries)
            {
                byte[] EntryData = entries.GetContent(true); 
                string name;
                if (_hash.ContainsKey(entries.XXHash))
                {
                    name = _hash[entries.XXHash].ToLower();                   
                    if (Regex.IsMatch(name, expression))
                    {
                        if (name.Length > 100) //255
                        {
                            long_bin += string.Format("{0} = {1}\r\n", entries.XXHash.ToString("X16") + ".bin", name);
                            name = entries.XXHash.ToString("X16") + ".bin";
                        }
                    }
                }
                else
                {                    
                    name = entries.XXHash.ToString("X16") + "." + Utilities.GetEntryExtension(Utilities.GetLeagueFileExtensionType(EntryData));
                }
                //System.Diagnostics.Debug.WriteLine(name);
                _list.Add(name, EntryData);
            }
            if (long_bin.Length > 0 )
            {
                Directory.CreateDirectory(app_directory + "Export all 导出" );
                File.WriteAllText(app_directory + "Export all 导出\\longbin.txt", long_bin);
            }
            foreach (KeyValuePair<string, byte[]> kvp in _list)
            {              
                Directory.CreateDirectory(app_directory + "Export all 导出\\" + Path.GetDirectoryName(kvp.Key.ToString()));
                //System.Diagnostics.Debug.WriteLine( Path.GetDirectoryName(kvp.Key.ToString()));
                File.WriteAllBytes(app_directory + "Export all 导出\\" + kvp.Key.ToString(), kvp.Value);
            }
            Console.WriteLine("Output:" + app_directory + "Export all 导出");
        }

        public static void LoadHash(string filepath)
        {
            //38513989304b828c assets/characters/ashe/skins/base/ashe.skn
            //assets/characters/ashe/skins/base/ashe.skn
            foreach (string line in File.ReadAllLines(filepath))
            {
                string[] array = line.Split(' ');
                ulong hash;
                string name = string.Empty;
                if (array.Length == 1)
                {
                    hash = XXHash.XXH64(Encoding.ASCII.GetBytes(array[0].ToLower()));
                    name = array[0];
                }
                else
                {
                    for (int i = 1; i < array.Length; i++)
                    {
                        name += array[i];

                        if (i + 1 != array.Length)
                        {
                            name += ' ';
                        }
                    }
                    hash = ulong.Parse(array[0], NumberStyles.HexNumber);
                }
                if (!_hash.ContainsKey(hash))
                {
                    _hash.Add(hash, name);
                }            
            }
        }
    }
}
