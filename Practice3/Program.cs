using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Practice3
{
    class RelationShip
    {
        public Person Person { get; }
        public string TypeRS { get; }
        
        public RelationShip(Person person, string typeRs)
        {
            Person = person;
            TypeRS = typeRs;
        }
    }
    class Person
    {
        public int Id { get; }
        public string Firstname { get; }
        public string Lastname { get; }
        public DateTime BirthDate { get; }
        public List<RelationShip> RelationShips;

        public Person(int id, string firstname, string lastname, DateTime birthDate)
        {
            Id = id;
            Firstname = firstname;
            Lastname = lastname;
            BirthDate = birthDate;
        }

        public void ShowAllRelationShip()
        {
            foreach (var RS in RelationShips)
            {
                Console.WriteLine($"{RS.Person.Firstname} {RS.TypeRS}");
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            List<Person> persons = ReadFile();
            int first = int.Parse(Console.ReadLine());
            int second = int.Parse(Console.ReadLine());
            Console.WriteLine(FindRSInList(persons,first,second));
        }

        static string FindRSInList(List<Person> persons, int firstId, int SecondId)
        {
            Person first = persons[firstId - 1];
            Person second = persons[SecondId - 1];
            return first.RelationShips.Find(x => x.Person == second).TypeRS;
        }

        static List<Person>ReadFile()
        {
            string file = File.ReadAllText("File.txt");
            string[] split = Regex.Split(file, @"^\r\n", RegexOptions.Multiline);
            
            
            List<Person> persons = ReadPersons(split[0]);
            persons = ReadRS(persons, split[1]);
            return persons;
        }

        private static List<Person> ReadPersons(string persons)
        {
            List<Person> personList = new List<Person>();
            string[] stringPersons = persons.Remove(persons.Length - 1).Split("\n");
            
            string[] pattern = stringPersons[0].Remove(stringPersons[0].Length - 1).Split(';');
            int idIndex = Array.IndexOf(pattern, "Id");
            int firstNameIndex = Array.IndexOf(pattern, "FirstName");
            int lastNameIndex = Array.IndexOf(pattern, "LastName");
            int birthDateIndex = Array.IndexOf(pattern, "BirthDate");
            
            for (int i = 1; i < stringPersons.Length; i++)
            {
                string[] split = stringPersons[i].Split(';');
                Person person = new Person(int.Parse(split[idIndex]), split[firstNameIndex], split[lastNameIndex],
                    DateTime.Parse(split[birthDateIndex]));
                personList.Add(person);
            }
            return personList;
        }

        private static List<Person> ReadRS(List<Person> persons, string relationShips)
        {
            string[] stringRS = relationShips.Replace("\r", "").Split("\n");
            foreach (var RS in stringRS)
            {
                string[] split = RS.Split(new []{"<->", "="}, StringSplitOptions.None);
                Person personFirst = persons[int.Parse(split[0]) - 1];
                Person personSecond = persons[int.Parse(split[1]) - 1];
                

                if (personFirst.RelationShips == null)
                    personFirst.RelationShips = new List<RelationShip>(){new (personSecond,split[2])};
                else
                    personFirst.RelationShips.Add(new RelationShip(personSecond,split[2]));
                
                if (personSecond.RelationShips == null)
                    personSecond.RelationShips = new List<RelationShip>(){new (personFirst,split[2])};
                else
                    personSecond.RelationShips.Add(new RelationShip(personFirst,split[2]));
            }
            return persons;
        }

    }
    
}