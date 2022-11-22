using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;   // Для отслеживания времени работы программы
using System.Threading;     // Использую многопоточность

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

/*
    Сделать многопоточность только при рекрусии
*/

namespace TIP_01
{
    public class Program
    {
        static MainCore mainCore = new MainCore();

        static void Main(string[] args)
        {
            Stopwatch st = new Stopwatch();
            st.Start();

            // Всего в программе записано 5 наборов правил. Раскомментируйте одну из этих 5и строк:

            mainCore.LoadExample_notLanguage();         // (1) - Нерекурсивынй без языка
            //mainCore.LoadNormalExample();             // (2) - Нерекурсивный с языком
            //mainCore.LoadExampleRecurce();            // (3) - Рекурсивный с языком 
            //mainCore.LoadNumberExample();             // (4) - Пример от преподавателя (Рекурсивный с языком)
            //mainCore.LoadExample_notLanguageRecurce();// (5) - Рекурсивный без языка

            // Выше мы загрузили один из 5и наборов правил, и сейчас запускаем лексер, подавая ему на вход аксиому:
            mainCore.Lexer(mainCore.Axioma);

            if (mainCore.isRecurceOn == false)
            {
                mainCore.ToDoprintCanBelanguage(); // Выводит, пуст ли язык
                //if (mainCore.isOutpResult == false) mainCore.cout("Язык пуст");
                st.Stop();
                mainCore.cout("\nВремя выполнения алгоритма: " + Math.Round(st.Elapsed.TotalSeconds, 2) + " сек");
            }
            else st.Stop();
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
        public static List<List<string>> Rules = new List<List<string>>();
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

        public void LoadExample_notLanguageRecurce()
        {
            notLanguageRecurceExample();

            AllPrint();
        }

        public void LoadNumberExample()
        {
            numberExample();

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

        void notLanguageRecurceExample()
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
            list2.Add("D");

            Rules.Add(list2);

            List<string> list3 = new List<string>();

            list3.Add("D");
            list3.Add("A");

            Rules.Add(list3);
        }

        void numberExample()
        {
            NoTerminals.Add("Ч");
            NoTerminals.Add("Ц");

            Terminals.Add("1");
            Terminals.Add("2");
            Terminals.Add("3");
            Terminals.Add("4");
            Terminals.Add("5");
            Terminals.Add("6");
            Terminals.Add("7");
            Terminals.Add("8");
            Terminals.Add("9");
            Terminals.Add("0");

            Axioma = "Ч";

            List<string> list1 = new List<string>();

            list1.Add("Ч");
            list1.Add("ЧЦ");
            list1.Add("Ц");

            Rules.Add(list1);

            List<string> list2 = new List<string>();

            list2.Add("Ц");
            list2.Add("1");
            list2.Add("2");
            list2.Add("3");
            list2.Add("4");
            list2.Add("5");
            list2.Add("6");
            list2.Add("7");
            list2.Add("8");
            list2.Add("9");
            list2.Add("0");

            Rules.Add(list2);
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
        int[] countUseRules; // Ведёт учёт, сколько раз было использовано каждое правило при построении вывода. Понадобится при рекурсии
        bool isTrue = false;

        public static long Fact(long n)
        {
            if (n == 0)
                return 1;
            else
                return n * Fact(n - 1);
        }        

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

            if (isTrue == false) // Инициализирую массив учёта использования правил (только один раз)
            {
                countUseRules = new int[Rules.Count];

                isTrue = true;
                for (int i = 0; i < Rules.Count; i++)
                {
                    countUseRules[i] = 0;
                }

                countActiveRules = Rules.Count;

                int allCountRules = 0;

                for (int i = 0; i < Rules.Count; i++)
                {
                    for (int j = 0; j < Rules[i].Count; j++)
                    {
                        allCountRules++; // Считаю сколько вообще у меня правил
                    }
                }

                maxCountIteration = Fact(Terminals.Count) * allCountRules;
            }
        }

        long allCountIteration;
        long maxCountIteration;
        public bool isOutpResult = false;
        public void printCanBelanguage()
        {
            /*
            if (AllTermWord == true)
            {
                if (isOutpResult == false)
                {
                    isOutpResult = true;
                    if (isMaxRecurse == false)
                    {
                        cout(" ");
                        if (AllTermWord == true) cout("Язык не пуст");
                        else cout("Язык пуст");
                        isEndProgramm = true;
                    }
                }
            }
            else if (allCountIteration >= maxCountIteration)
            {
                cout("Язык пуст");
                isEndProgramm = true;
            }
            */

            //ToDoprintCanBelanguage();
        }

        public void ToDoprintCanBelanguage() // Принудительно выводит данные о том, не пуст ли язык
        {
            //if (isOutpResult == false)
            {
                Console.Clear();
                AllPrint();
                //isOutpResult = true;
                if (AllTermWord == true) cout("Язык не пуст");
                else cout("Язык пуст");
            }
        }

        int maxRecurs = 50; // Максимальная глубина рекурсии, прежде чем программа начнёт ругаться)
        bool isMaxRecurse = false;

        public void Lexer(string Axioma)
        {
            //cout("----------"); cout("Вывод всех лексем языка:"); cout(" ");
            Lexer(Axioma, 0, polarity);
        }

        bool polarity = true; // Полярность - это режим работы лексера. Он используется для перевывода грамматик, при рекурсии
        public bool isEndProgramm = false;
        public void Lexer(string Axioma, int range, bool inpPolarity) // Разбивает входящую строку на единичные (и не только) символы, которые далее анализирует
        {
            if ((AllTermWord == true) && (isRecurceOn == true))
            {
                if (isEndProgramm == false)
                {
                    // Не делаю лишних вычеслений. Если мы уже были в рекурсии, и нашли слово состоящее из терминалов, вывожу это на консоль и завершаю программу
                    isEndProgramm = true;
                    printCanBelanguage();
                }
            }
            else if (isEndProgramm != true)
            {
                if (inpPolarity == polarity) // Лексер работает только если полярности вызова функции и внешняя полярность совпадают
                {
                    if ((range < maxRecurs) && (isMaxRecurse == false)) // Защита от бесконечной рекурсии
                    {
                        allCountIteration++;
                        if (false) //(isRecurceOn == false)
                        {
                            if (range != 0)
                            {
                                for (int i = 0; i < range - 1; i++) { coutnn("|   "); }  // Тут добавляем отступы, что бы знать, на каком шаге рекурсии мы сейчас
                                coutnn("|→ ");
                            }

                            cout(Axioma); // Вот тут печатаем, что бы знать, что обрабатывает программа
                        }

                        for (int i = 0; i < Axioma.Length; i++)
                        {
                            string curr = Axioma[i].ToString();

                            if (Anal(i, range, curr, Axioma, inpPolarity) == false)
                            {
                                continue; // Сразу начинаем рассматривать следующий символ
                            }
                        }

                        AnalMultiplix(range, Axioma, inpPolarity);
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
            }
            else if(isEndProgramm == true)
            {
                printCanBelanguage();
            }
        }

        //bool whyLexerUses = true; // Показывает, какую процедуру лексера использовать, при перезагрузке вывода // true = 1, false = 2
        bool isOnceRecurce = false;
        int countActiveRules; // Если активных правил останется 0, то мы прекращаем попытки вывода
        public bool isRecurceOn = false; // Эта переменная становится = true, если мы хотя бы один раз попали в рекурсию, и поменяли полярность

        // Автоматическая защита от рекурсий
        void onInfinitRecurse()
        {
            isMaxRecurse = true;


            /*
            if (isOnceRecurce == false)
            {
                isOnceRecurce = true;
                Console.Clear();
                AllPrint();

                cout("------------");
                cout("Похоже программа попала в бесконечную рекурсию");
                cout("Это значит, что отобразить полный вывод языка не получится. \n");
                cout("Подождите, пока она выполняет перевывод грамматики");
                cout("------------");
                cout(" ");

                ToDoprintCanBelanguage();
            }
            */

            new Thread(() => Lexer(Axioma, 0, polarity)).Start();

            //ToDoprintCanBelanguage();

            //cout("*** Остановка вывода языка №" + (Rules.Count - countActiveRules) + " ***");

            /*
            if (countActiveRules > 0)
            {
                countActiveRules--;
                isRecurceOn = true; 

                int maxCountRul = 0;
                int indexMaxCountRul = 0;

                //cout(" ");
                for (int i = 0; i < Rules.Count; i++) // Ищу правило с максимальным колл-вом использований
                {
                    if (countUseRules[i] > maxCountRul) indexMaxCountRul = i;

                    //cout(countUseRules[i] + ", ");
                }

                countUseRules[indexMaxCountRul] = -1; // "удаляю" его. Теперь программа не будет использовать его при выводе

                polarity = !polarity; // Изменяем полярность. Теперь все старые рекурсивные процедур не будут работать
                isMaxRecurse = false;

                //Thread t = new Thread(Lexer(Axioma, 0, polarity)); // Запускаем вывод заново
                //t.Start();

                new Thread(() => Lexer(Axioma, 0, polarity)).Start();
                //Lexer(Axioma, 0, polarity);


                // Запускаем вывод заново
                // Используем "полярность", для верного вызова процедур лексера 
            }
            */

            /*
            else // Завершаем программу
            {
                cout(" ");
                cout("------------");
                printCanBelanguage();
                cout("------------");
                cout(" ");
            }
            */

            /*
            cout("------------");
            cout("Ошибка! Не удалось построить вывод языка.");
            cout("Скорее всего в грамматике есть рекурсивные правила, из-за которых построить вывод невозможно.");
            cout(" ");
            cout("Попробуйте изменить некоторые правила, или увеличить значение переменной maxRecurs");
            cout("------------");
            */
        }

        bool AllTermWord = false; // Если существует выводимое слово, состоящее только из терминалов, эта переменная становится true  

        bool Anal(int i, int range, string curr, string Axioma, bool inpPolarity) // Анализирует символы, но только по одному. Возвращает false, если символ не является нетерминалом
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
                isEndProgramm = true;
                //printCanBelanguage();
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
                        // Если это правило не "удалено", то мы прибавляем ему счётчик исползований
                        if(countUseRules[j]!=-1)
                        {
                            countUseRules[j] += 1;

                            if (isVisibleLambdaOnOutput == false)
                            {
                                if (Rules[j][k] == "&")
                                {
                                    string newAxioma1 = prevSimv + endSimv;

                                    //if(isRecurceOn)
                                    new Thread(() => Lexer(newAxioma1, range + 1, inpPolarity)).Start();
                                    //else Lexer(newAxioma1, range + 1, inpPolarity); // И отправляем значение обратно в распознавание
                                    return true;
                                }
                            }

                            string newAxioma = prevSimv + Rules[j][k] + endSimv;
                            //if(isRecurceOn) 
                                new Thread(() => Lexer(newAxioma, range + 1, inpPolarity)).Start();
                            //else Lexer(newAxioma, range + 1, inpPolarity); // И отправляем значение обратно в распознавание
                        }
                    }
                }
            }
            return true;
        }

