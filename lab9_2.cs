using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab9_dod
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // ----------------------------------------------------------------------------
        // КЛАС: Seat (представляє одне місце)
        // ----------------------------------------------------------------------------
        public class Seat
        {
            // номер місця (1..n)
            public int Number { get; set; }

            // поле для перевірки місця: зайняте чи ні
            public bool IsReserved { get; set; }

            // Конструктор: задаємо номер і секцію (секція опціональна)
            public Seat(int number)
            {
                Number = number;        // присвоюємо номер місця
                IsReserved = false;     // за замовчуванням місце вільне
            }

            // Зручне текстове представлення для виводу в UI
            public override string ToString()
            {
                // Повертаємо рядок, який користувачу легко прочитати
                return $"Місце {Number} " + (IsReserved ? "зайняте" : "вільне");
            }
        }

        // ----------------------------------------------------------------------------
        // КЛАС: SeatCollection — колекція місць з індексатором
        // ----------------------------------------------------------------------------
        public class SeatCollection
        {
            private Seat[] seats;     // внутрішній масив для зберігання Seat
            public int Length { get; private set; }   // кількість місць
            public int ErrorKod { get; private set; } // код помилки останньої операції
                                                      // 0 - OK, 1 - некоректний індекс

            // Конструктор: створюємо масив місць заданого розміру
            public SeatCollection(int size)
            {
                if (size < 0) size = 0;
                seats = new Seat[size];
                Length = size;

                // Ініціалізуємо елементи масиву об'єктами Seat з номерами від 1 до size
                for (int i = 0; i < size; i++)
                {
                    seats[i] = new Seat(i + 1);
                }

                ErrorKod = 0; // початковий код — без помилок
            }

            // Перевірка індексу
            private bool OkIndex(int i)
            {
                return i >= 0 && i < Length;
            }

            // Індексатор: дозволяє читати/писати елементи через collection[index]
            public Seat this[int index]
            {
                get
                {
                    if (OkIndex(index))
                    {
                        ErrorKod = 0;       // успішно
                        return seats[index]; // повертаємо посилання на Seat
                    }
                    else
                    {
                        ErrorKod = 1;       // помилковий індекс
                        return null;
                    }
                }
                set
                {
                    if (!OkIndex(index))
                    {
                        ErrorKod = 1; // помилковий індекс 
                        return;
                    }

                    // Якщо value == null — звільняємо місце (ставимо новий об'єкт в масив)
                    if (value == null)
                    {
                        seats[index] = null;
                        ErrorKod = 0;
                        return;
                    }

                    // Вставляємо переданий Seat в масив
                    seats[index] = value;
                    ErrorKod = 0;
                }
            }

            // Метод для перемикання статусу резервування місця по індексу
            public void ToggleReservation(int index)
            {
                if (!OkIndex(index))
                {
                    ErrorKod = 1;
                    return;
                }

                // Перевіряємо що Seat існує (на випадок, якщо в масиві null)
                if (seats[index] == null)
                {
                    ErrorKod = 1;
                    return;
                }

                seats[index].IsReserved = !seats[index].IsReserved;
                ErrorKod = 0;
            }
        }
        // ----------------------------------------------------------------------------
        // ОБРОБНИК КНОПКИ button1_Click
        // створити колекцію місць (SeatCollection), потім за допомогою індексатора
        // записати у неї кілька об'єктів Seat і показати результати
        // ----------------------------------------------------------------------------

        private void button1_Click(object sender, EventArgs e)
        {
            // Створюємо колекцію з 8 місць
            SeatCollection hall = new SeatCollection(5);

            // Змінна для накопичення повідомлень про хід операцій
            string sMessage = "";

            // Спробуємо записати окремі місця в колекцію через індексатор.
            // Робимо кілька операцій запису і перевіряємо ErrorKod
            hall[0] = new Seat(1); // записуємо у 0-й елемент
            if (hall.ErrorKod != 0)
                sMessage += "\nПомилка при додаванні місця 1 (індекс 0) - код " + hall.ErrorKod;
            else
                sMessage += "\nДодано місце 1 (індекс 0)";

            hall[1] = new Seat(2);
            if (hall.ErrorKod != 0)
                sMessage += "\nПомилка при додаванні місця 2 (індекс 1) - код " + hall.ErrorKod;
            else
                sMessage += "\nДодано місце 2 (індекс 1)";

            hall[2] = new Seat(3);
            if (hall.ErrorKod != 0)
                sMessage += "\nПомилка при додаванні місця 3 (індекс 2) - код " + hall.ErrorKod;
            else
                sMessage += "\nДодано місце 3 (індекс 2)";
            hall[3] = new Seat(4);
            if (hall.ErrorKod != 0)
                sMessage += "\nПомилка при додаванні місця 4 (індекс 3) - код " + hall.ErrorKod;
            else
                sMessage += "\nДодано місце 4 (індекс 3)";
            hall[4] = new Seat(5);
            if (hall.ErrorKod != 0)
                sMessage += "\nПомилка при додаванні місця 5 (індекс 4) - код " + hall.ErrorKod;
            else
                sMessage += "\nДодано місце 5 (індекс 4)";
            // Приклад некоректного запису
            hall[20] = new Seat(21); // цей запис не вдасться; ErrorKod стане 1
            if (hall.ErrorKod != 0)
                sMessage += "\nСпроба записати за межі масиву: індекс 20 - код " + hall.ErrorKod;

            // Перемикаємо резервування деяких місць через метод ToggleReservation
            hall.ToggleReservation(1); // місце з індексом 1 (номер 2)
            if (hall.ErrorKod == 0) sMessage += "\nМісце 2 зарезервовано.";
            else sMessage += "\nПомилка при резервуванні місця 2 - код " + hall.ErrorKod;

            hall.ToggleReservation(4); // індекс 4 -> номер 5
            if (hall.ErrorKod == 0) sMessage += "\nМісце 5 зарезервовано.";
            else sMessage += "\nПомилка при резервуванні місця 5 - код " + hall.ErrorKod;

            // Відображаємо повідомлення про операції у label1
            label1.Text = sMessage;

            // Формуємо детальний список всіх наявних місць і виводимо у label2
            string list = "";
            for (int i = 0; i < hall.Length; i++)
            {
                // Використовуємо індексатор для читання. 
                Seat seat = hall[i];
                if (hall.ErrorKod != 0)
                {
                    // Якщо при читанні виникла помилка (наприклад, індекс за межами),
                    // записуємо відповідне повідомлення
                    list += $"\nІндекс {i}: помилка читання (код {hall.ErrorKod})";
                }
                else
                {
                    if (seat != null)
                        list += "\n" + seat.ToString();
                    else
                        list += $"\nМісце {i + 1} — порожнє (null)";
                }
            }

            label2.Text = list;
        }
    }
}
