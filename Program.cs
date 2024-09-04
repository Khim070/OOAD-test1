using System.Text.Json;

namespace OOAD_test1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Tasks.Load("expense.txt");
            }
            catch (Exception ex) { 
                Console.WriteLine(ex.Message);
                return;
            }
            List<Expense> expenses = Tasks.Expenses;
            Expense? max = Tasks.MaxAmountExpense;
            double total = Tasks.AmountTotal;
            Console.WriteLine($"{"No"}{"Amount",10}{"Descr",10}"); // interpolated String
            string line = new string('-', 50);
            Console.WriteLine(line);
            expenses.ForEach(x => Console.WriteLine($"{x.No}{x.Amount, 10}{x.Descr, 10}"));
            Console.WriteLine(line);

            Console.WriteLine($"AmountTotal = {Tasks.AmountTotal:C2}");
            Console.WriteLine($"Max-amount expense contain no: {max?.No}, descr: {max?.Descr}"); // safe navigation oerator or Elvis operator
        }

        public static class Tasks
        {
            private static List<Expense>? expense = null;
            public static void Load(string filename)
            {
                try
                {
                    string jsonData = File.ReadAllText(filename);
                    JsonSerializerOptions options = new JsonSerializerOptions()
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    };
                    expense = JsonSerializer.Deserialize<List<Expense>>(jsonData, options) ?? new();
                }
                catch (Exception) { throw; }
            }
            public static List<Expense> Expenses => expense ?? new();
            public static double AmountTotal => expense?.Sum(x => x.Amount) ?? 0;
            public static Expense? MaxAmountExpense
            {
                get
                {
                    if (expense == null) { return null; }
                    double maxAmount = expense.Max(x => x.Amount);
                    return expense.FirstOrDefault(x => x.Amount == maxAmount);
                }
            }
        }
    }
}
