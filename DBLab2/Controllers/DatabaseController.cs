using DBLab2.Constants;
using DBLab2.Data;
using DBLab2.Models;

namespace DBLab2.Controllers
{
    internal class DatabaseController
    {
        private DbService _dbService;

        public DatabaseController(DbService dbService)
        {
            _dbService = dbService;
        }

        /// <summary>
        /// Lists all the inventory for the stores.
        /// </summary>
        /// <returns>A Task</returns>
        public async Task ListInventory()
        {
            var storeInventory = await _dbService.GetAllInventory();
            var stores = await _dbService.GetAllStores();
            var books = await _dbService.GetAllBooks();

            var result = from store in stores
                         join unit in storeInventory
                         on store.StoreId equals unit.StoreId
                         join book in books
                         on unit.Isbn13 equals book.Isbn13
                         group (book, unit) by store.StoreName into g
                         select new
                         {
                             StoreName = g.Key,
                             Books = g.ToList()
                         };

            foreach (var s in result)
            {
                Console.WriteLine($"Store: {s.StoreName}");

                foreach (var i in s.Books)
                {
                    Console.WriteLine($"\tBook: {i.book.Title} | ISBN13: {i.book.Isbn13} | Amount: {i.unit.Amount}");
                }

                Console.WriteLine();
            }

            PromptForInputAndWait();
        }

        /// <summary>
        /// Lists all the books.
        /// </summary>
        /// <returns>A Task</returns>
        public async Task ListAllBooks()
        {
            var allBooks = await _dbService.GetAllBooks();
            var allAuthors = await _dbService.GetAllAuthors();
            var allLanguages = await _dbService.GetAllLanguages();
            var allPublishers = await _dbService.GetAllPublishers();

            var result = from b in allBooks
                         join a in allAuthors
                         on b.AuthorId equals a.AuthorId
                         join l in allLanguages
                         on b.LanguageId equals l.LanguageId
                         join p in allPublishers
                         on b.PublisherId equals p.PublisherId
                         select b;

            foreach (var b in allBooks)
            {
                Console.WriteLine(
                    $"==={b.Title}===" +
                    $"\n\tAuthor: {b.Author.FirstName} {b.Author.LastName}" +
                    $"\n\tISBN13: {b.Isbn13}" +
                    $"\n\tPrice: {b.Price}" +
                    $"\n\tLanguage: {b.Language.LanguageName}" +
                    $"\n\tPublisher: {b.Publisher.PublisherName}" +
                    $"\n\tDate published: {b.DatePublished}\n");
            }

            PromptForInputAndWait();
        }

        /// <summary>
        /// Lists all the authors.
        /// </summary>
        /// <returns>A Task</returns>
        public async Task ListAllAuthors()
        {
            var allAuthors = await _dbService.GetAllAuthors();

            foreach (var a in allAuthors)
            {
                Console.WriteLine(
                    $"==={a.FirstName} {a.LastName}===" +
                    $"\n\tBirthdate: {a.Birthdate}\n");
            }

            PromptForInputAndWait();
        }

        /// <summary>
        /// Adds a new book to a store by asking the user for information.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task AddBookToStore()
        {
            var availableBooks = await _dbService.GetAllBooks();
            var availableStores = await _dbService.GetAllStores();
            var storeInventory = await _dbService.GetAllInventory();

            int storeId;
            int? storeIndex;
            int? bookIndex;
            int? amount;

            Book selectedBook;

            storeIndex = RequestStoreIndex(availableStores);

            if (storeIndex == null)
            {
                return;
            }

            bookIndex = RequestBookIndex(availableBooks);

            if (bookIndex == null)
            {
                return;
            }

            selectedBook = availableBooks[(int)bookIndex - 1];

            amount = RequestBookAmount();

            if (amount == null)
            {
                return;
            }

            storeId = availableStores[(int)storeIndex - 1].StoreId;

            var booksInStore = from inventory in storeInventory
                               where inventory.StoreId == storeId
                               select inventory;

            if (booksInStore.Any(b => b.Isbn13 == selectedBook.Isbn13))
            {
                var bookToUpdate = booksInStore.Where(b => b.Isbn13 == selectedBook.Isbn13).First();
                bookToUpdate.Amount += (int)amount;

                await _dbService.UpdateInventory(bookToUpdate);
            }
            else
            {
                await _dbService.AddInventoryItem(
                    new() { StoreId = storeId, Isbn13 = selectedBook.Isbn13, Amount = (int)amount });

                Console.WriteLine("\n" + MessageConstants.MESSAGE_BOOK_ADDED_TO_STORE + "\n");
                PromptForInputAndWait();
            }
        }

