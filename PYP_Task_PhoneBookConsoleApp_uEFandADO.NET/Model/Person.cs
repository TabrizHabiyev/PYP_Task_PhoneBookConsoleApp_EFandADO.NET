namespace PYP_Task_PhoneBookConsoleApp_EFandADO.NET.Model;

public class Person
{
    public int Id { get; set; } 
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Phone { get; set; }
    public string Mail { get; set; }

    public override string ToString()
    {
        return $"Id {Id} Name {Name} Surname {Surname} Phone {Phone} Mail {Mail}";
    }
}
