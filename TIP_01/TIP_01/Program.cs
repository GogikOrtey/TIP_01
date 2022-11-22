using System;
using System.Collections.Generic;
using System.Text;

/*
    Короче, я на звание лучшего программиста не претендую, по этому оставлю программу в таком виде.

    Командное задание по Теории ЯПов
    
    Выполнено:
    
    + Не пуст ли язык
    - Устранение бесполезных символов
    - Удаление лямбда правил
    - Устранение недостижимых символов    
    
    Работает на листах. Код написан на C#.
    
    Ограничения программы: 
    • Добавление правил не автоматизировано. 
    • Автоматического тестирования грамматик нет. 
    • Работает только с терминалами и нетреминалами, состоящими из одного символа
    
    Возможности программы:
    • Добавляет нетерминалы, терминалы, правила и аксиому
    • Строит полный вывод, используя правила (с любым количеством символов, и с любым количеством правил)
    • Автоматически останавливается в случае бесконечной рекурсии
    • Красиво выводит полученный вывод
*/

namespace TIP_01
{
    public class Program
    {
        static MainCore mainCore = new MainCore();

        static void Main(string[] args)
        {
            // Всего в программе записано 3 набора правил. Раскомментируйте одну из этих 3х строк:

            mainCore.LoadExample_notLanguage();     // (1) - Простой пример из 3х правил, в котором нет языка (терминалы не выводятся)
            //mainCore.LoadNormalExample();         // (2) - Стандартный пример, с большим набором правил
            //mainCore.LoadExampleRecurce();        // (3) - Тот-же 2й пример, но последние правило уводит в рекурсию

            // Выше мы загрузили один из 3х наборов правил, и сейчас запускаем лексер, подавая ему на вход аксиому:
            mainCore.Lexer(mainCore.Axioma);

            // Печатаю, не пуст ли язык:
            mainCore.printCanBelanguage();
        }
    }

    public class MainCore
    {
        #region cout

        // Переопределяю метод вывода на консоль, для удобства
        // Теперь вывод работает почти так-же, как и в С++
        public void cout<Type>(Type Input)
        {
            Console.WriteLine(Input);
        }

        public void coutnn<Type>(Type Input) // Тот-же вывод значений, только без переноса каретки 
        {
            Console.Write(Input);
        }

        #endregion

        // Обявляю NEPS
        public List<string> NoTerminals = new List<string>();
        public List<string> Terminals = new List<string>();
        List<List<string>> Rules = new List<List<string>>();
        public string Axioma;

        /*
            Например правило A -> BC, Bc, c;
            Будет выглядеть: rules[n] = [["A", "Bc", "Bc", "c"]], где n - это номер этого правила

            Лямбда - будет & 
        */

        bool isVisibleLambdaOnOutput = false; // Если true, то символ & будет выведен в консоли, при выводе

        #region Rules

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

        #endregion

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
            cout(" ");
        }

        public int maxLengthRules; // Нужен для правильного обхода значений цепочек, для выполнения правил

        public void AllPrint()
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

        public void printCanBelanguage()
        {
            if (isMaxRecurse == false)
            {
                cout(" ");
                if (AllTermWord == true) cout("Язык не пуст");
                else cout("Язык пуст");
            }
        }

        int maxRecurs = 50; // Максимальная глубина рекурсии, прежде чем программа начнёт ругаться)
        bool isMaxRecurse = false;

        public void Lexer(string Axioma)
        {
            cout("----------"); cout("Вывод всех лексем языка:"); cout(" ");
            Lexer(Axioma, 0);
        }

        public void Lexer(string Axioma, int range) // Разбивает входящую строку на единичные (и не только) символы, которые далее анализирует
        {
            if ((range < maxRecurs) && (isMaxRecurse == false)) // Защита от бесконечной рекурсии
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
                if (isMaxRecurse == false)
                {
                    //cout("*** Мы попали в рекурсию более " + maxRecurs + " раз. Скроее всего, это ветка бесконечного цикла ***");
                    onInfinitRecurse();
                }
            }
        }

        // Автоматическая защита от рекурсий
        void onInfinitRecurse()
        {
            isMaxRecurse = true;
            Console.Clear();
            AllPrint();

            cout("------------");
            cout("Ошибка! Не удалось построить вывод языка.");
            cout("Скорее всего в грамматике есть рекурсивные правила, из-за которых построить вывод невозможно.");
            cout(" ");
            cout("Попробуйте изменить некоторые правила, или увеличить значение переменной maxRecurs");
            cout("------------");
        }

        bool AllTermWord = false; // Если существует выводимое слово, состоящее только из терминалов, эта переменная становится true  

        bool Anal(int i, int range, string curr, string Axioma) // Анализирует символы, но только по одному. Возвращает false, если символ не является нетерминалом
        {
            bool isNeterminal = false;
            bool allSimvolTerminal = true;

            for (int j = 0; j < NoTerminals.Count; j++) // Смотрим, что бы все символы в рассматриваемом куске были нетреминалами
            {
                if (NoTerminals[j] == curr) 
                {
                    isNeterminal = true;
                    allSimvolTerminal = false;
                    break;
                }
            }

            if (allSimvolTerminal == true)
            {
                AllTermWord = true;
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
                        if (isVisibleLambdaOnOutput == false)
                        {
                            if (Rules[j][k] == "&")
                            {
                                string newAxioma1 = prevSimv + endSimv;
                                Lexer(newAxioma1, range + 1); // И отправляем значение обратно в распознавание
                                return true;
                            }
                        }

                        string newAxioma = prevSimv + Rules[j][k] + endSimv;
                        Lexer(newAxioma, range + 1); // И отправляем значение обратно в распознавание
                    }
                }
            }
            return true;
        }

        bool AnalMultiplix(int range, string Axioma) 
        // Анализирует символы по группам, начиная с 2х, и до maxLengthRules
        // Этот блок кода нужен для того, что бы работали правила, например BC -> ..., или CCD -> ...
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

        bool useMultiplyRules(string groupSimv, string Axioma, int jj,  int mult, int range) // Используем правила, которые содержат больше 1 символа в начале
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
                        if (isVisibleLambdaOnOutput == false)
                        {
                            if (Rules[j][k] == "&")
                            {
                                string newAxioma1 = prevSimv + endSimv;
                                Lexer(newAxioma1, range + 1); // И отправляем значение обратно в распознавание
                                return true;
                            }
                        }

                        string newAxioma = prevSimv + Rules[j][k] + endSimv;
                        Lexer(newAxioma, range + 1); // И отправляем значение обратно в распознавание
                    }
                }
            }
            return true;
        }
    }
}