        /// <summary>
        /// Removes s book from the a store.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task RemoveBookFromStore()
        {
            var availableStores = await _dbService.GetAllStores();
            var allBooks = await _dbService.GetAllBooks();
            var allInventory = await _dbService.GetAllInventory();

            List<StoreInventory> storeInventory;

            int storeId;
            int? storeIndex;
            int? bookIndex;
            int? amount;

            storeIndex = RequestStoreIndex(availableStores);

            if (storeIndex == null)
            {
                return;
            }

            storeId = availableStores[(int)storeIndex - 1].StoreId;

            storeInventory = await GetAllBooksFromStore(storeId);

            if (storeInventory.Count <= 0)
            {
                Console.WriteLine(MessageConstants.MESSAGE_NO_BOOKS_IN_STORE);
                Console.WriteLine(MessageConstants.PROMPT_PRESS_ANY_KEY_TO_CONTINUE);
                return;
            }

            Console.WriteLine(string.Format(MessageConstants.MESSAGE_BOOKS_IN_STORE, availableStores.FirstOrDefault(s => s.StoreId == storeId)?.StoreName));

            var booksInStore = from book in allBooks
                               join inventory in allInventory
                               on book.Isbn13 equals inventory.Isbn13
                               where inventory.StoreId == storeId
                               select book;

            bookIndex = RequestBookIndex(booksInStore.ToList());

            if (bookIndex == null)
            {
                return;
            }

            var selectedBook = booksInStore.ElementAt((int)bookIndex - 1);

            amount = RequestBookAmount();

            if (amount == null)
            {
                return;
            }

            StoreInventory? inventoryToUpdate = allInventory.Where(i => i.StoreId == storeId && i.Isbn13.Equals(selectedBook.Isbn13)).FirstOrDefault();

            if (inventoryToUpdate != null)
            {
                if ((int)amount >= inventoryToUpdate.Amount)
                {
                    await _dbService.DeleteInventory(storeId, selectedBook.Isbn13);
                }
                else
                {
                    inventoryToUpdate.Amount -= (int)amount;
                    await _dbService.UpdateInventory(inventoryToUpdate);
                }

                Console.WriteLine("\n" + MessageConstants.MESSAGE_BOOK_REMOVED_FROM_STORE + "\n");
                PromptForInputAndWait();
            }
        }

        /// <summary>
        /// Adds a new book by asking the user for information.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task AddBook()
        {
            var allBooks = await _dbService.GetAllBooks();
            var allAuthors = await _dbService.GetAllAuthors();
            var allPublishers = await _dbService.GetAllPublishers();
            var allLanguages = await _dbService.GetAllLanguages();

            int? authorIndex;
            int? publisherIndex;
            int? languageIndex;

            string title;
            string isbn13;

            decimal? price;

            DateOnly? publishingDate;

            authorIndex = RequestAuthorIndex(allAuthors);

            if (authorIndex == null)
            {
                return;
            }

            publisherIndex = RequestPublisherIndex(allPublishers);

            if (publisherIndex == null)
            {
                return;
            }

            languageIndex = RequestLanguageIndex(allLanguages);

            if (languageIndex == null)
            {
                return;
            }

            Console.Write("\n" + MessageConstants.PROMPT_ENTER_TITLE);
            title = Console.ReadLine() ?? string.Empty;

            Console.Write("\n" + MessageConstants.PROMPT_ENTER_ISBN13);
            isbn13 = Console.ReadLine() ?? string.Empty;

            if (!ValidateISBN13(isbn13).Result)
            {
                PrintInvalidOption();
                return;
            }

            price = RequestBookPrice();

            if (price == null)
            {
                return;
            }

            publishingDate = RequestBookPublishingDate();

            if (publishingDate == null)
            {
                return;
            }

            Book bookToAdd = new Book()
            {
                AuthorId = allAuthors[(int)authorIndex - 1].AuthorId,
                PublisherId = allPublishers[(int)publisherIndex - 1].PublisherId,
                LanguageId = allLanguages[(int)languageIndex - 1].LanguageId,
                Title = title,
                Isbn13 = isbn13,
                Price = price,
                DatePublished = publishingDate
            };

            await _dbService.AddBook(bookToAdd);

            Console.WriteLine("\n" + MessageConstants.MESSAGE_BOOK_ADDED + "\n");
            PromptForInputAndWait();
        }

