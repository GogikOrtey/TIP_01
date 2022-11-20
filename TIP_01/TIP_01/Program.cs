using System;
using System.Collections.Generic;
using System.Text;

namespace TIP_01
{
    /*
        • В программе не будет возможность из консоли добавлять значения терминалов, нетерминалов и правил
        • Программа будет работать толькоо с терминалами и нетерминалами имеющими длинну 1 символ
    */


    /*
    Создаю класс со списком, который потом указываю как элемент списка.
    Итого получается двумерный динамический список)
    */


    public class MainCore
    {
        /*
            Переопределяю метод вывода на консоль, для удобства
            Теперь вывод работает почти так-же, как и в С++
        */
        public static void cout<Type>(Type Input)
        {
            Console.WriteLine(Input);
        }

        public static void coutnn<Type>(Type Input) // Вывод значений без переноса картеки 
        {
            Console.Write(Input);
        }

        public List<string> NoTerminals = new List<string>();
        public List<string> Terminals = new List<string>();
        //public List<AlternRules> Rules = new List<AlternRules>();
        List<List<string>> Rules = new List<List<string>>();
        public string Axioma;

        /*
            Например правило A -> BC, Bc, c;
            Будет выглядеть: rules[n] = [["A", "Bc", "Bc", "c"]];         
        */

        /*
            Лямбда - будет & 
        */

        public void ExampleSimpleGrammatic()
        {
            NoTerminals.Add("A");
            NoTerminals.Add("B");
            NoTerminals.Add("C");
            NoTerminals.Add("D");

            Terminals.Add("a");
            Terminals.Add("b");
            Terminals.Add("c");
            Terminals.Add("d");

            Axioma = "A";

            // Добавляем правила: 

            List<string> list1 = new List<string>();

            list1.Add("A");
            list1.Add("BC");
            list1.Add("Bc");
            list1.Add("&");

            Rules.Add(list1);

            /*---*/

            List<string> list2 = new List<string>();

            list2.Add("BC");
            list2.Add("bc");

            Rules.Add(list2);

            /*---*/

            List<string> list3 = new List<string>();

            list3.Add("B");
            list3.Add("D");
            list3.Add("d");

            Rules.Add(list3);

            /*---*/

            List<string> list4 = new List<string>();

            list4.Add("D");
            list4.Add("C");
            list4.Add("&");

            Rules.Add(list4);

            /*---*/

            List<string> list5 = new List<string>();

            list5.Add("CC");
            list5.Add("ABcd");
            list5.Add("abCD");

            Rules.Add(list5);

            /*---*/

            List<string> list6 = new List<string>();

            list6.Add("DCA");
            list6.Add("dca");

            Rules.Add(list6);
        }

        public void printingList(List<string> MyList)
        {
            for (int i = 0; i < MyList.Count; i++)
            {
                if (i > 0) coutnn(", ");
                coutnn(MyList[i]);
            }
        }

        public void sep() // Выводит разделитель
        {
            //cout("------------");
            cout(" ");
        }

        public void AllPrint() //(List<string> NoTerminals, List<string> Terminals, List<AlternRules> Rules, string Axioma)
        {
            cout("Все нетерминалы: "); 
            printingList(NoTerminals); sep();

            sep(); cout("Все терминалы: ");
            printingList(Terminals); sep();

            sep(); cout("Все правила: "); sep();
            cout(" & - это лямбда"); sep();

            for (int i = 0; i < Rules.Count; i++) // Чуть сложного кода для карсивого вывода)
            {
                coutnn("    " + Rules[i][0] + " -> ");
                for (int j = 1; j < Rules[i].Count; j++) // Проверить на ошибки при выводе 
                {
                    if (j > 1) coutnn(", ");
                    coutnn(Rules[i][j]);
                }
                cout(" ");
            }

            sep(); cout("Аксиома: "); 
            cout(Axioma); sep();
            cout(" ");
        }

        public void AutoCreateGrammatic()
        {

        }

        public int aaa = 10;
    }

    public class Program
    {
        public int a = 0;
        static MainCore mainCore = new MainCore();

        public static void cout<Type>(Type Input)
        {
            Console.WriteLine(Input);
        }

        static void Main(string[] args)
        {
            mainCore.ExampleSimpleGrammatic();
            mainCore.AllPrint();
        }
    }
}
