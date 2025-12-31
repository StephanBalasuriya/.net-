using System;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data;

public class GameStoreContext(DbContextOptions<GameStoreContext> options) : DbContext(options ) // DbContext is a class provided by Entity Framework Core
                                                                            
{
public  DbSet<Game> Games => Set<Game>();
public DbSet<Genre> Genres => Set<Genre>();
}
 