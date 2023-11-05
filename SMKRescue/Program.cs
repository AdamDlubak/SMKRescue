using OpenQA.Selenium;
using CsvHelper;
using System.Globalization;
using OpenQA.Selenium.Chrome;
using SMKRescue.Extensions;
using SMKRescue.Models;

namespace SMKRescue
{
    internal class Program
    {
        public static string SmkLink = "https://smk2.ezdrowie.gov.pl/login";
        public static string InputDataDirectory = "InputData";
        public static string SplitSelector = "|";

        private static void Main()
        {
            Console.Clear();
            var procedures = new List<Procedure>();

            var year = "";
            var author = "";
            var placeIndex = 0;
            var programNameIndex = 0;

            try
            {
                var configFile = Path.Combine(Environment.CurrentDirectory, InputDataDirectory, "config.txt");

                var configLines = File.ReadAllLines(configFile);

                year = configLines[0].Split(SplitSelector)[1].Trim();
                author = configLines[1].Split(SplitSelector)[1].Trim();
                placeIndex = int.Parse(configLines[2].Split(SplitSelector)[1].Trim());
                programNameIndex = int.Parse(configLines[3].Split(SplitSelector)[1].Trim());

                for (var index = 4; index < configLines.Length; index++)
                {
                    var elements = configLines[index].Split(SplitSelector);
                    procedures.Add(new Procedure(elements[0].Trim(), elements[1].Trim()));
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    "There was a problem with reading config file - please prepare it according to the instructions.");
                Console.WriteLine("Technical exception log below:");
                Console.WriteLine(ex);
                Console.ReadKey(false);
            }

            IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(SmkLink);

            for (var i = 0; i < 5; i++)
            {
                Thread.Sleep(TimeSpan.FromSeconds(1));
                Console.Clear();
                Console.WriteLine("Login to SMK and go to appropriate page with editable list of available procedures and then press ENTER...");
            }
            Console.ReadKey(false);

            foreach (var procedure in procedures)
            {
                using var reader = new StreamReader(Path.Combine(Environment.CurrentDirectory, InputDataDirectory,
                    procedure.FileName + ".csv"));
                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
                csv.Context.RegisterClassMap<ExcelModelMap>();
                var records = csv.GetRecords<ExcelModel>().ToList();

                Console.WriteLine($"Open tab with procedures for file: {procedure.FileName} and push ENTER...");
                Console.ReadKey(false);

                try
                {
                    var addButton = driver.FindElement((By.CssSelector(procedure.Selector)));
                    var parentDiv = addButton.GetParent().GetParent().GetParent().GetParent().GetParent();

                    foreach (var record in records)
                    {
                        addButton.Click();

                        parentDiv.InsertInput((int)TableIndexes.Data, Helpers.Helpers.ConvertDate(record.DataZnieczulenia));
                        parentDiv.InsertSelectByText((int)TableIndexes.Rok, year);
                        parentDiv.InsertSelectByText((int)TableIndexes.KodZabiegu, record.KodZabiegu);
                        parentDiv.InsertInput((int)TableIndexes.OsobaWykonujaca, author);
                        parentDiv.InsertSelectByIndex((int)TableIndexes.MiejsceWykonania, placeIndex);
                        parentDiv.InsertSelectByIndex((int)TableIndexes.NazwaStazu, programNameIndex);
                        parentDiv.InsertInput((int)TableIndexes.InicjalyPacjenta, $"{record.Inicjaly}{record.Wiek}");
                        parentDiv.InsertSelectByText((int)TableIndexes.PlecPacjenta, Helpers.Helpers.MapSex(record.Plec));
                        parentDiv.InsertInput((int)TableIndexes.WykonawcaAsysty, record.Nadzor);
                        parentDiv.InsertInput((int)TableIndexes.GrupaProcedury, Helpers.Helpers.PrepareProcedureGroup(record.ASA, record.NazwaZabiegu));
                    }

                    Console.Clear();
                    Console.WriteLine(
                        "Done! Before we go to next procedures file, let's save changes using 'Save' button below and then press ENTER...");
                    Console.ReadKey(false);

                }
                catch (Exception ex)
                {
                    Console.WriteLine("There was a problem with writing procedures...");
                    Console.WriteLine("Technical exception log below:");
                    Console.WriteLine(ex);
                    driver.Quit();

                    Console.ReadKey(false);
                }
            }

            Console.Clear();
            Console.WriteLine("That's all Folks! Thank you for your work, see you soon! :) Push ENTER to finish...");
            Console.ReadKey(false);
            driver.Quit();
        }
    }
}