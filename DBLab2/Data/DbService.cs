using DBLab2.Models;
using Microsoft.EntityFrameworkCore;

namespace DBLab2.Data
{
    internal class DbService
    {
        private readonly BookstoreContext _context;

        public DbService(BookstoreContext context)
        {
            _context = context;
        }

        // CREATE

        /// <summary>
        /// Adds a new inventory item.
        /// </summary>
        /// <param name="inventoryItem">The inventory to add.</param>
        /// <returns>The added inventory item.</returns>
        public async Task<StoreInventory> AddInventoryItem(StoreInventory inventoryItem)
        {
            _context.StoreInventories.Add(inventoryItem);
            await _context.SaveChangesAsync();
            return inventoryItem;
        }

        /// <summary>
        /// Adds a new book.
        /// </summary>
        /// <param name="book">The book to add.</param>
        /// <returns>The added book.</returns>
        public async Task<Book> AddBook(Book book)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            return book;
        }

        /// <summary>
        /// Adds a new author.
        /// </summary>
        /// <param name="author">The author to add.</param>
        /// <returns>The added author.</returns>
        public async Task<Author> AddAuthor(Author author)
        {
            _context.Authors.Add(author);
            await _context.SaveChangesAsync();
            return author;
        }

        // READ

        /// <summary>
        /// Gets the inventory of all stores.
        /// </summary>
        /// <returns>A list of all the inventory of all the stores.</returns>
        public async Task<List<StoreInventory>> GetAllInventory()
        {
            return await _context.StoreInventories.ToListAsync();
        }

        /// <summary>
        /// Gets all the stores.
        /// </summary>
        /// <returns>A list of all the stores.</returns>
        public async Task<List<Store>> GetAllStores()
        {
            return await _context.Stores.ToListAsync();
        }

        /// <summary>
        /// Gets all books.
        /// </summary>
        /// <returns>A list of all the books.</returns>
        public async Task<List<Book>> GetAllBooks()
        {
            return await _context.Books.ToListAsync();
        }

        /// <summary>
        /// Gets all authors.
        /// </summary>
        /// <returns>A list of all the authors.</returns>
        public async Task<List<Author>> GetAllAuthors()
        {
            return await _context.Authors.ToListAsync();
        }

        /// <summary>
        /// Gets all publishers.
        /// </summary>
        /// <returns>A list of all publishers.</returns>
        public async Task<List<Publisher>> GetAllPublishers()
        {
            return await _context.Publishers.ToListAsync();
        }

        /// <summary>
        /// Gets all the languages.
        /// </summary>
        /// <returns>A list of all the languages.</returns>
        public async Task<List<Language>> GetAllLanguages()
        {
            return await _context.Languages.ToListAsync();
        }

        // UPDATE

        /// <summary>
        /// Updates the store inventory.
        /// </summary>
        /// <param name="storeInventory">The store inventory to update.</param>
        /// <returns>The updated store inventory.</returns>
        public async Task<StoreInventory> UpdateInventory(StoreInventory storeInventory)
        {
            _context.StoreInventories.Update(storeInventory);
            await _context.SaveChangesAsync();
            return storeInventory;
        }

        /// <summary>
        /// Updates a book.
        /// </summary>
        /// <param name="book">The book to update.</param>
        /// <returns>The updated book.</returns>
        public async Task<Book> UpdateBook(Book book)
        {
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
            return book;
        }

        /// <summary>
        /// Updates an author.
        /// </summary>
        /// <param name="author">The author to update.</param>
        /// <returns>The updated author.</returns>
        public async Task<Author> UpdateAuthor(Author author)
        {
            _context.Authors.Update(author);
            await _context.SaveChangesAsync();
            return author;
        }

        // DELETE

        /// <summary>
        /// Deletes a store inventory record from the database..
        /// </summary>
        /// <param name="id">The ID of the store inventory to delete.</param>
        /// <returns>True if the record was deleted, false otherwise.</returns>
        public async Task<bool> DeleteInventory(int storeId, string isbn13)
        {
            var storeInventoryToDelete = await _context.StoreInventories.FindAsync(storeId, isbn13);

            if (storeInventoryToDelete == null)
            {
                return false;
            }

            _context.StoreInventories.Remove(storeInventoryToDelete);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Deletes a book from the database.
        /// </summary>
        /// <param name="isbn13">The ISBN13 of the book to delete.</param>
        /// <returns>True if the book was deleted, false otherwise.</returns>
        public async Task<bool> DeleteBook(string isbn13)
        {
            var foundBook = await _context.Books.FindAsync(isbn13);

            if (foundBook == null)
            {
                return false;
            }

            _context.Books.Remove(foundBook);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Deletes an uthor from the database.
        /// </summary>
        /// <param name="id">The ID of the author.</param>
        /// <returns>True if the author was deleted, false otherwise.</returns>
        public async Task<bool> DeleteAuthor(int id)
        {
            var foundAuthor = await _context.Authors.FindAsync(id);

            if (foundAuthor == null)
            {
                return false;
            }

            _context.Authors.Remove(foundAuthor);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
