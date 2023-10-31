using LibraryManagementSystem.Models;
using LibraryManagementSystem.DAL;
using LibraryManagementSystem.Controllers;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
переделать связь многие ко многим   +
добавить try catch  
добавить [] и сообщения об ошибках  +
мб улучшить интерфейс (удаление ...)

 */
class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        using (var context = new LibraryContext())
        {
            // Create database 
            var databaseManager = new DatabaseManager(context);

            var controller = new LibraryController(databaseManager);

            controller.Start();

            context.Dispose();
            
        }
    }
}