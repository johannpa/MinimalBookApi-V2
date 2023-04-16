using MinimalBookApiV2;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//var books = new List<Book>
//    {
//        new Book { Id = 1, Title = "The Hitchhiker's Guide to the Galaxy", Author = "Douglas Adams"},
//        new Book{ Id = 2, Title = "1984", Author = "George Orwell"},
//        new Book{ Id = 3, Title = "Ready Player One", Author = "Ernest Cline"},
//        new Book{ Id = 4, Title = "The Martian", Author = "Andy Weir"}
//    };

app.MapGet("/book", async (DataContext context) =>
{
    return await context.Books.ToListAsync();
});

app.MapGet("/book/{id}", async (DataContext context, int id) =>
    await context.Books.FindAsync(id) is Book book ?
    Results.Ok(book) :
    Results.NotFound("Sorry, book not found"));

app.MapPost("/book", async (DataContext context, Book book) =>
{
    context.Books.Add(book);
    await context.SaveChangesAsync();
    return Results.Ok(await context.Books.ToListAsync());
});

app.MapPut("/book/{id}", async (DataContext context, Book updatedBook, int id) =>
{
    var book = await context.Books.FindAsync(id);
    if (book is null) return Results.NotFound("Sorry, this book doesn't exist.");

    book.Title = updatedBook.Title;
    book.Author = updatedBook.Author;
    await context.SaveChangesAsync();

    return Results.Ok(await context.Books.ToListAsync());
});

app.MapDelete("/book/{id}", async (DataContext context ,int id) =>
{
    var book = await context.Books.FindAsync(id);
    if (book is null) return Results.NotFound("Sorry, this book doesn't exist.");

    context.Books.Remove(book);
    await context.SaveChangesAsync();

    return Results.Ok(await context.Books.ToListAsync());
});

app.Run();

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
}