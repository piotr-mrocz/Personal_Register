using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace personal_register
{
    public enum Sex
    {
        man = 1,
        woman
    }
    public class Person : IPersonal_register
    {
        public string name { get; set; }
        public string surname { get; set; }
        public uint age { get; set; }
        public Sex sex { get; set; }
        public string street { get; set; }
        public uint house_number { get; set; }
        public uint apartment_number { get; set; }
        public string postcode { get; set; }
        public string city { get; set; }

        public Person()
        {}

        public Person(string name, string surname, uint age, Sex sex, string street, uint house_number, uint apartment_number, string postcode, string city)
        {
            this.name = name;
            this.surname = surname;
            this.age = age;
            this.sex = sex;
            this.street = street;
            this.house_number = house_number;
            this.apartment_number = apartment_number;
            this.postcode = postcode;
            this.city = city;
        }

        public void Display()
        {
            Console.WriteLine("Imię i nazwisko: {0} {1}\n" +
                "Wiek: {2}\n" +
                "Płeć: {3}\n" +
                "Adres: {4} {5}/{6}\n" +
                "Miasto: {7} {8}\n", FirstLetterToUpper(this.name), FirstLetterToUpper(this.surname), this.age, DisplaySex(),
                FirstLetterToUpper(this.street), this.house_number, this.apartment_number, this.postcode, FirstLetterToUpper(this.city));
        }

        public string FirstLetterToUpper(string word)
        {
            string restWord = default;
            for (int i = 1; i < word.Length; i++)
            {
                restWord += word[i];
            }
            string newWord = word[0].ToString().ToUpper() + restWord;
           
            return newWord;
        }

        public string DisplaySex()
        {
            if (sex == Sex.man)
            {
                return "Mężczyzna";
            }
            else
            {
                return "Kobieta";
            }
        }
    }
}
