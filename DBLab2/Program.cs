using DBLab2.Constants;
using DBLab2.Controllers;
using DBLab2.Data;
using DBLab2.Models;

namespace DBLab2
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var context = new BookstoreContext();
            var dbService = new DbService(context);
            var databaseController = new DatabaseController(dbService);
            bool isRunning = true;

            while (isRunning)
            {
                Console.Clear();
                Console.WriteLine(MessageConstants.WELCOME_MESSAGE);

                for (int i = 0; i < MessageConstants.OPTIONS.Length; i++)
                {
                    Console.WriteLine($"{i + 1}. {MessageConstants.OPTIONS[i]}");
                }

                Console.Write("\n" + MessageConstants.PROMPT_SELECT_OPTION);

                var choice = Console.ReadLine();

                switch (choice?.Trim()) // Trims leading and trailing whitespaces if choice is not null.
                {
                    case "1":
                        await databaseController.ListInventory();
                        break;
                    case "2":
                        await databaseController.ListAllBooks();
                        break;
                    case "3":
                        await databaseController.ListAllAuthors();
                        break;
                    case "4":
                        await databaseController.AddBookToStore();
                        break;
                    case "5":
                        await databaseController.RemoveBookFromStore();
                        break;
                    case "6":
                        await databaseController.AddBook();
                        break;
                    case "7":
                        await databaseController.AddAuthor();
                        break;
                    case "8":
                        await databaseController.EditBook();
                        break;
                    case "9":
                        await databaseController.EditAuthor();
                        break;
                    case "10":
                        await databaseController.RemoveBook();
                        break;
                    case "11":
                        await databaseController.RemoveAuthor();
                        break;
                    case "12":
                        isRunning = false;
                        break;
                    default:
                        Console.WriteLine();
                        break;
                }
            }
        }
    }
}