        /// <summary>
        /// Adds a new author by asking the user for information.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task AddAuthor()
        {
            string firstName;
            string lastName;

            DateOnly birthdate;

            Console.Write("\n" + MessageConstants.PROMPT_ENTER_AUTHOR_FIRST_NAME);
            firstName = Console.ReadLine() ?? string.Empty;

            Console.Write("\n" + MessageConstants.PROMPT_ENTER_AUTHOR_LAST_NAME);
            lastName = Console.ReadLine() ?? string.Empty;

            Console.Write("\n" + MessageConstants.PROMPT_ENTER_AUTHOR_BIRTHDATE);

            if (!DateOnly.TryParse(Console.ReadLine(), out birthdate))
            {
                PrintInvalidOption();
                return;
            }

            Author authorToAdd = new Author()
            {
                FirstName = firstName,
                LastName = lastName,
                Birthdate = birthdate
            };

            await _dbService.AddAuthor(authorToAdd);

            Console.WriteLine("\n" + MessageConstants.MESSAGE_AUTHOR_ADDED + "\n");
            PromptForInputAndWait();
        }

        /// <summary>
        /// Edits a book by asking the user for information.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task EditBook()
        {
            var allBooks = await _dbService.GetAllBooks();
            var allAuthors = await _dbService.GetAllAuthors();
            var allPublishers = await _dbService.GetAllPublishers();
            var allLanguages = await _dbService.GetAllLanguages();

            Book selectedBook;

            int? bookIndex = RequestBookIndex(allBooks);

            if (bookIndex == null)
            {
                return;
            }

            selectedBook = allBooks.ElementAt((int)bookIndex - 1);

            int? authorIndex = RequestAuthorIndex(allAuthors);

            if (authorIndex == null)
            {
                return;
            }

            int? publisherIndex = RequestPublisherIndex(allPublishers);

            if (publisherIndex == null)
            {
                return;
            }

            int? languageIndex = RequestLanguageIndex(allLanguages);

            if (languageIndex == null)
            {
                return;
            }

            string title = RequestBookTitle();

            decimal? price = RequestBookPrice();

            if (price == null)
            {
                return;
            }

            DateOnly? publishingDate = RequestBookPublishingDate();

            if (publishingDate == null)
            {
                return;
            }

            selectedBook.AuthorId = allAuthors[(int)authorIndex - 1].AuthorId;
            selectedBook.PublisherId = allPublishers[(int)publisherIndex - 1].PublisherId;
            selectedBook.LanguageId = allLanguages[(int)languageIndex - 1].LanguageId;
            selectedBook.Title = title;
            selectedBook.Price = (decimal)price;
            selectedBook.DatePublished = (DateOnly)publishingDate;

            await _dbService.UpdateBook(selectedBook);

            Console.WriteLine("\n" + MessageConstants.MESSAGE_BOOK_EDITED + "\n");
            PromptForInputAndWait();
        }

        /// <summary>
        /// Edits an author by asking the user for information.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task EditAuthor()
        {
            var allAuthors = await _dbService.GetAllAuthors();

            int? authorId = RequestAuthorIndex(allAuthors);

            if (authorId == null)
            {
                return;
            }

            string firstName = RequestInputFromPrompt(MessageConstants.PROMPT_ENTER_AUTHOR_FIRST_NAME);
            string lastName = RequestInputFromPrompt(MessageConstants.PROMPT_ENTER_AUTHOR_LAST_NAME);

            DateOnly? birthdate = RequestAuthorBirthdate();

            if (birthdate == null)
            {
                return;
            }

            Author selectedAuthor = allAuthors.ElementAt((int)authorId - 1);

            selectedAuthor.FirstName = firstName;
            selectedAuthor.LastName = lastName;
            selectedAuthor.Birthdate = birthdate;

            await _dbService.UpdateAuthor(selectedAuthor);

            Console.WriteLine("\n" + MessageConstants.MESSAGE_AUTHOR_EDITED + "\n");
            PromptForInputAndWait();
        }

        /// <summary>
        /// Removes a book by asking the user for the book ID.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task RemoveBook()
        {
            var allBooks = await _dbService.GetAllBooks();

            int? bookIndex = RequestBookIndex(allBooks);

            if (bookIndex == null)
            {
                return;
            }

            await _dbService.DeleteBook(allBooks[(int)bookIndex - 1].Isbn13);

            Console.WriteLine("\n" + MessageConstants.MESSAGE_BOOK_REMOVED + "\n");
            PromptForInputAndWait();
        }

