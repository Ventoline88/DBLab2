namespace DBLab2.Constants
{
    internal static class MessageConstants
    {
        /// <summary>
        /// Message for the welcome message.
        /// </summary>
        public const string WELCOME_MESSAGE = "=== Bookstore Database Admin Tools ===";
        /// <summary>
        /// Message to indicate an invalid option.
        /// </summary>
        public const string MESSAGE_INVALID_OPTION = "Invalid Option";
        /// <summary>
        /// Message to indicate that a store has no books.
        /// </summary>
        public const string MESSAGE_NO_BOOKS_IN_STORE = "There are no books in the selected store";
        /// <summary>
        /// Message to indicate the books in a store.
        /// </summary>
        public const string MESSAGE_BOOKS_IN_STORE = "Books in store {0}:";
        /// <summary>
        /// Message to indicate that a book was added to a store
        /// </summary>
        public const string MESSAGE_BOOK_ADDED_TO_STORE = "Book added to store.";
        /// <summary>
        /// Message to indicate that a book was removed from a store.
        /// </summary>
        public const string MESSAGE_BOOK_REMOVED_FROM_STORE = "Book removed from store";
        /// <summary>
        /// Message to indicate that a book was added.
        /// </summary>
        public const string MESSAGE_BOOK_ADDED = "Book added.";
        /// <summary>
        /// Message to indicate that an author was added.
        /// </summary>
        public const string MESSAGE_AUTHOR_ADDED = "Author added";
        /// <summary>
        /// Message to indicate that a book was edited.
        /// </summary>
        public const string MESSAGE_BOOK_EDITED = "Book edited";
        /// <summary>
        /// Message to indicate that an author was edited.
        /// </summary>
        public const string MESSAGE_AUTHOR_EDITED = "Author edited";
        /// <summary>
        /// Message to indicate that a book was removed.
        /// </summary>
        public const string MESSAGE_BOOK_REMOVED = "Book removed";
        /// <summary>
        /// Message to indicate that an author was removed.
        /// </summary>
        public const string MESSAGE_AUTHOR_REMOVED = "Author removed";

        /// <summary>
        /// Prompt to indicate that the user should select an option.
        /// </summary>
        public const string PROMPT_SELECT_OPTION = "Select an option: ";
        /// <summary>
        /// Prompt to indicate that the user should select a store.
        /// </summary>
        public const string PROMPT_SELECT_STORE = "Select a store: ";
        /// <summary>
        /// Prompt to indicate that the user should select a book.
        /// </summary>
        public const string PROMPT_SELECT_BOOK = "Select a book: ";
        /// <summary>
        /// Prompt to indicate that the user should input an amount.
        /// </summary>
        public const string PROMP_ENTER_AMOUNT = "Enter an amount: ";
        /// <summary>
        /// Prompt to indicate that the user should select an author.
        /// </summary>
        public const string PROMPT_SELECT_AUTHOR = "Select the author: ";
        /// <summary>
        /// Prompt to indicate that the user should select a publisher.
        /// </summary>
        public const string PROMPT_SELECT_PUBLISHER = "Select the publisher: ";
        /// <summary>
        /// Prompt to indicate that the user shoul select a language.
        /// </summary>
        public const string PROMPT_SELECT_LANGUAGE = "Select the language: ";
        /// <summary>
        /// Prompt to indicate that the user should enter the title of the book.
        /// </summary>
        public const string PROMPT_ENTER_TITLE = "Enter the title of the book: ";
        /// <summary>
        /// Prompt to indicate that the user should enter the ISBN13 of the book.
        /// </summary>
        public const string PROMPT_ENTER_ISBN13 = "Enter the ISBN13 of the book: ";
        /// <summary>
        /// Prompt to indicate that the user should enter the price of the book.
        /// </summary>
        public const string PROMPT_ENTER_PRICE = "Enter the price of the book: ";
        /// <summary>
        /// Prompt to indicate that the user should enter the publishing date of the book.
        /// </summary>
        public const string PROMPT_ENTER_DATE_PUBLISHED = "Enter the date the book was published (yyyy-mm-dd): ";
        /// <summary>
        /// Prompt to indicate that the user should enter the first name of an author.
        /// </summary>
        public const string PROMPT_ENTER_AUTHOR_FIRST_NAME = "Enter the first name of the author: ";
        /// <summary>
        /// Prompt to indicate that the user should enter the last name of the author.
        /// </summary>
        public const string PROMPT_ENTER_AUTHOR_LAST_NAME = "Enter the last name of the author: ";
        /// <summary>
        /// Prompt to indicate that the user should enter the birthdate of the author.
        /// </summary>
        public const string PROMPT_ENTER_AUTHOR_BIRTHDATE = "Enter the birthdate of the author (yyyy-mm-dd): ";
        /// <summary>
        /// Prompt to indicate to the user to press a key to continue.
        /// </summary>
        public const string PROMPT_PRESS_ANY_KEY_TO_CONTINUE = "Press any key to continue";

        /// <summary>
        /// Array of options that the user can choose from.
        /// </summary>
        public static readonly string[] OPTIONS =
        {
            "List inventory",
            "List all books",
            "List all authors",
            "Add Book To Store",
            "Remove Book From Store",
            "Add Book",
            "Add Author",
            "Edit Book",
            "Edit Author",
            "Remove Book",
            "Remove Author",
            "Exit"
        };
    }
}