        bool AnalMultiplix(int range, string Axioma, bool inpPolarity) 
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

                        useMultiplyRules(groupSimv, Axioma, j, mult, range, inpPolarity);
                    }
                }
            }
            return true;
        }

        bool useMultiplyRules(string groupSimv, string Axioma, int jj,  int mult, int range, bool inpPolarity) // Используем правила, которые содержат больше 1 символа в начале
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
                        // Если это правило не "удалено", то мы прибавляем ему счётчик исползований
                        if (countUseRules[j] != -1)
                        {
                            countUseRules[j] += 1;
                            if (isVisibleLambdaOnOutput == false)
                            {
                                if (Rules[j][k] == "&")
                                {
                                    string newAxioma1 = prevSimv + endSimv;
                                    //if(isRecurceOn) 
                                        new Thread(() => Lexer(newAxioma1, range + 1, inpPolarity)).Start();
                                    //else Lexer(newAxioma1, range + 1, inpPolarity); // И отправляем значение обратно в распознавание
                                    return true;
                                }
                            }

                            string newAxioma = prevSimv + Rules[j][k] + endSimv;
                            //if(isRecurceOn) 
                                new Thread(() => Lexer(newAxioma, range + 1, inpPolarity)).Start();
                            //else Lexer(newAxioma, range + 1, inpPolarity); // И отправляем значение обратно в распознавание
                        }
                    }
                }
            }
            return true;
        }
    }
}