        /// <summary>
        /// Removes an author by asking the user for the author ID.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task RemoveAuthor()
        {
            var allAuthors = await _dbService.GetAllAuthors();

            int? authorIndex = RequestAuthorIndex(allAuthors);

            if (authorIndex == null)
            {
                return;
            }

            await _dbService.DeleteAuthor(allAuthors[(int)authorIndex - 1].AuthorId);

            Console.WriteLine("\n" + MessageConstants.MESSAGE_AUTHOR_REMOVED + "\n");
            PromptForInputAndWait();
        }

        /// <summary>
        /// Prompts the user to press any key and waits until one is pressed.
        /// </summary>
        private void PromptForInputAndWait()
        {
            Console.WriteLine(MessageConstants.PROMPT_PRESS_ANY_KEY_TO_CONTINUE);
            Console.ReadKey();
        }

        /// <summary>
        /// Prints out that the user selected an invalid option and waits for
        /// the user to press any key.
        /// </summary>
        private void PrintInvalidOption()
        {
            Console.WriteLine(MessageConstants.MESSAGE_INVALID_OPTION);
            Console.WriteLine(MessageConstants.PROMPT_PRESS_ANY_KEY_TO_CONTINUE);
            Console.ReadKey();
        }

        /// <summary>
        /// Displays a list of stores and asks the user to enter the ID of a store.
        /// </summary>
        /// <param name="availableStores">The stores available to choose from.</param>
        /// <returns>The ID the user entered or null if the input was invalid.</returns>
        private int? RequestStoreIndex(List<Store> availableStores)
        {
            int storeId;

            for (int i = 0; i < availableStores.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {availableStores[i].StoreName}");
            }

            Console.Write("\n" + MessageConstants.PROMPT_SELECT_STORE);

            if (!int.TryParse(Console.ReadLine(), out storeId) ||
                storeId <= 0 ||
                storeId > availableStores.Count)
            {
                PrintInvalidOption();
                return null;
            }

            return storeId;
        }

        /// <summary>
        /// Displays a list of books and asks the user to enter the ID of a book.
        /// </summary>
        /// <param name="availableBooks">The books available to choose from.</param>
        /// <returns>The ID the user entered or null if the input was invalid.</returns>
        private int? RequestBookIndex(List<Book> availableBooks)
        {
            int bookId;

            for (int i = 0; i < availableBooks.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {availableBooks[i].Title}");
            }

            Console.Write("\n" + MessageConstants.PROMPT_SELECT_BOOK);

            if (!int.TryParse(Console.ReadLine(), out bookId) ||
                bookId <= 0 ||
                bookId > availableBooks.Count)
            {
                PrintInvalidOption();
                return null;
            }

            return bookId;
        }

        /// <summary>
        /// Asks the user to enter an amount of books.
        /// </summary>
        /// <returns>The amount entered or null if the input was invalid.</returns>
        private int? RequestBookAmount()
        {
            int amount;

            Console.Write("\n" + MessageConstants.PROMP_ENTER_AMOUNT);

            if (!int.TryParse(Console.ReadLine(), out amount) ||
                amount <= 0)
            {
                PrintInvalidOption();
                return null;
            }

            return amount;
        }

        /// <summary>
        /// Displays a list of authors and asks the user to enter the ID of an author.
        /// </summary>
        /// <param name="availableAuthors">The authors available to choose from.</param>
        /// <returns>The ID the user entered or null if the input was invalid.</returns>
        private int? RequestAuthorIndex(List<Author> availableAuthors)
        {
            int authorIndex;

            for (int i = 0; i < availableAuthors.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {availableAuthors[i].FirstName} {availableAuthors[i].LastName}");
            }

            Console.Write("\n" + MessageConstants.PROMPT_SELECT_AUTHOR);

            if (!int.TryParse(Console.ReadLine(), out authorIndex) ||
                authorIndex <= 0 ||
                authorIndex > availableAuthors.Count)
            {
                PrintInvalidOption();
                return null;
            }

            return authorIndex;
        }

        /// <summary>
        /// Displays a list of publishers and asks the user to enter the ID of a publisher.
        /// </summary>
        /// <param name="availablePublishers">The publishers available to choose from.</param>
        /// <returns>The ID the user entered or null if the input was invalid.</returns>
        private int? RequestPublisherIndex(List<Publisher> availablePublishers)
        {
            int publisherIndex;

            for (int i = 0; i < availablePublishers.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {availablePublishers[i].PublisherName}");
            }

            Console.Write("\n" + MessageConstants.PROMPT_SELECT_PUBLISHER);

            if (!int.TryParse(Console.ReadLine(), out publisherIndex) ||
                publisherIndex <= 0 ||
                publisherIndex > availablePublishers.Count)
            {
                PrintInvalidOption();
                return null;
            }

            return publisherIndex;
        }

