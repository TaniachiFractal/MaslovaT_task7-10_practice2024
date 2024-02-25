using System;

namespace MaslovaT_task8_practice2024
{
    internal static class prog8
    {
        static void Main()
        {
            Console.WriteLine("Введите координаты двух шахматных полей в формате e3, b6, т.д: ");
            string inputLine = Console.ReadLine();
            string cell1 = inputLine.Split()[0];
            string cell2 = inputLine.Split()[1];

            if (!TwoChessPiecesInputValid(cell1, cell2))
            {
                Console.WriteLine("Вы ввели некорректные координаты");
                Console.ReadKey();
                return;
            }

            if (ChessCellsHaveSameColor(cell1, cell2))
            {
                Console.WriteLine("Одного цвета");
                Console.ReadKey();
                return;
            }
            else
            {
                Console.WriteLine("Разного цвета");
                Console.ReadKey();
                return;
            }
        }
        #region input validation

        /// <summary>
        /// Выдаёт правильно или неправильно записана координата шахматной фигуры. Правильный формат: h8 - [латинская буква a-h][цифра 1-8]
        /// </summary>
        static bool ChessPiecePositionValid(string input)
        {
            if (input.Length != 2) return false;
            if (!(input[0] >= 'a' && input[0] <= 'h')) return false;
            if (!(input[1] >= '1' && input[1] <= '8')) return false;
            return true;
        }
        /// <summary>
        /// Проверка двух шахматных координат на верность и на неравенство
        /// </summary>
        static bool TwoChessPiecesInputValid(string piecePosition1, string piecePosition2)
        {
            if (!ChessPiecePositionValid(piecePosition1) || !ChessPiecePositionValid(piecePosition2))
            {
                return false;
            }
            else if (piecePosition1 == piecePosition2)
            {
                return false;
            }
            return true;
        }

        #endregion

        #region split input string

        /// <summary>
        /// Получить букву координаты фигуры в числовом формате. Пример: ВВОД - b1, ВЫВОД - 2; ВВОД - h1, ВЫВОД - 8
        /// </summary>
        static int LetterPartOfPositionInt(string input)
        {
            return (int)(input[0]) - 96;
        }
        /// <summary>
        /// Получить цифру координаты фигуры в числовом формате. Пример: ВВОД - a1, ВЫВОД - 1
        /// </summary>
        static int DigitPartOfPositionInt(string input)
        {
            return (int)(input[1]) - 48;
        }

        #endregion


        /// <summary>
        /// Сумма двух координат шахматной фигуры, если а = 1; h = 8
        /// </summary>
        static int SumOfaPosition(string chessPiecePosition)
        {
            return LetterPartOfPositionInt(chessPiecePosition) + DigitPartOfPositionInt(chessPiecePosition);
        }
        /// <summary>
        /// Whether do 2 cells have the same color: their sums are both odd or both even
        /// </summary>
        static bool ChessCellsHaveSameColor(string cell1, string cell2)
        {
            if ((SumOfaPosition(cell1) & 1) == (SumOfaPosition(cell2) & 1))
            {
                return true;
            }
            return false;
        }


    }
}