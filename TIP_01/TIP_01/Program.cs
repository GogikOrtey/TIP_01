using System;
using System.Collections.Generic;
using System.Text;

namespace TIP_01
{
    public class MainCore
    {
        // Переопределяю метод вывода на консоль, для удобства
        // Теперь вывод работает почти так-же, как и в С++
        public void cout<Type>(Type Input)
        {
            Console.WriteLine(Input);
        }

        public void coutnn<Type>(Type Input) // Тот-же вывод значений, только без переноса картеки 
        {
            Console.Write(Input);
        }

        // Обявляю NEPS
        public List<string> NoTerminals = new List<string>();
        public List<string> Terminals = new List<string>();
        List<List<string>> Rules = new List<List<string>>();
        public string Axioma;

        /*
            Например правило A -> BC, Bc, c;
            Будет выглядеть: rules[n] = [["A", "Bc", "Bc", "c"]];         

            Лямбда - будет & 
        */

        // В основной программе, при загрузке значений используйте любые из этих 3х методов:

        public void LoadNormalExample()
        {
            SimpleExample();
            notRecurceExample();

            AllPrint(); // Не убирать это отсюда, процедура нужна для вычисления максимальной длинны правил
        }

        public void LoadExampleRecurce()
        {
            SimpleExample();
            RecurceExample();

            AllPrint(); 
        }

        public void LoadExample_notLanguage()
        {
            notLanguageExample();

            AllPrint(); 
        }

        // Но не используйте эти 4. Я специально сделал их без public

        void SimpleExample()
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
            list1.Add("&"); // Лямбда-правило

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
            list4.Add("&"); // Лямбда-правило

            Rules.Add(list4);

            /*---*/

            List<string> list5 = new List<string>(); // Бесполезное правило (никогда не выполнится)

            list5.Add("DCA");
            list5.Add("dca");

            Rules.Add(list5);
        }

        void notLanguageExample()
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

            List<string> list1 = new List<string>();

            list1.Add("A");
            list1.Add("B");
            list1.Add("C");

            Rules.Add(list1);

            List<string> list2 = new List<string>();

            list2.Add("C");
            list2.Add("DD");

            Rules.Add(list2);

            List<string> list3 = new List<string>();

            list3.Add("DD");
            list3.Add("D");

