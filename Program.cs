using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace personal_register
{
    class Program
    {
        enum Menu
        {
            DisplayRegister = 1, AddToRegister, UpdateRegister, RemoveFromRegister, Exit
        }



        static void Main(string[] args)
        {
            string name = default;
            string surname = default;
            uint age = default;
            string street = default;
            uint house_number = default;
            uint apartment_number = default;
            string postcode = default;
            string city = default;
            Sex sex = default;

            bool endProgram = false;
            bool endLoop = false;
            string answer;
            string namePersonToChangeOrRemove;
            string surnamePersonToChangeOrRemove;
            string phrase;

            List<Person> personList = new List<Person>();
            XmlSerializer serializer = new XmlSerializer(typeof(List<Person>));



            while (!endProgram)
            {


                Console.WriteLine("**********************************");
                Console.WriteLine("* 1. Wyświetlanie rejestru       *");
                Console.WriteLine("* 2. Dodanie do rejestru         *");
                Console.WriteLine("* 3. Edycja danych w rejestrze   *");
                Console.WriteLine("* 4. Usuwanie danych w rejestrze *");
                Console.WriteLine("* 5. Wyjście                     *");
                Console.WriteLine("**********************************");
                Deserialize();
                Console.WriteLine("\nWybierz opcję");
                Menu option;

                while (!Enum.TryParse<Menu>(Console.ReadLine(), out option))
                {
                    Console.WriteLine("Coś poszło nie tak!");
                }

                switch (option)
                {
                    case Menu.DisplayRegister:
                        if (!personList.Any())
                        {
                            Console.WriteLine("Brak danych w rejestrze");
                            CleanScreen();

                        }
                        else
                        {
                            foreach (var item in personList)
                            {
                                item.Display();
                            }

                            Console.WriteLine("Aby wyszukać, wciśnij Tab");
                            Console.WriteLine("Aby wyjść do ekranu głównego, wciśnij ESC");

                            if (Console.ReadKey().Key == ConsoleKey.Tab)
                            {
                                Console.Write("Podaj frazę, którą mam znaleźć:");
                                phrase = Console.ReadLine();
                                FindPhrase();
                            }
                            Console.ReadKey();
                            CleanScreen();

                            
                            if (Console.ReadKey().Key == ConsoleKey.Escape)
                            {
                                CleanScreen();
                            }
                            
                        }
                        break;


                    case Menu.AddToRegister:
                        while (!endLoop)
                        {
                            LoadPersonalData();

                            if (CheckIfEmpty() == true)
                            {
                                Console.WriteLine("Nie podano wszystkich danych!");
                            }
                            else
                            {
                                personList.Add(
                                new Person(
                                name,
                                surname,
                                age,
                                sex,
                                street,
                                house_number,
                                apartment_number,
                                postcode,
                                city)
                                );
                                Serialize();
                                Console.WriteLine("Dodano dane do rejestru");
                            }


                            CleanScreen();


                            Console.WriteLine("Czy chcesz dodać następną osobę? T/N");
                            answer = Console.ReadLine();

                            switch (answer.ToLower())
                            {
                                case "n":
                                    endLoop = true;
                                    break;

                                case "t":
                                    endLoop = false;
                                    break;

                                default:
                                    Console.WriteLine("Niepoprawna odpowiedź!");
                                    break;

                            }

                            CleanScreen();

                        }
                        break;


                    case Menu.UpdateRegister:
                        Console.WriteLine("Wybierz osobę do zmiany");

                        LoadDataToChangeOrRemove();
                        if (CheckIfPersonExists() == true)
                        {
                            LoadPersonalData();

                            foreach (var item in personList.Where(n => n.name.ToLower().Equals(namePersonToChangeOrRemove.ToLower()) && n.surname.ToLower().Equals(surnamePersonToChangeOrRemove.ToLower())))
                            {
                                item.name = name;
                                item.surname = surname;
                                item.age = age;
                                item.sex = sex;
                                item.street = street;
                                item.house_number = house_number;
                                item.apartment_number = apartment_number;
                                item.postcode = postcode;
                                item.city = city;
                            }
                            Serialize();

                            Console.WriteLine("Dane zostały zmienione");
                        }
                        else
                        {
                            Console.WriteLine("Brak osoby o podanych danych!");
                        }
                        break;


                    case Menu.RemoveFromRegister:
                        LoadDataToChangeOrRemove();
                        var removeItem = personList.SingleOrDefault(n => n.name.ToLower().Equals(namePersonToChangeOrRemove.ToLower()) && n.surname.ToLower().Equals(surnamePersonToChangeOrRemove.ToLower()));
                        if (CheckIfPersonExists() == true)
                        {
                            if (removeItem != null)
                            {
                                Console.WriteLine("Usunięto");
                                personList.Remove(removeItem);

                                File.Delete("Personal_Register.xml");
                                Serialize();
                            }
                        }
                        break;


                    case Menu.Exit:
                        Console.WriteLine("Żegnam");
                        Thread.Sleep(500);
                        endProgram = true;
                        break;


                    default:
                        Console.WriteLine("Wystąpił błąd");
                        break;

                }
            }

            void LoadDataToChangeOrRemove()
            {
                Console.Write("Podaj imię: ");
                namePersonToChangeOrRemove = Console.ReadLine();
                Console.Write("Podaj nazwisko: ");
                surnamePersonToChangeOrRemove = Console.ReadLine();
            }
            void LoadPersonalData()
            {
                Console.WriteLine("Wprowadź nowe dane");
                // NAME
                Console.Write("Imię: ");
                name = Console.ReadLine();

                // SURNAME
                Console.Write("Nazwisko: ");
                surname = Console.ReadLine();

                // AGE
                do
                {
                    Console.Write("Wiek: ");
                    try
                    {
                        age = uint.Parse(Console.ReadLine());
                        break;
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Niepoprawne dane!");
                    }
                } while (true);



                // SEX
                do
                {
                    Console.WriteLine("Płeć: ");
                    Console.WriteLine(" 1. Mężczyzna\n 2. Kobieta");
                    Sex sexOption;

                    while (!Enum.TryParse<Sex>(Console.ReadLine(), out sexOption))
                    {
                        Console.WriteLine("Coś poszło nie tak!");
                        Console.WriteLine("Płeć: ");
                        Console.WriteLine(" 1. Mężczyzna\n 2. Kobieta");
                    }


                    switch (sexOption)
                    {
                        case Sex.man:
                            sex = Sex.man;
                            endLoop = true;
                            break;

                        case Sex.woman:
                            sex = Sex.woman;
                            endLoop = true;
                            break;

                        default:
                            Console.WriteLine("Niepoprawna wartość!");
                            break;
                    }

                } while (!endLoop);


                // STREET
                Console.Write("Ulica: ");
                street = Console.ReadLine();

                // HOUSE NUMBER
                do
                {
                    Console.Write("Numer domu: ");
                    try
                    {
                        house_number = uint.Parse(Console.ReadLine());
                        break;
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Niepoprawne dane!");
                    }
                } while (true);

                // APARTMENT NUMBER
                do
                {
                    Console.Write("Numer lokalu: ");
                    try
                    {
                        apartment_number = uint.Parse(Console.ReadLine());
                        break;
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Niepoprawne dane!");
                    }
                } while (true);


                // POSTCODE
                Console.Write("Kod pocztowy: ");
                postcode = Console.ReadLine();

                // CITY
                Console.Write("Miejscowość: ");
                city = Console.ReadLine();

                CleanScreen();
            }

            bool CheckIfPersonExists()
            {
                if (personList.Where(n => n.name.ToLower().Equals(namePersonToChangeOrRemove.ToLower())).FirstOrDefault() == null || personList.Where(s => s.surname.ToLower().Equals(surnamePersonToChangeOrRemove.ToLower())).FirstOrDefault() == null)
                {
                    CleanScreen();
                    return false;

                }
                else
                {
                    CleanScreen();
                    return true;
                }
            }

            bool CheckIfEmpty()
            {
                if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(surname) || age == default ||
                   sex == default || string.IsNullOrWhiteSpace(street) || house_number == default || apartment_number == default
                   || string.IsNullOrWhiteSpace(postcode) || string.IsNullOrWhiteSpace(city))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            void CleanScreen()
            {
                Thread.Sleep(500);
                Console.Clear();

            }

            bool CheckIfFileExist(string fileName)
            {
                if (File.Exists(fileName))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            void FindPhrase()
            {
                foreach (var item in personList)
                {
                    if (item.name.ToLower().Contains(phrase.ToLower()) || item.surname.ToLower().Contains(phrase.ToLower()) || item.age.ToString().ToLower().Contains(phrase.ToLower()) || 
                        item.sex.ToString().ToLower().Contains(phrase.ToLower()) || item.street.ToLower().Contains(phrase.ToLower()) || item.house_number.ToString().ToLower().Contains(phrase.ToLower()) || 
                        item.apartment_number.ToString().ToLower().Contains(phrase.ToLower()) || item.postcode.ToLower().Contains(phrase.ToLower()) || item.city.ToLower().Contains(phrase.ToLower()))
                    {
                        item.Display();
                    }
                }
            }

            void Serialize()
            {
                if (CheckIfFileExist("Personal_Register.xml") == true)
                {
                    // append new person if file exists
                    using (Stream steam = File.OpenWrite("Personal_Register.xml"))
                    {
                        serializer.Serialize(steam, personList);
                        steam.Close();
                    }
                }
                else
                {
                    // create new file if not exists
                    Stream stream = File.Create("Personal_Register.xml");
                    serializer.Serialize(stream, personList);
                    stream.Close();
                }
            }

            List<Person> Deserialize()
            {
                if (CheckIfFileExist("Personal_Register.xml") == true)
                {
                    StreamReader streamReader = new StreamReader("Personal_Register.xml");
                    personList = (List<Person>)serializer.Deserialize(streamReader);
                    streamReader.Close();
                    return personList;
                }
                return personList;
            }

        }
    }
}
