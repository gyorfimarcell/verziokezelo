using System;
using System.Linq;
using System.IO;

namespace ConsoleApp8
{
    class Program
    {
        static string szerzo = "";

        static void Main(string[] args)
        {
            Directory.SetCurrentDirectory("C:/Users/tanulo/Desktop/teszt");

            Console.Write("Szerző: ");
            szerzo = Console.ReadLine();

            while (true)
            {
                Menu();
            }
        }

        static void Menu() {
            Console.WriteLine("Kiválasztott mappa: " + Directory.GetCurrentDirectory());
            if (Directory.Exists(".dusza"))
            {
                Console.WriteLine("A mappa inicializálva van.");
            }
            else
            {
                Console.WriteLine("A mappa nincs inicializálva.");
            }

            Console.WriteLine("\n(1) mappa kiválasztása");
            Console.WriteLine("(2) mappa inicializálása");
            Console.WriteLine("(3) új commit");
            Console.WriteLine("(0) kilépés");

            Console.WriteLine("Mit szeretnél tenni? ");
            char be = Convert.ToChar(Console.ReadLine());
            switch (be) {
                case '1':
                    Console.WriteLine("Mappa elérési útja: ");
                    string ujPath = Console.ReadLine();
                    Directory.SetCurrentDirectory(ujPath);
                    break;
                case '2':
                    MappaInicializalas();
                    break;
                case '3':
                    if (Directory.Exists(".dusza"))
                    {
                        Commit();
                    }
                    else {
                        Console.WriteLine("Előbb inicializáld a mappát!");
                    }
                    break;
                case '0':
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Nem érvényes bemenet!");
                    break;
            }
        }

        static void MappaInicializalas() {
            if (Directory.Exists(".dusza"))
            {
                Console.WriteLine("A mappa már inicializálva van!");
                return;
            }

            Directory.CreateDirectory(".dusza");
            File.WriteAllText("./.dusza/head.txt", "1");

            Commit(true);
        }

        static void Commit(bool elsoCommit = false) {
            int sorszam = Directory.GetDirectories("./.dusza").Length + 1;

            string szulo = File.ReadAllText("./.dusza/head.txt");
            string valtozott = Valtozott(elsoCommit ? new string[0] : Directory.GetFiles($"./.dusza/{szulo}.commit"));

            Directory.CreateDirectory($"./.dusza/{sorszam}.commit");
            File.WriteAllText("./.dusza/head.txt", Convert.ToString(sorszam));

            Masol(".", $"./.dusza/{sorszam}.commit");      

            Console.Write("Leírás: ");
            string leiras = Console.ReadLine();

            StreamWriter details = File.CreateText($"./.dusza/{sorszam}.commit/commit.details");
            details.WriteLine($"Szulo:{(elsoCommit ? "-" : szulo)}");
            details.WriteLine($"Szerző:{szerzo}");
            details.WriteLine($"Dátum:{DateTime.Now}");
            details.WriteLine($"Commit leírás:{leiras}");
            details.WriteLine("Változott:");
            details.WriteLine(valtozott);
            details.Close();
        }

        static string Valtozott(string[] elozoFajlok) {
            string[] jelenlegiFajlok = Directory.GetFiles(".");
            string[] ujFajlok = jelenlegiFajlok.Except(elozoFajlok).ToArray();

            string valtozott = "";
            foreach (string fajl in ujFajlok)
            {
                string nev = fajl.Split('\\')[^1];
                valtozott += $"új {nev} {File.GetLastWriteTime(fajl)}";
            }

            return valtozott;
        }

        static void Masol(string forras, string cel) {
            string[] fajlok = Directory.GetFiles(forras);
            foreach (string fajl in fajlok)
            {
                string nev = fajl.Split('\\')[^1];
                File.Copy($"{forras}/{nev}", $"{cel}/{nev}", true);
            }
            string[] mappak = Directory.GetDirectories(forras);
            
            foreach (string mappa in mappak) {
                string nev = mappa.Split('\\')[^1];
                if (nev != ".dusza")
                {
                    Directory.CreateDirectory($"{cel}/{nev}");
                    Masol($"{forras}/{nev}", $"{cel}/{nev}");
                }
            }
        }
    }
}