            Rules.Add(list3);
        }

        void RecurceExample()
        {
            List<string> list6 = new List<string>();

            list6.Add("C");
            list6.Add("C");
            list6.Add("c");

            Rules.Add(list6);
        }

        void notRecurceExample()
        {
            List<string> list6 = new List<string>();

            list6.Add("C");
            list6.Add("c");
            list6.Add("&");

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

        public int maxLengthRules; // Нужен для правильного обхода значений цепочек, для выполнения правил

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
                    if (Rules[i][j].Length > maxLengthRules) maxLengthRules = Rules[i][j].Length;
                }
                cout(" ");                
            }

            sep(); cout("Аксиома: "); 
            cout(Axioma); sep();
        }

        int maxRecurs = 20; // Максимальная глубина рекурсии, прежде чем программа начнёт ругаться)

        public void Lexer(string Axioma, int range) // Разбивает входящую строку на единичные (и не только) символы, которые далее анализирует
        {
            if (range == 0) // Срабатывает только на первом элементе. Встаил сюда, что бы программа была чуть красивее
            {
                cout("----------"); cout("Вывод всех лексем языка:"); cout(" ");
            }
            if (range < maxRecurs) // Защита от бесконечной рекурсии
            {
                if (range != 0)
                {
                    for (int i = 0; i < range - 1; i++) { coutnn("|   "); }  // Тут добавляем отступы, что бы знать, на каком шаге рекурсии мы сейчас
                    coutnn("|→ "); 
                }

                cout(Axioma); // Вот тут печатаем, что бы знать, что обрабатывает программа

                for (int i = 0; i < Axioma.Length; i++)
                {
                    string curr = Axioma[i].ToString();

                    if (Anal(i, range, curr, Axioma) == false)
                    {
                        continue; // Сразу начинаем рассматривать следующий символ
                    }
                }

                AnalMultiplix(range, Axioma); 
            }
            else
            {
                cout("*** Мы попали в рекурсию более " + maxRecurs + " раз. Скроее всего, это ветка бесконечного цикла ***");
            }
        }

        bool Anal(int i, int range, string curr, string Axioma) // Анализирует символы, но только по одному. Возвращает false, если символ не является нетерминалом
        {
            bool isNeterminal = false;
            for (int j = 0; j < NoTerminals.Count; j++) // Смотрим, что бы все символы в рассматриваемом куске были нетреминалами
            {
                if (NoTerminals[j] == curr) 
                {
                    isNeterminal = true;
                    break;
                }
            }

            if (isNeterminal == false) // Если это терминал
            {
                return false;
            }

            // Теперь мы точно знаем, что curr - это нетерминал
            // Попробуем найти правила, которые начинаются с этого символа

            for (int j = 0; j < Rules.Count; j++)
            {
                if (Rules[j][0] == curr)
                {
                    // Вот тут мы уже нашли правило, по которому можно преобразовать наш нетерминал
                    // Если у правила есть варианты, то для каждого из них мы изменим нетерминал,
                    // и заново запустим Lexer, порождая множественные рекурсии
                    // printingList(Rules[j]);

                    // Тут перед нами стоит задача изменить рассматриваемый нетерминал внутри цепочки, на комбинацию других символов
                    // Для этого, мы разбиваем исходную цепочку (Axioma) на 2 цепочки: до рассматриваемого нетерминала, и после
                    // Затем мы делаем бутерброд: символ до нетреминала + символы, после замены нетерминала по правилу + символы после нетерминала

                    string endSimv = "";
                    string prevSimv = "";

                    for (int n = 0; n < Axioma.Length; n++)
                    {
                        if (n < i) prevSimv += Axioma[n];
                        else if (n > i) endSimv += Axioma[n];
                    }

                    for (int k = 1; k < Rules[j].Count; k++) // Начинаем со 2го элемента, потому что 1й - это начало нашего правила
                    {
                        string newAxioma = prevSimv + Rules[j][k] + endSimv;

                        Lexer(newAxioma, range + 1); // И отправляем значение обратно в распознавание
                    }
                }
            }
            return true;
        }

        bool AnalMultiplix(int range, string Axioma) // Анализирует символы по группам, начиная с 2х, и до maxLengthRules
        {
            //cout("Получили аксиому для обработки: " + Axioma);
            for (int mult = 2; mult <= maxLengthRules; mult++)
            {
                for (int j = 0; j < Axioma.Length - mult + 1; j++) // Верно задаю границы рассматриваемых смиволов
                {
                    string groupSimv = "";
                    bool thisNotTerminals = true;

                    for (int t = 0; t < mult; t++) // Делаю "формочки" из символов входной аксиомы, равные размеру рассматриваемых областей // Формочки = группы символов
                    {
                        groupSimv += Axioma[j + t];

                        for (int y = 0; y < Terminals.Count; y++)
                        {
                            if (Axioma[j + t] == Terminals[y][0])
                            {
                                // В нашей формочке оказался терминал, мы больше не будем её рассматривать, т.к. все правила у нас работают только с нетреминалами
                                thisNotTerminals = false;
                            }
                        }
                    }

                    if (thisNotTerminals == true)
                    {
                        //Вот тут мы точно знаем, что в нашей формочке нет терминалов. Прогоним её по всем правилам, и используем их, если найдём подходящие

                        // Используем процедуру, для поиска и применения правил, там внутри встраиваем правила в аксиому
                        // И далее отправляем дальше в лексер

                        useMultiplyRules(groupSimv, Axioma, j, mult, range);
                    }
                }
            }
            return true;
        }

        void useMultiplyRules(string groupSimv, string Axioma, int jj,  int mult, int range) // Используем правила, которые содержат больше 1 символа в начале
        {
            //cout("Получили группу символов " + groupSimv + " в аксиоме " + Axioma);

            for (int j = 0; j < Rules.Count; j++)
            {
                //cout("Rules[j].ToString() = " + Rules[j].ToString() + " groupSimv = " + groupSimv);
                if (Rules[j][0] == groupSimv)
                {
                    // Нашли правило, которое подходит нам

                    string endSimv = "";
                    string prevSimv = "";

                    for (int n = 0; n < Axioma.Length; n++)
                    {
                        if (n < jj) prevSimv += Axioma[n];
                        else if (n > jj + mult-1) endSimv += Axioma[n];
                    }

                    // Разбили нашу аксиому на цепочки до и после группы символов

                    for (int k = 1; k < Rules[j].Count; k++) // Начинаем со 2го элемента, потому что 1й - это начало нашего правила
                    {
                        string newAxioma = prevSimv + Rules[j][k] + endSimv;

                        Lexer(newAxioma, range + 1); // И отправляем значение обратно в распознавание
                    }
                }
            }
        }
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
            mainCore.LoadExample_notLanguage();
            //mainCore.LoadNormalExample();
            //mainCore.LoadExampleRecurce();

            mainCore.Lexer(mainCore.Axioma, 0);
        }
    }
}
