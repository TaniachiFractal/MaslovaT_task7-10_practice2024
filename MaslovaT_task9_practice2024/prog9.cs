using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace MaslovaT_task9_practice2024
{
    internal static class prog9
    {
        static void Main()
        {
            Writeln("Введите названия и координаты двух шахматных фигур разного цвета в формате \"queen h3\", \"bishop b6\", т.п, \n" +
                    "   а также координату клетки, куда хочет попасть первая фигура в формате h3, e3:\n");
            Write("Пример ввода: "); Write("king a1 queen h8 e4\n\n",ConsoleColor.White);
            
            Write(">_ ", ConsoleColor.Yellow);

            string inputLine = Console.ReadLine();
            if (inputLine.Split().Length != 5)
            {
                ErrorOutput("Некорректный ввод");
                return;
            }

            #region put data
            string mainPieceName = inputLine.Split()[0].ToLower();
            string mainPiecePos = inputLine.Split()[1].ToLower();
            string otherPieceName = inputLine.Split()[2].ToLower();
            string otherPiecePos = inputLine.Split()[3].ToLower();
            string destinationCell = inputLine.Split()[4].ToLower();
            #endregion

            #region recolor
            Console.SetCursorPosition(3, 5);

            Write(mainPieceName+' ', ConsoleColor.Green);
            Write(mainPiecePos + ' ', ConsoleColor.Green);

            Write(otherPieceName + ' ', ConsoleColor.Blue);
            Write(otherPiecePos + ' ', ConsoleColor.Blue);

            Write(destinationCell + ' ', ConsoleColor.Cyan);
            Writeln(" ");
            #endregion

            #region validation
            if (!CorrectChessPieceNameEng(mainPieceName) || !CorrectChessPieceNameEng(otherPieceName))
            {
                ErrorOutput("Некорректные названия фигур");
                return;
            }

            if (!TwoChessPiecesInputValid(mainPiecePos, otherPiecePos) 
                || !TwoChessPiecesInputValid(mainPiecePos,destinationCell) 
                    || !TwoChessPiecesInputValid(otherPiecePos,destinationCell))
            {
                ErrorOutput("Вы ввели некорректные координаты");
                return;
            }
            #endregion

            ChangeColor(ConsoleColor.Gray);
            Writeln("\nОТВЕТ:");

            if (ChessPieceCanEatPiece(mainPieceName, mainPiecePos, destinationCell))
            {
                if (ChessPieceCanEatPiece(otherPieceName, otherPiecePos, mainPiecePos))
                {
                    Write(mainPieceName, ConsoleColor.Green);
                    Write(" не дойдёт до ", ConsoleColor.Red);
                    Write(destinationCell, ConsoleColor.Cyan);
                }
                else
                {
                    Write(mainPieceName, ConsoleColor.Green);
                    Write(" дойдёт до ", ConsoleColor.White);
                    Write(destinationCell, ConsoleColor.Cyan);
                }
            }
            else
            {
                Write(mainPieceName, ConsoleColor.Green);
                Write(" не дойдёт до ", ConsoleColor.Red);
                Write(destinationCell, ConsoleColor.Cyan);
            }
            
            Console.ReadKey(true);
        }

        #region функции для удобства работы с консолью
        /// <summary>
        /// Convenient output
        /// </summary>
        static void Writeln(string message)
        {
            Console.WriteLine(message);
        }
        static void Write(string message)
        {
            Console.Write(message);
        }
        static void Write(string message, ConsoleColor color)
        {
            ChangeColor(color);
            Write(message);
        }
        /// <summary>
        /// Convenient color change
        /// </summary>
        static void ChangeColor(ConsoleColor inputColor)
        {
            Console.ForegroundColor = inputColor;
        }
        static void ErrorOutput(string message)
        {
            ChangeColor(ConsoleColor.Red);
            Writeln('\n' + message);
            Console.ReadKey(true);
        }
        #endregion
      
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
            if (piecePosition1 == piecePosition2)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// Английские названия шахматных фигур + Дословные переводы русских названий
        /// </summary>
        static readonly string[] chessPieceNamesEng = { "rook",   "boat",  
                                                         "knight", "horse", 
                                                          "bishop", "elephant", 
                                                           "queen",  "ferz",  
                                                            "king"};
        /// <summary>
        /// Проверка ввода названия фигуры
        /// </summary>
        static bool CorrectChessPieceNameEng(string pieceName)
        {
            for (int i = 0; i < chessPieceNamesEng.Length; i++)
            {
                if (chessPieceNamesEng[i] == pieceName.ToLower())
                {
                    return true;
                }
            }
            return false;
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

        #region 1st calc step functions

        /// <summary>
        /// Сумма двух координат шахматной фигуры, если а = 1; h = 8
        /// </summary>
        static int SumOfaPosition(string chessPiecePosition)
        {
            return LetterPartOfPositionInt(chessPiecePosition) + DigitPartOfPositionInt(chessPiecePosition);
        }
        /// <summary>
        /// Сумма двух координат шахматной фигуры, если а = 8; h = 1
        /// </summary>
        static int SumOfaPositionInverted(string chessPiecePosition)
        {
            return (9 - LetterPartOfPositionInt(chessPiecePosition)) + DigitPartOfPositionInt(chessPiecePosition);
        }
        /// <summary>
        /// Количество столбцов между фигурами
        /// </summary>
        static int ChessLetterColsDistance(string piecePosition1, string piecePosition2)
        {
            return Math.Abs(LetterPartOfPositionInt(piecePosition1) - LetterPartOfPositionInt(piecePosition2));
        }
        /// <summary>
        /// Количество строк между фигурами
        /// </summary>
        static int ChessDigitRowsDistance(string piecePosition1, string piecePosition2)
        {
            return Math.Abs(DigitPartOfPositionInt(piecePosition1) - DigitPartOfPositionInt(piecePosition2));
        }

        #endregion

        #region 2nd step functions

        /// <summary>
        /// Проверка, что у 2 фигур одинаковая буква -> стоят в одном столбце -> ладья может съесть
        /// </summary>
        static bool TwoChessPiecesHaveSameLetter(string piecePosition1, string piecePosition2)
        {
            if (LetterPartOfPositionInt(piecePosition1) == LetterPartOfPositionInt(piecePosition2))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Проверка, что у 2 фигур одинаковая цифра -> стоят в одной строке -> ладья может съесть
        /// </summary>
        static bool TwoChessPiecesHaveSameDigit(string piecePosition1, string piecePosition2)
        {
            if (DigitPartOfPositionInt(piecePosition1) == DigitPartOfPositionInt(piecePosition2))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Узнать, находятся ли 2 шахматные фигуры в одной диагонали, параллельной a8-h1 (их сумма кооржинат равна)
        /// </summary>
        static bool TwoChessPiecesInTheSameMainDiagonal(string piecePosition1, string piecePosition2)
        {
            return SumOfaPosition(piecePosition1) == SumOfaPosition(piecePosition2);
        }
        /// <summary>
        /// Узнать, находятся ли 2 шахматные фигуры в одной диагонали, параллельной a1-h8 (их сумма кооржинат равна, если одну координатную прямую перевернуть)
        /// </summary>
        static bool TwoChessPiecesInTheSameSecondaryDiagonal(string piecePosition1, string piecePosition2)
        {
            return SumOfaPositionInverted(piecePosition1) == SumOfaPositionInverted(piecePosition2);
        }

        #endregion

        #region Main functions for each piece

        /// <summary>
        /// Узнать, может ли ладья съесть фигуру
        /// </summary>
        static bool RookCanEatPiece(string rookPosition, string otherPiecePosition)
        {
            if (TwoChessPiecesHaveSameDigit(otherPiecePosition, rookPosition) || TwoChessPiecesHaveSameLetter(otherPiecePosition, rookPosition))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Узнать, может ли слон может побить фигуру
        /// </summary>
        static bool BishopCanEatPiece(string bishopPosition, string piecePosition)
        {
            if (TwoChessPiecesInTheSameMainDiagonal(piecePosition, bishopPosition) || TwoChessPiecesInTheSameSecondaryDiagonal(piecePosition, bishopPosition))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Узнать, может ли ферзь может побить фигуру
        /// </summary>
        static bool QueenCanEatPiece(string queenPosition, string piecePosition)
        {
            if (BishopCanEatPiece(queenPosition, piecePosition) || RookCanEatPiece(queenPosition, piecePosition))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Узнать, может ли король может побить фигуру
        /// </summary>
        static bool KingCanEatPiece(string kingPosition, string piecePosition)
        {
            if (ChessDigitRowsDistance(kingPosition, piecePosition) <= 1 && ChessLetterColsDistance(kingPosition, piecePosition) <= 1)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Узнать, может ли конь может побить фигуру
        /// </summary>
        static bool KnightCanEatPiece(string knightPosition, string piecePosition)
        {
            if (ChessDigitRowsDistance(knightPosition, piecePosition) == 2)
            {
                if (ChessLetterColsDistance(knightPosition, piecePosition) == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (ChessDigitRowsDistance(knightPosition, piecePosition) == 1)
            {
                if (ChessLetterColsDistance(knightPosition, piecePosition) == 2)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
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
        #endregion

        /// <summary>
        /// Выдаёт, может ли фигура съесть другую по заданным 
        /// </summary>
        static bool ChessPieceCanEatPiece(string mainPieceName,string mainPiecePos, string otherPiecePos)
        {
            switch (mainPieceName) 
            {
                case "rook":
                    return RookCanEatPiece(mainPiecePos, otherPiecePos);
                case "boat":
                    return RookCanEatPiece(mainPiecePos, otherPiecePos);
                case "knight":
                    return KnightCanEatPiece(mainPiecePos,otherPiecePos);
                case "horse":
                    return KnightCanEatPiece(mainPiecePos, otherPiecePos);
                case "bishop":
                    return BishopCanEatPiece(mainPiecePos, otherPiecePos);
                case "elephant":
                    return BishopCanEatPiece(mainPiecePos, otherPiecePos);
                case "queen":
                    return QueenCanEatPiece(mainPiecePos, otherPiecePos);
                case "ferz":
                    return QueenCanEatPiece(mainPiecePos, otherPiecePos);
                case "king":
                    return KingCanEatPiece(mainPiecePos, otherPiecePos);
                default: 
                    return false;
            }
        }
    }
}