        /// <summary>
        /// Displays a list of languages and asks the user to enter the ID of a language.
        /// </summary>
        /// <param name="availableLanguages">The languages available to choose from.</param>
        /// <returns>The ID the user entered or null if the input was invalid.</returns>
        private int? RequestLanguageIndex(List<Language> availableLanguages)
        {
            int languageIndex;

            for (int i = 0; i < availableLanguages.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {availableLanguages[i].LanguageName}");
            }

            Console.Write("\n" + MessageConstants.PROMPT_SELECT_LANGUAGE);

            if (!int.TryParse(Console.ReadLine(), out languageIndex) ||
                languageIndex <= 0 ||
                languageIndex > availableLanguages.Count)
            {
                PrintInvalidOption();
                return null;
            }

            return languageIndex;
        }

        /// <summary>
        /// Asks the user to enter the title of a book.
        /// </summary>
        /// <returns>The string entered or an empty string if the input was null.</returns>
        private string RequestBookTitle()
        {
            Console.Write("\n" + MessageConstants.PROMPT_ENTER_TITLE);
            return Console.ReadLine() ?? string.Empty;
        }

        /// <summary>
        /// Asks the user to enter the price of a book.
        /// </summary>
        /// <returns>The decimal entered or null if the price entered was invalid.</returns>
        private decimal? RequestBookPrice()
        {
            decimal price;

            Console.Write("\n" + MessageConstants.PROMPT_ENTER_PRICE);

            if (!decimal.TryParse(Console.ReadLine(), out price) ||
                price <= 0)
            {
                PrintInvalidOption();
                return null;
            }

            return price;
        }

        /// <summary>
        /// Asks the user to enter the publishing date of a book.
        /// </summary>
        /// <returns>The decimal entered or null if the price entered was invalid.</returns>
        private DateOnly? RequestBookPublishingDate()
        {
            DateOnly publishingDate;

            Console.Write("\n" + MessageConstants.PROMPT_ENTER_DATE_PUBLISHED);

            if (!DateOnly.TryParse(Console.ReadLine(), out publishingDate))
            {
                PrintInvalidOption();
                return null;
            }

            return publishingDate;
        }

        /// <summary>
        /// Displays a prompt message and asks the user to enter information
        /// related to this message.
        /// </summary>
        /// <returns>The input, or an empty string if the input was null.</returns>
        private string RequestInputFromPrompt(string messagePrompt)
        {
            Console.Write("\n" + messagePrompt);
            return Console.ReadLine() ?? string.Empty;
        }

        /// <summary>
        /// Asks the user to enter the birthdate of an author.
        /// </summary>
        /// <returns>The birthdate entered or null if the birthdate entered was invalid.</returns>
        private DateOnly? RequestAuthorBirthdate()
        {
            DateOnly birthdate;

            Console.Write("\n" + MessageConstants.PROMPT_ENTER_AUTHOR_BIRTHDATE);

            if (!DateOnly.TryParse(Console.ReadLine(), out birthdate))
            {
                PrintInvalidOption();
                return null;
            }

            return birthdate;
        }

        /// <summary>
        /// Returns the store inventory of a specified store.
        /// </summary>
        /// <param name="storeId">The ID of the store to get the inventory from.</param>
        /// <returns>A list of store inventory from the specified store.</returns>
        private async Task<List<StoreInventory>> GetAllBooksFromStore(int storeId)
        {
            var allInventory = await _dbService.GetAllInventory();
            var storeInventory = allInventory.Where(i => i.Store.StoreId == storeId).ToList();

            return storeInventory;
        }

        /// <summary>
        /// Validates that the entered ISBN13 is 13 characters long
        /// and that there are no other books with the same ISBN13.
        /// </summary>
        /// <param name="dbService">The DB service to use.</param>
        /// <param name="isbn13">The ISBN13 to validate.</param>
        /// <returns>True if the entered ISBN13 is exactly 13 characters long and there
        /// is no other book with the same ISBN13, false otherwise.</returns>
        private async Task<bool> ValidateISBN13(string isbn13)
        {
            var allBooks = await _dbService.GetAllBooks();

            bool isCorrectLength = isbn13.Length == 13 ? true : false;
            bool isUnique = !allBooks.Any(b => b.Isbn13.Equals(isbn13));

            return isCorrectLength && isUnique;
        }
    }
}
