﻿using BookStore.Application.DTOs.BookDtos;
using FluentValidation;

namespace BookStore.Application.Validators.Books;
public class CreateBookDtoValidator : AbstractValidator<CreateBookDto>
{
    public CreateBookDtoValidator()
    {
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.Description).NotEmpty();
        RuleFor(x => x.PageCount).GreaterThan(0);
        RuleFor(x => x.Language).IsInEnum();
        RuleFor(x => x.Publisher).IsInEnum();
        RuleFor(x => x.CategoryIds).NotEmpty();
        RuleFor(x => x.AuthorIds).NotEmpty();
        RuleFor(x => x.PdfFile).NotNull().Must(f => f.ContentType == "application/pdf").WithMessage("Only PDF allowed");
        RuleFor(x => x.CoverImage).NotNull().Must(f => f.ContentType.StartsWith("image/")).WithMessage("Only image files allowed");
    }
}

