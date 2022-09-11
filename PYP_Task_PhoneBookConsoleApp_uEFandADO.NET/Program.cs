using PYP_Task_PhoneBookConsoleApp_EFandADO.NET.Infrastructure;
using PYP_Task_PhoneBookConsoleApp_EFandADO.NET.Model;

namespace PYP_Task_PhoneBookConsoleApp_EFandADO.NET
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            AdoRepository<Person> adoRepository = new AdoRepository<Person>();
            while (true)
            {
                Console.WriteLine("1-Add Person");
                Console.WriteLine("2-Update Person");
                Console.WriteLine("3-Delete Person");
                Console.WriteLine("4-Get All Person");
                Console.WriteLine("5-Get Person By Id");
                Console.WriteLine("6-Exit");
                Console.WriteLine("Select Option");
                var option = Console.ReadLine();
                switch (option)
                {
                    case "1":
                        Console.WriteLine("Name");
                        var name = Console.ReadLine();
                        Console.WriteLine("Surname");
                        var surname = Console.ReadLine();
                        Console.WriteLine("Phone");
                        var phone = Console.ReadLine();
                        Console.WriteLine("Mail");
                        var mail = Console.ReadLine();
                        var person = new Person
                        {
                            Name = name,
                            Surname = surname,
                            Phone = phone,
                            Mail = mail
                        };
                        var result = adoRepository.AddAync(person).Result;
                        if (result > 0)
                        {
                            Console.WriteLine("Person Added");
                        }
                        else
                        {
                            Console.WriteLine("Person Not Added");
                        }
                        break;
                    case "2":
                        Console.WriteLine("Id");
                        var id = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("Name");
                        var name2 = Console.ReadLine();
                        Console.WriteLine("Surname");
                        var surname2 = Console.ReadLine();
                        Console.WriteLine("Phone");
                        var phone2 = Console.ReadLine();
                        Console.WriteLine("Mail");
                        var mail2 = Console.ReadLine();
                        var person2 = new Person
                        {
                            Id = id,
                            Name = name2,
                            Surname = surname2,
                            Phone = phone2,
                            Mail = mail2
                        };
                        var result2 = adoRepository.UpdateAsync(person2).Result;
                        if (result2 > 0)
                        {
                            Console.WriteLine("Person Updated");
                        }
                        else
                        {
                            Console.WriteLine("Person Not Updated");
                        }
                        break;
                    case "3":
                        Console.WriteLine("Id");
                        var id3 = Convert.ToInt32(Console.ReadLine());
                        var person3 = new Person
                        {
                            Id = id3
                        };
                        var result3 = adoRepository.DeleteAsync(person3).Result;
                        if (result3 > 0)
                        {
                            Console.WriteLine("Person Deleted");
                        }
                        else
                        {
                            Console.WriteLine("Person Not Deleted");
                        }
                        break;
                    case "4":
                        var list = adoRepository.GetAllAsync().Result;
                        foreach (var item in list)
                        {
                            Console.WriteLine($"Id : {item.Id} Name : {item.Name} Surname : {item.Surname} Phone : {item.Phone} Mail : {item.Mail}");
                        }
                        break;
                    case "5":
                        Console.WriteLine("Id");
                        var id5 = Convert.ToInt32(Console.ReadLine());
                        var result5 = adoRepository.GetByIdAsync(id5).Result;
                        Console.WriteLine($"Id : {result5.Id} Name : {result5.Name} Surname : {result5.Surname} Phone : {result5.Phone} Mail : {result5.Mail}");
                        break;
                    case "6":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Wrong Option");
                        break;
                }
            }
        }
    }
}