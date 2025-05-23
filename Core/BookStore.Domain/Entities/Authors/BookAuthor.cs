﻿using BookStore.Domain.Entities.Books;

namespace BookStore.Domain.Entities.Authors;
public class BookAuthor
{
    public int AuthorId { get; set; }
    public Author Author { get; set; }

    public int BookId { get; set; }
    public Book Book { get; set; }
}


