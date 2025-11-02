using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace lab9
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public class CaseTransistors
        {
            public string transType { get; set; } // тип транзистора
            public string transName { get; set; } // назва транзистора
            public string transModelName { get; set; } // назва математичної моделі транзистора
            transPrefixName[] Prefixs; // масив для зберігання правильних префіксів імен транзисторів
            CaseTransistors[] transistors; // масив для зберігання транзисторів із модифікатором
                                           // доступу private (за замовчуванням)
            public int Length; // кількість транзисторів у масиві
            public int ErrorKod; // код завершення операції записування або читання
                                 // 0 - все добре, 1 - поганий індекс, 2 - поганий префікс імені
                                 // Конструктор класу CaseTransistors, у якому створюємо масив для зберігання транзисторів - екземплярів // класу CaseTransistors
            public CaseTransistors(int size, string type, string tname, string modelName)
            {
                transistors = new CaseTransistors[size]; // створюємо масив розміру,який задано у параметрі size
                Length = size;
                setPrefixName(); // заповнюємо масив структур префіксів імен транзисторів можливими значеннями
                transType = type;
                transName = tname;
                transModelName = modelName;
            } // завершення тіла конструктора
              // Структура transPrefixName, яка призначена для оголошення префіксів імен транзисторів
            struct transPrefixName
            {
                public string PrefixName; // ім'я префіксу
                public string PrefixText; // пояснення значення префіксу
            }
            public override string ToString() // перевизначений метод для видачі інформації про транзистор
            {
                return " Транзистор " + transName + " Тип- " + transType + " модель- " + transModelName;
            }
            void setPrefixName()
            {
                // Масив структур для зберігання правильних префіксів імен транзисторів
                Prefixs = new transPrefixName[14];
                // Ця довідкова інформація є реальною. Див. http://en.wikipedia.org/wiki/Transistor
                Prefixs[0].PrefixName = "AC"; Prefixs[0].PrefixText = "Germanium small-signal AF transistor AC126";
                Prefixs[1].PrefixName = "AD"; Prefixs[0].PrefixText = "Germanium AF power transistor AD133";
                Prefixs[2].PrefixName = "AF"; Prefixs[0].PrefixText = "Germanium small-signal RF transistor AF117";
                Prefixs[3].PrefixName = "AL"; Prefixs[0].PrefixText = "Germanium RF power transistor ALZ10";
                Prefixs[4].PrefixName = "AS"; Prefixs[0].PrefixText = "Germanium switching transistor ASY28";
                Prefixs[5].PrefixName = "AU"; Prefixs[0].PrefixText = "Germanium power switching transistor AU103";
                Prefixs[6].PrefixName = "BC"; Prefixs[0].PrefixText = "Silicon, small signal transistor BC548B";
                Prefixs[7].PrefixName = "BD"; Prefixs[0].PrefixText = "Silicon, power transistor BD139";
                Prefixs[8].PrefixName = "BF"; Prefixs[0].PrefixText = "Silicon, RF (high frequency) BJT or FET BF245";
                Prefixs[9].PrefixName = "BS"; Prefixs[0].PrefixText = "Silicon, switching transistor (BJT or MOSFET) BS170";
                Prefixs[10].PrefixName = "BL"; Prefixs[0].PrefixText = "Silicon, high frequency, high power (for transmitters) BLW34";
                Prefixs[11].PrefixName = "BU"; Prefixs[0].PrefixText = "Silicon, high voltage (for CRT horizontal deflection circuits) BU508";
                Prefixs[12].PrefixName = "CF"; Prefixs[0].PrefixText = "Gallium Arsenide small-signal Microwave transistor (MESFET) CF300";
                Prefixs[13].PrefixName = "CL"; Prefixs[0].PrefixText = "Gallium Arsenide Microwave power transistor (FET) CLY10";
            }
            bool OkPrefixName(string prefix) // метод для визначення правильності префіксу в імені транзистора
            {
                for (int i = 0; i < 14; i++)
                {
                    if (Prefixs[i].PrefixName == prefix) return true;
                }
                return false;
            }
            bool OkIndex(int i) // метод для визначення правильності індексу
            {
                if (i >= 0 && i < Length) return true;
                else return false;
            }
            public CaseTransistors this[int index] // індексатор для класу CaseTransistors по полю transistors
            {
                get // вибрати об'єкт типу транзистор з індексом index
                {
                    if (OkIndex(index)) // якщо правильний індекс, то повертаємо елемент масиву і код завершення 0
                    {
                        ErrorKod = 0;
                        return transistors[index];
                    }
                    else
                    {
                        ErrorKod = 1; // якщо індекс не правильний, то повертаємо null і код завершення 1
                        return null;
                    }
                }
                set // записати транзистор у масив транзисторів у елемент з індексом index
                {
                    // Перед записом транзистора у масив перевіряємо індекс,
                    if (!OkIndex(index))
                    {
                        ErrorKod = 1;
                        return;
                    }
                    // а також перевіряємо, чи належать 2 перших символи імені до таблиці префіксів імен транзисторів
                    // Якщо префікс не правильний, то повертаємо код завершення 2 і транзистор у бібліотеку не записуємо
                    if (!OkPrefixName(value.transName.Substring(0, 2)))
                    {
                        ErrorKod = 2;
                        return;
                    }
                    transistors[index] = value; // Якщо індекс та ім'я правильні, то записуємо транзистор, який
                                                // передано у змінній value у масив транзисторів
                    ErrorKod = 0; // і встановлюємо код завершення 0
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Створюємо екземпляри класу CaseTransistors з допомогою конструктора з параметрами
            // (Тут типи транзисторів обрано навмання, вони не відповідають своїм іменам)
            CaseTransistors MyTr = new CaseTransistors(5, "Bipolar", "AC126", "EbbersMoll");
            CaseTransistors MyTr1 = new CaseTransistors(1, "Field-effet", "AC126", "Gummel-Poon");
            CaseTransistors MyTr2 = new CaseTransistors(1, "Field-effet", "AD133", "Gummel-Poon");
            CaseTransistors MyTr3 = new CaseTransistors(1, "Schottky", "BD139", "Gummel-Poon");
            CaseTransistors MyTr4 = new CaseTransistors(1, "Avalanche", "OO117", "EbbersMoll");
            CaseTransistors MyTr5 = new CaseTransistors(1, "Darlington", "BLW34", "EbbersMoll");
            CaseTransistors MyTr6 = new CaseTransistors(1, "Photo", "BU508", "EbbersMoll");
            CaseTransistors MyTr7 = new CaseTransistors(1, "Bipolar", "CLY10", "EbbersMoll");
            // Поміщаємо створені транзистори у масив транзисторів з допомогою індексатора.
            // Перевіряємо код завершення після кожної операції
            // Накопичуємо інформацію про хід роботи програми у змінній sMessage
            string sMessage = " ";
            MyTr[0] = MyTr1;
            if (MyTr.ErrorKod > 0) sMessage = sMessage + "\n 1 Транзистор не додано " + MyTr1.transName + " код помилки -" + MyTr.ErrorKod.ToString();
            else sMessage = sMessage + "\n 1 Транзистор додано " + MyTr1.transName + " ";
            MyTr[1] = MyTr2;
            if (MyTr.ErrorKod > 0) sMessage = sMessage + "\n 2 Транзистор не додано " + MyTr2.transName + " код помилки -" + MyTr.ErrorKod.ToString();
            else sMessage = sMessage + "\n 2 Транзистор додано " + MyTr2.transName + " ";
            MyTr[2] = MyTr3;
            if (MyTr.ErrorKod > 0) sMessage = sMessage + "\n 3 Транзистор не додано " + MyTr3.transName + " код помилки -" + MyTr.ErrorKod.ToString();
            else sMessage = sMessage + "\n 3 Транзистор додано " + MyTr3.transName + " ";
            MyTr[3] = MyTr4;
            if (MyTr.ErrorKod > 0) sMessage = sMessage + "\n 4 Транзистор не додано " + MyTr4.transName + " код помилки -" + MyTr.ErrorKod.ToString();
            else sMessage = sMessage + "\n 4 Транзистор додано " + MyTr4.transName + " ";
            MyTr[4] = MyTr5;
            if (MyTr.ErrorKod > 0) sMessage = sMessage + "\n 5 Транзистор не додано " + MyTr5.transName + " код помилки -" + MyTr.ErrorKod.ToString();
            else sMessage = sMessage + "\n 5 Транзистор додано " + MyTr5.transName + " ";
            MyTr[5] = MyTr6;
            if (MyTr.ErrorKod > 0) sMessage = sMessage + "\n 6 Транзистор не додано " + MyTr6.transName + " код помилки -" + MyTr.ErrorKod.ToString();
            else sMessage = sMessage + "\n 6 Транзистор додано " + MyTr6.transName + " ";
            MyTr[6] = MyTr7;
            if (MyTr.ErrorKod > 0) sMessage = sMessage + "\n 7 Транзистор не додано " + MyTr7.transName + " код помилки -" + MyTr.ErrorKod.ToString();
            else sMessage = sMessage + "\n 7 Транзистор додано " + MyTr7.transName + " ";
            label1.Text = sMessage;
            // Виведемо інформацію про додані (чи не додані) транзистори у мітку label1
            // Сформуємо інформацію про записані транзистори з допомогою перевизначеного нами методу ToString у
            // змінній sMessage
            sMessage = "";
            for (int i = 0; i < MyTr.Length; i++)
            {
                if (MyTr[i] != null) sMessage = sMessage + "\n " + MyTr[i].ToString();
            }
            label2.Text = sMessage;
        }
    }
}
