using BookStore.Application.DTOs.BookDtos;
using FluentValidation;

namespace BookStore.Application.Validators.Books;
public class UpdateBookDtoValidator : AbstractValidator<UpdateBookDto>
{
    public UpdateBookDtoValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.Description).NotEmpty();
        RuleFor(x => x.PageCount).GreaterThan(0);
        RuleFor(x => x.Language).IsInEnum();
        RuleFor(x => x.Publisher).IsInEnum();
        RuleFor(x => x.CategoryIds).NotEmpty();
        RuleFor(x => x.AuthorIds).NotEmpty();

        When(x => x.PdfFile != null, () =>
        {
            RuleFor(x => x.PdfFile.ContentType).Equal("application/pdf").WithMessage("Only PDF allowed");
        });

        When(x => x.CoverImage != null, () =>
        {
            RuleFor(x => x.CoverImage.ContentType).Must(type => type.StartsWith("image/")).WithMessage("Only image files allowed");
        });
    }
}

